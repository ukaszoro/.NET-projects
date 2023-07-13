class Mark_hell
{
    const int KEY_LENGTH = 8;

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

    public static int Mod_inv(int r, int q)
    {
        int nwd = Ext_Euclidian(r, q, out int x, out int y);
        return (x % q + q) % q;
    }

    public static int[] Gen_prv_key()
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

    public static int[] Gen_pub_key(int[] prv_key)
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

    public static int[] Encrypt(string message, int[] key)
{
    int[] encryptedMessage = new int[message.Length];

    for (int i = 0; i < message.Length; i++) {
        char c = message[i];
        int asciiValue = (int)c;

        int sum = 0;
        for (int j = KEY_LENGTH - 1; j >= 0; j--)
        {
            if (asciiValue >= sum + key[j])
            {
                encryptedMessage[i] += (int)Math.Pow(2, KEY_LENGTH - j - 1);
                sum += key[j];
            }
        }
    }

    return encryptedMessage;
}


    public static string Decrypt(int[] encryptedMessage, int[] prv_key)
    {
        string decryptedMessage = "";
        int q = prv_key[KEY_LENGTH];
        int r = prv_key[KEY_LENGTH + 1];

        for (int i = 0; i < encryptedMessage.Length; i++)
        {
            int num = encryptedMessage[i];
            int asciiValue = 0;
            int value = (num * Mod_inv(r, q)) % q; //needed for decryption

            for (int j = KEY_LENGTH - 1; j >= 0; j--)
            {
                if (value >= prv_key[j])
                {
                    asciiValue += (int)Math.Pow(2, KEY_LENGTH - j - 1);
                    value -= prv_key[j];
                }
            }

            decryptedMessage += (char)asciiValue;
        }

        return decryptedMessage.TrimEnd('\0');
    }
}

class Program
{
    public static void Main()
    {
        Console.WriteLine("Enter a message for the Hellman algorithm:");
        string message = Console.ReadLine();

        if (message == null)
        {
            Environment.Exit(1);
        }

        int[] prv_key = Mark_hell.Gen_prv_key();
        int[] pub_key = Mark_hell.Gen_pub_key(prv_key);
        int[] encrypted_message = Mark_hell.Encrypt(message, pub_key);
       	for (int i = 0; i < encrypted_message.Length; i++)
        Console.WriteLine(" " + encrypted_message[i]);
	Console.WriteLine(" " + encrypted_message.Length);
	
        message = Mark_hell.Decrypt(encrypted_message, prv_key);

        Console.WriteLine($"{message}");
    }
}
