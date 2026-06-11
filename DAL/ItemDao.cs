using DataAccess.Interfaces;
using Models;

namespace DataAccess;

/// <summary>
/// Generic DAO implementation for Straight approach that uses a data source to load and filter Items
/// </summary>
/// <typeparam name="T">Type of Item to handle</typeparam>
public class ItemDao<T>(ISourceReader<T> reader, ISourceWriter<T> writer) : IItemReader<T>, IItemWriter<T>
    where T : Item
{
    /// <summary>
    /// Finds Items matching the specified predicate
    /// </summary>
    /// <param name="predicate">Predicate to filter Items</param>
    /// <returns>List of matching Items</returns>
    public async Task<IList<T>> FindAsync(Predicate<T> predicate)
    {
        var all = await reader.GetAllAsync();
        return all.Where(i => predicate(i)).ToList();
    }

    /// <summary>
    /// Finds all Items of the specified type
    /// </summary>
    /// <returns>List of all Items</returns>
    public async Task<IList<T>> FindAllAsync()
    {
        var all = await reader.GetAllAsync();
        return all.ToList();
    }

    /// <inheritdoc/>>
    public async Task AddAsync(T item)
    {
        if (item == null)
        {
            throw new DaoException(nameof(item));
        }

        await writer.AddAsync(item);
    }

    /// <inheritdoc/>>
    public async Task DeleteAsync(int id)
    {
        if (id <= 0)
        {
            throw new DaoException(nameof(id));
        }

        await writer.DeleteAsync(id);
    }

    /// <inheritdoc/>>
    public async Task UpdateAsync(T item)
    {
        if (item == null)
        {
            throw new DaoException(nameof(item));
        }

        await writer.UpdateAsync(item);
    }
}
