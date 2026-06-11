using Controller.Interfaces;

namespace Controller.Commands;

/// <summary>
/// Command to exit the application.
/// </summary>
public class ExitCommand : ICommand
{
    /// <inheritdoc/>>
    public async Task<Response> ExecuteAsync(Request request) =>  Response.Exit();
}
