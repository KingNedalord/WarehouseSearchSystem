namespace DataAccess;
/// <summary>
/// Exception thrown when a data access operation fails in the DAO layer.
/// </summary>
public class DaoException : Exception
{
    /// <summary>
    /// Creates a new DAO exception with the specified message
    /// </summary>
    /// <param name="message">Error message</param>
    public DaoException(string message) : base(message) { }
}
