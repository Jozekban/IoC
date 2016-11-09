using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Sample
{
    public class Connection : IConnection
    {
        public Connection()
        {
            Port = 3000;
            SetIpAddr();
        }

        private void SetIpAddr()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    IPAddr = ip;
                }
            }
        }

        public IPAddress IPAddr { get; set; }

        public int Port { get; set; }
    }
}
