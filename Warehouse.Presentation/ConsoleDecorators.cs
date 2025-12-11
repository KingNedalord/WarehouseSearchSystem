using System.Diagnostics;

/// <summary>
/// For colorful output
/// </summary>
public static class ColorConsole
{
    public static void WriteSuccess(string message)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteError(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteWarning(string message)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteLineInfo(string message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public static void WriteInfo(char message)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write(message);
        Console.ResetColor();
    }
}


static class AnimatedLoading
{
    public static void Loading()
    {
        if (IsConsoleAvailable())
        {


            string[] frames = {
    @"
    O
   /|\
   / \
    ",
    @"
    O
   \|/
   / \
    "
};

            // Randomly get a seconds
            int[] numbers = { 1, 1, 2 };
            Random random = new();
            int seconds = numbers[random.Next(numbers.Length)];

            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed.TotalSeconds < seconds)
            {
                foreach (var frame in frames)
                {
                    Console.Clear();
                    Console.WriteLine(frame);
                    Thread.Sleep(300);

                    if (stopwatch.Elapsed.TotalSeconds >= seconds)
                        break;
                }
            }
            stopwatch.Stop();
        }
    }

    public static void TypewriterWelcome(string message)
    {
        if (IsConsoleAvailable())
        {
            Console.Clear();
            foreach (char c in message)
            {
                ColorConsole.WriteInfo(c);
                Thread.Sleep(50);
            }
            Console.WriteLine("\n");
        }
        else
        {
            Console.WriteLine(message);
        }
    }

    public static bool IsConsoleAvailable()
    {
        try
        {
            var _ = Console.WindowHeight;
            return true;
        }
        catch
        {
            return false;
        }
    }
}

