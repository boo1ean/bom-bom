namespace AirHockey.Client.WinPhone.Network
{
    internal enum ServerNotifications : byte
    {
        Hit          = 1,
        Win          = 2,
        FieldUpdated = 3,
        Miss         = 4,
        Goal         = 5,
        Start        = 6
    }
}
