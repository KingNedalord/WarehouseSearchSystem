using Microsoft.EntityFrameworkCore;
using Models;

namespace DataAccess;

/// <summary>
/// Class representing model to Entity framework
/// </summary>
public class ItemContext: DbContext
{
    public DbSet<Clothing> Clothes { get; set; }
    public DbSet<Footwear>  Footwear { get; set; }

    public ItemContext(DbContextOptions<ItemContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Name).IsRequired().HasMaxLength(50);
            entity.Property(i => i.Price).HasColumnType("numeric(18,2)");
            entity.Property(i => i.Quantity).HasDefaultValue(0);
        });

        modelBuilder.Entity<Item>()
            .Property(i => i.Size)
            .HasConversion<string>();

        modelBuilder.Entity<Item>()
            .Property(i => i.Gender)
            .HasConversion<string>();

        // TPC(Table per concrete type) mapping strategy
        modelBuilder.Entity<Item>().UseTpcMappingStrategy();

        // Clothes
        modelBuilder.Entity<Clothing>().ToTable("Clothes");
        modelBuilder.Entity<Clothing>()
            .Property(i => i.Id)
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Clothing>()
            .Property(c => c.ClothingType)
            .HasConversion<string>();

        // Footwear
        modelBuilder.Entity<Footwear>().ToTable("Footwear");
        modelBuilder.Entity<Footwear>()
            .Property(i => i.Id)
            .UseIdentityColumn()
            .ValueGeneratedOnAdd();
        modelBuilder.Entity<Footwear>()
            .Property(f => f.FootwearType)
            .HasConversion<string>();
    }
}
