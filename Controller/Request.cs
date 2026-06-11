namespace Controller;

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
    /// Gets the target name to execute
    /// </summary>
    public TargetType Target { get; }
    /// <summary>
    /// Gets the parameters for the command
    /// </summary>
    public string[] Parameters { get; }

    /// <summary>
    /// Creates a new request with specified command and optional parameters
    /// </summary>
    /// <param name="command">Command name to execute</param>
    /// <param name="target">Target item</param>
    /// <param name="parameters">Optional parameters for the command</param>
    public Request(string command, TargetType target, string[]? parameters = null)
    {
        this.Command = command;
        this.Target = target;
        this.Parameters = parameters ?? [];
    }
}

/// <summary>
/// Types of targets in command.
/// </summary>
public enum TargetType
{
    Footwear,
    Clothing,
    All,
    Admin,
    User,
    NoTarget
}
