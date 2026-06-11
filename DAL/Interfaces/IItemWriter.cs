using Models;

namespace DataAccess.Interfaces;

public interface IItemWriter<T> where T : Item
{
    /// <summary>
    /// Adds an item to database.
    /// </summary>
    /// <param name="item">Item to add</param>
    public Task AddAsync(T item);

    /// <summary>
    /// Deletes an item from database.
    /// </summary>
    /// <param name="id">Identification number of an item</param>
    public Task DeleteAsync(int id);

    /// <summary>
    /// Updates existing item.
    /// </summary>
    /// <param name="item">item update</param>
    public Task UpdateAsync(T item);
}
