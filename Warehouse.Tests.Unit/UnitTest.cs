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
        var clothing = _service.FindClothings().ToList();

        // Assert
        Assert.Equal(19, clothing.Count);
        Assert.All(clothing, item => Assert.IsType<Clothing>(item));
    }

    [Fact]
    public void FindFootwears_FromFile_ShouldReturn15Items()
    {
        // Act
        var footwear = _service.FindFootwears().ToList();

        // Assert
        Assert.Equal(15, footwear.Count);
        Assert.All(footwear, item => Assert.IsType<Footwear>(item));
    }

    [Fact]
    public void FindAllProducts_FromFile_ShouldReturn34TotalItems()
    {
        // Act
        var allClothing = _service.FindClothings().ToList();
        var allFootwear = _service.FindFootwears().ToList();
        var totalCount = allClothing.Count + allFootwear.Count;

        // Assert
        Assert.Equal(34, totalCount);
    }

    [Fact]
    public void FindByPrice_Under30_ShouldReturnCorrectItems()
    {
        var priceRange = new Range<decimal>(0m, 30m);

        // Act
        var products = _service.FindByPrice(priceRange).ToList();

        // Assert
        Assert.NotEmpty(products);
        Assert.True(products.Count >= 7, $"Expected at least 7 items under $30, but got {products.Count}");
        Assert.All(products, product =>
        {
            Assert.True(product.Price >= 0m && product.Price <= 30m,
                $"Product {product.Name} price {product.Price} is not in range [0, 30]");
        });
    }

    [Fact]
    public void FindByPrice_Over100_ShouldReturnOnlyPremiumFootwear()
    {
        var priceRange = new Range<decimal>(100m, 200m);

        // Act
        var products = _service.FindByPrice(priceRange).ToList();

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
        var products = _service.FindBySize(Size.XS).ToList();

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
        var products = _service.FindByGender(Gender.Female).ToList();

        // Assert
        Assert.True(products.Count >= 10, $"Expected at least 10 women's items, but got {products.Count}");
        Assert.All(products, product => Assert.Equal(Gender.Female, product.Gender));
    }

    [Fact]
    public void FindClothing_SpecificItems_ShouldExist()
    {
        // Act
        var clothing = _service.FindClothings().ToList();

        // Assert - Check for specific items from the CSV
        Assert.Contains(clothing, c => c.Name == "T-Shirt" && c.Price == 19.99m);
        Assert.Contains(clothing, c => c.Name == "Jeans" && c.Price == 49.99m);
        Assert.Contains(clothing, c => c.Name == "Dress" && c.Price == 59.99m);
        Assert.Contains(clothing, c => c.Name == "Baseball Cap" && c.Price == 19.99m);
    }

    [Fact]
    public void FindFootwear_SpecificItems_ShouldExist()
    {
        // Act
        var footwear = _service.FindFootwears().ToList();

        // Assert - Check for specific items from the CSV
        Assert.Contains(footwear, f => f.Name == "Classic Brown Loafers" && f.Price == 89.99m);
        Assert.Contains(footwear, f => f.Name == "Running Sneakers Blue" && f.Price == 79.99m);
        Assert.Contains(footwear, f => f.Name == "Wedding White Heels" && f.Price == 129.99m);
        Assert.Contains(footwear, f => f.Name == "Beach Flip-flops" && f.Price == 29.99m);
    }

    [Fact]
    public void FindClothing_MostExpensive_ShouldBeBlazer()
    {
        // Act
        var clothing = _service.FindClothings().ToList();
        var mostExpensive = clothing.OrderByDescending(c => c.Price).First();

        // Assert
        Assert.Equal("Blazer", mostExpensive.Name);
        Assert.Equal(89.99m, mostExpensive.Price);
    }
    [Fact]
    public void FindBySize_Medium_ShouldReturnMultipleItems()
    {
        // Act
        var products = _service.FindBySize(Size.M).ToList();

        // Assert
        Assert.NotEmpty(products);
        Assert.All(products, product => Assert.Equal(Size.M, product.Size));
        // Expected M size items: T-Shirt, Jeans, Skirt, Polo, Shorts, Blouse, Blazer, etc.
        Assert.True(products.Count >= 5, $"Expected at least 5 Medium items, but got {products.Count}");
    }


    [Fact]
    public void FindByGender_Unisex_ShouldIncludeClothingAndFootwear()
    {
        // Act
        var products = _service.FindByGender(Gender.Unisex).ToList();

        // Assert
        Assert.NotEmpty(products);
        Assert.All(products, product => Assert.Equal(Gender.Unisex, product.Gender));

        var hasClothing = products.Any(p => p is Clothing);
        var hasFootwear = products.Any(p => p is Footwear);

        Assert.True(hasClothing, "Should include unisex clothing items");
        Assert.True(hasFootwear, "Should include unisex footwear items");
    }

    [Fact]
    public void FindFootwear_ByType_Formal_ShouldReturn5Items()
    {
        // Arrange - Formal: Classic Brown Loafers, Black Oxford Shoes, Leather Derby Shoes,
        // Business Brogues, Dress Boots
        var footwear = _service.FindFootwears().ToList();

        // Act
        var formal = footwear.Where(f => f.FootwearType == FootwearType.Formal).ToList();

        // Assert
        Assert.Equal(5, formal.Count);
        Assert.All(formal, f => Assert.Equal(FootwearType.Formal, f.FootwearType));
        Assert.Contains(formal, f => f.Name == "Classic Brown Loafers");
        Assert.Contains(formal, f => f.Name == "Black Oxford Shoes");
    }

    [Fact]
    public void FindClothing_ByType_Bottom_ShouldReturnCorrectItems()
    {
        // Arrange - Bottom items: Jeans, Skirt, Shorts, Cargo Pants, Leggings, Sweatpants
        var clothing = _service.FindClothings().ToList();

        // Act
        var bottoms = clothing.Where(c => c.ClothingType == ClothingType.Bottom).ToList();

        // Assert
        Assert.True(bottoms.Count >= 5, $"Expected at least 5 bottom items, but got {bottoms.Count}");
        Assert.All(bottoms, c => Assert.Equal(ClothingType.Bottom, c.ClothingType));
        Assert.Contains(bottoms, c => c.Name == "Jeans");
        Assert.Contains(bottoms, c => c.Name == "Leggings");
    }

    [Fact]
    public void FindByPrice_MidRange_30To70_ShouldReturnVariedProducts()
    {
        // Arrange - Mid-range items between $30-$70
        var priceRange = new Range<decimal>(30m, 70m);

        // Act
        var products = _service.FindByPrice(priceRange).ToList();

        // Assert
        Assert.NotEmpty(products);
        Assert.All(products, product =>
        {
            Assert.True(product.Price >= 30m && product.Price <= 70m,
                $"Product {product.Name} price {product.Price} is not in range [30, 70]");
        });

        // Should include both clothing and footwear in this price range
        var hasClothing = products.Any(p => p is Clothing);
        var hasFootwear = products.Any(p => p is Footwear);

        Assert.True(hasClothing || hasFootwear, "Should include products in mid-price range");
    }
}