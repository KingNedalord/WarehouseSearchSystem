using Warehouse.Services;
namespace Warehouse.Controller;

/// <summary>
/// Main controller that handles user requests by delegating to appropriate commands
/// </summary>
public class ItemController : IController
{
    private readonly IServiceFactory serviceFactory;

    /// <summary>
    /// Creates a new Controller with default service factory
    /// </summary>
    public ItemController() : this(ServiceFactory.Instance)
    {
    }

    /// <summary>
    /// Creates a new Controller with injected service factory
    /// </summary>
    public ItemController(IServiceFactory serviceFactory)
    {
        this.serviceFactory = serviceFactory ?? ServiceFactory.Instance;
    }

    /// <summary>
    /// Executes a request by finding the appropriate command and delegating execution
    /// </summary>
    public Response Execute(Request request)
    {
        var command = CommandProvider.GetCommand(request.Command, serviceFactory);
        return command.Execute(request);
    }

    /// <summary>
    /// Parses user input string and creates a Request object
    /// </summary>
    public Request GetRequest(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new Request("", "", []);
        }

        if (input.Contains("exit", StringComparison.CurrentCultureIgnoreCase))
        {
            return new Request("Exit", "", []);
        }
        var parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        var parameters = new string[parts.Length - 1];
        for (int i = 1; i < parts.Length; i++)
        {
            parameters[i - 1] = parts[i];
        }

        return new Request(parts[0], parts[1], parameters);
    }
}