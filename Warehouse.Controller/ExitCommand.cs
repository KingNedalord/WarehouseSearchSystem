namespace Warehouse.Controller;
/// <summary>
/// Command to exit the application.
/// </summary>
public class ExitCommand : ICommand
{
    /// <summary>
    /// Executes the exit command.
    /// </summary>
    public Response Execute(Request request) => new(true, "Exiting application...", null, true);
}