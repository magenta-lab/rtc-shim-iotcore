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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace rtc_shim_iotcore
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ChangeTimePage : Page
    {
        private DateTime default_date;
        private static RTCManagerAsync timeManager;
        internal static void setTimeManager(RTCManagerAsync t)
        {
            timeManager = t;
        }
        public ChangeTimePage()
        {
            this.InitializeComponent();
            default_date = new DateTime(2019, 10, 5, 14, 32, 0, 0);
            updateDefaultdate(default_date);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (timeManager!=null)
            {
                DateTime dt = datePicker.Date.DateTime;
                TimeSpan t = timePicker.SelectedTime.Value;
                DateTime complete_date = new DateTime(dt.Year, dt.Month, dt.Day, t.Hours, t.Minutes, t.Seconds, t.Milliseconds);
                bool ok = timeManager.SetTime(complete_date);
            }

            this.Frame.Navigate(typeof(MainPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
           if (e.Parameter is DateTime)
            {
                DateTime time_arrived = (DateTime)e.Parameter;
                updateDefaultdate(time_arrived);
            }
        }

        private void updateDefaultdate(DateTime date)
        {
            this.datePicker.Date = date.Date;
            this.timePicker.Time = new TimeSpan(date.Hour, date.Minute, date.Second);
        }
    }
}