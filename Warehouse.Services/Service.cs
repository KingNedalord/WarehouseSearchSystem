using Warehouse.DAO;
using Warehouse.Models;
namespace Warehouse.Services;

/// <summary>
/// Service implementation for Item operations with dependency injection support
/// </summary>
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
    /// Finds all available Items
    /// </summary>
    public List<Item> FindAllItems()
    {
        try
        {
            var res = new List<Item>();
            var clothingDao = ItemDaoFactory.CreateItemDao<Clothing>();
            res.AddRange(clothingDao.FindAll());

            var footwearDao = ItemDaoFactory.CreateItemDao<Footwear>();
            res.AddRange(footwearDao.FindAll());

            return res;
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Items: ", ex);
        }
    }

    /// <summary>
    /// Filters footwear or clothing by given predicate
    /// </summary>
    public List<T> FilterBy<T>(Predicate<T> predicate) where T : Item
    {
        try
        {
            var clothingDao = ItemDaoFactory.CreateItemDao<T>();
            return [.. clothingDao.Find(predicate)];
        }
        catch (DaoException ex)
        {
            throw new ServiceException($"Error searching for {typeof(T).Name}", ex);
        }
    }
    /// <summary>
    /// Finds all available Items matching the predicate
    /// </summary>
    public List<Item> FilterAllBy(Predicate<Item> predicate)
    {
        try
        {
            return [.. FindAllItems().Where(i => predicate(i))];
        }
        catch (DaoException ex)
        {
            throw new ServiceException($"Error searching for {typeof(Item).Name}", ex);
        }
    }
}