/// <summary>
/// Interface for view components that handle user interaction and display
/// </summary>
public interface IView
{
    /// <summary>
    /// Starts the view and begins user interaction
    /// </summary>
    void Start();

    /// <summary>
    /// Handles critical errors and displays error information to the user
    /// </summary>
    void Crash();
}


/// <summary>
/// Interface for a factory that creates view objects.
/// </summary>
public interface IViewFactory
{
    /// <summary>
    /// Creates a new view instance
    /// </summary>
    /// <returns>New view instance</returns>
    IView CreateView();

    /// <summary>
    /// Configures the factory with a specific view instance
    /// </summary>
    /// <param name="view">View instance to configure</param>
    void Configure(IView view);
}