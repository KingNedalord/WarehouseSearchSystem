namespace Models;

/// <summary>
/// Types of Clothing.
/// </summary>
public enum ClothingType
{
    Top,
    Bottom,
    FullBody,
    Outerwear,
    Accessory,
}

/// <summary>
/// Model representing Clothing objects.
/// </summary>
public class Clothing : Item
{
    public ClothingType ClothingType { get; set;}

    public Clothing(string name, Size size, Gender gender, decimal price, int quantity, ClothingType clothingType)
        : base(name, size, gender, price, quantity)
    {
        if (!Enum.IsDefined(typeof(ClothingType), clothingType))
        {
            throw new ArgumentException($"Invalid footwear type: {clothingType}", nameof(clothingType));
        }

        this.ClothingType = clothingType;
    }
}
