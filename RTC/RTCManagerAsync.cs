using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtc_shim_iotcore
{
    class RTCManagerAsync
    {
        MCP7940N rtc_controller;
        Windows.UI.Xaml.DispatcherTimer dt;

        public event EventHandler TimeChanged;

        private DateTime current_time;

        public DateTime GetTime()
        {
            return current_time;
        }

        public DateTime ReadTime()
        {
            if (rtc_controller != null)
            {
                return rtc_controller.GetTime();
            }
            else
            {
                return new DateTime();
            }
        }

        public RTCManagerAsync()
        {
            current_time = new DateTime();
        }

        public async void start(string i2c_channel = "I2C1")
        {
            if (rtc_controller == null)
            {
                rtc_controller = new MCP7940N(i2c_channel);
                await rtc_controller.Initialize();
            }
            dt = new Windows.UI.Xaml.DispatcherTimer();
            dt.Interval = TimeSpan.FromSeconds(20);
            dt.Tick += dt_ticker;
            dt.Start();
        }

        private void dt_ticker(object sender, object e)
        {
            DateTime newTime = rtc_controller.GetTime();
            if (differentTime(newTime, current_time))
            {
                current_time = newTime;
                if (TimeChanged != null)
                {
                    TimeChanged.Invoke(this, new TimeEventArgs(current_time));
                }
            }
        }
        /// <summary>
        /// Return true if time is different.
        /// No seconds and milliseconds controlled.
        /// </summary>
        /// <param name="newTime"></param>
        /// <param name="current_time"></param>
        /// <returns></returns>
        private bool differentTime(DateTime newTime, DateTime current_time)
        {
            return newTime.Year != current_time.Year || newTime.Month != current_time.Month || newTime.Day != current_time.Day || newTime.Hour != current_time.Hour || newTime.Minute != current_time.Minute;
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