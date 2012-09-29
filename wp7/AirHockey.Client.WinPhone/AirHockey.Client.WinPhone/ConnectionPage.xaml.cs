using System;
using System.Net.Sockets;
using System.Windows.Input;
using AirHockey.Client.WinPhone.Infrastructure;
using AirHockey.Client.WinPhone.Network;
using Microsoft.Phone.Shell;

namespace AirHockey.Client.WinPhone
{
    public partial class ConnectionPage
    {
        private readonly IOClass _ioClass = new IOClass();

        private readonly ProgressIndicator _indicator = new ProgressIndicator
        {
            IsVisible = false,
            IsIndeterminate = true,
            Text = string.Empty
        };

        private readonly SocketClient _socketClient = GameNetworkClient.GlobalSocketClient;
        private readonly GameNetworkClient _gameClient = GameNetworkClient.GlobalGameNetworkClient;

        // Constructor
        public ConnectionPage()
        {
            InitializeComponent();
            SystemTray.SetProgressIndicator(this, _indicator);
            _socketClient.ConnectCompleted += socketClient_ConnectCompleted;
            _socketClient.ConnectFailed += socketClient_ConnectFailed;
            IpText.Text=_ioClass.getIpAddress();
            UserName.Text=_ioClass.getUserName();
        }

        private void startGame(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative)); 
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
                _ioClass.saveIpAddress(IpText.Text);
                _ioClass.saveUserName(UserName.Text);
                var ip = string.Empty;
                var port = string.Empty;
                var ipParser = new IpParser();
                try
                {
                    ipParser.Parse(IpText.Text, ref ip, ref port);
                    _socketClient.Connect(ip, Int32.Parse(port));
                    _indicator.IsVisible = true;
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
            Dispatcher.BeginInvoke(connectFailed);
        }

        void socketClient_ConnectCompleted()
        {
            _gameClient.SendInitInfo();
            Dispatcher.BeginInvoke(() => _gameClient.SendName(UserName.Text));
            Dispatcher.BeginInvoke(connectCompleted);
        }

        private void connectCompleted()
        {
            _indicator.IsVisible = false;
            MessageText.Text = "Connected!!!!!!";
            var playButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            playButton.IsEnabled = true;
        }

        private void connectFailed()
        {
            _indicator.IsVisible = false;
            MessageText.Text = "Connection failed!!!!!!";
            var playButton = (ApplicationBarIconButton)ApplicationBar.Buttons[1];
            playButton.IsEnabled = false;
        }

        private void IpText_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _ioClass.saveIpAddress(IpText.Text);
        }

        private void UserName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            _ioClass.saveUserName(UserName.Text);
        }

        private void KeyUp_Focus(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Enter)
            {
                Focus();
            }
        }
    }
}