using System.Globalization;
using System.Text;
using Warehouse.Models;

namespace Warehouse.DAO;

/// <summary>
/// Generic CSV source for Clothing and Footwear (and similar items)
/// </summary>
/// <typeparam name="TItem">The item type (e.g., Clothing, Footwear)</typeparam>
/// <typeparam name="TEnum">The specific enum type for the item category (e.g., ClothingType, FootwearType)</typeparam>
public abstract class GenericCsvSource<TItem, TEnum> : ISource<TItem>
    where TItem : Item
    where TEnum : struct, Enum
{
    private readonly string filePath;

    protected GenericCsvSource(string filePath)
    {
        this.filePath = filePath;
    }

    public string FilePath() => filePath;

    protected static string[] SplitCsvLine(string line)
    {
        if (string.IsNullOrEmpty(line)) return Array.Empty<string>();
        var fields = new List<string>();
        var sb = new StringBuilder();
        for (int i = 0; i < line.Length; i++)
        {
            var c = line[i];
            if (c == ',')
            {
                fields.Add(sb.ToString());
                sb.Clear();
            }
            else
            {
                sb.Append(c);
            }
        }
        fields.Add(sb.ToString());
        return fields.ConvertAll(f => f.Trim()).ToArray();
    }

    /// <summary>
    /// Parses common fields: Id, Name, Size, Gender, Price, Quantity, and specific enum
    /// </summary>
    protected (int id, string name, Size size, Gender gender, decimal price, int quantity, TEnum type) ParseCommon(string line)
    {
        var f = SplitCsvLine(line);
        if (f.Length < 7) throw new FormatException("Invalid CSV line: " + line);

        if (!int.TryParse(f[0], out var id))
            throw new FormatException("Invalid Id: " + f[0]);

        var name = f[1];
        var sizeText = f[2].Replace(" ", "");
        if (!Enum.TryParse<Size>(sizeText, true, out var size))
            throw new FormatException("Invalid Size: " + f[2]);

        if (!Enum.TryParse<Gender>(f[3], true, out var gender))
            throw new FormatException("Invalid Gender: " + f[3]);

        if (!decimal.TryParse(f[4], NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            throw new FormatException("Invalid Price: " + f[4]);

        if (!int.TryParse(f[5], out var quantity))
            throw new FormatException("Invalid Quantity: " + f[5]);

        if (!Enum.TryParse<TEnum>(f[6], true, out var type))
            throw new FormatException($"Invalid Type: {f[6]}");

        return (id, name, size, gender, price, quantity, type);
    }

    public abstract TItem Parse(string line);
}

public class ClothingCsvSource : GenericCsvSource<Clothing, ClothingType>
{
    public ClothingCsvSource(string filePath) : base(filePath) { }

    public override Clothing Parse(string line)
    {
        var (id, name, size, gender, price, quantity, type) = ParseCommon(line);
        return new Clothing(id, name, size, gender, price, quantity, type);
    }
}

public class FootwearCsvSource : GenericCsvSource<Footwear, FootwearType>
{
    public FootwearCsvSource(string filePath) : base(filePath) { }

    public override Footwear Parse(string line)
    {
        var (id, name, size, gender, price, quantity, type) = ParseCommon(line);
        return new Footwear(id, name, size, gender, price, quantity, type);
    }
}