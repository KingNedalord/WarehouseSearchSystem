using Warehouse.DAO;
using Warehouse.Models;
namespace Warehouse.Services;
/// <summary>
/// Service implementation for Item operations with dependency injection support
/// </summary>
/// <example>
public class ItemService : IItemService
{
    private readonly IItemDaoFactory ItemDaoFactory;

    /// <summary>
    /// Creates a service with injected DAO factory
    /// </summary>
    /// <param name="ItemDaoFactory">DAO factory</param>
    public ItemService(IItemDaoFactory ItemDaoFactory)
    {
        this.ItemDaoFactory = ItemDaoFactory ?? throw new ArgumentNullException(nameof(ItemDaoFactory));
    }

    /// <summary>
    /// Finds all available Clothings
    /// </summary>
    /// <returns>List of all Clothings</returns>
    /// <exception cref="ServiceException">When DAO operations fail</exception>
    public IList<Clothing> FindClothings()
    {
        try
        {
            var ClothingDao = ItemDaoFactory.CreateItemDao<Clothing>();
            return new List<Clothing>(ClothingDao.FindAll());
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Clothings", ex);
        }
    }

    /// <summary>
    /// Finds all available Footwears
    /// </summary>
    /// <returns>List of all Footwears</returns>
    /// <exception cref="ServiceException">When DAO operations fail</exception>
    public IList<Footwear> FindFootwears()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Finds Items within the specified price range
    /// </summary>
    /// <param name="range">Price range to search within</param>
    /// <returns>List of Items matching the price criteria</returns>
    /// <exception cref="ServiceException">When DAO operations fail</exception>
    public IList<Item> FindByPrice(Range<decimal> range)
    {
        throw new NotImplementedException();
    }
}