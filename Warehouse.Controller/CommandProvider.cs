using Warehouse.Services;

/// <summary>
/// Provides access to available commands by name
/// </summary>
public static class CommandProvider
{
    /// <summary>
    /// Gets a command by its name using default factories
    /// </summary>
    /// <param name="commandName">Name of the command to retrieve</param>
    /// <returns>Command instance or WrongCommand if command not found</returns>
    public static ICommand GetCommand(string? commandName) => GetCommand(commandName, ServiceFactory.Instance);

    /// <summary>
    /// Gets a command by its name using specified service factory
    /// </summary>
    /// <param name="commandName">Name of the command to retrieve</param>
    /// <param name="serviceFactory">Service factory to use for command creation</param>
    /// <returns>Command instance or WrongCommand if command not found</returns>
    public static ICommand GetCommand(string? commandName, IServiceFactory serviceFactory) => commandName?.ToUpper() switch
    {
        "FIND" => new FindCommand(serviceFactory),
        "COST" => new CostCommand(serviceFactory),
        "EXIT" => new ExitCommand(),
        _ => new WrongCommand()
    };
}