using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess;

public class PostgreSqlSource<T>(ItemContext dbContext) : ISourceReader<T>, ISourceWriter<T>
    where T : Item
{
    /// <inheritdoc/>>
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await dbContext
            .Set<T>()
            // tells EF not to track the entities it loads into memory
            .AsNoTracking()
            .ToListAsync();
    }

    /// <inheritdoc/>>
    public async Task AddAsync(T item)
    {
        await dbContext.Set<T>().AddAsync(item);
        await dbContext.SaveChangesAsync();
        // Stop tracking entity after write
        dbContext.Entry(item).State = EntityState.Detached;
    }

    /// <inheritdoc/>>
    public async Task DeleteAsync(int id)
    {
        var item = await dbContext.Set<T>().FindAsync(id);
        if (item == null)
        {
            return;
        }

        dbContext.Set<T>().Remove(item);
        await dbContext.SaveChangesAsync();
    }

    /// <inheritdoc/>>
    public async Task UpdateAsync(T item)
    {
        var tracked = await dbContext.Set<T>().FindAsync(item.Id);
        if (tracked == null)
        {
            return;
        }

        dbContext.Entry(tracked).CurrentValues.SetValues(item);
        await dbContext.SaveChangesAsync();
    }
}
