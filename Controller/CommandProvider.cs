using Controller.Commands;
using Controller.Interfaces;
using DataAccess;

namespace Controller;

/// <summary>
/// Provides access to available commands by name
/// </summary>
public class CommandProvider : ICommandProvider
{
    private readonly AdminCommands _adminCommands;
    private readonly FindCommand _findCommand;
    private readonly ExitCommand _exitCommand;
    private readonly HelpCommand _helpCommand;
    private readonly SwitchCommand _switchCommand;
    private readonly ForbiddenCommand _forbiddenCommand;
    private readonly WrongCommand _wrongCommand;

    public CommandProvider(
        AdminCommands adminCommands,
        FindCommand findCommand,
        ExitCommand exitCommand,
        HelpCommand helpCommand,
        SwitchCommand switchCommand,
        ForbiddenCommand forbiddenCommand,
        WrongCommand wrongCommand)
    {
        this._adminCommands = adminCommands;
        this._findCommand = findCommand;
        this._exitCommand = exitCommand;
        this._helpCommand = helpCommand;
        this._switchCommand = switchCommand;
        this._forbiddenCommand = forbiddenCommand;
        this._wrongCommand = wrongCommand;
    }

    /// <summary>
    /// Gets a command by its name using specified service factory
    /// </summary>
    /// <param name="commandName">Name of the command to retrieve</param>
    /// <returns>Command instance or WrongCommand if command not found</returns>
    public ICommand GetCommand(string commandName) => commandName.ToUpper() switch
    {
        "ADD" or "DELETE" or "UPDATE" => Configuration.AdminEnabled ? this._adminCommands : this._forbiddenCommand,
        "EXIT"   => this._exitCommand,
        "FIND"   => this._findCommand,
        "HELP"   => this._helpCommand,
        "SWITCH" => this._switchCommand,
        _        => this._wrongCommand,
    };
}
