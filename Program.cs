using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Web.Script.Serialization;

public class Program
{
    private static HttpListener listener;
    private static readonly String root = AppDomain.CurrentDomain.BaseDirectory;
    private static Config config = new Config();
    private static readonly String ruleName = "StaticWebServer";

    public class Config
    {
        public Int32 Port { get; set; } = 8888;
        public String StartPage { get; set; } = "index.html";
    }

    [DllImport("kernel32")]
    private static extern Boolean SetConsoleCtrlHandler(ConsoleCtrlDelegate lpfn, Boolean Add);
    private delegate Boolean ConsoleCtrlDelegate(Int32 type);

    [STAThread]
    private static void Main()
    {
        // ==============================================
        // 🔥 自动判断：不是管理员 → 自动重启为管理员
        // ==============================================
        if (!IsAdmin())
        {
            RunAsAdmin();
            return;
        }

        _ = SetConsoleCtrlHandler(OnExit, true);
        LoadConfig();

        try
        {
            AddFirewallRule();

            listener = new HttpListener();
            listener.Prefixes.Add("http://*:" + config.Port + "/");
            listener.Start();

            _ = Process.Start("http://127.0.0.1:" + config.Port);

            Console.WriteLine("==================================");
            Console.WriteLine("服务已启动 ✅");
            Console.WriteLine("本地：http://127.0.0.1:" + config.Port);
            Console.WriteLine("启动页：" + config.StartPage);
            Console.WriteLine("");
            Console.WriteLine("【可访问地址】");
            PrintLocalIpv4();
            PrintIpv6();
            Console.WriteLine("==================================");
            Console.WriteLine("关闭窗口自动清理防火墙");
            Console.WriteLine("==================================");

            while (true)
            {
                var ctx = listener.GetContext();
                ProcessRequest(ctx);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("错误：" + ex.Message);
            Stop();
        }
    }

    // ==============================================
    // 管理员判断
    // ==============================================
    private static Boolean IsAdmin()
    {
        var identity = WindowsIdentity.GetCurrent();
        var principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    // ==============================================
    // 自动以管理员重启
    // ==============================================
    private static void RunAsAdmin()
    {
        var psi = new ProcessStartInfo
        {
            FileName = Assembly.GetExecutingAssembly().Location,
            Verb = "runas",
            UseShellExecute = true
        };

        try
        {
            _ = Process.Start(psi);
            Environment.Exit(0);
        }
        catch { }
    }

    // ==============================================
    // 读取 JSON 配置
    // ==============================================
    private static void LoadConfig()
    {
        try
        {
            var path = Path.Combine(root, "config.json");
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                var serializer = new JavaScriptSerializer();
                config = serializer.Deserialize<Config>(json);
            }
        }
        catch { }
    }

    private static void PrintLocalIpv4()
    {
        try
        {
            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    Console.WriteLine("内网IPv4：http://" + ip + ":" + config.Port);
                }
            }
        }
        catch { }
    }

    private static void PrintIpv6()
    {
        try
        {
            foreach (var ip in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    var ipStr = ip.ToString();
                    if (!ipStr.StartsWith("fe80:") && ipStr != "::1")
                    {
                        Console.WriteLine("外网IPv6：http://[" + ipStr + "]:" + config.Port);
                    }
                }
            }
        }
        catch { }
    }

    private static void ProcessRequest(HttpListenerContext ctx)
    {
        try
        {
            var path = ctx.Request.Url.LocalPath.TrimStart('/');
            if (String.IsNullOrEmpty(path))
            {
                path = config.StartPage;
            }

            var file = Path.Combine(root, path);
            if (File.Exists(file))
            {
                var bytes = File.ReadAllBytes(file);
                ctx.Response.OutputStream.Write(bytes, 0, bytes.Length);
            }
            else
            {
                ctx.Response.StatusCode = 404;
            }
        }
        catch { }
        finally
        {
            try { ctx.Response.Close(); } catch { }
        }
    }

    private static void AddFirewallRule() => RunNetsh("advfirewall firewall add rule name=\"" + ruleName + "\" dir=in action=allow protocol=TCP localport=" + config.Port + " profile=any");

    private static void RemoveFirewallRule() => RunNetsh("advfirewall firewall delete rule name=\"" + ruleName + "\"");

    private static void RunNetsh(String args)
    {
        var p = new Process();
        p.StartInfo.FileName = "netsh";
        p.StartInfo.Arguments = args;
        p.StartInfo.CreateNoWindow = true;
        _ = p.Start();
        p.WaitForExit();
    }

    private static Boolean OnExit(Int32 type)
    {
        Stop();
        return false;
    }

    private static void Stop()
    {
        try { listener.Stop(); } catch { }
        RemoveFirewallRule();
        Environment.Exit(0);
    }
}