namespace AirHockey.Recognition.Client.Networking
{
    using System;
    using System.Collections.Generic;

    using AirHockey.Recognition.Client.Data;

    public class SendPolygonCommand
    {
        public byte[] Serialize(IEnumerable<Polygon> polygon)
        {
            var data = new List<byte>
            {
                ServerCommands.SendPolygon
            };

            foreach (var item in polygon)
            {
                data.AddRange(BitConverter.GetBytes(item.MinPoint.X));
                data.AddRange(BitConverter.GetBytes(item.MinPoint.Y));
                data.AddRange(BitConverter.GetBytes(item.MaxPoint.X));
                data.AddRange(BitConverter.GetBytes(item.MaxPoint.Y));
            }

            var array = data.ToArray();
            return array;
        }
    }
}