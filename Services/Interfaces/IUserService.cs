using Models;

namespace Services.Interfaces;

/// <summary>
/// Interface for Item service operations
/// </summary>
public interface IUserService
{
    /// <summary>
    /// Filters footwear or clothing by given predicate
    /// </summary>
    public Task<List<T>> FilterByAsync<T>(Predicate<T> predicate) where T : Item;

    /// <summary>
    /// Finds all available Items matching the predicate
    /// </summary>
    public Task<List<Item>> FilterAllByAsync(Predicate<Item> predicate);
}
