using Warehouse.DAO;
using Warehouse.Models;
using Warehouse.Services;
using Xunit;
using System.Collections;

namespace Warehouse.Tests.Unit;

public class TestServiceBuilder
{
    private List<Clothing> _clothing;
    private List<Footwear> _footwear;

    public TestServiceBuilder()
    {
        _clothing = new List<Clothing>();
        _footwear = new List<Footwear>();
    }

    public TestServiceBuilder WithClothing(List<Clothing> clothing)
    {
        _clothing = clothing;
        return this;
    }

    public TestServiceBuilder WithFootwear(List<Footwear> footwear)
    {
        _footwear = footwear;
        return this;
    }

    public TestServiceBuilder WithDefaultClothing()
    {
        _clothing = TestDataFactory.CreateTestClothing();
        return this;
    }

    public TestServiceBuilder WithDefaultFootwear()
    {
        _footwear = TestDataFactory.CreateTestFootwear();
        return this;
    }

    public ItemService Build()
    {
        var clothingDao = new ItemDao<Clothing>(new InMemorySource<Clothing>(_clothing));
        var footwearDao = new ItemDao<Footwear>(new InMemorySource<Footwear>(_footwear));
        var daoFactory = new InMemoryDaoFactory(clothingDao, footwearDao);
        return new ItemService(daoFactory);
    }

    /// <summary>
    /// A test-specific in-memory data source for unit testing DAOs.
    /// </summary>
    public class InMemorySource<T> : ISource<T>, IEnumerable<T> where T : Item
    {
        private readonly IEnumerable<T> _data;
        // private readonly bool _useInMemoryData;
        // public bool UseInMemoryData { get { return _useInMemoryData; } }

        public bool IsInMemory { get; }

        public InMemorySource(IEnumerable<T>? data = null)
        {
            _data = data ?? new List<T>();
            IsInMemory = data != null;
        }
        public IEnumerator<T> GetEnumerator() => _data.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // Minimal implementations to satisfy the ISource contract
        public void Init()
        {
            if (!IsInMemory && !File.Exists(FilePath()))
            {
                throw new FileNotFoundException($"CSV file not found at {FilePath()}");
            }
        }
        public string FilePath() => Path.Combine(
          AppDomain.CurrentDomain.BaseDirectory,
          "Data",
          typeof(T) == typeof(Clothing) ? "clothing.csv" : "footwear.csv");
        public object Clone() => new InMemorySource<T>(_data);

        public T Parse(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
                throw new ArgumentException("Line cannot be empty or whitespace.");

            var columns = line.Split(',');
            if (columns.Length < 7)
                throw new FormatException($"Invalid CSV format: expected 7 columns, got {columns.Length}");

            try
            {
                if (typeof(T) == typeof(Clothing))
                {
                    return (T)(object)new Clothing(
                        id: int.Parse(columns[0]),
                        name: columns[1],
                        size: Enum.Parse<Size>(columns[2], true),
                        gender: Enum.Parse<Gender>(columns[3], true),
                        price: decimal.Parse(columns[4]),
                        quantity: int.Parse(columns[5]),
                        clothingType: Enum.Parse<ClothingType>(columns[6], true)
                    );
                }
                if (typeof(T) == typeof(Footwear))
                {
                    return (T)(object)new Footwear(
                        id: int.Parse(columns[0]),
                        name: columns[1],
                        size: Enum.Parse<Size>(columns[2], true),
                        gender: Enum.Parse<Gender>(columns[3], true),
                        price: decimal.Parse(columns[4]),
                        quantity: int.Parse(columns[5]),
                        footwearType: Enum.Parse<FootwearType>(columns[6], true)
                    );
                }
                throw new NotSupportedException($"Type {typeof(T).Name} is not supported.");
            }
            catch (Exception ex)
            {
                throw new FormatException($"Failed to parse CSV line: {line}", ex);
            }
        }
    }

    // TestInMemoryDependencies.cs (continued)

    /// <summary>
    /// A factory for unit tests that provides the configured InMemory DAOs.
    /// </summary>
    public class InMemoryDaoFactory : IItemDaoFactory
    {
        private readonly IItemDao<Clothing> _clothingDao;
        private readonly IItemDao<Footwear> _footwearDao;

