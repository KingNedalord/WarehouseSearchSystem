// using Warehouse.Controller;
// using Warehouse.DAO;
// using Warehouse.Models;
// using Warehouse.Services;
// using Xunit;

// namespace Warehouse.Tests.Integration;

// /// <summary>
// /// Integration tests for FindCommand controller using actual CSV files
// /// </summary>
// public class FindCommandControllerTests
// {
//     private readonly FindCommand _findCommand;
//     private readonly ItemController _controller;

//     public FindCommandControllerTests()
//     {
//         var service = new FileBasedServiceBuilder().Build();
//         var serviceFactory = new TestServiceFactory(service);
//         _findCommand = new FindCommand(serviceFactory);
//         _controller = new ItemController(serviceFactory);
//     }

//     #region Find All Items Tests

//     [Fact]
//     public void FindCommand_AllWithoutPredicate_ShouldReturnAllItems()
//     {
//         // Arrange
//         var request = new Request("find", "all", null);

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.Equal("all without predicate", response.Message);
//         Assert.NotNull(response.Items);
//         Assert.Equal(34, response.Items.Count); // 19 clothing + 15 footwear
//     }

//     [Fact]
//     public void FindCommand_AllWithPriceFilter_ShouldReturnFilteredItems()
//     {
//         // Arrange
//         var request = new Request("find", "all", new[] { "price=20;50" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.Equal("all with parameters", response.Message);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item =>
//         {
//             Assert.True(item.Price >= 20 && item.Price <= 50);
//         });
//     }

//     [Fact]
//     public void FindCommand_AllWithSizeFilter_ShouldReturnMatchingSize()
//     {
//         // Arrange
//         var request = new Request("find", "all", new[] { "size=M" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item => Assert.Equal(Size.M, item.Size));
//     }

//     [Fact]
//     public void FindCommand_AllWithGenderFilter_ShouldReturnMatchingGender()
//     {
//         // Arrange
//         var request = new Request("find", "all", new[] { "gender=Female" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item => Assert.Equal(Gender.Female, item.Gender));
//     }

//     [Fact]
//     public void FindCommand_AllWithMultipleFilters_ShouldReturnMatchingItems()
//     {
//         // Arrange
//         var request = new Request("find", "all", new[] { "price=10;100", "size=M", "gender=Unisex" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item =>
//         {
//             Assert.True(item.Price >= 10 && item.Price <= 100);
//             Assert.Equal(Size.M, item.Size);
//             Assert.Equal(Gender.Unisex, item.Gender);
//         });
//     }

//     #endregion

//     #region Find Clothing Tests

//     [Fact]
//     public void FindCommand_ClothingWithoutPredicate_ShouldReturnAllClothing()
//     {
//         // Arrange
//         var request = new Request("find", "clothing", null);

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.Equal("clothing without predicate", response.Message);
//         Assert.NotNull(response.Items);
//         Assert.Equal(19, response.Items.Count);
//         Assert.All(response.Items, item => Assert.IsType<Clothing>(item));
//     }

//     [Fact]
//     public void FindCommand_ClothingWithPriceFilter_ShouldReturnFilteredClothing()
//     {
//         // Arrange
//         var request = new Request("find", "clothing", new[] { "price=30;60" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item =>
//         {
//             Assert.IsType<Clothing>(item);
//             Assert.True(item.Price >= 30 && item.Price <= 60);
//         });
//     }

//     [Fact]
//     public void FindCommand_ClothingWithTypeFilter_ShouldReturnMatchingType()
//     {
//         // Arrange
//         var request = new Request("find", "clothing", new[] { "type=Top" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item =>
//         {
//             var clothing = Assert.IsType<Clothing>(item);
//             Assert.Equal(ClothingType.Top, clothing.ClothingType);
//         });
//     }

//     [Fact]
//     public void FindCommand_ClothingWithMultipleFilters_ShouldReturnMatchingItems()
//     {
//         // Arrange
//         var request = new Request("find", "clothing", new[] { "type=Bottom", "gender=Female", "size=M" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item =>
//         {
//             var clothing = Assert.IsType<Clothing>(item);
//             Assert.Equal(ClothingType.Bottom, clothing.ClothingType);
//             Assert.Equal(Gender.Female, item.Gender);
//             Assert.Equal(Size.M, item.Size);
//         });
//     }

//     #endregion

//     #region Find Footwear Tests

//     [Fact]
//     public void FindCommand_FootwearWithoutPredicate_ShouldReturnAllFootwear()
//     {
//         // Arrange
//         var request = new Request("find", "footwear", null);

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.Equal("footwear without predicate", response.Message);
//         Assert.NotNull(response.Items);
//         Assert.Equal(15, response.Items.Count);
//         Assert.All(response.Items, item => Assert.IsType<Footwear>(item));
//     }

//     [Fact]
//     public void FindCommand_FootwearWithPriceFilter_ShouldReturnFilteredFootwear()
//     {
//         // Arrange
//         var request = new Request("find", "footwear", new[] { "price=100;200" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item =>
//         {
//             Assert.IsType<Footwear>(item);
//             Assert.True(item.Price >= 100 && item.Price <= 200);
//         });
//     }

