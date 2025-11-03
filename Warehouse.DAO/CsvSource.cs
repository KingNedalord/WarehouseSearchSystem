using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

/// <summary>
/// Abstract base class for CSV-based data sources in Straight implementation
/// </summary>
/// <typeparam name="T">Type of Item to load from CSV</typeparam>
public abstract class AbstractCsvSource<T> : ISource<T> where T : Item
{
    /// <summary>
    /// Name of the CSV file
    /// </summary>
    private readonly string filePath;

    /// <summary>
    /// Creates a new CSV source with the specified file name
    /// </summary>
    /// <param name="filePath">Path to the CSV file</param>
    protected AbstractCsvSource(string filePath)
    {
        this.filePath = filePath;
    }

    /// <summary>
    /// Gets the name of the CSV file
    /// </summary>
    /// <returns>CSV file name</returns>
    public string FilePath() => filePath;

    public virtual object Clone()
    {
        return this.MemberwiseClone();
    }


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
    /// Splits a CSV line by delimiter
    /// </summary>
    /// <param name="line">CSV line to split</param>
    /// <returns>Array of values</returns>
    protected string[] GetSplitted(string line)
    {
        const string delimiter = ";";
        return line.Split(delimiter);
    }

    /// <summary>
    /// Parses a CSV line into an Item object
    /// </summary>
    /// <param name="line">CSV line to parse</param>
    /// <returns>Parsed Item object</returns>
    public abstract T Parse(string line);
}


/// <summary>
/// CSV data source for Clothings in Straight implementation
/// </summary>
public class ClothingCsvSource : AbstractCsvSource<Clothing>
{
    /// <summary>
    /// Creates a new ClothingCsvSource that reads from the specified file
    /// </summary>
    /// <param name="csvFileName">Name of the CSV file</param>
    public ClothingCsvSource(string csvFileName) : base(csvFileName) { }

    /// <summary>
    /// Parses a CSV line into a Clothing object
    /// </summary>
    /// <param name="line">CSV line to parse</param>
    /// <returns>Parsed Clothing object</returns>
    public override Clothing Parse(string line)
    {
        // Id,Name,Size,Gender,Price,Quantity,Type
        var f = SplitCsvLine(line);
        if (f.Length < 7) throw new FormatException("Invalid clothing CSV line: " + line);

        if (!int.TryParse(f[0], out var id)) throw new FormatException("Invalid Id: " + f[0]);
        var name = f[1];
        var sizeText = f[2];

        // requires a method in your models: Size.Parse(string) -> Size
        var size = Enum.Parse<Size>(sizeText.Replace(" ", ""), true); // Convert "One Size" to "OneSize" and parse case-insensitive

        if (!Enum.TryParse<Gender>(f[3], true, out var gender)) throw new FormatException("Invalid Gender: " + f[3]);

        if (!decimal.TryParse(f[4], NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            throw new FormatException("Invalid Price: " + f[4]);

        if (!int.TryParse(f[5], out var qty)) throw new FormatException("Invalid Quantity: " + f[5]);

        if (!Enum.TryParse<ClothingType>(f[6], true, out var clothingType))
            throw new FormatException("Invalid Clothing Type: " + f[6]);

        return new Clothing(id, name, size, gender, price, qty, clothingType);
    }
}


/// <summary>
/// CSV data source for loading Footwear data from Footwears.csv file
/// </summary>
public class FootwearCsvSource : AbstractCsvSource<Footwear>
{
    /// <summary>
    /// Creates a new FootwearCsvSource that reads from the specified file
    /// </summary>
    /// <param name="filePath">Path to the CSV file</param>
    public FootwearCsvSource(string filePath) : base(filePath) { }

    /// <summary>
    /// Parses CSV line into Footwear object
    /// </summary>
    /// <param name="line">CSV line to parse</param>
    /// <returns>Parsed Footwear object</returns>
    /// <exception cref="ArgumentException">When line format is invalid</exception>
    public override Footwear Parse(string line)
    {
        // Id,Name,Size,Gender,Price,Quantity,Type
        var f = SplitCsvLine(line);
        if (f.Length < 7) throw new FormatException("Invalid clothing CSV line: " + line);

        if (!int.TryParse(f[0], out var id)) throw new FormatException("Invalid Id: " + f[0]);
        var name = f[1];
        var sizeText = f[2];

        // requires a method in your models: Size.Parse(string) -> Size
        var size = Enum.Parse<Size>(sizeText.Replace(" ", ""), true); // Convert "One Size" to "OneSize" and parse case-insensitive

        if (!Enum.TryParse<Gender>(f[3], true, out var gender)) throw new FormatException("Invalid Gender: " + f[3]);

        if (!decimal.TryParse(f[4], NumberStyles.Number, CultureInfo.InvariantCulture, out var price))
            throw new FormatException("Invalid Price: " + f[4]);

        if (!int.TryParse(f[5], out var qty)) throw new FormatException("Invalid Quantity: " + f[5]);

        if (!Enum.TryParse<FootwearType>(f[6], true, out var footwearType))
            throw new FormatException("Invalid Clothing Type: " + f[6]);

        return new Footwear(id, name, size, gender, price, qty, footwearType);
    }
}