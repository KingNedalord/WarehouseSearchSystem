using Controller.Interfaces;

namespace Controller.Commands;

/// <summary>
/// Command that handles users to run admin commands
/// </summary>
public class ForbiddenCommand : ICommand
{
   /// <inheritdoc/>
    public async Task<Response> ExecuteAsync(Request request) => Response.Forbidden();
}
