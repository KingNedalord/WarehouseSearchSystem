namespace Controller.Interfaces;

/// <summary>
/// Interface for item controllers that handle user requests
/// </summary>
public interface IController
{
    /// <summary>
    /// Parses user input string and creates a Request object
    /// </summary>
    /// <param name="input">Input string.</param>
    /// <returns></returns>
    Request GetRequest(string input);

    /// <summary>
    /// Executes a request and returns a response
    /// </summary>
    Task<Response> ExecuteAsync(Request request);
}
