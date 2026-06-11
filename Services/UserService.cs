using DataAccess;
using DataAccess.Interfaces;
using Models;
using Services.Interfaces;

namespace Services;

public class UserService : IUserService
{
    private readonly IItemReader<Clothing> _clothingDao;
    private readonly IItemReader<Footwear> _footwearDao;

    public UserService(IItemReader<Clothing> clothingDao,IItemReader<Footwear> footwearDao)
    {
        this._clothingDao = clothingDao;
        this._footwearDao = footwearDao;
    }

    /// <summary>
    ///  Filters specific(T) Item type by given predicate
    /// </summary>
    /// <param name="predicate">Filter options</param>
    /// <typeparam name="T">Type of Item</typeparam>
    /// <returns>Filtered collection of specific Item type</returns>
    /// <exception cref="NotSupportedException"></exception>
    /// <exception cref="ServiceException"></exception>
    public async Task<List<T>> FilterByAsync<T>(Predicate<T> predicate) where T : Item
    {
        try
        {
            if (typeof(T) == typeof(Clothing))
            {
                var all = await this._clothingDao.FindAllAsync();
                return all.OfType<T>().Where(i => predicate(i)).ToList();
            }

            if (typeof(T) == typeof(Footwear))
            {
                var all = await this._footwearDao.FindAllAsync();
                return all.OfType<T>().Where(i => predicate(i)).ToList();
            }

            throw new NotSupportedException($"{typeof(T).Name} is not supported");
        }
        catch (DaoException ex)
        {
            throw new ServiceException($"Failed to load items: {ex.Message}", ex);
        }
    }

    /// <summary>
    ///  Filters All Items by given predicate
    /// </summary>
    /// <param name="predicate">Filter options</param>
    /// <returns>Filtered collection of Items</returns>
    /// <exception cref="ServiceException"></exception>
    public async Task<List<Item>> FilterAllByAsync(Predicate<Item> predicate)
    {
        try
        {
            var all = await this.FindAllItemsAsync();
            return all.Where(i => predicate(i)).ToList();
        }
        catch (DaoException ex)
        {
            throw new ServiceException($"Failed to load items: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Find all items in source.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ServiceException"></exception>
    private async Task<List<Item>> FindAllItemsAsync()
    {
        var clothing = await this._clothingDao.FindAllAsync();
        var footwear = await this._footwearDao.FindAllAsync();

        var result = new List<Item>();
        result.AddRange(clothing);
        result.AddRange(footwear);
        return result;
    }
}
