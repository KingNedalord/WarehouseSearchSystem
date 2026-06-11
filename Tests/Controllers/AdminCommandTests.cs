using Controller;
using Controller.Commands;
using Models;
using Moq;
using Services.Interfaces;

namespace Tests.Controllers;

[TestFixture]
public class AdminCommandTests
{
    private Mock<IAdminService> _adminServiceMock;
    private Mock<IUserService> _userServiceMock;
    private AdminCommands _command;

    [SetUp]
    public void SetUp()
    {
        this._adminServiceMock = new Mock<IAdminService>();
        this._userServiceMock = new Mock<IUserService>();
        this._command = new AdminCommands(this._adminServiceMock.Object, this._userServiceMock.Object);
    }

    // Positive
    [Test]
    public async Task ExecuteAsyncDeleteClothingReturnsSuccess()
    {
        var request = new Request("delete", TargetType.Clothing, ["1"]);

        var response = await this._command.ExecuteAsync(request);

        Assert.That(response.Success, Is.True);
        this._adminServiceMock.Verify(s => s.DeleteAsync<Clothing>(1), Times.Once);
    }

    [Test]
    public async Task ExecuteAsyncDeleteFootwearReturnsSuccess()
    {
        var request = new Request("delete", TargetType.Footwear, ["2"]);

        var response = await this._command.ExecuteAsync(request);

        Assert.That(response.Success, Is.True);
        this._adminServiceMock.Verify(s => s.DeleteAsync<Footwear>(2), Times.Once);
    }

    // Negative
    [Test]
    public async Task ExecuteAsyncDeleteWithNonIntIdThrowsArgumentException()
    {
        var request = new Request("delete", TargetType.Clothing, ["abc"]);
        var response = await this._command.ExecuteAsync(request);

        Assert.That(response.Success,  Is.False);
        Assert.That(response.Message,  Is.EqualTo("Parameter must be an integer"));
    }

    [Test]
    public async Task ExecuteAsyncDeleteWithEmptyParametersThrowsArgumentException()
    {
        var request = new Request("delete", TargetType.Clothing, []);
        var response = await this._command.ExecuteAsync(request);

        Assert.That(response.Success,  Is.False);
        Assert.That(response.Message,  Is.EqualTo("ID is required"));
    }

    [Test]
    public async Task ExecuteAsyncUnknownCommandReturnsFailure()
    {
        var request = new Request("explode", TargetType.Clothing, []);

        var response = await this._command.ExecuteAsync(request);

        Assert.That(response.Success, Is.False);
    }
}
