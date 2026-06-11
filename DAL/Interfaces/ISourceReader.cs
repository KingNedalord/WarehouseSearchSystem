using Models;

namespace DataAccess.Interfaces;

public interface ISourceReader<T> where T : Item
{
    /// <summary>
    /// Gets all items
    /// </summary>
    /// <returns></returns>
    public Task<IEnumerable<T>> GetAllAsync();
}
