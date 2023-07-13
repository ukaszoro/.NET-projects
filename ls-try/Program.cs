// See https://aka.ms/new-console-template for more information
// ls, simple program to list all files in a specified directory

using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the directory you want to search");
        string path = Console.ReadLine();
        if (path == null) {
            Console.WriteLine("Nope");
            Environment.Exit(1);
        }
        try
            {
                var Folders = Directory.EnumerateDirectories(path);
                Console.WriteLine("\nFolders in the directory");
                foreach (string currentFolder in Folders)
                {
                    Console.WriteLine($"{currentFolder}");
                }

                var Files = Directory.EnumerateFiles(path);
                Console.WriteLine("\nFiles in the directory");
                foreach (string currentFile in Files)
                {
                    Console.WriteLine($"{currentFile}");
                }  
            }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}

