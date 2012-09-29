using System;

namespace AirHockey.Client.WinPhone.Network
{
    internal enum ServerCommands : byte
    {
        AccelerometerData = 0,
        Init              = 1,
        Name              = 2,
        Notification      = 3
    }
}
