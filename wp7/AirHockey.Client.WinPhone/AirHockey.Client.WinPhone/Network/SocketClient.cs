using System;
using System.Net;
using System.Net.Sockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace AirHockey.Client.WinPhone.Network
{
    public class SocketClient
    {
        readonly Socket _socket;

        private const int ConnectTimeout = 5000;

        public delegate void ConnectCompletedEventHandler();
        public delegate void ConnectFailedEventHandler(SocketError result);
        public event ConnectCompletedEventHandler ConnectCompleted;
        public event ConnectFailedEventHandler ConnectFailed;

        public SocketClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string hostName, int portNumber)
        {
            var endPoint = new DnsEndPoint(hostName, portNumber);
            var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = endPoint };

            var subject = new Subject<int>();
            socketEventArg.Completed += (s, e) => subject.OnNext(0);

            var timeout = subject.Timeout(TimeSpan.FromSeconds(ConnectTimeout));
            timeout.Subscribe(i => invokeConnectCompleted(), ex => invokeConnectFailed(socketEventArg.SocketError));

            _socket.ConnectAsync(socketEventArg);
        }

        public void Send(string data)
        {
            
        }

        private void invokeConnectCompleted()
        {
            var handler = ConnectCompleted;
            if (handler != null) handler();
        }

        private void invokeConnectFailed(SocketError result)
        {
            var handler = ConnectFailed;
            if (handler != null) handler(result);
        }
    }
}
