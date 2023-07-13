// See https://aka.ms/new-console-template for more information
// 19 count words in a string

class Program
{
	public static void Main()
	{
		Console.WriteLine("Enter a String for word counting");
		string text = Console.ReadLine();
		
		char[] delimiterChars = {' ', ',', '.', ':', '\t'};
		var words = text.Split(delimiterChars, StringSplitOptions.RemoveEmptyEntries);
		var wordCount = words.Count();
		
		Console.WriteLine("Your string has " + wordCount + " word/s");
	}	
}
