namespace Services;
/// <summary>
/// Exception thrown when a service layer operation fails.
/// </summary>
public class ServiceException : Exception
{
    /// <summary>
    /// Creates a new service exception with the specified message
    /// </summary>
    /// <param name="message">Error message</param>
    public ServiceException(string message) : base(message) { }

    /// <summary>
    /// Creates a new service exception with the specified message and inner exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="innerException">Inner exception that caused this error</param>
    public ServiceException(string message, Exception innerException) : base(message, innerException) { }
}
