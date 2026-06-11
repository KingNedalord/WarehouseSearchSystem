using Models;

namespace DataAccess.Interfaces;

public interface IItemReader<T> where T : Item
{
    /// <summary>
    /// Finds all Items of the specified type
    /// </summary>
    /// <returns>List of all Items</returns>
    Task<IList<T>> FindAllAsync();
}
