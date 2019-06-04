using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rtc_shim_iotcore
{
    class TimeEventArgs : EventArgs
    {
        public DateTime Time { get; private set; }
        public TimeEventArgs(DateTime time)
        {
            Time = time;
        }
    }
}
