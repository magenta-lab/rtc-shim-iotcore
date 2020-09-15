using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace rtc_shim_iotcore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private static RTCManagerAsync timeManager;
        private DateTime _currentTime;
        private DateTime CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                updatetime();
            }
        }
        internal static void setTimeManager(RTCManagerAsync t)
        {
            timeManager = t;
        }
        public MainPage()
        {
            this.InitializeComponent();
            timeManager.TimeChanged += TimeManager_TimeChanged;
            if (timeManager != null)
            {
                CurrentTime = timeManager.ReadTime();
            }
        }

        private void TimeManager_TimeChanged(object sender, EventArgs e)
        {
            TimeEventArgs eventArgs = (TimeEventArgs)e;
            CurrentTime = eventArgs.Time;
        }

        private void updatetime()
        {
            txtDate.Text = _currentTime.ToLongDateString();
            txtClock.Text = _currentTime.ToShortTimeString();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentTime = timeManager.ReadTime();
        }

        private void BtnChangeTime_Click(object sender, RoutedEventArgs e)
        {
            timeManager.TimeChanged -= TimeManager_TimeChanged;
            this.Frame.Navigate(typeof(ChangeTimePage), CurrentTime);
        }
    }
}