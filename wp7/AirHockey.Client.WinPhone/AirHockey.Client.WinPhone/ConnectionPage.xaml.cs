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
using Microsoft.Phone.Shell;

namespace AirHockey.Client.WinPhone
{
    public partial class ConnectionPage : PhoneApplicationPage
    {
        private ProgressIndicator indicator=new ProgressIndicator()
        {
            IsVisible = false,
            IsIndeterminate = true,
            Text = ""
        };

        // Constructor
        public ConnectionPage()
        {
            InitializeComponent();
            SystemTray.SetProgressIndicator(this, indicator);
        }

        private void StartGame(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Accelerometr.xaml", UriKind.Relative)); 
        }

        private void ApplicationBarFindButton_Click(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            
            //some method to find server
            MessageText.Text = "Server Finded!";
            //indicator.IsVisible = false;

        }

        private void ApplicationBarConnectButton_Click(object sender, EventArgs e)
        {
            //indicator.IsVisible = true;
            MessageText.Text = "Connect succesful. You can start the game!";
            //some method to connect to server
            indicator.IsVisible = false;
        }
    }
}