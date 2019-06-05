using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtc_shim_iotcore
{
    class RTCManager
    {

        MCP7940N rtc_controller;

        public DateTime GetTime()
        {
            return rtc_controller.GetTime();
        }

        public RTCManager()
        {

        }

        public async void Start(string i2c_channel = "I2C1")
        {
            rtc_controller = new MCP7940N(i2c_channel);
            await rtc_controller.Initialize();
        }
        public bool SetTime(DateTime new_time)
        {
            if (rtc_controller == null)
            {
                return false;
            }
            rtc_controller.SetTime(new_time);
            return true;
        }

        public bool isAlive()
        {
            if (rtc_controller == null)
            {
                return false;
            }
            return rtc_controller.IsConnected();
        }
    }
}