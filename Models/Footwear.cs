namespace Models;

/// <summary>
/// Types of Footwear
/// </summary>
public enum FootwearType
{
    Casual,
    Formal,
    Special,
}

/// <summary>
/// Model representing Footwear objects.
/// </summary>
public class Footwear : Item
{
    public FootwearType FootwearType { get; set;}

    public Footwear(string name, Size size, Gender gender, decimal price, int quantity, FootwearType footwearType)
        : base(name, size, gender, price, quantity)
    {
        if (!Enum.IsDefined(typeof(FootwearType), footwearType))
        {
            throw new ArgumentException($"Invalid footwear type: {footwearType}", nameof(footwearType));
        }

        this.FootwearType = footwearType;
    }
}
