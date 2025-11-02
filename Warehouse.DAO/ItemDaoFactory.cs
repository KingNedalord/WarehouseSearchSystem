using System;
using System.Collections.Generic;

/// <summary>
/// Factory class for creating and configuring DAO objects and data sources.
/// Implements the Singleton pattern and IItemDaoFactory interface.
/// </summary>
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
    /// <typeparam name="T">Type of Item</typeparam>
    /// <param name="dao">DAO instance to configure</param>
    public void ConfigureItemDao<T>(IItemDao<T> dao) where T : Item
    {
        configuredDaos[typeof(T)] = dao;
    }

    /// <summary>
    /// Configures a data source for a specific Item type.
    /// </summary>
    /// <typeparam name="T">Type of Item</typeparam>
    /// <param name="source">Data source instance to configure</param>
    public void ConfigureSource<T>(ISource<T> source) where T : Item
    {
        configuredSources[typeof(T)] = source;
    }

    /// <summary>
    /// Creates a DAO for a specific Item type.
    /// </summary>
    /// <typeparam name="T">Type of Item</typeparam>
    /// <returns>DAO instance for the specified type</returns>
    public IItemDao<T> CreateItemDao<T>() where T : Item
    {
        if (configuredDaos.TryGetValue(typeof(T), out var dao))
        {
            return (IItemDao<T>)dao;
        }

        var source = GetConfiguredSource<T>();
        return new ItemDao<T>(source);
    }
}