/// Factory class for creating controller layer objects.
/// </summary>
public class ControllerFactory : IControllerFactory
{
    private static readonly ControllerFactory instance = new ControllerFactory();
    private IController? controller;

    private ControllerFactory() { }

    /// <summary>
    /// Single instance of ControllerFactory (Singleton pattern).
    /// </summary>
    public static ControllerFactory Instance => instance;

    /// <summary>
    /// Configures the application controller.
    /// After configuration, when calling CreateController(), the registered instance will be returned instead of creating a new one.
    /// </summary>
    /// <param name="controller">Controller to register</param>
    public void Configure(IController controller)
    {
        this.controller = controller;
    }

    /// <summary>
    /// Creates an application controller.
    /// If a controller was configured, returns it; otherwise, creates a new Controller instance.
    /// </summary>
    /// <returns>IController instance</returns>
    public IController CreateController()
    {
        if (controller != null)
        {
            return controller;
        }

        return new Controller();
    }
}