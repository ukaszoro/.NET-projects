using System;
using System.Net;
using System.Net.Sockets;

class Program
{
    static void Main()
    {
    	for (;;) {
    	Thread.Sleep(2000);
        string ntpServer = "pool.ntp.org"; // NTP server address
        int ntpPort = 123; // NTP server port

        try
        {
            // Create a UDP socket for communication with the NTP server
            using (var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
            {
                socket.Connect(ntpServer, ntpPort);

                // Send the NTP request packet
                var ntpData = new byte[48]; // NTP request packet size is 48 bytes
                ntpData[0] = 0x1B; // Set the request mode
                socket.Send(ntpData);

                // Receive the NTP response packet
                socket.Receive(ntpData);

                // Close the socket
                socket.Close();

                // Extract the seconds part from the NTP response
                ulong timestamp = ((ulong)ntpData[40] << 24) | ((ulong)ntpData[41] << 16) | ((ulong)ntpData[42] << 8) | (ulong)ntpData[43];

                // Convert NTP timestamp to Unix timestamp (seconds since January 1, 1900)
                const ulong ntpEpochOffset = 2208988800UL;
                ulong unixTimestamp = timestamp - ntpEpochOffset;

                // Create a DateTime object from the Unix timestamp
                var utcDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp);

                // Convert the UTC time to Warsaw timezone
                var warsawTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central European Standard Time");
                var warsawDateTime = TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, warsawTimeZone);

                // Display the atomic clock time in Warsaw timezone
                Console.WriteLine("Atomic Clock Time in Warsaw: " + warsawDateTime.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error fetching atomic clock time: " + ex.Message);
        }
        }
    }
}

