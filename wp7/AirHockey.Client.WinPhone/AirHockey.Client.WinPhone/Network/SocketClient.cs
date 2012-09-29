using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Microsoft.Phone.Net.NetworkInformation;

namespace AirHockey.Client.WinPhone.Network
{
    internal class SocketClient : IDisposable
    {
        private int _maxBufferSize = 2;

        private Socket _socket;
        private SocketAsyncEventArgs _socketEventArg;

        public delegate void ConnectCompletedEventHandler();
        public delegate void ConnectFailedEventHandler(SocketError result);
        public delegate void ReceiveEventHandler(ServerNotifications notification);

        public event ConnectCompletedEventHandler ConnectCompleted;
        public event ConnectFailedEventHandler ConnectFailed;
        public event ReceiveEventHandler Received;

        public bool IsConnected { get; private set; }

        public int MaxReceiveBufferSize
        {
            get
            {
                return _maxBufferSize;
            }
            set
            {
                _maxBufferSize = value;
                if (IsConnected)
                    _socketEventArg.SetBuffer(new byte[_maxBufferSize], 0, _maxBufferSize);
            }
        }

        public void Connect(string hostName, int portNumber)
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            var endPoint = new DnsEndPoint(hostName, portNumber);
            _socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = endPoint };

            _socketEventArg.SetBuffer(new byte[_maxBufferSize], 0, _maxBufferSize);

            _socketEventArg.Completed += socketEventArgOnCompleted;

            _socket.ConnectAsync(_socketEventArg);
        }

        private void socketEventArgOnCompleted(object sender, SocketAsyncEventArgs e)
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
                    if (e.Buffer[0] == (byte)ServerCommands.Notification)
                        invokeReceived((ServerNotifications)e.Buffer[1]);
                    Receive();
                }
            }
            else
                invokeConnectFailed(e.SocketError);
        }

        public void Send(byte[] data)
        {
            if (_socket == null)
                throw new NetworkException(NetworkError.SocketNotConnected);

            var socketEventArg = new SocketAsyncEventArgs { RemoteEndPoint = _socket.RemoteEndPoint, UserToken = null };
            socketEventArg.SetBuffer(data, 0, data.Length);
            _socket.SendAsync(socketEventArg);
        }

        public void SendInitInfo()
        {
            var data = insertCommand(new byte[] { 2 }, ServerCommands.Name);
            Send(data);
        }

        public void SendName(string name)
        {
            var data = insertCommand(Encoding.UTF8.GetBytes(name), ServerCommands.Name);
            Send(data);
        }

        public void SendAccelerometerData(float x, float y, float z)
        {
            var coordinates = floatArrayToByteArray(new[] { x, y, z });
            var data = insertCommand(coordinates, ServerCommands.AccelerometerData);
            Send(data);
        }

        public void Receive()
        {
            _socket.ReceiveAsync(_socketEventArg);
        }

        private static byte[] insertCommand(IEnumerable<byte> data, ServerCommands command)
        {
            var list = data.ToList();
            list.Insert(0, (byte)command);
            return list.ToArray();
        }

        private static IEnumerable<byte> floatArrayToByteArray(float[] floatArray)
        {
            var byteArray = new byte[floatArray.Length * 4];
            Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);
            return byteArray;
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

        private void invokeReceived(ServerNotifications notification)
        {
            var handler = Received;
            if (handler != null) handler(notification);
        }

        public void Close()
        {
            if (IsConnected)
            {
                _socket.Close();
            }
        }

        public void Dispose()
        {
            if (_socket != null)
            {
                _socket.Dispose();
                _socket = null;
            }
        }
    }
}
