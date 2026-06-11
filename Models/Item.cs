namespace Models;

/// <summary>
/// Abstract base class for all clothing in the system
/// </summary>
///
/// <summary>
/// Size categories for clothing items.
/// </summary>
public enum Size
{
    Xs,
    S,
    M,
    L,
    Xl,
    Xxl
}
/// <summary>
/// Gender categories for clothing items.
/// </summary>
public enum Gender
{
    Male,
    Female,
    Unisex
}
public abstract class Item
{
    /// <summary>
    /// Gets the id of the Clothing
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets the name of the Clothing
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets the size of the Clothing from the ClothingSize enum
    /// </summary>
    public Size Size { get; set; }

    /// <summary>
    /// Gets the gender of the Clothing from the Gender enum
    /// </summary>
    public Gender Gender { get; set;}

    /// <summary>
    /// Gets the price of the Clothing in USD
    /// </summary>
    public decimal Price { get; set;}

    /// <summary>
    /// Gets the quantity of the Item
    /// </summary>
    public int Quantity { get; set;}

    /// <summary>
    /// Creates a new Clothing with the specified properties
    /// </summary>
    /// <param name="name">Name of the Clothing</param>
    /// <param name="size">Size in kilograms</param>
    /// <param name="gender">Gender category</param>
    /// <param name="price">Price in USD</param>
    /// <param name="quantity">Inventory quantity</param>
    /// <exception cref="ArgumentException">When parameters are invalid</exception>
    protected Item(string name, Size size, Gender gender, decimal price, int quantity)
    {
        this.Name = !string.IsNullOrWhiteSpace(name)
            ? name
            : throw new ArgumentException("Name cannot be null, empty or whitespace", nameof(name));

        this.Size = size;
        this.Gender = gender;
        this.Price = price > 0
            ? price
            : throw new ArgumentException("Price must be positive", nameof(price));

        this.Quantity = quantity > 0
            ? quantity
            : throw new ArgumentException("quantity must be positive", nameof(quantity));
    }

    /// <summary>
    /// Returns a string representation of the Clothing
    /// </summary>
    /// <returns>String representation</returns>
    public override string ToString()
    {
        return $"{this.GetType().Name} - {{id={this.Id}, name={this.Name}, size={this.Size}, gender = {this.Gender}, price={this.Price}, quantity={this.Quantity}}}";
    }
}
