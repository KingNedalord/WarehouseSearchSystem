namespace Warehouse.Controller;
/// <summary>
/// Interface for a factory that creates controller objects.
/// </summary>
public interface IControllerFactory
{
    /// <summary>
    /// Creates a new controller instance
    /// </summary>
    /// <returns>New controller instance</returns>
    IController CreateController();

    /// <summary>
    /// Configures a controller for future use
    /// </summary>
    /// <param name="controller">Controller to configure</param>
    void Configure(IController controller);
}

/// <summary>
/// Interface for item controllers that handle user requests
/// </summary>
public interface IController
{
    /// <summary>
    /// Parses user input string and creates a Request object
    /// </summary>
    /// <param name="input">Input string.</param>
    /// <returns></returns>
    Request GetRequest(string input);

    /// <summary>
    /// Executes a request and returns a response
    /// </summary>
    Response Execute(Request request);
}

/// <summary>
/// Interface for commands that can be executed by the controller
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Executes the command with the specified request
    /// </summary>
    Response Execute(Request request);
}
