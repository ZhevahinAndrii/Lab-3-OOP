using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCP_Server
{
    internal class ClientObject
    {
        protected internal string Id { get; }=Guid.NewGuid().ToString();
        protected internal StreamWriter Writer { get;}
        protected internal StreamReader Reader { get; }
        TcpClient client;
        ServerObject server;
        public ClientObject(TcpClient client,ServerObject server)
        {
            this.client = client;
            this.server = server;
            Stream stream = client.GetStream();
            Reader = new(stream);
            Writer = new(stream);
        }
        public async Task ProcessAsync()
        {
            try
            {
                string? username = await Reader.ReadLineAsync();
                string? message = $"{username} entered the chat!";
                Console.WriteLine(message);
                while (true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message is null) continue;
                        message = $"{username}:{message}";
                        Console.WriteLine(message);
                        await server.BroadCastMessage(message, Id);

                    }
                    catch
                    {
                        message = $"{username} has left the chat";
                        Console.WriteLine(message);
                        await server.BroadCastMessage(message, Id);
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
