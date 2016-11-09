using Castle.MicroKernel.Registration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Register(Component.For<IConnection>().ImplementedBy<Connection>());
            container.Register(Component.For<IClient>().ImplementedBy<Client>());
            container.Register(Component.For<IServer>().ImplementedBy<Server>());
            var client = container.Resolve<IClient>();
            var server = container.Resolve<IServer>();

            var str = @"
________▄▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▀▄______
_______█░░▒▒▒▒▒▒▒▒▒▒▒▒▒▒▒░░░█_____
_______█░▒▒▒▒▒▒▒▒▒▒▄▀▀▄▒▒▒░░█▄▀▀▄_
__▄▄___█░▒▒▒▒▒▒▒▒▒▒█▓▓▓▀▄▄▄▄▀▓▓▓█_
█▓▓█▄▄█░▒▒▒▒▒▒▒▒▒▄▀▓▓▓▓▓▓▓▓▓▓▓▓▀▄_
_▀▄▄▓▓█░▒▒▒▒▒▒▒▒▒█▓▓▓▄█▓▓▓▄▓▄█▓▓█_
_____▀▀█░▒▒▒▒▒▒▒▒▒█▓▒▒▓▄▓▓▄▓▓▄▓▒▒█
______▄█░░▒▒▒▒▒▒▒▒▒▀▄▓▓▀▀▀▀▀▀▀▓▄▀_
____▄▀▓▀█▄▄▄▄▄▄▄▄▄▄▄▄██████▀█▀▀___
____█▄▄▀_█▄▄▀_______█▄▄▀_▀▄▄█_____ "
.Split(Environment.NewLine.ToCharArray(),StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
            var rand = new Random(421);
            new Thread(async () =>
            {
                await server.Listen();
                while (true)
                {
                    foreach (var text in server.Receive())
                    {
                        Thread.Sleep(200);
                        server.SendToAll(text);
                    }
                }

            }).Start();
            try
            {
                client.Connect().Wait();
                var enumerator = str.GetEnumerator();
                enumerator.MoveNext();
                while (true)
                {
                    client.Send(enumerator.Current);
                    if (!enumerator.MoveNext())
                    {
                        enumerator.Reset();
                        enumerator.MoveNext();
                    }
                    Thread.Sleep(200);
                    Console.WriteLine(client.Receive());
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                Console.ReadLine();
            }
        }
    }
}
