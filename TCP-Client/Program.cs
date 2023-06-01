using System.Net.Sockets;
string host = "127.0.0.1";
int port = 8888;

using TcpClient client= new TcpClient();
Console.Write("Enter your name:");
string? username = Console.ReadLine();
Console.WriteLine($"Welcome,{username}");
StreamReader? Reader = null;
StreamWriter? Writer = null;

try
{
    client.Connect(host, port);
    Reader = new(client.GetStream());
    Writer = new(client.GetStream());
    if (Writer is null || Reader is null) return;
    
}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
Writer?.Close();
Reader?.Close();
async Task SendMessageAsync(StreamWriter writer)
{
    await writer.WriteLineAsync(username);
    await writer.FlushAsync();
    Console.WriteLine("To send messages type the text and push the ENTER button");
    while (true)
    {
        string? message = Console.ReadLine();
        await writer.WriteLineAsync(message);
        await writer.FlushAsync();
    }
}
async Task ReceiveMessageAsync(StreamReader reader)
{
    while (true)
    {
        try
        {
            string? message = await reader.ReadLineAsync();
            if (string.IsNullOrEmpty(message)) continue;
            Print(message);
        }
    }
}