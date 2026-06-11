using DataAccess;
using DataAccess.Interfaces;
using Models;
using Moq;

namespace Tests.DAO;

[TestFixture]
public class ItemDaoTests
{
    private Mock<ISourceReader<Clothing>> _readerMock;
    private Mock<ISourceWriter<Clothing>> _writerMock;
    private ItemDao<Clothing> _dao;

    private static readonly List<Clothing> SampleItems =
    [
        new("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top),
        new("Jacket", Size.L, Gender.Unisex, 80m, 2, ClothingType.Outerwear)
    ];

    [SetUp]
    public void SetUp()
    {
        this._readerMock = new Mock<ISourceReader<Clothing>>();
        this._writerMock = new Mock<ISourceWriter<Clothing>>();
        this._dao = new ItemDao<Clothing>(this._readerMock.Object, this._writerMock.Object);

        this._readerMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(SampleItems);
    }

    // Positive
    [Test]
    public async Task FindAllAsyncReturnsAllItems()
    {
        var result = await this._dao.FindAllAsync();

        Assert.That(result.Count, Is.EqualTo(2));
    }

    [Test]
    public async Task FindAsyncWithMatchingPredicateReturnsFilteredItems()
    {
        var result = await this._dao.FindAsync(c => c.Size == Size.M);

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Shirt"));
    }

    [Test]
    public async Task AddAsyncCallsWriterAdd()
    {
        var item = new Clothing("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top);

        await this._dao.AddAsync(item);

        this._writerMock.Verify(w => w.AddAsync(item), Times.Once);
    }

    // Negative
    [Test]
    public void AddAsyncNullItemThrowsDaoException()
    {
        Assert.ThrowsAsync<DaoException>(() => this._dao.AddAsync(null!));
    }

    [Test]
    public void DeleteAsyncInvalidIdThrowsDaoException()
    {
        Assert.ThrowsAsync<DaoException>(() => this._dao.DeleteAsync(-1));
    }

    [Test]
    public async Task FindAsyncNoMatchingPredicateReturnsEmpty()
    {
        var result = await this._dao.FindAsync(c => c.Price > 1000m);

        Assert.That(result, Is.Empty);
    }
}
