using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Server
{
    private TcpListener listener;

    public Server(int port)
    {
        listener = new TcpListener(IPAddress.Any, port);
    }

    public void Start()
    {
        listener.Start();
        Console.WriteLine("Server started. Waiting for clients...");

        while (true)
        {
            TcpClient client = listener.AcceptTcpClient();
            Console.WriteLine("Client connected.");

            // Handle communication with the client here
            HandleClient(client);
        }
    }

    private void HandleClient(TcpClient client)
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
    public void Connect(string ipAddress, int port)
    {
        TcpClient client = new TcpClient();
        client.Connect(ipAddress, port);

        NetworkStream stream = client.GetStream();

        // Example: Receiving a message from the server
        byte[] buffer = new byte[1024];
        int bytesRead = stream.Read(buffer, 0, buffer.Length);
        string receivedMessage = Encoding.ASCII.GetString(buffer, 0, bytesRead);
        Console.WriteLine("Received from server: " + receivedMessage);

        // Example: Sending a message to the server
        string message = "Hello from client!";
        byte[] data = Encoding.ASCII.GetBytes(message);
        stream.Write(data, 0, data.Length);

        // Close the client connection
        client.Close();
    }
}

class Program
{
    static void Main(string[] args)
    {
        int port = 12345;
        Server server = new Server(port);

        // Start the server in the main thread
        Task.Run(() => server.Start());

        // Wait a little to ensure the server has started
        Thread.Sleep(1000);

        // Connect the client to the server in a separate thread
        string serverIpAddress = "127.0.0.1"; // Replace with the server's IP address
        Client client = new Client();
        Task.Run(() => client.Connect(serverIpAddress, port));

        // Wait for a key press to exit the program
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
