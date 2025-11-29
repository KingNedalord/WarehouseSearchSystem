// using Warehouse.Controller;
// using Warehouse.Models;
// using Warehouse.Presentation;
// using Xunit;

// namespace Warehouse.Tests.Integration;

// /// <summary>
// /// Integration tests for ConsoleView presentation layer
// /// </summary>
// public class ConsoleViewTests
// {
//     /// <summary>
//     /// Mock controller factory for testing
//     /// </summary>
//     private class MockControllerFactory : IControllerFactory
//     {
//         private readonly IController _controller;

//         public MockControllerFactory(IController controller)
//         {
//             _controller = controller;
//         }

//         public void Configure(ItemController controller)
//         {
//             // No-op for tests
//         }

//         public void Configure(IController controller)
//         {
//             // No-op for tests
//         }

//         // Helper to return concrete type when needed by tests (may be null if not applicable)
//         public ItemController CreateController() => _controller as ItemController;

//         IController IControllerFactory.CreateController()
//         {
//             return _controller;
//         }
//     }

//     /// <summary>
//     /// Mock controller for testing view behavior
//     /// </summary>
//     private class MockController : IController
//     {
//         public List<string> ReceivedInputs { get; } = new List<string>();
//         public Queue<Response> ResponseQueue { get; } = new Queue<Response>();

//         public Request GetRequest(string input)
//         {
//             ReceivedInputs.Add(input);

//             // Parse input to create request
//             var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
//             if (parts.Length == 0)
//                 return new Request("", "", null);

//             var action = parts[0];
//             var target = parts.Length > 1 ? parts[1] : "";
//             var parameters = parts.Length > 2 ? parts[2..] : null;

//             return new Request(action, target, parameters);
//         }

//         public Response Execute(Request request)
//         {
//             return ResponseQueue.Count > 0
//                 ? ResponseQueue.Dequeue()
//                 : new Response(true, "Mock response", new List<Item>());
//         }
//     }

//     /// <summary>
//     /// Helper class to capture console output
//     /// </summary>
//     private class ConsoleCapture : IDisposable
//     {
//         private readonly StringWriter _stringWriter;
//         private readonly TextWriter _originalOutput;

//         public ConsoleCapture()
//         {
//             _stringWriter = new StringWriter();
//             _originalOutput = Console.Out;
//             Console.SetOut(_stringWriter);
//         }

//         public string GetOutput() => _stringWriter.ToString();

//         public void Dispose()
//         {
//             Console.SetOut(_originalOutput);
//             _stringWriter.Dispose();
//         }
//     }

//     /// <summary>
//     /// Helper class to provide console input
//     /// </summary>
//     private class ConsoleInput : IDisposable
//     {
//         private readonly TextReader _originalInput;

//         public ConsoleInput(string input)
//         {
//             _originalInput = Console.In;
//             Console.SetIn(new StringReader(input));
//         }

//         public void Dispose()
//         {
//             Console.SetIn(_originalInput);
//         }
//     }

//     #region ConsoleView Constructor Tests

//     [Fact]
//     public void ConsoleView_Constructor_WithNullFactory_ShouldUseDefaultFactory()
//     {
//         // Act
//         var view = new ConsoleView(null);

//         // Assert
//         Assert.NotNull(view);
//     }

//     [Fact]
//     public void ConsoleView_Constructor_WithFactory_ShouldUseProvidedFactory()
//     {
//         // Arrange
//         var mockController = new MockController();
//         var factory = new MockControllerFactory(mockController);

//         // Act
//         var view = new ConsoleView(factory);

//         // Assert
//         Assert.NotNull(view);
//     }

//     #endregion

//     #region ConsoleView Start Method Tests

//     [Fact]
//     public void ConsoleView_Start_ShouldDisplayWelcomeMessage()
//     {
//         // Arrange
//         var mockController = new MockController();
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>(), true)); // Exit response
//         var factory = new MockControllerFactory(mockController);
//         var view = new ConsoleView(factory);

//         using var consoleCapture = new ConsoleCapture();
//         using var consoleInput = new ConsoleInput("exit\n");

//         // Act
//         view.Start();

//         // Assert
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("Welcome to the Item Management System!", output);
//     }

//     [Fact]
//     public void ConsoleView_Start_ShouldDisplayAvailableCommands()
//     {
//         // Arrange
//         var mockController = new MockController();
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>(), true));
//         var factory = new MockControllerFactory(mockController);
//         var view = new ConsoleView(factory);

//         using var consoleCapture = new ConsoleCapture();
//         using var consoleInput = new ConsoleInput("exit\n");

//         // Act
//         view.Start();

//         // Assert
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("Available commands:", output);
//         Assert.Contains("find clothing", output);
//         Assert.Contains("find footwear", output);
//         Assert.Contains("find all", output);
//         Assert.Contains("exit", output);
//     }

//     [Fact]
//     public void ConsoleView_Start_WithExitCommand_ShouldDisplayGoodbye()
//     {
//         // Arrange
//         var mockController = new MockController();
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>(), true));
//         var factory = new MockControllerFactory(mockController);
//         var view = new ConsoleView(factory);

//         using var consoleCapture = new ConsoleCapture();
//         using var consoleInput = new ConsoleInput("exit\n");

//         // Act
//         view.Start();

