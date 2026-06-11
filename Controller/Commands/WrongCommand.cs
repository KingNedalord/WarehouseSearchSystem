using Controller.Interfaces;

namespace Controller.Commands;

/// <summary>
/// Fallback command that handles unknown or invalid commands
/// </summary>
public class WrongCommand : ICommand
{
    /// <inheritdoc/>
    public async Task<Response> ExecuteAsync(Request request) => Response.WrongCommand(request.Command);
}
