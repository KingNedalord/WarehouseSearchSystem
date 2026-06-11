using Controller;
using Controller.Commands;
using Models;
using Moq;
using Services;
using Services.Interfaces;

namespace Tests.Controllers;

[TestFixture]
public class FindCommandTests
{
    private Mock<IUserService> _serviceMock;
    private FindCommand _command;

    [SetUp]
    public void SetUp()
    {
        this._serviceMock = new Mock<IUserService>();
        this._command = new FindCommand(this._serviceMock.Object);

        this._serviceMock
            .Setup(s => s.FilterByAsync(It.IsAny<Predicate<Clothing>>()))
            .ReturnsAsync([
                new Clothing("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top),
            ]);

        this._serviceMock
            .Setup(s => s.FilterAllByAsync(It.IsAny<Predicate<Item>>()))
            .ReturnsAsync([
                new Clothing("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top),
                new Footwear("Sneakers", Size.L, Gender.Male, 60m, 3, FootwearType.Casual),
            ]);
    }

    // Positive
    [Test]
    public async Task ExecuteAsyncFindClothingReturnsSuccess()
    {
        var request = new Request("find", TargetType.Clothing, []);

        var response = await this._command.ExecuteAsync(request);

        Assert.That(response.Success, Is.True);
        Assert.That(response.Items.Count, Is.EqualTo(1));
    }

    [Test]
    public async Task ExecuteAsyncFindAllReturnsBothTypes()
    {
        var request = new Request("find", TargetType.All, []);

        var response = await this._command.ExecuteAsync(request);

        Assert.That(response.Success, Is.True);
        Assert.That(response.Items.Count, Is.EqualTo(2));
    }

    // Negative
    [Test]
    public async Task ExecuteAsyncUnknownTargetReturnsFailure()
    {
        var request = new Request("find", TargetType.NoTarget, []);

        var response = await this._command.ExecuteAsync(request);

        Assert.That(response.Success, Is.False);
    }

    [Test]
    public async Task ExecuteAsyncServiceThrowsReturnsFailureResponse()
    {
        this._serviceMock
            .Setup(s => s.FilterByAsync(It.IsAny<Predicate<Clothing>>()))
            .ThrowsAsync(new ServiceException("DB error"));

        var request = new Request("find", TargetType.Clothing, []);

        var response = await this._command.ExecuteAsync(request);

        Assert.That(response.Success, Is.False);
        Assert.That(response.Message, Does.Contain("DB error"));
    }
}
