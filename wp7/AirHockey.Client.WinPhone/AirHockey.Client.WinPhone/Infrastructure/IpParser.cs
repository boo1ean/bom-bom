using System;

namespace AirHockey.Client.WinPhone.Infrastructure
{
    public class IpParser
    {
        public void Parse(string address, ref string ip, ref string port)
        {
            char[] separator = {':'};
            var array=address.Split(separator);
            if (array.Length==2)
            {
                ip = array[0];
                port = array[1];
            }
            else
            {
                throw new Exception("Wrong format of ip address");
            }
        }
    }
}
