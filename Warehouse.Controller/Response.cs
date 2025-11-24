using Warehouse.Models;

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
    public Response(bool success, string message = "", IList<Item>? items = null, bool isExit = false)
    {
        Success = success;
        Message = message;
        Items = items?.ToList() ?? [];
        IsExit = isExit;
    }
}