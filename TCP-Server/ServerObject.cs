﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

using System.Net;
namespace TCP_Server
{
    internal class ServerObject
    {
        private  TcpListener tcpListener = new(IPAddress.Any, 8888);
        //сервер для відслідковування і реєстрування підключень за портом 8888
        private List<ClientObject> clients = new();
        protected internal async Task ListenAsync()
        {
            try
            {
                tcpListener.Start();
                Console.WriteLine("Server is running.Waiting for connections...");
                while (true)
                {
                    TcpClient client = await tcpListener.AcceptTcpClientAsync();
                    ClientObject clientObject = new(client, this);
                    clients.Add(clientObject);
                    Task.Run(clientObject.ProcessAsync);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message); 
            }
            finally
            {
                Disconnect();
            }
        }
       protected internal void RemoveConnection(string id)
        {
            ClientObject? client = clients.FirstOrDefault(client => client.Id == id);
            if (client!= null) clients.Remove(client);
            client?.Close();
           
        }
       protected internal async Task BroadCastMessageAsync(string message,string id)
        {
            foreach(var client in clients)
            {
                if (client.Id != id)
                {
                    await client.Writer.WriteLineAsync(message);
                    await client.Writer.FlushAsync();
                }
            }
        }
       protected internal void Disconnect()
        {
            foreach(var client in clients)
            {
                client.Close();

            }
            tcpListener.Stop();
        }
    }
}
