using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public static class Udp_listener
{
    private const int listenPort = 11000;
    
    public static void StartListener()
    {
        UdpClient listener = new UdpClient(listenPort);
        IPEndPoint groupEP = new IPEndPoint(IPAddress.Any, listenPort);

        try
        {
            while(true)
            {
                Console.WriteLine("Waiting for broadcast");
                byte[] bytes = listener.Receive(ref groupEP);

                Console.WriteLine($"Recieved broadcast from {groupEP} :");
                Console.WriteLine($" {Encoding.ASCII.GetString(bytes, 0 ,bytes.Length)}");
            }
        }
        catch (SocketException e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            listener.Close();
        }
    }
}

public static class Udp_sender
{
    public static void Send(string message, string ip4, int port)
    {
        Socket s = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        IPAddress broadcast = IPAddress.Parse(ip4);

        byte[] sendbuf = Encoding.ASCII.GetBytes(message);
        IPEndPoint ep = new IPEndPoint(broadcast, port);

        s.SendTo(sendbuf, ep);

        Console.WriteLine("Message sent");
    }
}