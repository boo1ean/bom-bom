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
using AirHockey.Client.WinPhone.Network;
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

        private SocketClient socketClient = new SocketClient();

        // Constructor
        public ConnectionPage()
        {
            InitializeComponent();
            SystemTray.SetProgressIndicator(this, indicator);
            socketClient.ConnectCompleted += socketClient_ConnectCompleted;
            socketClient.ConnectFailed += socketClient_ConnectFailed;
        }

        private void StartGame(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Accelerometr.xaml", UriKind.Relative)); 
        }

        //private void ApplicationBarFindButton_Click(object sender, EventArgs e)
        //{
        //    indicator.IsVisible = true;
            
        //    //some method to find server
        //    MessageText.Text = "Server Finded!";
        //    //indicator.IsVisible = false;

        //}

        private void ApplicationBarConnectButton_Click(object sender, EventArgs e)
        {
            indicator.IsVisible = true;
            socketClient.Connect(IpText.Text,5000);
        }

        void socketClient_ConnectFailed(System.Net.Sockets.SocketError result)
        {
            Dispatcher.BeginInvoke(() => ConnectFailed(result));
        }

        void socketClient_ConnectCompleted()
        {
            Dispatcher.BeginInvoke(ConnectCompleted);
        }

        private void ConnectCompleted()
        {
            indicator.IsVisible = false;
            MessageText.Text = "Connected!!!!!!";
            var playButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            playButton.IsEnabled = true;
        }

        private void ConnectFailed(System.Net.Sockets.SocketError result)
        {
            indicator.IsVisible = false;
            MessageText.Text = "Connection failed!!!!!!";
            var playButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            playButton.IsEnabled = false;
        }
    }
}