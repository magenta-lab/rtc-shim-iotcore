using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtc_shim_iotcore
{
    public class RTCManager
    {
        private MCP7940N rtc_controller;

        public DateTime GetTime()
        {
            return rtc_controller.GetTime();
        }

        public RTCManager(MCP7940N instance)
        {
            rtc_controller = instance;
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