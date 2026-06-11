using Models;

namespace Controller;

/// <summary>
/// Represents the result of executing a command
/// </summary>
public class Response
{
    /// <summary>
    /// Gets whether the command execution was successful
    /// </summary>
    public bool Success { get; }

    /// <summary>
    /// Gets the message describing the result
    /// </summary>
    public string Message { get; }

    /// <summary>
    /// Gets the list of items returned by the command
    /// </summary>
    public List<Item> Items { get; }

    /// <summary>
    /// Gets whether this response signals application exit
    /// </summary>
    public bool IsExit { get; }

    /// <summary>
    /// Creates a new response with specified success status, message, items, and exit flag
    /// </summary>
    /// <param name="success">Whether the command execution was successful</param>
    /// <param name="message">Message describing the result</param>
    /// <param name="items">Optional list of items returned by the command</param>
    /// <param name="isExit">Whether this response signals application exit</param>
    public Response(bool success, string message = "", IEnumerable<Item>? items = null, bool isExit = false)
    {
        this.Success = success;
        this.Message = message;
        this.Items = items?.ToList() ?? [];
        this.IsExit = isExit;
    }

    public static Response Ok(string message)
        => new(true, message);

    public static Response Ok(string message, IEnumerable<Item> items)
        => new(true, message, items);

    public static Response Fail(string message)
        => new(false, message);

    public static Response Exit()
        => new(true, "Exiting application...", isExit: true);

    public static Response Forbidden()
        => new(false, "Access denied");

    public static Response WrongCommand(string command)
    => new(false, $"Unknown command: {command}");
}
