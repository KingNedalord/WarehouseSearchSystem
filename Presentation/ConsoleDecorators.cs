using System.Diagnostics;

namespace Presentation;

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

/// <summary>
/// For animation in console
/// </summary>
static class AnimatedLoading
{
    /// <summary>
    /// Animated stick man
    /// </summary>
    public static void Loading()
    {
        if (!IsConsoleAvailable())
        {
            return;
        }

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

        // Randomly gets amount of time animation runs
        int[] numbers = { 1, 1, 2 };
        Random random = new();
        var seconds = numbers[random.Next(numbers.Length)];

        var stopwatch = Stopwatch.StartNew();
        while (stopwatch.Elapsed.TotalSeconds < seconds)
        {
            foreach (var frame in frames)
            {
                Console.Clear();
                Console.WriteLine(frame);
                Thread.Sleep(250);

                if (stopwatch.Elapsed.TotalSeconds >= seconds)
                {
                    break;
                }
            }
        }
        stopwatch.Stop();
    }

    /// <summary>
    /// Typing animation
    /// </summary>
    /// <param name="message"></param>
    public static void TypewriterWelcome(string message)
    {
        if (IsConsoleAvailable())
        {
            Console.Clear();
            foreach (var c in message)
            {
                ColorConsole.WriteInfo(c);
                Thread.Sleep(30);
            }
            Console.WriteLine("\n");
        }
        else
        {
            Console.WriteLine(message);
        }
    }

    private static bool IsConsoleAvailable()
    {
        try
        {
            _ = Console.WindowHeight;
            return true;
        }
        catch
        {
            return false;
        }
    }
}
