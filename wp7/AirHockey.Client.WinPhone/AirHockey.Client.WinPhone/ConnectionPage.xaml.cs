using System;
using System.Net.Sockets;
using System.Windows.Input;
using AirHockey.Client.WinPhone.Infrastructure;
using AirHockey.Client.WinPhone.Network;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace AirHockey.Client.WinPhone
{
    public partial class ConnectionPage : PhoneApplicationPage
    {
        private IOClass ioClass = new IOClass();

        private ProgressIndicator indicator=new ProgressIndicator()
        {
            IsVisible = false,
            IsIndeterminate = true,
            Text = ""
        };

        private SocketClient socketClient = SocketClient.Client;

        // Constructor
        public ConnectionPage()
        {
            InitializeComponent();
            SystemTray.SetProgressIndicator(this, indicator);
            socketClient.ConnectCompleted += socketClient_ConnectCompleted;
            socketClient.ConnectFailed += socketClient_ConnectFailed;
            IpText.Text=ioClass.getIpAddress();
            UserName.Text=ioClass.getUserName();
        }

        private void StartGame(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative)); 
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
            if (IpText.Text != "" && UserName.Text != "")
            {
                ioClass.saveIpAddress(IpText.Text);
                ioClass.saveUserName(UserName.Text);
                var ip = string.Empty;
                var port = string.Empty;
                var ipParser = new IpParser();
                try
                {
                    ipParser.Parse(IpText.Text, ref ip, ref port);
                    socketClient.Connect(ip, Int32.Parse(port));
                    indicator.IsVisible = true;
                }
                catch (Exception)
                {
                    MessageText.Text = "Error!";
                }
            }
            else
            {
                MessageText.Text = "Ip address or User name is empty!";
            }
        }

        void socketClient_ConnectFailed(SocketError result)
        {
            Dispatcher.BeginInvoke(() => ConnectFailed(result));
        }

        void socketClient_ConnectCompleted()
        {
            socketClient.SendInitInfo();
            Dispatcher.BeginInvoke(() => socketClient.SendName(UserName.Text));
            Dispatcher.BeginInvoke(ConnectCompleted);
        }

        private void ConnectCompleted()
        {
            indicator.IsVisible = false;
            MessageText.Text = "Connected!!!!!!";
            var playButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            playButton.IsEnabled = true;
        }

        private void ConnectFailed(SocketError result)
        {
            indicator.IsVisible = false;
            MessageText.Text = "Connection failed!!!!!!";
            var playButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            playButton.IsEnabled = false;
        }

        private void IpText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ioClass.saveIpAddress(IpText.Text);
        }

        private void UserName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            ioClass.saveUserName(UserName.Text);
        }

        private void KeyUp_Focus(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                this.Focus();
            }
        }
    }
}