namespace Controller.Interfaces;

public interface ICommandProvider
{
    ICommand GetCommand(string commandName);
}
