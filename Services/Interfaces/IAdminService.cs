using Models;

namespace Services.Interfaces;

public interface IAdminService
{
    /// <summary>
    /// Adds clothing to database.
    /// </summary>
    /// <param name="item">Item to add</param>
    public Task AddAsync<T>(T item);

    /// <summary>
    /// Deletes clothing from database.
    /// </summary>
    /// <param name="id">Identification number of an item</param>
    public Task DeleteAsync<T>(int id) where T : Item;

    /// <summary>
    /// Updates existing Clothing.
    /// </summary>
    /// <param name="item">item update</param>
    public Task UpdateAsync<T>(T item);
}
