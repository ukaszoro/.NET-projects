// See https://aka.ms/new-console-template for more information
// 02 Temperature converter

using System;
class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("What do you want to convert");
        Console.WriteLine("1. Celcius ---> Farenheit");
        Console.WriteLine("2. Farenheit ---> Celcius");
        int choice = Convert.ToInt32(Console.ReadLine());
        double Fare;
        double Celc;

        switch (choice) {
            case 1:
            Console.WriteLine("Enter Celcius: ");
            Celc = Convert.ToDouble(Console.ReadLine());

            Fare = Celc * 9/5.0 + 32;
            Console.WriteLine($"{Celc} in Celcius equals to {Fare} in Farenheit");
            break;

            case 2:
            Console.WriteLine("Enter Celcius: ");
            Fare = Convert.ToDouble(Console.ReadLine());

            Celc = (Fare - 32) * 5/9.0;
            Console.WriteLine($"{Fare} in Farenheit equals to {Celc} in Celcius");
            break;

            default:
            Console.WriteLine("Wrong action, cya");
            break;
        }
    }
}
