using System;
using System.Text;
using System.IO;

namespace Udp_file_transfer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <=0)
                goto Error_close;
                
            switch (args[0])
            {
                case "send":
                    try
                    {       
                        if (args.Length <= 2)
                            goto Error_close;
                            
                        using (var file = new StreamReader(args[1]))
                        {
                            Udp_sender.Send(file.ReadToEnd(), args[2], 11000);
                        }
                    }
                    catch (IOException e)
                    {
                        Console.WriteLine("The file could not be read:");
                        Console.WriteLine(e.Message);
                    }
                    break;
                    
                case "listen":
                    Udp_listener.StartListener();
                    break;
                    
                default:
                    goto Error_close;
            }
            
            Environment.Exit(0);
            Error_close:
                    Console.WriteLine("Unsupported argument\nCorrect command usage: \"Command send/listen filepath ip:port\"");
                    Environment.Exit(1);
        }
    }
}
