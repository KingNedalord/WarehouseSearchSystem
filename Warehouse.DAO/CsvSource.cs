using System;

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
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}