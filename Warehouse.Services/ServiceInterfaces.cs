using Warehouse.Models;
namespace Warehouse.Services;
/// <summary>
/// Interface for Item service operations
/// </summary>
public interface IItemService
{
    /// <summary>
    /// Finds all available Items(Clothings/Footwears)
    /// </summary>
    public List<T> FindAll<T>() where T : Item;

    /// <summary>
    /// Finds all available Items
    /// </summary
    public List<Item> FindAllItems();

    /// <summary>
    /// Filters footwear or clothing by given predicate
    /// </summary>
    public List<T> FilterBy<T>(Predicate<T> predicate) where T : Item;

    /// <summary>
    /// Finds all available Items matching the predicate
    /// </summary>
    public List<Item> FilterAllBy(Predicate<Item> predicate);
}


/// <summary>
/// Interface for a factory that creates Item service objects.
/// </summary>
public interface IServiceFactory
{
    /// <summary>
    /// Creates a new Item service instance
    /// </summary>
    /// <returns>New Item service instance</returns>
    IItemService CreateService();

    /// <summary>
    /// Configures the factory with a specific service instance
    /// </summary>
    /// <param name="service">Service instance to configure</param>
    void Configure(IItemService service);
}