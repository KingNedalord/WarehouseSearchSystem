using Controller.Interfaces;
using Presentation.Interfaces;

namespace Presentation;

/// <summary>
/// Console-based view implementation that provides command-line interface for item management
/// </summary>
public class ConsoleView : IView
{
    private readonly IController _controller;

    /// <summary>
    /// Creates a new ConsoleView with the specified controller factory
    /// </summary>
    public ConsoleView(IController controller)
    {
        this._controller = controller;
    }

    /// <summary>
    /// Starts the console application and begins user interaction loop
    /// </summary>
    public async Task StartAsync()
    {
        AnimatedLoading.TypewriterWelcome("Welcome to the Item Management System!");
        ColorConsole.WriteLineInfo("Available commands:");
        ColorConsole.WriteLineInfo("  find clothing - find all clothing");
        ColorConsole.WriteLineInfo("  find footwear - find all footwear");
        ColorConsole.WriteLineInfo("  find all - find all items");
        ColorConsole.WriteLineInfo("  find all price=min;max - find items by price range");
        ColorConsole.WriteLineInfo("  exit - exit the application");
        Console.WriteLine();


        while (true)
        {
            ColorConsole.WriteLineInfo("Enter command: ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                continue;
            }

            var request = this._controller.GetRequest(input);
            var response = await this._controller.ExecuteAsync(request);

            if (response.IsExit)
            {
                break;
            }

            if (response.Success)
            {
                AnimatedLoading.Loading();
                ColorConsole.WriteLineInfo(response.Message);
                foreach (var item in response.Items)
                {
                    ColorConsole.WriteSuccess($"  {item}");
                }
            }
            else
            {
                ColorConsole.WriteError($"Error: {response.Message}");
            }
            Console.WriteLine();
        }

        ColorConsole.WriteWarning("Goodbye!");
    }

    /// <summary>
    /// Handles critical errors by displaying error message to the user
    /// </summary>
    public void Crash(string message)
    {
        ColorConsole.WriteError(message);
    }
}
