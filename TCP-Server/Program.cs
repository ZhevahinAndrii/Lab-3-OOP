using System.Net;
using TCP_Server;

ServerObject server = new();
await server.ListenAsync();