        // Constructor matches the dependencies created in TestServiceBuilder.Build()
        public InMemoryDaoFactory(IItemDao<Clothing> clothingDao, IItemDao<Footwear> footwearDao)
        {
            _clothingDao = clothingDao;
            _footwearDao = footwearDao;
        }

        public IItemDao<T> CreateItemDao<T>() where T : Item
        {
            if (typeof(T) == typeof(Clothing))
                return (IItemDao<T>)_clothingDao;

            if (typeof(T) == typeof(Footwear))
                return (IItemDao<T>)_footwearDao;

            throw new NotSupportedException($"Type {typeof(T).Name} is not supported by this InMemoryDaoFactory.");
        }

        // Minimal implementation for the factory contract
        public void ConfigureItemDao<T>(IItemDao<T> dao) where T : Item { }

        public void ConfigureSource<T>(ISource<T> source) where T : Item
        {
            throw new NotImplementedException();
        }
    }
}

// TestDataFactory.cs
// Centralized test data creation
public static class TestDataFactory
{
    public static List<Clothing> CreateTestClothing()
    {
        return new List<Clothing>
        {
            new Clothing(1, "T-Shirt", Size.M, Gender.M, 15000.00m, 50, ClothingType.Top),
            new Clothing(2, "Jeans", Size.L, Gender.F, 35000.00m, 30, ClothingType.Bottom),
            new Clothing(3, "Dress", Size.S, Gender.F, 45000.00m, 20, ClothingType.Fullbody),
            new Clothing(4, "Jacket", Size.XL, Gender.M, 65000.00m, 15, ClothingType.Outerwear),
            new Clothing(5, "Sweater", Size.M, Gender.U, 25000.00m, 40, ClothingType.Top)
        };
    }

    public static List<Footwear> CreateTestFootwear()
    {
        return new List<Footwear>
        {
            new Footwear(1, "Running Shoes", Size.M, Gender.M, 55000.00m, 25, FootwearType.Casual),
            new Footwear(2, "Boots", Size.L, Gender.F, 75000.00m, 15, FootwearType.Special),
            new Footwear(3, "Sandals", Size.S, Gender.U, 20000.00m, 50, FootwearType.Casual),
            new Footwear(4, "Formal Shoes", Size.M, Gender.M, 85000.00m, 10, FootwearType.Formal),
            new Footwear(5, "Slippers", Size.L, Gender.F, 12000.00m, 60, FootwearType.Casual)
        };
    }

    public static Clothing CreateClothing(
        int id = 1,
        string name = "Test Clothing",
        Size size = Size.M,
        Gender gender = Gender.U,
        decimal price = 30000.00m,
        int quantity = 20,
        ClothingType clothingType = ClothingType.Top)
    {
        return new Clothing(id, name, size, gender, price, quantity, clothingType);
    }

    public static Footwear CreateFootwear(
        int id = 1,
        string name = "Test Footwear",
        Size size = Size.M,
        Gender gender = Gender.U,
        decimal price = 40000.00m,
        int quantity = 15,
        FootwearType footwearType = FootwearType.Casual)
    {
        return new Footwear(id, name, size, gender, price, quantity, footwearType);
    }

    public static List<Clothing> CreateClothingWithPrices(params decimal[] prices)
    {
        return prices.Select((price, index) =>
            CreateClothing(id: index + 1, price: price)).ToList();
    }

    public static List<Footwear> CreateFootwearWithPrices(params decimal[] prices)
    {
        return prices.Select((price, index) =>
            CreateFootwear(id: index + 1, price: price)).ToList();
    }

    public static List<Clothing> CreateClothingBySizes(params Size[] sizes)
    {
        return sizes.Select((size, index) =>
            CreateClothing(id: index + 1, size: size)).ToList();
    }

    public static List<Footwear> CreateFootwearBySizes(params Size[] sizes)
    {
        return sizes.Select((size, index) =>
            CreateFootwear(id: index + 1, size: size)).ToList();
    }

    public static List<Clothing> CreateClothingByGender(Gender gender, int count = 5)
    {
        return Enumerable.Range(1, count)
            .Select(i => CreateClothing(id: i, gender: gender))
            .ToList();
    }

    public static List<Footwear> CreateFootwearByGender(Gender gender, int count = 5)
    {
        return Enumerable.Range(1, count)
            .Select(i => CreateFootwear(id: i, gender: gender))
            .ToList();
    }
}

