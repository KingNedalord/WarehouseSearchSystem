namespace Warehouse.Models;

/// <summary>
/// Abstract base class for all clothing in the system
/// </summary>
/// 
/// <summary>
/// Size categories for clothing items.
/// </summary>
public enum Size
{
    XS,
    S,
    M,
    L,
    XL,
    XXL
}
/// <summary>
/// Gender categories for clothing items.
/// </summary>
public enum Gender
{
    M,
    F,
    U
}
public abstract class Item
{
    private readonly int id;
    private readonly string name;
    private readonly Size size;
    private readonly Gender gender;
    private readonly decimal price;
    private readonly int quantity;

    /// <summary>
    /// Gets the id of the Clothing
    /// </summary>
    public int Id => id;

    /// <summary>
    /// Gets the name of the Clothing
    /// </summary>
    public string Name => name;

    /// <summary>
    /// Gets the size of the Clothing from the ClothingSize enum
    /// </summary>
    public Size Size => size;

    /// <summary>
    /// Gets the gender of the Clothing from the Gender enum
    /// </summary>
    public Gender Gender => gender;

    /// <summary>
    /// Gets the price of the Clothing in USD
    /// </summary>
    public decimal Price => price;

    /// <summary>
    /// Gets the inventory number of the Clothing
    /// </summary>
    public int Quantity => quantity;

    /// <summary>
    /// Creates a new Clothing with the specified properties
    /// </summary>
    /// <param name="id">Id of the Clothing</param>
    /// <param name="name">Name of the Clothing</param>
    /// <param name="size">Size in kilograms</param>
    /// <param name="gender">Gender category</param>
    /// <param name="price">Price in USD</param>
    /// <param name="quantity">Inventory quantity</param>
    /// <exception cref="ArgumentException">When parameters are invalid</exception>
    protected Item(int id, string name, Size size, Gender gender, decimal price, int quantity)
    {
        this.id = id > 0
            ? id
            : throw new ArgumentException("Id must be positive", nameof(id));

        this.name = !string.IsNullOrWhiteSpace(name)
            ? name
            : throw new ArgumentException("Name cannot be null, empty or whitespace", nameof(name));

        this.size = size;
        this.gender = gender;
        this.price = price > 0
            ? price
            : throw new ArgumentException("Price must be positive", nameof(price));

        this.quantity = quantity > 0
            ? quantity
            : throw new ArgumentException("quantity must be positive", nameof(quantity));
    }

    /// <summary>
    /// Returns a string representation of the Clothing
    /// </summary>
    /// <returns>String representation</returns>
    public override string ToString()
    {
        return $"{GetType().Name}{{name={Name}, size={Size}, gender = {Gender}, price={Price}, quantity={Quantity}}}";
    }
}