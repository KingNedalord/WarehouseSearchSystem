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
            return [.. ClothingDao.FindAll()];
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
        try
        {
            var FootwearDao = ItemDaoFactory.CreateItemDao<Footwear>();
            return [.. FootwearDao.FindAll()];
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Footwears", ex);
        }
    }

    /// <summary>
    /// Finds Items within the specified price range
    /// </summary>
    /// <param name="range">Price range to search within</param>
    /// <returns>List of Items matching the price criteria</returns>
    /// <exception cref="ServiceException">When DAO operations fail</exception>
    public IList<Item> FindByPrice(Range<decimal> range)
    {
        var allItems = new List<Item>();
        try
        {
            var allClothing = FindClothings();
            var filteredCLothing = allClothing
            .Where(f => f.Price >= range.Min && f.Price <= range.Max)
            .ToList();
            allItems.AddRange(filteredCLothing);
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Clothing by price", ex);
        }
        try
        {
            var allFootwear = FindFootwears();

            var filteredFootwear = allFootwear
                .Where(f => f.Price >= range.Min && f.Price <= range.Max)
                .ToList();

            allItems.AddRange(filteredFootwear);
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Footwear by price", ex);
        }

        return allItems;
    }

    public IList<Item> FindBySize(Size s)
    {
        var allItems = new List<Item>();
        try
        {
            var allClothing = FindClothings();
            var filteredCLothing = allClothing
            .Where(f => f.Size == s)
            .ToList();
            allItems.AddRange(filteredCLothing);
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Clothing by size", ex);
        }
        try
        {
            var allFootwear = FindFootwears();

            var filteredFootwear = allFootwear
                .Where(f => f.Size == s)
                .ToList();

            allItems.AddRange(filteredFootwear);
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Footwear by size", ex);
        }

        return allItems;
    }


    public IList<Item> FindByGender(Gender g)
    {
        var allItems = new List<Item>();
        try
        {
            var allClothing = FindClothings();
            var filteredCLothing = allClothing
            .Where(f => f.Gender == g)
            .ToList();
            allItems.AddRange(filteredCLothing);
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Clothing by gender", ex);
        }
        try
        {
            var allFootwear = FindFootwears();

            var filteredFootwear = allFootwear
                .Where(f => f.Gender == g)
                .ToList();

            allItems.AddRange(filteredFootwear);
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Footwear by gender", ex);
        }

        return allItems;
    }
}