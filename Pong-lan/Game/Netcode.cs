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

            // // Handle communication with the client here
            // HandleClient(client);
        }
    }
    public void Send(string message)
    {
        if (client is null)
            return;
            
        NetworkStream stream = client.GetStream();
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
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

    private void HandleClient()
    {
        NetworkStream stream = client.GetStream();

        // Example: Sending a message to the client
        string message = "Hello from server!";
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);

        // Example: Receiving a message from the client
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Received from client: " + receivedMessage);

        // Close the client connection
        client.Close();
    }
}

public class Client
{
    private TcpClient client;
    
    public void Connect(string ipAddress, int port)
    {
        client = new TcpClient();
        client.Connect(ipAddress, port);

        // NetworkStream stream = client.GetStream();

        // // Example: Receiving a message from the server
        // byte[] buffer = new byte[1024];
        // int bytesRead = stream.Read(buffer, 0, buffer.Length);
        // string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        // Console.WriteLine("Received from server: " + receivedMessage);

        // // Example: Sending a message to the server
        // string message = "Hello from client!";
        // byte[] data = Encoding.ASCII.GetBytes(message);
        // stream.Write(data, 0, data.Length);

        // // Close the client connection
        // client.Close();
    }
    public void Send(string message)
    {
        if (client is null)
            return;
            
        NetworkStream stream = client.GetStream();
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);
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

// class Program
// {
//     static void Main(string[] args)
//     {
//         int port = 12345;
//         Server server = new Server(port);

//         // Start the server in the main thread
//         Task.Run(() => server.Start());

//         // Wait a little to ensure the server has started
//         Thread.Sleep(1000);

//         // Connect the client to the server in a separate thread
//         string serverIpAddress = "127.0.0.1"; // Replace with the server's IP address
//         Client client = new Client();
//         Task.Run(() => client.Connect(serverIpAddress, port));

//         // Wait for a key press to exit the program
//         Console.WriteLine("Press any key to exit...");
//         Console.ReadKey();
//     }
// }
