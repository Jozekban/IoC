using System.Threading.Tasks;

namespace Sample
{
    public interface IClient
    {
        Task Connect();
        void Send(string text);
        string Receive();
        void Disconnect();
    }
}