# WAMP 2.x on .NET Core

A quick-and-dirty demo of WampSharp on .NET Core.

Note that client and server demonstrate different ways of enabling cross-platform WebSocket support (without which you'll see something like `System.PlatformNotSupportedException: The WebSocket protocol is not supported on this platform` on MacOS or Linux):

* Server uses the WebSockets support bundled with WampSharp (`WampSharp.AspNetCore.WebSockets.Server`)
* Client uses .NET Core 1.1 (`System.Net.WebSockets.Client` v4.3.0)
