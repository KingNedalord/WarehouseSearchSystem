/// <summary>
/// Factory class for creating and configuring DAO objects and data sources.
/// Implements the Singleton pattern and IItemDaoFactory interface.
/// </summary>
using Warehouse.Models;
namespace Warehouse.DAO;

public class ItemDaoFactory : IItemDaoFactory
{
    private static readonly ItemDaoFactory instance = new ItemDaoFactory();
    private readonly Dictionary<Type, object> configuredDaos = new();
    private readonly Dictionary<Type, object> configuredSources = new();

    /// <summary>
    /// Private constructor to enforce Singleton pattern.
    /// </summary>
    private ItemDaoFactory() { }

    /// <summary>
    /// Gets the single instance of ItemDaoFactory (Singleton pattern).
    /// </summary>
    public static ItemDaoFactory Instance => instance;

    /// <summary>
    /// Configures a DAO for a specific Item type.
    /// </summary>
    public void ConfigureItemDao<T>(IItemDao<T> dao) where T : Item
    {
        configuredDaos[typeof(T)] = dao;
    }

    /// <summary>
    /// Configures a data source for a specific Item type.
    /// </summary>
    public void ConfigureSource<T>(ISource<T> source) where T : Item
    {
        configuredSources[typeof(T)] = source;
    }

    /// <summary>
    /// Creates a DAO for a specific Item type.
    /// </summary>
    public IItemDao<T> CreateItemDao<T>() where T : Item
    {
        if (configuredDaos.TryGetValue(typeof(T), out var dao))
        {
            return (IItemDao<T>)dao;
        }

        var source = GetConfiguredSource<T>();
        return new ItemDao<T>(source);
    }

    private ISource<T> GetConfiguredSource<T>() where T : Item
    {
        if (!configuredSources.TryGetValue(typeof(T), out var stored))
            throw new InvalidOperationException($"No source configured for {typeof(T).Name}");

        var source = (ISource<T>)stored;
        // return a clone so caller can't mutate the stored prototype
        return source;
    }
}