// ProductAssertions.cs
// Custom assertions for cleaner test code
public static class ProductAssertions
{
    public static void AssertPriceInRange(this IEnumerable<Item> products, decimal min, decimal max)
    {
        Assert.All(products, product =>
        {
            Assert.True(product.Price >= min && product.Price <= max,
                $"Product {product.Name} price {product.Price} is not in range [{min}, {max}]");
        });
    }

    public static void AssertAllOfType<T>(this IEnumerable<T> products) where T : Item
    {
        Assert.All(products, product => Assert.IsType<T>(product));
    }

    public static void AssertCount(this IEnumerable<Item> products, int expectedCount)
    {
        Assert.Equal(expectedCount, products.Count());
    }

    public static void AssertEmpty(this IEnumerable<Item> products)
    {
        Assert.Empty(products);
    }

    public static void AssertNotEmpty(this IEnumerable<Item> products)
    {
        Assert.NotEmpty(products);
    }

    public static void AssertAllSize(this IEnumerable<Item> products, Size expectedSize)
    {
        Assert.All(products, product =>
        {
            Assert.Equal(expectedSize, product.Size);
        });
    }

    public static void AssertAllGender(this IEnumerable<Item> products, Gender expectedGender)
    {
        Assert.All(products, product =>
        {
            Assert.Equal(expectedGender, product.Gender);
        });
    }

    public static void AssertQuantityInRange(this IEnumerable<Item> products, int min, int max)
    {
        Assert.All(products, product =>
        {
            Assert.True(product.Quantity >= min && product.Quantity <= max,
                $"Product {product.Name} quantity {product.Quantity} is not in range [{min}, {max}]");
        });
    }
}

// RangeExtensions.cs
// Helper methods for creating ranges
public static class RangeExtensions
{
    public static Range<decimal> ToPriceRange(this (decimal min, decimal max) tuple)
    {
        return new Range<decimal>(tuple.min, tuple.max);
    }

    public static Range<decimal> LowPriceRange() => new Range<decimal>(0m, 25000m);
    public static Range<decimal> MidPriceRange() => new Range<decimal>(25000m, 60000m);
    public static Range<decimal> HighPriceRange() => new Range<decimal>(60000m, 100000m);
    public static Range<decimal> VeryHighPriceRange() => new Range<decimal>(100000m, decimal.MaxValue);

    public static Range<int> ToQuantityRange(this (int min, int max) tuple)
    {
        return new Range<int>(tuple.min, tuple.max);
    }

    public static Range<int> LowStockRange() => new Range<int>(0, 10);
    public static Range<int> MediumStockRange() => new Range<int>(10, 30);
    public static Range<int> HighStockRange() => new Range<int>(30, 100);
}

// ServiceTestFixture.cs
// Reusable test fixture for xUnit
public class ServiceTestFixture : IDisposable
{
    public List<Clothing> TestClothing { get; }
    public List<Footwear> TestFootwear { get; }
    public ItemService Service { get; }

    public ServiceTestFixture()
    {
        TestClothing = TestDataFactory.CreateTestClothing();
        TestFootwear = TestDataFactory.CreateTestFootwear();
        Service = new TestServiceBuilder()
            .WithClothing(TestClothing)
            .WithFootwear(TestFootwear)
            .Build();
    }

    public void Dispose()
    {
        // Cleanup if needed
    }
}

// Updated ServiceTests.cs showing usage
public class ServiceTests : IClassFixture<ServiceTestFixture>
{
    private readonly ServiceTestFixture _fixture;

