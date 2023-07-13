// 04 but more?
using System;
using System.Globalization;
using System.Text;

interface Crypt_algorythm
{
    void start(){}
    void Encrypt(){}
    void Decrypt(){}
}
﻿class Mark_hell : Crypt_algorythm
{
    const int KEY_LENGTH = 8;

    public static void Start()
    {
        Console.WriteLine("Enter a message for the Hellman algorithm:");
            string message = Console.ReadLine();

            if (message == null)
            {
                Environment.Exit(1);
            }   

            int[] prv_key = Mark_hell.Gen_prv_key();
            int[] pub_key = Mark_hell.Gen_pub_key(prv_key);
            int[] encrypted_message = Mark_hell.Encrypt(pub_key, message);
        
        
       	
            for (int i = 0; i < encrypted_message.Length; i++)
            Console.WriteLine(" " + encrypted_message[i]);
	
            message = Mark_hell.Decrypt(prv_key, encrypted_message);

            Console.WriteLine($"{message}");
    }
    static int Ext_Euclidian(int a, int b, out int x, out int y)
    {
        if (b == 0)
        {
            x = 1;
            y = 0;
            return a;
        }

        int gcd = Ext_Euclidian(b, a % b, out int x1, out int y1);

        x = y1;
        y = x1 - (a / b) * y1;

        return gcd;
    }

    static int Mod_inv(int r, int q)
    {
        int nwd = Ext_Euclidian(r, q, out int x, out int y);
        return (x % q + q) % q;
    }

    static int[] Gen_prv_key()
    {
        int[] prv_key = new int[KEY_LENGTH + 2];
        int sum = 0;
        Random rand = new Random();

        for (int i = 0; i < KEY_LENGTH; i++)
        {
            prv_key[i] = rand.Next(5) + sum + 1;
            sum += prv_key[i];
        }

        int q = rand.Next(sum + 5, sum + 100);
        int r;

        for (;;)
        {
            r = rand.Next(50, q);
            if (Ext_Euclidian(r, q, out int x, out int y) == 1)
                break;
        }

        prv_key[KEY_LENGTH] = q;
        prv_key[KEY_LENGTH + 1] = r;

        return prv_key;
    }

    static int[] Gen_pub_key(int[] prv_key)
    {
        int[] pub_key = new int[KEY_LENGTH];
        int q = prv_key[KEY_LENGTH];
        int r = prv_key[KEY_LENGTH + 1];

        for (int i = 0; i < KEY_LENGTH; i++)
        {
            pub_key[i] = (prv_key[i] * r) % q;
        }

        return pub_key;
    }

    static int[] Encrypt(int[] publicKey, string message)
    {
        StringInfo yes = new StringInfo(message);
        
        int[] encryptedText = new int[yes.LengthInTextElements];

        for (int j = 0; j < yes.LengthInTextElements; j++)
        {
            int sum = 0;
            int bit;
            int currentChar = (int)message[j];

            for (int i = 7; i >= 0; i--)
            {
                bit = (int)(currentChar / Math.Pow(2, i)) & 1;
                sum += bit * publicKey[7 - i];
            } 

            encryptedText[j] = sum;
        }

        return encryptedText;
    }

    static string Decrypt(int[] prv_key, int[] encryptedText)
    {
        string decryptedMessage = "";
        int q = prv_key[KEY_LENGTH];
        int r = prv_key[KEY_LENGTH + 1];

        for (int j = 0; j < encryptedText.Length; j++)
        {
            char message = '\0';
            int value = (encryptedText[j] * Mod_inv(r, q)) % q;

            for (int i = KEY_LENGTH - 1; i >= 0; i--)
            {
                if (value >= prv_key[i])
                {
                    message += (char)Math.Pow(2, KEY_LENGTH - i - 1);
                    value -= prv_key[i];
                }
            }

            decryptedMessage += message;
        }

        return decryptedMessage;
    }
}
class Cezar : Crypt_algorythm
{
    public static void Start()
    {
        Console.WriteLine("Enter the message for the Cezar's cipher");
        string words = Console.ReadLine();
        
        Console.WriteLine("Enter a key (how many spaces should I shift)");
        string key_s = Console.ReadLine();
        if (!Int32.TryParse(key_s, out int key)) {
            Console.WriteLine("Invalid input. Exiting.");
            Environment.Exit(1);
        }
        key = key % 26;

        Console.WriteLine("Choose an operation\n 1.Encrypt\n 2.Decrypt");
        string choice_s = Console.ReadLine();
        if (!Int32.TryParse(choice_s, out int choice)) {
            Console.WriteLine("Invalid input. Exiting.");
            Environment.Exit(1);
        }

        switch (choice)
        {
            case 1:
                words = Encrypt(words, key);
                break;

            case 2:
                words = Encrypt(words, 26 - key);
                break;

            default:
                Console.WriteLine("Invalid input. Exiting.");
                Environment.Exit(1);
                break;
        }

        Console.WriteLine(words);
    }
    static string Encrypt(string words, int key)
    {
        char[] encryptedChars = new char[words.Length];

        for (int i = 0; i < words.Length; i++)
        {
            if (char.IsLetter(words[i]))
            {
                if (char.IsUpper(words[i]))
                {
                    encryptedChars[i] = (char)(((words[i] - 'A' + key) % 26) + 'A');
                }
                else
                {
                    encryptedChars[i] = (char)(((words[i] - 'a' + key) % 26) + 'a');
                }
            }
            else
            {
                encryptedChars[i] = words[i]; // Preserve non-alphabetic characters
            }
        }

        return new string(encryptedChars);
    }
        
}
class Program
{
    public static void Main()
    {
        Console.WriteLine("Wchich algorythm you want to use(they work only on the english alphabet)\n1.Hellman's\n2.Ceeeeezaaa's");
        string input = Console.ReadLine();

        if (!Int32.TryParse(input, out int choice)) {
            Console.WriteLine("Invalid input. Exiting.");
            Environment.Exit(1);
        }
        switch (choice) {
            case 1:
                Mark_hell.Start();
                break;

            case 2:
                Cezar.Start();
                break;
        }
    }
}