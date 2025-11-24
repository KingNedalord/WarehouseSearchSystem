/// <summary>
/// Fallback command that handles unknown or invalid commands
/// </summary>
public class WrongCommand : ICommand
{
    /// <summary>
    /// Executes the wrong command to handle unknown commands
    /// </summary>
    /// <param name="request">Request containing the unknown command</param>
    /// <returns>Response indicating that the command is unknown</returns>
    public Response Execute(Request request) => new Response(false, $"Unknown command: {request.Command}");
}