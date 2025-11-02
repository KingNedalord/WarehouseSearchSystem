using System;

public enum FootwearType
{
    Casual,
    Formal,
    Special,
}

public class Footwear : Item
{
    private readonly FootwearType footwearType;

    public FootwearType FootwearType => footwearType;
    public Footwear(int id, string name, Size size, Gender gender, decimal price, int quantity, FootwearType footwearType)
        : base(id, name, size, gender, price, quantity)
    {
        if (!Enum.IsDefined(typeof(FootwearType), footwearType))
        {
            throw new ArgumentException($"Invalid footwear type: {footwearType}", nameof(footwearType));
        }
        this.footwearType = footwearType;
    }
}