//     [Fact]
//     public void FindCommand_FootwearWithTypeFilter_ShouldReturnMatchingType()
//     {
//         // Arrange
//         var request = new Request("find", "footwear", new[] { "type=Formal" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item =>
//         {
//             var footwear = Assert.IsType<Footwear>(item);
//             Assert.Equal(FootwearType.Formal, footwear.FootwearType);
//         });
//     }

//     [Fact]
//     public void FindCommand_FootwearWithMultipleFilters_ShouldReturnMatchingItems()
//     {
//         // Arrange
//         var request = new Request("find", "footwear", new[] { "type=Casual", "gender=Unisex", "price=50;100" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item =>
//         {
//             var footwear = Assert.IsType<Footwear>(item);
//             Assert.Equal(FootwearType.Casual, footwear.FootwearType);
//             Assert.Equal(Gender.Unisex, item.Gender);
//             Assert.True(item.Price >= 50 && item.Price <= 100);
//         });
//     }

//     #endregion

//     #region Invalid Input Tests

//     [Fact]
//     public void FindCommand_InvalidTarget_ShouldReturnErrorResponse()
//     {
//         // Arrange
//         var request = new Request("find", "invalid", null);

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.False(response.Success);
//         Assert.Contains("Command pattern", response.Message);
//     }

//     [Fact]
//     public void FindCommand_InvalidPriceFormat_ShouldReturnError()
//     {
//         // Arrange
//         var request = new Request("find", "clothing", new[] { "price=invalid" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.False(response.Success);
//         Assert.Contains("Invalid parameters", response.Message);
//     }

//     [Fact]
//     public void FindCommand_InvalidSize_ShouldReturnError()
//     {
//         // Arrange
//         var request = new Request("find", "clothing", new[] { "size=InvalidSize" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.False(response.Success);
//         Assert.Contains("Invalid size", response.Message);
//     }

//     [Fact]
//     public void FindCommand_InvalidGender_ShouldReturnError()
//     {
//         // Arrange
//         var request = new Request("find", "footwear", new[] { "gender=InvalidGender" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.False(response.Success);
//         Assert.Contains("Invalid gender", response.Message);
//     }

//     [Fact]
//     public void FindCommand_InvalidClothingType_ShouldReturnError()
//     {
//         // Arrange
//         var request = new Request("find", "clothing", new[] { "type=InvalidType" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.False(response.Success);
//         Assert.Contains("Invalid type", response.Message);
//     }

//     [Fact]
//     public void FindCommand_PriceMinGreaterThanMax_ShouldReturnError()
//     {
//         // Arrange
//         var request = new Request("find", "all", new[] { "price=100;50" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.False(response.Success);
//         Assert.Contains("Minimum price cannot be greater than maximum price", response.Message);
//     }

//     [Fact]
//     public void FindCommand_NegativePrice_ShouldReturnError()
//     {
//         // Arrange
//         var request = new Request("find", "clothing", new[] { "price=-10;50" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.False(response.Success);
//         Assert.Contains("Invalid minimum price", response.Message);
//     }

//     #endregion

//     #region Controller Integration Tests

//     [Fact]
//     public void Controller_FindAllClothing_ShouldWorkThroughFullStack()
//     {
//         // Arrange
//         var request = _controller.GetRequest("find clothing");

//         // Act
//         var response = _controller.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.Equal(19, response.Items.Count);
//     }

//     [Fact]
//     public void Controller_FindWithParameters_ShouldWorkThroughFullStack()
//     {
//         // Arrange
//         var request = _controller.GetRequest("find clothing price=20;50 size=M");

//         // Act
//         var response = _controller.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.All(response.Items, item =>
//         {
//             Assert.True(item.Price >= 20 && item.Price <= 50);
//             Assert.Equal(Size.M, item.Size);
//         });
//     }

//     #endregion

//     #region Edge Cases

//     [Fact]
//     public void FindCommand_EmptyResultSet_ShouldReturnEmptyList()
//     {
//         // Arrange - filter that matches nothing
//         var request = new Request("find", "clothing", new[] { "price=1;2" });

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.Empty(response.Items);
//     }

//     [Fact]
//     public void FindCommand_CaseInsensitiveTarget_ShouldWork()
//     {
//         // Arrange
//         var request = new Request("find", "CLOTHING", null);

//         // Act
//         var response = _findCommand.Execute(request);

//         // Assert
//         Assert.True(response.Success);
//         Assert.NotNull(response.Items);
//         Assert.Equal(19, response.Items.Count);
//     }

//     #endregion
// }

// /// <summary>
// /// Test service factory that returns a specific service instance
// /// </summary>
// public class TestServiceFactory : IServiceFactory
// {
//     private readonly IItemService _service;

//     public TestServiceFactory(IItemService service)
//     {
//         _service = service;
//     }

//     public void Configure(IItemService service)
//     {
//         throw new NotImplementedException();
//     }

//     public IItemService CreateService() => _service;
// }