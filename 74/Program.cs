// See https://aka.ms/new-console-template for more information
// 74 hanoi tower

using System;

class Program
{
    public static void Solve(int disks, char source, char auxiliary, char destination)
    {
        if (disks == 1)
        {
            Console.WriteLine($"Move disk 1 from {source} to {destination}");
            return;
        }

        Solve(disks - 1, source, destination, auxiliary);
        Console.WriteLine($"Move disk {disks} from {source} to {destination}");
        Solve(disks - 1, auxiliary, source, destination);
    }

    static void Main()
    {
    	Console.WriteLine("How many pegs do you have?");
		string input = Console.ReadLine();
		
		if (!Int32.TryParse(input, out int n)) {
			Console.WriteLine("Error in readin input");
			Environment.Exit(1);
			}
			
        char sourceRod = 'A';
        char auxiliaryRod = 'B';
        char destinationRod = 'C';

        Solve(n, sourceRod, auxiliaryRod, destinationRod);
    }
}