//         // Assert
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("Goodbye!", output);
//     }

//     [Fact]
//     public void ConsoleView_Start_WithSuccessfulCommand_ShouldDisplayMessage()
//     {
//         // Arrange
//         var mockController = new MockController();
//         mockController.ResponseQueue.Enqueue(new Response(true, "Success message", new List<Item>()));
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>(), true)); // Exit
//         var factory = new MockControllerFactory(mockController);
//         var view = new ConsoleView(factory);

//         using var consoleCapture = new ConsoleCapture();
//         using var consoleInput = new ConsoleInput("find clothing\nexit\n");

//         // Act
//         view.Start();

//         // Assert
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("Success message", output);
//     }

//     [Fact]
//     public void ConsoleView_Start_WithFailedCommand_ShouldDisplayError()
//     {
//         // Arrange
//         var mockController = new MockController();
//         mockController.ResponseQueue.Enqueue(new Response(false, "Error message", new List<Item>()));
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>(), true)); // Exit
//         var factory = new MockControllerFactory(mockController);
//         var view = new ConsoleView(factory);

//         using var consoleCapture = new ConsoleCapture();
//         using var consoleInput = new ConsoleInput("invalid command\nexit\n");

//         // Act
//         view.Start();

//         // Assert
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("Error: Error message", output);
//     }

//     [Fact]
//     public void ConsoleView_Start_WithItems_ShouldDisplayItems()
//     {
//         // Arrange
//         var mockController = new MockController();
//         var items = new List<Item>
//         {
//             new Clothing(1, "Test Shirt", Size.M, Gender.Unisex,  29.99m, 1,ClothingType.Top),
//             new Clothing(2, "Test Pants", Size.L, Gender.Male, 49.99m, 2, ClothingType.Bottom)
//         };
//         mockController.ResponseQueue.Enqueue(new Response(true, "Found items", items));
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>(), true)); // Exit
//         var factory = new MockControllerFactory(mockController);
//         var view = new ConsoleView(factory);

//         using var consoleCapture = new ConsoleCapture();
//         using var consoleInput = new ConsoleInput("find clothing\nexit\n");

//         // Act
//         view.Start();

//         // Assert
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("Test Shirt", output);
//         Assert.Contains("Test Pants", output);
//     }

//     [Fact]
//     public void ConsoleView_Start_WithEmptyInput_ShouldContinue()
//     {
//         // Arrange
//         var mockController = new MockController();
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>(), true)); // Exit
//         var factory = new MockControllerFactory(mockController);
//         var view = new ConsoleView(factory);

//         using var consoleCapture = new ConsoleCapture();
//         using var consoleInput = new ConsoleInput("\n\nexit\n");

//         // Act
//         view.Start();

//         // Assert - Should not crash and should eventually exit
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("Goodbye!", output);
//     }

//     [Fact]
//     public void ConsoleView_Start_ShouldPassInputToController()
//     {
//         // Arrange
//         var mockController = new MockController();
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>()));
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>(), true)); // Exit
//         var factory = new MockControllerFactory(mockController);
//         var view = new ConsoleView(factory);

//         using var consoleCapture = new ConsoleCapture();
//         using var consoleInput = new ConsoleInput("find clothing\nexit\n");

//         // Act
//         view.Start();

//         // Assert
//         Assert.Contains("find clothing", mockController.ReceivedInputs);
//         Assert.Contains("exit", mockController.ReceivedInputs);
//     }

//     [Fact]
//     public void ConsoleView_Start_WithMultipleCommands_ShouldProcessAll()
//     {
//         // Arrange
//         var mockController = new MockController();
//         mockController.ResponseQueue.Enqueue(new Response(true, "Command 1", new List<Item>()));
//         mockController.ResponseQueue.Enqueue(new Response(true, "Command 2", new List<Item>()));
//         mockController.ResponseQueue.Enqueue(new Response(true, "Command 3", new List<Item>()));
//         mockController.ResponseQueue.Enqueue(new Response(true, "", new List<Item>(), true)); // Exit
//         var factory = new MockControllerFactory(mockController);
//         var view = new ConsoleView(factory);

//         using var consoleCapture = new ConsoleCapture();
//         using var consoleInput = new ConsoleInput("find clothing\nfind footwear\nfind all\nexit\n");

//         // Act
//         view.Start();

//         // Assert
//         Assert.Equal(4, mockController.ReceivedInputs.Count);
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("Command 1", output);
//         Assert.Contains("Command 2", output);
//         Assert.Contains("Command 3", output);
//     }

//     #endregion

//     #region ConsoleView Crash Method Tests

//     [Fact]
//     public void ConsoleView_Crash_ShouldDisplayErrorMessage()
//     {
//         // Arrange
//         var view = new ConsoleView();
//         using var consoleCapture = new ConsoleCapture();

//         // Act
//         view.Crash();

//         // Assert
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("Critical error occurred", output);
//     }

//     [Fact]
//     public void ConsoleView_Crash_ShouldDisplayHelpfulMessage()
//     {
//         // Arrange
//         var view = new ConsoleView();
//         using var consoleCapture = new ConsoleCapture();

//         // Act
//         view.Crash();

//         // Assert
//         var output = consoleCapture.GetOutput();
//         Assert.Contains("check the configuration", output);
//     }

//     #endregion
// }