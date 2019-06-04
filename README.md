# rtc-shim-iotcore
A Simple .NET c# module (App and Connector classes) to deal with SHIM RTC for Raspberry Pi having MCP7940N module.
### Compatibility:
This project requires a minimum version of **Windows 10 version 1809 (10.0; build 17763)** so make sure to select the right version when creating a new project (see image below).
  ![MinimumCompatibility2](https://user-images.githubusercontent.com/27868408/58882354-5fd17380-86dc-11e9-8bd4-0918de673ac0.png)
### Usage:
- Clone the repo, build it and run it into your iotcore device.
- Use connector class [MCP7940N](MCP7940N.cs) or a time manager class between [RTCManager](RTCManager.cs) and [RTCManagerAsync](RTCManagerAsync.cs) and incorporate them into your project.
