using DataAccess;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Tests.DAL;

[TestFixture]
public class PostgreSqlSourceTests
{
    private ItemContext _context;
    private PostgreSqlSource<Clothing> _source;

    [SetUp]
    public void SetUp()
    {
        var options = new DbContextOptionsBuilder<ItemContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        this._context = new ItemContext(options);
        this._source = new PostgreSqlSource<Clothing>(this._context);
    }

    [TearDown]
    public void TearDown() => this._context.Dispose();

    // Positive
    [Test]
    public async Task GetAllAsyncReturnsAllItems()
    {
        this._context.Set<Clothing>().AddRange(
            new Clothing("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top),
            new Clothing("Jacket", Size.L, Gender.Unisex, 80m, 2, ClothingType.Outerwear)
        );
        await this._context.SaveChangesAsync();

        var result = await this._source.GetAllAsync();

        Assert.That(result.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task AddAsyncPersistsItemToDatabase()
    {
        var item = new Clothing("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top);

        await this._source.AddAsync(item);

        Assert.That(this._context.Set<Clothing>().Count(), Is.EqualTo(1));
    }

    [Test]
    public async Task DeleteAsyncRemovesItemFromDatabase()
    {
        var item = new Clothing("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top);
        this._context.Set<Clothing>().Add(item);
        await this._context.SaveChangesAsync();

        await this._source.DeleteAsync(item.Id);

        Assert.That(this._context.Set<Clothing>().Count(), Is.EqualTo(0));
    }

    // Negative
    [Test]
    public async Task DeleteAsyncNonExistentIdDoesNotThrow()
    {
        Assert.DoesNotThrowAsync(() => this._source.DeleteAsync(999));
    }

    [Test]
    public async Task GetAllAsyncEmptyDatabaseReturnsEmptyList()
    {
        var result = await this._source.GetAllAsync();

        Assert.That(result, Is.Empty);
    }
}
