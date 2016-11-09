using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    public class Client : IClient
    {
        private TcpClient _client;
        private IConnection _connection;
        private Socket _socket;
        private Encoding _encoding;

        public Client(IConnection connection)
        {
            _client = new TcpClient();
            _encoding = new UnicodeEncoding();
            _connection = connection;    
        }
        public async Task Connect()
        {
            await _client.ConnectAsync(_connection.IPAddr,_connection.Port);
            _socket = _client.Client;
        }

        public void Disconnect()
        {
            _socket.Close();
            _socket = null;
            _client.Close();
        }

        public string Receive()
        {
            var bytesCount = 0;
            while ((bytesCount = _socket.Available)==0);
            var buffer = new byte[bytesCount];
            _socket.Receive(buffer, SocketFlags.None);
            return _encoding.GetString(buffer);
        }

        public void Send(string text)
        {
            var data = _encoding.GetBytes(text);
            _socket.Send(data,SocketFlags.None);
        }
    }
}