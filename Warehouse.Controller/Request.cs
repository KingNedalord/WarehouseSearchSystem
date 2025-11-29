namespace Warehouse.Controller;
/// <summary>
/// Represents a user request containing a command and optional parameters
/// </summary>
public class Request
{
    /// <summary>
    /// Gets the command name to execute
    /// </summary>
    public string Command { get; }

    /// <summary>
    /// Gets the parameters for the command
    /// </summary>
    public string[] Parameters { get; }

    /// <summary>
    /// Creates a new request with specified command and optional parameters
    /// </summary>
    /// <param name="command">Command name to execute</param>
    /// <param name="parameters">Optional parameters for the command</param>
    public Request(string command, string[]? parameters = null)
    {
        Command = command;
        Parameters = parameters ?? [];
    }
}