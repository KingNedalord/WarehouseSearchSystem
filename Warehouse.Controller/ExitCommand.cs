namespace Warehouse.Controller;
/// <summary>
/// Command to exit the application.
/// </summary>
public class ExitCommand : ICommand
{
    /// <summary>
    /// Executes the exit command.
    /// </summary>
    /// <param name="request">Request containing the exit command.</param>
    /// <returns>Response indicating that the application should exit.</returns>
    public Response Execute(Request request) => new Response(true, "Exiting application...", null, true);
}