using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

var endPoint = new IPEndPoint(IPAddress.Loopback, 50000);
var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
listener.Bind(endPoint);
listener.Listen(10);

var client = await listener.AcceptAsync();

while (true)
{
    var messageText = "Hello World!";
    var message = Encoding.UTF8.GetBytes(messageText);
    await client.SendAsync(message);
    
    await Task.Delay(100);
    
    var buffer = new byte[1024];
    var size = await client.ReceiveAsync(buffer);
    var text = Encoding.UTF8.GetString(buffer, 0, size);
    Console.WriteLine(text);
    
    if (text == "exit") break;
}

client.Close();
listener.Close();
