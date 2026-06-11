using Controller.Interfaces;

namespace Controller;

/// <summary>
/// Main controller that handles user requests by delegating to appropriate commands
/// </summary>
public class ItemController : IController
{
    private readonly ICommandProvider _commandProvider;

    /// <summary>
    /// Creates a new Controller with injected service factory
    /// </summary>
    public ItemController(ICommandProvider commandProvider)
    {
        this._commandProvider = commandProvider ?? throw new ArgumentNullException(nameof(commandProvider));
    }

    /// <summary>
    /// Executes a request by finding the appropriate command and delegating execution
    /// </summary>
    public async Task<Response> ExecuteAsync(Request request)
    {
        var command = this._commandProvider.GetCommand(request.Command);
        return await command.ExecuteAsync(request);
    }

    /// <summary>
    /// Parses user input string and creates a Request object
    /// </summary>
    public Request GetRequest(string input)
    {
        var parts = input.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

        var command = parts[0];
        var target = GetTarget(parts.ElementAtOrDefault(1));
        var parameters = GetParameters(parts.ElementAtOrDefault(2));

        return new Request(command, target, parameters);
    }

    private static TargetType GetTarget(string? target)
    {
        if (target == null)
        {
            return TargetType.NoTarget;
        }

        if (target.StartsWith("cloth"))
        {
            return TargetType.Clothing;
        }

        if (target.StartsWith("foot"))
        {
            return TargetType.Footwear;
        }

        if (target == "all")
        {
            return TargetType.All;
        }

        if (target.StartsWith("admin"))
        {
            return TargetType.Admin;
        }

        if (target.StartsWith("user"))
        {
            return TargetType.User;
        }

        return TargetType.NoTarget;
    }

    private static string[] GetParameters(string? parameters)
    {
        if (parameters == null)
        {
            return [];
        }

        return parameters.Split(',', StringSplitOptions.TrimEntries);
    }
}
