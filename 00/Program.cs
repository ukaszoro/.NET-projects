// See https://aka.ms/new-console-template for more information
// 00 name generator + credentials

using System;

class Program
{
	static void Main(string[] args)
	{
		string[] Firstname = new string[] {"Emma", "Noah", "Olivia", "Liam", "Ava", "Isabella", "Sophia", "Mia", "Charlotte", "Amelia", "Harper", "Evelyn", "Abigail", "Emily", "Elizabeth", "Mila", "Ella", "Avery", "Sofia", "Camila", "Aria", "Scarlett", "Victoria", "Madison", "Luna", "Grace", "Chloe", "Penelope", "Layla", "Riley", "Zoey", "Nora", "Lily", "Eleanor", "Hannah", "Lillian", "Addison", "Aubrey", "Ellie", "Stella", "Natalie", "Zoe", "Leah", "Hazel", "Violet", "Aurora", "Savannah", "Audrey", "Brooklyn", "Bella"};
		string[] Secondname = new string[] {"Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez", "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin", "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Lewis", "Young", "Allen", "Hall", "King", "Wright", "Scott", "Green", "Baker", "Adams", "Nelson", "Hill", "Ramirez", "Campbell", "Mitchell", "Roberts", "Carter", "Phillips", "Evans", "Turner", "Torres", "Parker", "Collins", "Edwards"};
        string[] Email = new string[] {"gmail.com", "yahoo.com", "hotmail.com", "outlook.com", "aol.com", "icloud.com", "msn.com", "live.com", "me.com", "mac.com", "rocketmail.com", "yandex.com", "mail.com", "zoho.com", "gmx.com", "protonmail.com", "inbox.com", "tutanota.com", "rediffmail.com", "fastmail.com", "yahoo.co.uk", "outlook.co.uk", "icloud.co.uk", "mail.ru", "hotmail.co.uk", "gmail.co.uk", "yahoo.co.in", "outlook.in", "hotmail.in", "live.co.uk", "protonmail.ch", "yandex.ru", "me.co.uk", "aol.co.uk", "fastmail.fm", "mail.co.uk", "zoho.eu", "tutanota.de", "outlook.fr", "gmail.fr", "yahoo.fr", "hotmail.fr", "outlook.de", "gmail.de", "yahoo.de", "icloud.de", "mail.de", "hotmail.es", "outlook.es", "gmail.es", "yahoo.es"};

		Random rnd = new();
		int FnIndex = rnd.Next(Firstname.Length);
		int SnIndex = rnd.Next(Secondname.Length);
        int EmIndex = rnd.Next(Email.Length);
		
		Console.WriteLine($"Random Name Gerenator\nFirst Name: {Firstname[FnIndex]}\nSecond Name: {Secondname[SnIndex]}");
        Console.WriteLine($"Random Credentials: {Firstname[FnIndex]}{Secondname[SnIndex]}{rnd.Next(101)}@{Email[EmIndex]}");

	}
}



