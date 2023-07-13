// See https://aka.ms/new-console-template for more information
// 16 string reverse (reverse from https://stackoverflow.com/a/56937817) fast and handles unicode combinations

class Program
{
	public static void Main()
	{
		Console.WriteLine("Enter a string for reversing:");
		string message = Console.ReadLine();
		
		message = Reverse(message);
		Console.WriteLine($"{message}");
	}
	
	public static string Reverse(string input) {
    	return string.Create<string>(input.Length, input, (chars, state) =>
    	{
        	state.AsSpan().CopyTo(chars);
        	chars.Reverse();
    	});
	}
}
