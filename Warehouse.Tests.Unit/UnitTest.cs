using Warehouse.DAO;
using Warehouse.Models;
using Warehouse.Services;
using Xunit;

namespace Warehouse.Tests.Integration;

/// <summary>
/// Builder for creating services that read from actual CSV files
/// </summary>
public class FileBasedServiceBuilder
{
    public ItemService Build()
    {
        var dataFolder = GetDataFolder();

        var daoFactory = ItemDaoFactory.Instance;
        daoFactory.ConfigureItemDao(new ItemDao<Clothing>(new ClothingCsvSource(Path.Combine(dataFolder, "clothing.csv"))));
        daoFactory.ConfigureItemDao(new ItemDao<Footwear>(new FootwearCsvSource(Path.Combine(dataFolder, "footwear.csv"))));

        return new ItemService(daoFactory);
    }

    private static string GetDataFolder() =>
        Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\Data"));
}

/// <summary>
/// Integration tests that use actual CSV files from the Data directory
/// </summary>
public class FileBasedServiceTests
{
    private readonly ItemService _service;

    public FileBasedServiceTests()
    {
        _service = new FileBasedServiceBuilder().Build();
    }

    [Fact]
    public void FindClothings_FromFile_ShouldReturn19Items()
    {
        // Act
        var clothing = _service.FindAll<Clothing>();

        // Assert
        Assert.Equal(19, clothing.Count);
        Assert.All(clothing, item => Assert.IsType<Clothing>(item));
    }

    [Fact]
    public void FindFootwears_FromFile_ShouldReturn15Items()
    {
        // Act
        var footwear = _service.FindAll<Footwear>();

        // Assert
        Assert.Equal(15, footwear.Count);
        Assert.All(footwear, item => Assert.IsType<Footwear>(item));
    }


    [Fact]
    public void FindByPrice_Over100_ShouldReturnOnlyPremiumFootwear()
    {
        var priceRange = new Range<decimal>(100m, 200m);

        // Act
        var products = _service.FindBy<Footwear>(c => c.Price >= 100m && c.Price <= 200m);

        // Assert
        Assert.NotEmpty(products);
        Assert.All(products, product =>
        {
            Assert.True(product.Price >= 100m && product.Price <= 200m);
            Assert.IsType<Footwear>(product);
        });
    }

    [Fact]
    public void FindBySize_XS_ShouldReturnTankTopAndRomper()
    {
        // Act
        var products = _service.FindBy<Clothing>(c => c.Size == Size.XS);

        // Assert
        Assert.Equal(3, products.Count);
        Assert.All(products, product => Assert.Equal(Size.XS, product.Size));
        Assert.Contains(products, p => p.Name == "Tank Top");
        Assert.Contains(products, p => p.Name == "Romper");
    }

    [Fact]
    public void FindByGender_Women_ShouldReturnAtLeast10Items()
    {
        // Act
        var products = _service.FindAllBy(c => c.Gender == Gender.Female);

        // Assert
        Assert.True(products.Count == 13, $"Expected at least 10 women's items, but got {products.Count}");
        Assert.All(products, product => Assert.Equal(Gender.Female, product.Gender));
    }

    [Fact]
    public void FindClothing_SpecificItems_ShouldExist()
    {
        // Act
        var clothing = _service.FindBy<Footwear>(f => f.FootwearType == FootwearType.Special).ToList();

        // Assert - Check for specific items from the CSV
        Assert.Contains(clothing, c => c.Name == "Wedding White Heels" && c.Price == 189.99m);
        Assert.Contains(clothing, c => c.Name == "Evening Stilettos" && c.Price == 219.99m);
        Assert.Contains(clothing, c => c.Name == "Prom Glitter Pumps" && c.Price == 199.99m);
        Assert.Contains(clothing, c => c.Name == "Party Platforms" && c.Price == 179.99m);
    }

    [Fact]
    public void FindFootwear_SpecificItems_ShouldExist()
    {
        // Act
        var footwear = _service.FindBy<Clothing>(c => c.ClothingType == ClothingType.Fullbody).ToList();

        // Assert - Check for specific items from the CSV
        Assert.NotEmpty(footwear);
        Assert.Equal(3, footwear.Count);
        Assert.All(footwear, product => Assert.Equal(ClothingType.Fullbody, product.ClothingType));
    }

    [Fact]
    public void FindClothing_MostExpensive_ShouldBeBlazer()
    {
        // Act
        // var clothing = _service.Find().ToList();
        var mostExpensive = _service.FindBy<Clothing>(c => c.Price > 80);

        // Assert
        Assert.Contains(mostExpensive, c => c.Name == "Blazer" && c.Price == 89.99m);

    }
    [Fact]
    public void FindBySize_Medium_ShouldReturnMultipleItems()
    {
        // Act
        var products = _service.FindAllBy(i => i.Size == Size.M).ToList();

        // Assert
        Assert.NotEmpty(products);
        Assert.Equal(10, products.Count);
        Assert.All(products, product => Assert.Equal(Size.M, product.Size));
        // Expected M size items: T-Shirt, Jeans, Skirt, Polo, Shorts, Blouse, Blazer, etc.
    }


    [Fact]
    public void FindByGender_Unisex_ShouldIncludeClothingAndFootwear()
    {
        // Act
        // var products = _service.FindByGender(Gender.Unisex).ToList();
        var footwears = _service.FindBy<Footwear>(p => p.Gender == Gender.Unisex);

        // Assert
        Assert.NotEmpty(footwears);
        Assert.Equal(5, footwears.Count);
        Assert.All(footwears, product => Assert.Equal(Gender.Unisex, product.Gender));
    }

    [Fact]
    public void FindFootwear_ByType_Formal_ShouldReturn5Items()
    {
        // Act
        var formalFootwears = _service.FindBy<Footwear>(f => f.FootwearType == FootwearType.Formal);

        // Assert
        Assert.NotEmpty(formalFootwears);
        Assert.Equal(5, formalFootwears.Count);
        Assert.All(formalFootwears, f => Assert.Equal(FootwearType.Formal, f.FootwearType));
        Assert.Contains(formalFootwears, f => f.Name == "Classic Brown Loafers");
        Assert.Contains(formalFootwears, f => f.Name == "Black Oxford Shoes");

    }

    [Fact]
    public void FindItem_ByGender_Unisex_ShouldReturnCorrectItems()
    {
        // Act
        var unisexItems = _service.FindAllBy(i => i.Gender == Gender.Unisex);

        // Assert
        Assert.NotEmpty(unisexItems);
        Assert.Equal(12, unisexItems.Count);
        Assert.All(unisexItems, i => Assert.Equal(Gender.Unisex, i.Gender));
        Assert.Contains(unisexItems, i => i.Name == "T-Shirt");
        Assert.Contains(unisexItems, i => i.Name == "Running Sneakers Blue");
    }

}