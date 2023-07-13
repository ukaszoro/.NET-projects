// See https://aka.ms/new-console-template for more information
// 03 Calc age in seconds
class Program
{
    static void Main()
    {
        Console.WriteLine("Enter your date of birth (YYYY-MM-DD):");
        string input = Console.ReadLine();

        if (DateTime.TryParse(input, out DateTime birthDate))
        {
            TimeSpan age = DateTime.Now - birthDate;
            double seconds = age.TotalSeconds;
            Console.WriteLine("Your age in seconds: " + seconds);
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid date of birth (YYYY-MM-DD).");
        }
    }
}
