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
        private  TcpClient client;
        private  ServerObject server;
        public ClientObject(TcpClient client,ServerObject server)
        {
            this.client = client;
            this.server = server;
            NetworkStream stream = client.GetStream();
            Reader = new(stream);
            Writer = new(stream);
        }
        public async Task ProcessAsync()
        {
            try
            {
                string? username = await Reader.ReadLineAsync();
                string? message = $"{username} entered the chat!";
                await server.BroadCastMessageAsync(message, Id);
                Console.WriteLine(message);
                
                while (true)
                {
                    try
                    {
                        message = await Reader.ReadLineAsync();
                        if (message==null) continue;
                        message = $"{username}:{message}";
                        Console.WriteLine(message);
                        await server.BroadCastMessageAsync(message, Id);

                    }
                    catch
                    {
                        message = $"{username} has left the chat";
                        Console.WriteLine(message);
                        await server.BroadCastMessageAsync(message, Id);
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                server.RemoveConnection(Id);
            }
        }
       protected internal void Close()
        {
            Writer.Close();
            Reader.Close();
            client.Close();
        }
    }
}
