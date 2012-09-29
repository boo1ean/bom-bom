using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace AirHockey.Client.WinPhone
{
    using System.Windows.Threading;

    public partial class GamePage : PhoneApplicationPage
    {
        DispatcherTimer timer;

        public GamePage()
        {
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Tick += this.Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs args)
        {
            int seconds = Convert.ToInt32(time.Text);
            if (seconds != 1)
            {
                seconds--;
                time.Text = seconds.ToString();
            }
            else
            {
                time.Visibility = Visibility.Collapsed;
                timer.Stop();
            }
        }
    }
}