using System;
using System.Net;
using System.Net.Sockets;
using Microsoft.Phone.Net.NetworkInformation;

namespace AirHockey.Client.WinPhone.Network
{
    internal class SocketClient : ISocketClient, IDisposable
    {
        private const int DefaultBufferSize = 2;

        private Socket _socket;
        private SocketAsyncEventArgs _socketEventArg;

        public event ConnectSucceedEventHandler ConnectCompleted;
        public event ConnectFailedEventHandler ConnectFailed;
        public event ReceivedEventHandler Received;

        public bool IsConnected { get; private set; }

        public void Connect(string hostName, int portNumber)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new DnsEndPoint(hostName, portNumber);
            _socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = endPoint };

            SetMaxReceiveBufferSize(DefaultBufferSize);

            _socketEventArg.Completed += socketCommandCompleted;

            _socket.ConnectAsync(_socketEventArg);
        }

        public void Send(byte[] data)
        {
            if (_socket == null)
                throw new NetworkException(NetworkError.SocketNotConnected);

            var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = _socket.RemoteEndPoint, UserToken = null };
            socketEventArg.SetBuffer(data, 0, data.Length);
            _socket.SendAsync(socketEventArg);
        }

        public void Receive()
        {
            _socket.ReceiveAsync(_socketEventArg);
        }

        public void SetMaxReceiveBufferSize(int count)
        {
            _socketEventArg.SetBuffer(new byte[count], 0, count);
        }

        private void socketCommandCompleted(object sender, SocketAsyncEventArgs e)
        {
            if (e.SocketError == SocketError.Success)
            {
                if (e.LastOperation == SocketAsyncOperation.Connect)
                {
                    invokeConnectCompleted();
                    IsConnected = true;
                }
                else if (e.LastOperation == SocketAsyncOperation.Receive)
                {
                    invokeReceived(e.Buffer);
                    Receive();
                }
            }
            else
                invokeConnectFailed(e.SocketError);
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

        private void invokeReceived(byte[] data)
        {
            var handler = Received;
            if (handler != null) handler(data);
        }

        public void Close()
        {
            if (IsConnected)
                _socket.Close();
        }

        public void Dispose()
        {
            if (_socket == null) return;
            
            _socket.Dispose();
            _socket = null;
        }
    }
}
