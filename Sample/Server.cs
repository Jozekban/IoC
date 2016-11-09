using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Sample
{
    public class Server : IServer
    {
        private IConnection _connection;
        private UnicodeEncoding _encoding;
        private TcpListener _listener;
        private List<Socket> _sockets;

        public Server(IConnection connection)
        {
            _sockets = new List<Socket>(5);
            _connection = connection;
            _encoding = new UnicodeEncoding();
            _listener = new TcpListener(_connection.IPAddr,_connection.Port);
        }

        public async Task Listen()
        {
            _listener.Start();
            var socket = await _listener.AcceptSocketAsync();
            _sockets.Add(socket);
        }

        public IEnumerable<string> Receive()
        {
            foreach (var socket in _sockets)
            {
                var data = 0;
                if((data = socket.Available) > 0)
                {
                    var bytes = new byte[data];
                    socket.Receive(bytes);
                    yield return _encoding.GetString(bytes);
                }
            }
        }

        public void SendToAll(string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                foreach (var socket in _sockets)
                {
                    try
                    {
                        socket.Send(_encoding.GetBytes(text));
                    }
                    catch (Exception exception)
                    {
                        _sockets.Remove(socket);
                    }
                }
            }
        }

        public void Stop()
        {
            _listener = null;
            _sockets.Clear();
        }

    }
}
