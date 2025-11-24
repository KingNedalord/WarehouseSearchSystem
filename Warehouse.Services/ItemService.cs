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
    /// Finds all available Items(Clothings/Footwears)
    /// </summary>
    /// <returns>List of all Items</returns>
    /// <exception cref="ServiceException">When DAO operations fail</exception>
    public List<T> FindAll<T>() where T : Item
    {
        try
        {
            var itemDao = ItemDaoFactory.CreateItemDao<T>();
            // items.AddRange(ClothingDao.FindAll());
            return [.. itemDao.FindAll()];
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Clothings", ex);
        }
    }


    /// <summary>
    /// Finds all available Items
    /// </summary>
    /// <returns>List of all Items</returns>
    /// <exception cref="ServiceException">When DAO operations fail</exception>
    public List<Item> FindAllItems()
    {
        try
        {
            var clothingDao = ItemDaoFactory.CreateItemDao<Clothing>();
            List<Item> items = [.. clothingDao.FindAll()];
            var footwearDao = ItemDaoFactory.CreateItemDao<Footwear>();
            foreach (var item in footwearDao.FindAll())
            {
                items.Add(item);
            }
            // return [.. itemDao.FindAll()];

            return items;
        }
        catch (DaoException ex)
        {
            throw new ServiceException("Error searching for Clothings", ex);
        }
    }

    /// <summary>
    /// Finds all available Items matching the predicate
    /// </summary>
    public List<Item> FindAllBy(Func<Item, bool> predicate)
    {
        try
        {
            // var dao = ItemDaoFactory.CreateItemDao<T>();
            var items = FindAllItems();
            var results = new List<Item>();
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    results.Add(item);
                }
            }
            return results;
        }
        catch (DaoException ex)
        {
            throw new ServiceException($"Error searching for {typeof(Item).Name}", ex);
        }
    }

    /// <summary>
    /// Finds all available Items of type T matching the predicate
    /// </summary>
    public List<T> FindBy<T>(Func<T, bool> predicate) where T : Item
    {
        try
        {
            // var dao = ItemDaoFactory.CreateItemDao<T>();
            var items = FindAll<T>();
            var results = new List<T>();
            foreach (var item in items)
            {
                if (predicate(item))
                {
                    results.Add(item);
                }
            }
            return results;
        }
        catch (DaoException ex)
        {
            throw new ServiceException($"Error searching for {typeof(T).Name}", ex);
        }
    }
}