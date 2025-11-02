using System;
using System.Collections.Generic;

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
        this.source = source;
    }

    /// <summary>
    /// Finds Items matching the specified predicate
    /// </summary>
    /// <param name="predicate">Predicate to filter Items</param>
    /// <returns>List of matching Items</returns>
    public IList<T> Find(Predicate<T> predicate)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Finds all Items of the specified type
    /// </summary>
    /// <returns>List of all Items</returns>
    public IList<T> FindAll()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the path to the CSV file
    /// </summary>
    /// <returns>Path to the CSV file</returns>
    /// <exception cref="DaoException">When file is not found</exception>
    private string GetPath()
    {
        throw new NotImplementedException();
    }
}