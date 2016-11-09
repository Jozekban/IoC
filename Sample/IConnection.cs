using System.Net;

namespace Sample
{
    public interface IConnection
    {
        IPAddress IPAddr { get; set; }
        int Port { get; set; }
    }
}
