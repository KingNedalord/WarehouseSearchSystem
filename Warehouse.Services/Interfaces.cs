using Warehouse.Models;
namespace Warehouse.Services;
/// <summary>
/// Interface for Item service operations
/// </summary>
public interface IItemService
{
    /// <summary>
    /// Finds all available Clothings
    /// </summary>
    /// <returns>List of all Clothings</returns>
    IList<Clothing> FindClothings();

    /// <summary>
    /// Finds all available Footwears
    /// </summary>
    /// <returns>List of all Footwears</returns>
    IList<Footwear> FindFootwears();

    /// <summary>
    /// Finds Items within the specified price range
    /// </summary
    /// <param name="range">Price range to search within</param>
    /// <returns>List of Items matching the price criteria</returns>
    /// <exception cref="ServiceException">When DAO operations fail</exception>
    IList<Item> FindByPrice(Range<decimal> range);
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