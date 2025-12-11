using Warehouse.Controller;
namespace Warehouse.Presentation;

/// <summary>
/// Console-based view implementation that provides command-line interface for item management
/// </summary>
public class ConsoleView : IView
{
    private readonly IControllerFactory controllerFactory;

    /// <summary>
    /// Creates a new ConsoleView with the specified controller factory
    /// </summary>
    public ConsoleView(IControllerFactory? controllerFactory = null)
    {
        this.controllerFactory = controllerFactory ?? ControllerFactory.Instance;
    }

    /// <summary>
    /// Starts the console application and begins user interaction loop
    /// </summary>
    public void Start()
    {
        var controller = controllerFactory.CreateController();

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
                continue;

            var request = controller.GetRequest(input);
            var response = controller.Execute(request);


            if (response.IsExit)
                break;

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
    public void Crash()
    {
        ColorConsole.WriteError("Critical error occurred in the application!");
        ColorConsole.WriteError("Please check the configuration and try again.");
    }
}