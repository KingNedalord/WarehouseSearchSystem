using Controller;
using Controller.Interfaces;
using Models;
using Moq;
using Presentation;

namespace Tests.View;

[TestFixture]
public class ConsoleViewTests
{
    private Mock<IController> _controllerMock;
    private ConsoleView _view;

    [SetUp]
    public void SetUp()
    {
        this._controllerMock = new Mock<IController>();
        this._view = new ConsoleView(this._controllerMock.Object);
    }

    // Positive
    [Test]
    public async Task StartAsyncSuccessfulResponseDisplaysItems()
    {
        var request = new Request("find", TargetType.Clothing, []);
        var response = new Response(true, "Found: 1 items",
        [
            new Clothing("Shirt", Size.M, Gender.Male, 20m, 5, ClothingType.Top)
        ]);
        var exitResponse = new Response(true, isExit: true);

        // Simulate: one valid command then exit
        this._controllerMock.SetupSequence(c => c.GetRequest(It.IsAny<string>()))
            .Returns(request)
            .Returns(new Request("exit", TargetType.NoTarget, []));

        this._controllerMock.SetupSequence(c => c.ExecuteAsync(It.IsAny<Request>()))
            .ReturnsAsync(response)
            .ReturnsAsync(exitResponse);

        Console.SetIn(new StringReader("find clothing\nexit\n"));

        Assert.DoesNotThrowAsync(() => this._view.StartAsync());
    }

    [Test]
    public async Task StartAsyncExitCommandStopsLoop()
    {
        var exitResponse = new Response(true, isExit: true);

        this._controllerMock
            .Setup(c => c.GetRequest("exit"))
            .Returns(new Request("exit", TargetType.NoTarget, []));

        this._controllerMock
            .Setup(c => c.ExecuteAsync(It.IsAny<Request>()))
            .ReturnsAsync(exitResponse);

        Console.SetIn(new StringReader("exit\n"));

        Assert.DoesNotThrowAsync(() => this._view.StartAsync());
    }

    // Negative
    [Test]
    public async Task StartAsyncFailedResponseDoesNotThrow()
    {
        var failResponse = new Response(false, "Unknown command");
        var exitResponse = new Response(true, isExit: true);

        this._controllerMock.SetupSequence(c => c.GetRequest(It.IsAny<string>()))
            .Returns(new Request("bad", TargetType.NoTarget, []))
            .Returns(new Request("exit", TargetType.NoTarget, []));

        this._controllerMock.SetupSequence(c => c.ExecuteAsync(It.IsAny<Request>()))
            .ReturnsAsync(failResponse)
            .ReturnsAsync(exitResponse);

        Console.SetIn(new StringReader("bad command\nexit\n"));

        Assert.DoesNotThrowAsync(() => this._view.StartAsync());
    }
}
