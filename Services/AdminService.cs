using DataAccess.Interfaces;
using Models;
using Services.Interfaces;

namespace Services;

public class AdminService : IAdminService
{
    private readonly IItemWriter<Clothing> _clothingDao;
    private readonly IItemWriter<Footwear> _footwearDao;

    public AdminService(IItemWriter<Clothing> clothingDao,IItemWriter<Footwear> footwearDao)
    {
        this._clothingDao = clothingDao;
        this._footwearDao = footwearDao;
    }

    /// <inheritdoc/>>
    public async Task AddAsync<T>(T item)
    {
        switch (item)
        {
            case Clothing clothing:
                await this._clothingDao.AddAsync(clothing);
                break;
            case Footwear footwear:
                await this._footwearDao.AddAsync(footwear);
                break;
            default: throw new ArgumentException($"Unknown item type: {typeof(T).Name}");
        }
    }

    /// <inheritdoc/>>
    public async Task DeleteAsync<T>(int id) where T : Item
    {
        if (typeof(T) == typeof(Clothing))
        {
            await this._clothingDao.DeleteAsync(id);
        }
        else if (typeof(T) == typeof(Footwear))
        {
            await this._footwearDao.DeleteAsync(id);
        }
    }

    /// <inheritdoc/>>
    public async Task UpdateAsync<T>(T item)
    {
        switch (item)
        {
            case Clothing clothing:
                await this._clothingDao.UpdateAsync(clothing);
                break;
            case Footwear footwear:
                await this._footwearDao.UpdateAsync(footwear);
                break;
            default: throw new ArgumentException($"Unknown item type: {typeof(T).Name}");
        }
    }
}
