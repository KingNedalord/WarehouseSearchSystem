using Warehouse.Controller;
namespace Warehouse.Presentation;
/// <summary>
/// Factory class for creating view layer objects.
/// </summary>
public class ViewFactory : IViewFactory
{
    private static readonly ViewFactory instance = new ViewFactory();
    private IView? view;

    /// <summary>
    /// Private constructor to enforce Singleton pattern
    /// </summary>
    private ViewFactory() { }

    /// <summary>
    /// Gets the singleton instance of ViewFactory
    /// </summary>
    public static ViewFactory Instance => instance;

    /// <summary>
    /// Configures the factory with a specific view instance
    /// </summary>
    public void Configure(IView? view)
    {
        this.view = view ?? throw new ArgumentNullException(nameof(view), "View cannot be null");
    }

    /// <summary>
    /// Creates a view instance
    /// </summary>
    /// <returns>Configured view instance or default ConsoleView with controller factory if none configured</returns>
    public IView CreateView()
    {
        return view ?? new ConsoleView(ControllerFactory.Instance);
    }
}