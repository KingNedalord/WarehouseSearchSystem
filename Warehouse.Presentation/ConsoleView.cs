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

        Console.WriteLine("Welcome to the Item Management System!");
        Console.WriteLine("Available commands:");
        Console.WriteLine("  find clothing - find all clothing");
        Console.WriteLine("  find footwear - find all footwear");
        Console.WriteLine("  find all - find all items");
        Console.WriteLine("  find all price=min;max - find items by price range");
        Console.WriteLine("  exit - exit the application");
        Console.WriteLine();

        while (true)
        {
            Console.Write("Enter command: ");
            var input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
                continue;

            var request = controller.GetRequest(input);
            var response = controller.Execute(request);

            if (response.IsExit)
                break;

            if (response.Success)
            {
                Console.WriteLine(response.Message);
                foreach (var item in response.Items)
                {
                    Console.WriteLine($"  {item}");
                }
            }
            else
            {
                Console.WriteLine($"Error: {response.Message}");
            }
            Console.WriteLine();
        }

        Console.WriteLine("Goodbye!");
    }

    /// <summary>
    /// Handles critical errors by displaying error message to the user
    /// </summary>
    public void Crash()
    {
        Console.WriteLine("Critical error occurred in the application!");
        Console.WriteLine("Please check the configuration and try again.");
    }
}