using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DataAccess;

public class ItemContextFactory : IDesignTimeDbContextFactory<ItemContext>
{
    public ItemContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ItemContext>();
        optionsBuilder.UseNpgsql(Configuration.SqlConnectionString);

        return new ItemContext(optionsBuilder.Options);
    }
}
