using Controller.Interfaces;
using DataAccess;

namespace Controller.Commands;

/// <summary>
/// Help command shows all commands available for both user and admin
/// </summary>
public class HelpCommand : ICommand
{
    private const string UserHelpText = """
                                    == Available Commands ==

                                    Query commands:
                                      find clothing             - Find all clothing
                                      find footwear             - Find all footwear
                                      find all                  - Find all(or clothing, footwear only) appliances
                                      find all price=min;max    - Find all(or clothing, footwear only) by price range
                                      find all size=M           - Find all(or clothing, footwear only) by size(Xs,S,M,L,Xl,Xxl)
                                      find all gender = male    - Find all(or clothing, footwear only) by gender(Male,Female,Unisex)

                                    Mode commands:
                                      switch admin password*    - Switch to admin mode

                                    Other commands:
                                      help                      - Show this help
                                      exit                      - Exit application
                                    """;

    private const string AdminHelpText = """
                                    == Available Commands ==

                                    Query commands:
                                      find clothing             - Find all clothing
                                      find footwear             - Find all footwear
                                      find all                  - Find all(or clothing, footwear only) appliances
                                      find all price=min;max    - Find all(or clothing, footwear only) by price range
                                      find all size=M           - Find all(or clothing, footwear only) by size(Xs,S,M,L,Xl,Xxl)
                                      find all gender = male    - Find all(or clothing, footwear only) by gender(Male,Female,Unisex)

                                    Admin commands (CRUD):
                                      add clothing              - Add new clothing (interactive)
                                      add footwear              - Add new footwear (interactive)
                                      update <id>               - Update item (interactive)
                                      delete <id>               - Delete item

                                    Mode commands:
                                      switch user               - Switch to user mode

                                    Other commands:
                                      help                      - Show this help
                                      exit                      - Exit application
                                    """;

    /// <inheritdoc/>
    public async Task<Response> ExecuteAsync(Request request)
    {
        if (Configuration.AdminEnabled)
        {
            return Response.Ok(AdminHelpText);
        }

        return Response.Ok(UserHelpText);
    }
}
