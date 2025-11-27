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
public class ServiceTest
{
    private readonly ItemService _service;

    public ServiceTest()
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
        var products = _service.FilterBy<Footwear>(c => c.Price >= 100m && c.Price <= 200m);
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
        var products = _service.FilterBy<Clothing>(c => c.Size == Size.XS && c.Gender == Gender.Female);

        // Assert
        Assert.Equal(2, products.Count);
        Assert.All(products, product => Assert.Equal(Size.XS, product.Size));
        Assert.Contains(products, p => p.Name == "Tank Top");
        Assert.Contains(products, p => p.Name == "Romper");
    }

    [Fact]
    public void FindByGender_Women_ShouldReturnAtLeast10Items()
    {
        // Act
        var products = _service.FilterAllBy(c => c.Gender == Gender.Female);

        // Assert
        Assert.True(products.Count == 13, $"Expected at least 10 women's items, but got {products.Count}");
        Assert.All(products, product => Assert.Equal(Gender.Female, product.Gender));
    }

    [Fact]
    public void FindClothing_SpecificItems_ShouldExist()
    {
        // Act
        var clothing = _service.FilterBy<Footwear>(f => f.FootwearType == FootwearType.Special);

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
        var footwear = _service.FilterBy<Clothing>(c => c.ClothingType == ClothingType.Fullbody).ToList();

        // Assert - Check for specific items from the CSV
        Assert.NotEmpty(footwear);
        Assert.Equal(3, footwear.Count);
        Assert.All(footwear, product => Assert.Equal(ClothingType.Fullbody, product.ClothingType));
    }

    [Fact]
    public void FindClothing_MostExpensive_ShouldBeBlazer()
    {
        // Act
        var mostExpensive = _service.FilterBy<Clothing>(c => c.Price > 80);

        // Assert
        Assert.Contains(mostExpensive, c => c.Name == "Blazer" && c.Price == 89.99m);

    }
    [Fact]
    public void FindBySize_Medium_ShouldReturnMultipleItems()
    {
        // Act
        var products = _service.FilterAllBy(i => i.Size == Size.M).ToList();

        // Assert
        Assert.NotEmpty(products);
        Assert.Equal(10, products.Count);
        Assert.All(products, product => Assert.Equal(Size.M, product.Size));
    }


    [Fact]
    public void FindByGender_Unisex_ShouldIncludeClothingAndFootwear()
    {
        // Act
        var footwears = _service.FilterBy<Footwear>(p => p.Gender == Gender.Unisex);

        // Assert
        Assert.NotEmpty(footwears);
        Assert.Equal(5, footwears.Count);
        Assert.All(footwears, product => Assert.Equal(Gender.Unisex, product.Gender));
    }

    [Fact]
    public void FindFootwear_ByType_Formal_ShouldReturn5Items()
    {
        // Act
        var formalFootwears = _service.FilterBy<Footwear>(f => f.FootwearType == FootwearType.Formal);

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
        var unisexItems = _service.FilterAllBy(i => i.Gender == Gender.Unisex);

        // Assert
        Assert.NotEmpty(unisexItems);
        Assert.Equal(12, unisexItems.Count);
        Assert.All(unisexItems, i => Assert.Equal(Gender.Unisex, i.Gender));
        Assert.Contains(unisexItems, i => i.Name == "T-Shirt");
        Assert.Contains(unisexItems, i => i.Name == "Running Sneakers Blue");
    }


    [Fact]
    public void FindClothing_ByType_Top_ShouldReturn6Items()
    {
        // Act
        var tops = _service.FilterBy<Clothing>(c => c.ClothingType == ClothingType.Top);

        // Assert
        Assert.NotEmpty(tops);
        Assert.Equal(6, tops.Count);
        Assert.All(tops, c => Assert.Equal(ClothingType.Top, c.ClothingType));
        Assert.Contains(tops, c => c.Name == "T-Shirt" && c.Price == 19.99m);
        Assert.Contains(tops, c => c.Name == "Polo" && c.Price == 24.99m);
        Assert.Contains(tops, c => c.Name == "Sweater" && c.Price == 44.99m);
    }

    [Fact]
    public void FindClothing_ByType_Bottom_ShouldReturn5Items()
    {
        // Act
        var bottoms = _service.FilterBy<Clothing>(c => c.ClothingType == ClothingType.Bottom);
        // Assert
        Assert.NotEmpty(bottoms);
        Assert.Equal(6, bottoms.Count);
        Assert.All(bottoms, c => Assert.Equal(ClothingType.Bottom, c.ClothingType));
        Assert.Contains(bottoms, c => c.Name == "Jeans" && c.Price == 49.99m);
        Assert.Contains(bottoms, c => c.Name == "Shorts" && c.Price == 22.99m);
        Assert.Contains(bottoms, c => c.Name == "Leggings" && c.Price == 29.99m);
    }

    [Fact]
    public void FindFootwear_ByType_Casual_ShouldReturn5Items()
    {
        // Act
        var casualFootwear = _service.FilterBy<Footwear>(f => f.FootwearType == FootwearType.Casual);
        // Assert
        Assert.NotEmpty(casualFootwear);
        Assert.Equal(5, casualFootwear.Count);
        Assert.All(casualFootwear, f => Assert.Equal(FootwearType.Casual, f.FootwearType));
        Assert.Contains(casualFootwear, f => f.Name == "Running Sneakers Blue" && f.Price == 119.99m);
        Assert.Contains(casualFootwear, f => f.Name == "Canvas Slip-ons" && f.Price == 59.99m);
        Assert.Contains(casualFootwear, f => f.Name == "Beach Flip-flops" && f.Price == 34.99m);
    }

    [Fact]
    public void FindByPrice_Under50_ShouldReturnAffordableItems()
    {
        // Act
        var affordableItems = _service.FilterAllBy(i => i.Price < 50m);
        // Assert
        Assert.NotEmpty(affordableItems);
        Assert.All(affordableItems, i => Assert.True(i.Price < 50m));
        Assert.Contains(affordableItems, i => i.Name == "T-Shirt" && i.Price == 19.99m);
        Assert.Contains(affordableItems, i => i.Name == "Tank Top" && i.Price == 14.99m);
        Assert.Contains(affordableItems, i => i.Name == "Beach Flip-flops" && i.Price == 34.99m);
    }

    [Fact]
    public void FindClothing_ByType_Outerwear_ShouldReturn3Items()
    {
        // Act
        var outerwear = _service.FilterBy<Clothing>(c => c.ClothingType == ClothingType.Outerwear);
        // Assert
        Assert.NotEmpty(outerwear);
        Assert.Equal(3, outerwear.Count);
        Assert.All(outerwear, c => Assert.Equal(ClothingType.Outerwear, c.ClothingType));
        Assert.Contains(outerwear, c => c.Name == "Hoodie" && c.Price == 39.99m);
        Assert.Contains(outerwear, c => c.Name == "Jacket" && c.Price == 79.99m);
        Assert.Contains(outerwear, c => c.Name == "Blazer" && c.Price == 89.99m);
    }

    [Fact]
    public void FindBySize_Large_ShouldReturnCorrectItems()
    {
        // Act
        var largeItems = _service.FilterAllBy(i => i.Size == Size.L);
        // Assert
        Assert.NotEmpty(largeItems);
        Assert.Equal(7, largeItems.Count);
        Assert.All(largeItems, i => Assert.Equal(Size.L, i.Size));
        Assert.Contains(largeItems, i => i.Name == "Hoodie");
        Assert.Contains(largeItems, i => i.Name == "Classic Brown Loafers");
        Assert.Contains(largeItems, i => i.Name == "Dress Boots");
    }

    [Fact]
    public void FindFootwear_MaleOnly_ShouldReturn5Items()
    {
        // Act
        var maleFootwear = _service.FilterBy<Footwear>(f => f.Gender == Gender.Male);
        // Assert
        Assert.NotEmpty(maleFootwear);
        Assert.Equal(5, maleFootwear.Count);
        Assert.All(maleFootwear, f => Assert.Equal(Gender.Male, f.Gender));
        Assert.Contains(maleFootwear, f => f.Name == "Classic Brown Loafers" && f.Price == 129.99m);
        Assert.Contains(maleFootwear, f => f.Name == "Black Oxford Shoes" && f.Price == 149.99m);
    }

    [Fact]
    public void FindByQuantity_LowStock_ShouldReturnItemsUnder30()
    {
        // Act
        var lowStockItems = _service.FilterAllBy(i => i.Quantity < 30);
        // Assert
        Assert.NotEmpty(lowStockItems);
        Assert.All(lowStockItems, i => Assert.True(i.Quantity < 30));
        Assert.Contains(lowStockItems, i => i.Name == "Wedding White Heels" && i.Quantity == 15);
        Assert.Contains(lowStockItems, i => i.Name == "Classic Brown Loafers" && i.Quantity == 25);
        Assert.Contains(lowStockItems, i => i.Name == "Prom Glitter Pumps" && i.Quantity == 12);
    }
}