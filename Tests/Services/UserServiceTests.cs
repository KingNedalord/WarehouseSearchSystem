using DataAccess.Interfaces;
using Models;
using Moq;
using Services;

namespace Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<IItemReader<Clothing>> _clothingReaderMock;
    private Mock<IItemReader<Footwear>> _footwearReaderMock;
    private UserService _service;

    [SetUp]
    public void SetUp()
    {
        this._clothingReaderMock = new Mock<IItemReader<Clothing>>();
        this._footwearReaderMock = new Mock<IItemReader<Footwear>>();
        this._service = new UserService(this._clothingReaderMock.Object, this._footwearReaderMock.Object);

        this._clothingReaderMock
            .Setup(r => r.FindAllAsync())
            .ReturnsAsync(new List<Clothing>
            {
                new("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top),
                new("Jacket", Size.L, Gender.Unisex, 80m, 2, ClothingType.Outerwear),
            });

        this._footwearReaderMock
            .Setup(r => r.FindAllAsync())
            .ReturnsAsync(new List<Footwear>
            {
                new("Sneakers", Size.L, Gender.Male, 60m, 3, FootwearType.Casual),
            });
    }

    // Positive
    [Test]
    public async Task FilterByAsyncClothingReturnsMatchingItems()
    {
        var result = await this._service.FilterByAsync<Clothing>(c => c.Size == Size.M);

        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Name, Is.EqualTo("Shirt"));
    }

    [Test]
    public async Task FilterAllByAsyncReturnsItemsFromBothTypes()
    {
        var result = await this._service.FilterAllByAsync(_ => true);

        Assert.That(result.Count, Is.EqualTo(3)); // 2 clothing + 1 footwear
    }

    [Test]
    public async Task FilterByAsyncFootwearReturnsMatchingItems()
    {
        var result = await this._service.FilterByAsync<Footwear>(f => f.FootwearType == FootwearType.Casual);

        Assert.That(result.Count, Is.EqualTo(1));
    }

    // Negative
    [Test]
    public async Task FilterByAsyncNoMatchesReturnsEmptyList()
    {
        var result = await this._service.FilterByAsync<Clothing>(c => c.Price > 9999m);

        Assert.That(result, Is.Empty);
    }

    [Test]
    public void FilterByAsyncUnsupportedTypeThrowsNotSupportedException()
    {
        Assert.ThrowsAsync<NotSupportedException>(() =>
            this._service.FilterByAsync<Item>(_ => true));
    }
}
