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
    /// <returns>List of all Items</returns>
    /// <exception cref="ServiceException">When DAO operations fail</exception>
    public List<T> FindAll<T>() where T : Item;

    /// <summary>
    /// Finds all available Items
    /// </summary>
    /// <returns>List of all Items</returns>
    /// <exception cref="ServiceException">When DAO operations fail</exception>
    public List<Item> FindAllItems();

    /// <summary>
    /// Finds all available Items matching the predicate
    /// </summary>
    public List<Item> FindAllBy(Func<Item, bool> predicate);

    /// <summary>
    /// Finds all available Items of type T matching the predicate
    /// </summary>
    public List<T> FindBy<T>(Func<T, bool> predicate) where T : Item;
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