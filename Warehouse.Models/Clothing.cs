using System;

public enum ClothingType
{
    Top,
    Bottom,
    Fullbody,
    Outerwear,
    Accessory,
}

public class Clothing : Item
{
    private readonly ClothingType clothingType;

    public ClothingType ClothingType => clothingType;
    public Clothing(int id, string name, Size size, Gender gender, decimal price, int quantity, ClothingType clothingType)
        : base(id, name, size, gender, price, quantity)
    {
        if (!Enum.IsDefined(typeof(ClothingType), clothingType))
        {
            throw new ArgumentException($"Invalid footwear type: {clothingType}", nameof(clothingType));
        }
        this.clothingType = clothingType;
    }
}