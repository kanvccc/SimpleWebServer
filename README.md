# Simple Static Web Server
## 简易静态网页服务器

> A lightweight static web server for Windows based on .NET Framework 4.8.
> 基于 .NET Framework 4.8 开发的 Windows 轻量静态网页服务器

### 项目简介 / Introduction
依托 Windows 系统自带的 .NET Framework 4.8 运行，**无需额外安装任何运行环境**。程序内置管理员权限自动提权逻辑，双击即可启动，默认零配置开箱即用。

支持通过 JSON 文件自定义监听端口与默认启动页；启动后自动配置防火墙规则，并展示内网 IPv4、公网 IPv6 访问地址，同时兼容内网与外网访问。关闭程序时会自动清理防火墙规则，保证系统环境整洁。

整体为单文件控制台程序，体积小巧、运行稳定，适用于前端本地调试、静态站点快速部署、小型文件共享等场景。

Built on the pre-installed .NET Framework 4.8 on Windows, **no extra dependencies required**.
The program automatically elevates to administrator privileges. Just double-click to run with default settings.

You can customize listening port and default page via JSON config file. It automatically manages firewall rules and displays local IPv4 & public IPv6 addresses for access. All firewall rules will be removed automatically after exit.

Compact, stable and easy to use. Perfect for front-end development, static website deployment and local file sharing.

---

### 核心特性 / Features
- ✅ Windows 原生支持，零依赖，Win10 / Win11 直接运行
- ✅ 自动申请管理员权限，无需手动右键提权
- ✅ 双击启动，默认零配置，上手简单
- ✅ JSON 配置文件，灵活自定义端口、启动页
- ✅ 防火墙自动管理，启动添加、退出清理
- ✅ 自动探测并展示 IPv4 / IPv6 全网络访问地址
- ✅ 纯单文件程序，体积小、运行高效

- ✅ Native Windows support, zero dependencies
- ✅ Auto administrator privilege elevation
- ✅ Double-click to run, no configuration required by default
- ✅ JSON-based configuration for port & homepage
- ✅ Automatic firewall rules management
- ✅ Auto detect IPv4 & IPv6 access addresses
- ✅ Single executable, lightweight and high performance

---

### 使用方法 / Usage
1. 将 `WebServer.exe`、`config.json` 与网页资源放在同一目录
2. 双击 `WebServer.exe`，程序将自动提权并启动服务
3. 控制台会输出所有可用访问地址，使用浏览器访问即可
4. 关闭控制台窗口，服务停止并自动清理防火墙规则

1. Place `WebServer.exe`, `config.json` and web files in the same folder
2. Double-click `WebServer.exe` to start the service
3. All access addresses are shown in the console
4. Close the console to stop service and clean firewall rules

---
### 配置说明 / Configuration (`config.json`)
```json
{
  "Port": 8888,
  "StartPage": "index.html"
}
```

---

### 开源协议 / License
This project is open source under the **MIT License**.

Copyright (c) 2026 kanvccc

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
