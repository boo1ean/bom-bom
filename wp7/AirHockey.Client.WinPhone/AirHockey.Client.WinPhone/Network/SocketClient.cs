using System.Net;
using System.Net.Sockets;

namespace AirHockey.Client.WinPhone.Network
{
    public class SocketClient
    {
        readonly Socket _socket;

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

            socketEventArg.Completed +=
                (s, e) =>
                {
                    if (e.SocketError == SocketError.Success)
                        invokeConnectCompleted();
                    else
                        invokeConnectFailed(e.SocketError);
                };

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
