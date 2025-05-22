using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

var endPoint = new IPEndPoint(IPAddress.Loopback, 50000);
var server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
await server.ConnectAsync(endPoint);

while (true)
{
    await Task.Delay(100);
    var buffer = new byte[1024];
    var sb = new StringBuilder();
    while (server.Available > 0)
    {
        var size = await server.ReceiveAsync(buffer);
        var text = Encoding.UTF8.GetString(buffer, 0, size);
        sb.Append(text);
    }
    Console.WriteLine(sb.ToString());
    
    await Task.Delay(100);
    
    Console.Write("> ");
    var messageText = Console.ReadLine();
    var message = Encoding.UTF8.GetBytes(messageText);
    await server.SendAsync(message);
    
    if (messageText == "exit") break;
}

server.Close();