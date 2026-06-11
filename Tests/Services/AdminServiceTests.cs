using DataAccess.Interfaces;
using Models;
using Moq;
using Services;

namespace Tests.Services;

[TestFixture]
public class AdminServiceTests
{
    private Mock<IItemWriter<Clothing>> _clothingWriterMock;
    private Mock<IItemWriter<Footwear>> _footwearWriterMock;
    private AdminService _service;

    [SetUp]
    public void SetUp()
    {
        this._clothingWriterMock = new Mock<IItemWriter<Clothing>>();
        this._footwearWriterMock = new Mock<IItemWriter<Footwear>>();
        this._service = new AdminService(this._clothingWriterMock.Object, this._footwearWriterMock.Object);
    }

    // Positive
    [Test]
    public async Task AddAsyncClothingCallsClothingWriter()
    {
        var item = new Clothing("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top);

        await this._service.AddAsync(item);

        this._clothingWriterMock.Verify(w => w.AddAsync(item), Times.Once);
    }

    [Test]
    public async Task DeleteAsyncFootwearCallsFootwearWriter()
    {
        await this._service.DeleteAsync<Footwear>(1);

        this._footwearWriterMock.Verify(w => w.DeleteAsync(1), Times.Once);
    }

    [Test]
    public async Task UpdateAsyncClothingCallsClothingWriter()
    {
        var item = new Clothing("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top);

        await this._service.UpdateAsync(item);

        this._clothingWriterMock.Verify(w => w.UpdateAsync(item), Times.Once);
    }

    // Negative
    [Test]
    public void AddAsyncUnknownTypeThrowsArgumentException()
    {
        Assert.ThrowsAsync<ArgumentException>(() =>
            this._service.AddAsync<Item>(null!));
    }

    [Test]
    public async Task DeleteAsyncClothingDoesNotCallFootwearWriter()
    {
        await this._service.DeleteAsync<Clothing>(1);

        this._footwearWriterMock.Verify(w => w.DeleteAsync(It.IsAny<int>()), Times.Never);
    }
}
