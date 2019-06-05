# rtc-shim-iotcore
A Simple .NET c# module (App and Connector classes) to deal with SHIM RTC for Raspberry Pi having MCP7940N module.

**Why** do we need this? Because there is no driver right now to comunicate with this hardware module and online solutions (mostly StackOverflow) refer to DS3231 module instead of MCP7940N (IC used by rtc shim module).
### Software Compatibility:
This project requires a minimum version of **Windows 10 version 1809 (10.0; build 17763)** so make sure to select the right version when creating a new project (see image below).
  ![MinimumCompatibility2](https://user-images.githubusercontent.com/27868408/58882354-5fd17380-86dc-11e9-8bd4-0918de673ac0.png)
### Usage:
- Clone the repo, build it and run it into your iotcore device.
- Use connector class [MCP7940N](RTC/MCP7940N.cs) or a time manager class between [RTCManager](RTC/RTCManager.cs) and [RTCManagerAsync](RTC/RTCManagerAsync.cs) and incorporate them into your project ([RTC](RTC) folder).

### Pages:
Main page:

![home](https://user-images.githubusercontent.com/27868408/58964717-a17f1e80-87af-11e9-9c87-e942e00336c0.jpg)

Change time page:

![change_hour](https://user-images.githubusercontent.com/27868408/58964773-c4a9ce00-87af-11e9-8ac3-12b17f427546.jpg)
