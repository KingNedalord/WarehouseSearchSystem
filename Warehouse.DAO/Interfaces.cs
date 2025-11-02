using System;
using System.Collections.Generic;

/// <summary>
/// Interface for Data Access Objects that handle Item data operations
/// </summary>
/// <typeparam name="T">Type of Item to handle</typeparam>
public interface IItemDao<T> where T : Item
{
    /// <summary>
    /// Finds all Items of the specified type
    /// </summary>
    /// <returns>List of all Items</returns>
    IList<T> FindAll();

    /// <summary>
    /// Finds Items that match the specified predicate
    /// </summary>
    /// <param name="predicate">Predicate to filter Items</param>
    /// <returns>List of Items matching the predicate</returns>
    IList<T> Find(Predicate<T> predicate);
}


/// <summary>
/// Interface for a factory that creates DAO objects and data sources.
/// </summary>
public interface IItemDaoFactory
{
    /// <summary>
    /// Creates a DAO for a specific Item type.
    /// </summary>
    /// <typeparam name="T">Type of Item</typeparam>
    /// <returns>DAO instance for the specified type</returns>
    IItemDao<T> CreateItemDao<T>() where T : Item;

    /// <summary>
    /// Configures a DAO for a specific Item type.
    /// </summary>
    /// <typeparam name="T">Type of Item</typeparam>
    /// <param name="dao">DAO instance to configure</param>
    void ConfigureItemDao<T>(IItemDao<T> dao) where T : Item;

    /// <summary>
    /// Configures a data source for a specific Item type.
    /// </summary>
    /// <typeparam name="T">Type of Item</typeparam>
    /// <param name="source">Data source instance to configure</param>
    void ConfigureSource<T>(ISource<T> source) where T : Item;
}


// ISource

/// <summary>
/// Interface for data sources in Straight implementation
/// </summary>
/// <typeparam name="T">Type of Item to parse</typeparam>
public interface ISource<out T> where T : Item
{
    /// <summary>
    /// Gets the path to the CSV file
    /// </summary>
    /// <returns>Path to the CSV file</returns>
    string FilePath();

    /// <summary>
    /// Parses a CSV line into an Item object
    /// </summary>
    /// <param name="line">CSV line to parse</param>
    /// <returns>Parsed Item object</returns>
    T Parse(string line);
}