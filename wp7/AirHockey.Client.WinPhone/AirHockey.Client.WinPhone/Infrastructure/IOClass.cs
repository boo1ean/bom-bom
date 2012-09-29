using System.IO.IsolatedStorage;

namespace AirHockey.Client.WinPhone.Infrastructure
{
    public class IOClass
    {
        const string keyUserName="UserName";
        const string keyIpAddress = "IpAddress";

        public string getIpAddress()
        {
            var result = "";
            if (IsolatedStorageSettings.ApplicationSettings.Contains(keyIpAddress))
            {
                result = (string)IsolatedStorageSettings.ApplicationSettings[keyIpAddress];
            }
            return result;
        }

        public string getUserName()
        {
            var result = "";
            if (IsolatedStorageSettings.ApplicationSettings.Contains(keyUserName))
            {
                result = (string)IsolatedStorageSettings.ApplicationSettings[keyUserName];
            }
            return result;
        }

        public void saveIpAddress(string ipAddress)
        {
            IsolatedStorageSettings.ApplicationSettings[keyIpAddress] = ipAddress;
        }

        public void saveUserName(string userName)
        {
            IsolatedStorageSettings.ApplicationSettings[keyUserName] = userName;
        }
    }
}
