using Controller.Interfaces;
using DataAccess;

namespace Controller.Commands;

/// <summary>
/// Command to change mode
/// </summary>
public class SwitchCommand : ICommand
{
    /// <inheritdoc/>
    public async Task<Response> ExecuteAsync(Request request)
    {
        if (request.Target == TargetType.Admin)
        {
           return SwitchToAdmin(request.Parameters.ElementAtOrDefault(0));
        }
        return request.Target == TargetType.User ? SwitchToUser() : Response.Fail("Switch to admin or user");
    }

    /// <summary>
    /// Switches to ordinary user mode
    /// </summary>
    /// <returns>Response indicating successful switch to user mode</returns>
    private static Response SwitchToUser()
    {
        Configuration.AdminEnabled = false;
        return Response.Ok("Switch to user role");
    }

    /// <summary>
    /// Switches to admin mode
    /// </summary>
    /// <param name="requestParameters"></param>
    /// <returns>Response indicating whether switch to admin mode was successful</returns>
    private static Response SwitchToAdmin(string? requestParameters)
    {
        if (requestParameters == null)
        {
            return Response.Fail("Password is required");
        }

        if (requestParameters != Configuration.AdminPassword)
        {
            return Response.Fail("Incorrect password");
        }

        Configuration.AdminEnabled = true;
        return Response.Ok("Switched to admin role");
    }
}
