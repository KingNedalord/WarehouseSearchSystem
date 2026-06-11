namespace Controller.Interfaces;

/// <summary>
/// Interface for commands that can be executed by the controller
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes the command with the specified request
    /// </summary>
    Task<Response> ExecuteAsync(Request request);
}
