namespace Presentation.Interfaces;

/// <summary>
/// Interface for view components that handle user interaction and display
/// </summary>
public interface IView
{
    /// <summary>
    /// Starts the view and begins user interaction
    /// </summary>
    Task StartAsync();

    /// <summary>
    /// Handles critical errors and displays error information to the user
    /// </summary>
    void Crash(string message);
}
