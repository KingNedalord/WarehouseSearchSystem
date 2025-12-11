using Warehouse.Models;
namespace Warehouse.DAO;
/// <summary>
/// Generic DAO implementation for Straight approach that uses a data source to load and filter Items
/// </summary>
/// <typeparam name="T">Type of Item to handle</typeparam>
public class ItemDao<T> : IItemDao<T> where T : Item
{
    private readonly ISource<T> source;

    /// <summary>
    /// Creates a new ItemDao with the specified data source
    /// </summary>
    /// <param name="source">Data source to use for parsing Items</param>
    public ItemDao(ISource<T> source)
    {
        this.source = source ?? throw new ArgumentNullException(nameof(source));
    }

    /// <summary>
    /// Finds Items matching the specified predicate
    /// </summary>
    /// <param name="predicate">Predicate to filter Items</param>
    /// <returns>List of matching Items</returns>
    public IList<T> Find(Predicate<T> predicate)
    {
        ArgumentNullException.ThrowIfNull(predicate);
        return [.. FindAll().Where(i => predicate(i))];
    }

    /// <summary>
    /// Finds all Items of the specified type
    /// </summary>
    /// <returns>List of all Items</returns>
    public IList<T> FindAll()
    {
        return [.. EnumerateAll()];
    }

    private IEnumerable<T> EnumerateAll()
    {
        var path = GetPath();
        // 'using' here to ensure the StreamReader is disposed properly
        using var sr = new StreamReader(path);
        string? line;
        bool first = true;
        int lineNumber = 0;

        while ((line = sr.ReadLine()) != null)
        {
            lineNumber++;
            if (first)
            {
                first = false;
                continue;
            }

            if (string.IsNullOrWhiteSpace(line)) continue;

            T item;
            try
            {
                item = source.Parse(line);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[ItemDao] Failed to parse line {lineNumber} in '{path}': {ex.Message}");
                continue;
            }

            yield return item;
        }
    }

    /// <summary>
    /// Gets the path to the CSV file
    /// </summary>
    /// <returns>Path to the CSV file</returns>
    /// <exception cref="DaoException">When file is not found</exception>
    private string GetPath()
    {
        var path = source.FilePath();
        if (string.IsNullOrWhiteSpace(path))
            throw new DaoException("Source returned empty file path.");

        if (!Path.IsPathRooted(path))
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);

        if (!File.Exists(path))
            throw new DaoException($"Data file not found: {path}");

        return path;
    }
}