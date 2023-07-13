// See https://aka.ms/new-console-template for more information
// 01 higher/lower, heads/tails

using System;
class Games
{
    public static void Higher_lower()
    {
        Console.WriteLine("Write how large you want the random number to be");
        int n = Convert.ToInt32(Console.ReadLine());
        Random rnd = new();
        int number = rnd.Next(n + 1);
        int guess;
        int counter = 0;

        Console.WriteLine("\nGame start");
        for (;;)
        {
            Console.WriteLine("What is your guess: ");
            guess = Convert.ToInt32(Console.ReadLine());
            counter++;
            if (guess == number) {
                Console.WriteLine("Congrats, you won in " + counter + " guesses\n");
                break;
            }
            Console.WriteLine("The number you guessed is " + (number < guess ? "higher" : "lower") + " than the random number");
        }
    }
    public static void Heads_tails()
    {
        Console.WriteLine("Pick which side of the coin you want: heads or tails? ");
        string guess = Console.ReadLine();
       // if (coin == null)
        Random rnd = new();
        int toss = rnd.Next(2);
        Console.Write("Tossing coin.");
        Thread.Sleep(1000);
        Console.Write(".");
        Thread.Sleep(1000);
        Console.Write(".");
        Thread.Sleep(1000);
        Console.Write("\n");
        if ((guess == "heads" || guess == "Heads") && toss == 0) 
            Console.WriteLine("You guessed right");
        
        else if ((guess == "tails" || guess == "Tails") && toss == 1) 
            Console.WriteLine("You guessed right");
        
        else
            Console.WriteLine("Wronk");
    }
    
}


class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, which game would you like to play today\n1.higher/lower\n2.heads/tails");
        int choice = Convert.ToInt32(Console.ReadLine());

        switch (choice)
        {
            case 1:
            Games.Higher_lower();
            break;

            case 2:
            Games.Heads_tails();
            break;

            default:
            Console.WriteLine("wronk game, shutting down");
            break;
        }
    }
}
