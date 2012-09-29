using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AirHockey.Client.WinPhone.Network
{
    internal class GameNetworkClient
    {
        #region Static clients

        private static GameNetworkClient _globalGameClient;
        private static SocketClient _globalSocketClient;

        public static SocketClient GlobalSocketClient
        {
            get { return _globalSocketClient ?? (_globalSocketClient = new SocketClient()); }
        }

        public static GameNetworkClient GlobalGameNetworkClient
        {
            get { return _globalGameClient ?? (_globalGameClient = new GameNetworkClient(GlobalSocketClient)); }
        }

        #endregion
        
        private readonly ISocketClient _socketClient;

        public delegate void ReceivedNotificationEventHandler(ServerNotifications notification);

        public event ReceivedNotificationEventHandler ReceivedNotification;

        public GameNetworkClient(ISocketClient socketClient)
        {
            _socketClient = socketClient;
            _socketClient.Received += socketClientOnReceived;
        }

        public void SendInitInfo()
        {
            var data = insertCommand(new byte[] { 2 }, ServerCommands.Name);
            _socketClient.Send(data);
        }

        public void SendName(string name)
        {
            var data = insertCommand(Encoding.UTF8.GetBytes(name), ServerCommands.Name);
            _socketClient.Send(data);
        }

        public void SendAccelerometerData(float x, float y, float z)
        {
            var coordinates = floatArrayToByteArray(new[] { x, y, z });
            var data = insertCommand(coordinates, ServerCommands.AccelerometerData);
            _socketClient.Send(data);
        }

        private void socketClientOnReceived(byte[] data)
        {
            if (data[0] == (byte)ServerCommands.Notification)
            {
                invokeReceivedNotification((ServerNotifications)data[1]);
            }
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

        private void invokeReceivedNotification(ServerNotifications notification)
        {
            var handler = ReceivedNotification;
            if (handler != null) handler(notification);
        }
    }
}
