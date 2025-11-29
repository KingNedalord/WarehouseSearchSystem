namespace Warehouse.Controller;
/// <summary>
/// Fallback command that handles unknown or invalid commands
/// </summary>
public class WrongCommand : ICommand
{
    /// <summary>
    /// Executes the wrong command to handle unknown commands
    /// </summary>
    public Response Execute(Request request) => new Response(false, $"Unknown command: {request.Command}");
}