    public ServiceTests(ServiceTestFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void FindClothing_ShouldReturnAllClothing()
    {
        // Arrange - using fixture
        var service = _fixture.Service;

        // Act
        var clothing = service.FindClothings();

        // Assert - using custom assertions
        clothing.AssertCount(_fixture.TestClothing.Count);
        clothing.AssertAllOfType<Clothing>();
    }

    [Fact]
    public void FindFootwear_ShouldReturnAllFootwear()
    {
        // Arrange
        var service = _fixture.Service;

        // Act
        var footwear = service.FindFootwears();

        // Assert
        footwear.AssertCount(_fixture.TestFootwear.Count);
        footwear.AssertAllOfType<Footwear>();
    }

    [Fact]
    public void FindByPrice_MidRange_ShouldReturnCorrectProducts()
    {
        // Arrange - using builder and range extensions
        var service = new TestServiceBuilder()
            .WithDefaultClothing()
            .WithDefaultFootwear()
            .Build();

        // Act
        var products = service.FindByPrice(RangeExtensions.MidPriceRange());

        // Assert - using custom assertions
        products.AssertNotEmpty();
        products.AssertPriceInRange(25000m, 60000m);
    }

    [Theory]
    [InlineData(0, 25000)]
    [InlineData(25000, 60000)]
    [InlineData(60000, 100000)]
    public void FindByPrice_VariousRanges_ShouldReturnCorrectProducts(decimal min, decimal max)
    {
        // Arrange
        var service = new TestServiceBuilder()
            .WithDefaultClothing()
            .WithDefaultFootwear()
            .Build();
        var priceRange = (min, max).ToPriceRange();

        // Act
        var products = service.FindByPrice(priceRange);

        // Assert
        products.AssertPriceInRange(min, max);
    }

    [Fact]
    public void FindBySize_ShouldReturnCorrectProducts()
    {
        // Arrange
        var service = new TestServiceBuilder()
            .WithDefaultClothing()
            .WithDefaultFootwear()
            .Build();

        // Act
        var mediumProducts = service.FindBySize(Size.M);

        // Assert
        mediumProducts.AssertNotEmpty();
        mediumProducts.AssertAllSize(Size.M);
    }

    [Fact]
    public void FindByGender_ShouldReturnCorrectProducts()
    {
        // Arrange
        var service = new TestServiceBuilder()
            .WithDefaultClothing()
            .WithDefaultFootwear()
            .Build();

        // Act
        var MProducts = service.FindByGender(Gender.M);

        // Assert
        MProducts.AssertNotEmpty();
        MProducts.AssertAllGender(Gender.M);
    }

    [Fact]
    public void FindByPrice_CustomData_ShouldWork()
    {
        // Arrange - using factory methods for custom test data
        var clothing = TestDataFactory.CreateClothingWithPrices(10000m, 50000m, 90000m);
        var footwear = TestDataFactory.CreateFootwearWithPrices(20000m, 60000m, 100000m);

        var service = new TestServiceBuilder()
            .WithClothing(clothing)
            .WithFootwear(footwear)
            .Build();

        // Act
        var cheapProducts = service.FindByPrice((0m, 40000m).ToPriceRange());

        // Assert
        cheapProducts.AssertCount(2);
    }
}

// ClothingBuilder.cs
// Fluent builder for individual test items
public class ClothingBuilder
{
    private int _id = 1;
    private string _name = "Test Clothing";
    private Size _size = Size.M;
    private Gender _gender = Gender.U;
    private decimal _price = 30000.00m;
    private int _quantity = 20;
    private ClothingType _clothingType = ClothingType.Top;

    public ClothingBuilder WithId(int id) { _id = id; return this; }
    public ClothingBuilder WithName(string name) { _name = name; return this; }
    public ClothingBuilder WithSize(Size size) { _size = size; return this; }
    public ClothingBuilder WithGender(Gender gender) { _gender = gender; return this; }
    public ClothingBuilder WithPrice(decimal price) { _price = price; return this; }
    public ClothingBuilder WithQuantity(int quantity) { _quantity = quantity; return this; }
    public ClothingBuilder WithClothingType(ClothingType clothingType) { _clothingType = clothingType; return this; }

    public Clothing Build() => new Clothing(_id, _name, _size, _gender, _price, _quantity, _clothingType);
}

// FootwearBuilder.cs
public class FootwearBuilder
{
    private int _id = 1;
    private string _name = "Test Footwear";
    private Size _size = Size.M;
    private Gender _gender = Gender.U;
    private decimal _price = 40000.00m;
    private int _quantity = 15;
    private FootwearType _footwearType = FootwearType.Casual;

    public FootwearBuilder WithId(int id) { _id = id; return this; }
    public FootwearBuilder WithName(string name) { _name = name; return this; }
    public FootwearBuilder WithSize(Size size) { _size = size; return this; }
    public FootwearBuilder WithGender(Gender gender) { _gender = gender; return this; }
    public FootwearBuilder WithPrice(decimal price) { _price = price; return this; }
    public FootwearBuilder WithQuantity(int quantity) { _quantity = quantity; return this; }
    public FootwearBuilder WithFootwearType(FootwearType footwearType) { _footwearType = footwearType; return this; }

    public Footwear Build() => new Footwear(_id, _name, _size, _gender, _price, _quantity, _footwearType);
}