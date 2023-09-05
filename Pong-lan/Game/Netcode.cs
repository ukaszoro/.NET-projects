using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Portfinder
{
    public static int GetAvaliablePort()
    {
        TcpListener tmp = new TcpListener(IPAddress.Any, 0);
        tmp.Start();
        int port = ((IPEndPoint)tmp.LocalEndpoint).Port;
        tmp.Stop();
        Console.WriteLine($"Available port found: {port}");
        return port;
    }
}
public class Server
{
    private TcpListener listener;
    private TcpClient client;

    public int port {get; set;}
    
    public Server(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
    }

        public void Start()
    {
        listener.Start();
        Console.WriteLine("Server started on port {0}. Waiting for clients...", ((IPEndPoint)listener.LocalEndpoint).Port);
        
        while (client is null)
        {
            client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            NetworkStream stream = client.GetStream();
            byte[] data = Encoding.ASCII.GetBytes("start");
            stream.Write(data, 0, data.Length);
        }
    }
    
    public string Recieve()
    {
        if (client is null)
            return null;
            
        NetworkStream stream = client.GetStream();
    
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string recievedMessage = Encoding.ASCII.GetString(buffer, 0 , bytesRead);
        return recievedMessage;
    }

}

public class Client
{
    private TcpClient client;
    public bool Connected {get; set;}
    
    public void Connect(string ipAddress, int port)
    {
        client = new TcpClient();
        client.Connect(ipAddress, port);

        Connected = client.Connected; 
    }
    public void Send(string message)
    {
        if (client is null)
            return;
            
        NetworkStream stream = client.GetStream();
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
    }
    
}
