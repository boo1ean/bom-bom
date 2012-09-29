using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AirHockey.Client.WinPhone.Network
{
    internal class SocketClient
    {
        private readonly Socket _socket;

        public delegate void ConnectCompletedEventHandler();
        public delegate void ConnectFailedEventHandler(SocketError result);
        public event ConnectCompletedEventHandler ConnectCompleted;
        public event ConnectFailedEventHandler ConnectFailed;

        public bool IsConnected { get; private set; }

        public SocketClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Connect(string hostName, int portNumber)
        {
            var endPoint = new DnsEndPoint(hostName, portNumber);
            var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = endPoint };

            socketEventArg.Completed +=
                (s, e) =>
                {
                    if (e.SocketError == SocketError.Success)
                    {
                        invokeConnectCompleted();
                        IsConnected = true;
                    }
                    else
                        invokeConnectFailed(e.SocketError);
                };

            _socket.ConnectAsync(socketEventArg);
        }

        public void Send(byte[] data)
        {
            var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = _socket.RemoteEndPoint, UserToken = null };
            socketEventArg.SetBuffer(data, 0, data.Length);
            _socket.SendAsync(socketEventArg);
        }

        public void SendInitInfo()
        {
            Send(new byte[] { (byte)ServerCommands.Init, 2 });
        }

        public void SendName(string name)
        {
            Send(Encoding.UTF8.GetBytes(name));
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
