using System;

/// <summary>
/// Extension methods for ICloneable to provide type-safe cloning
/// </summary>
public static class CloneableExtensions
{
    /// <summary>
    /// Creates a type-safe copy of an ICloneable object
    /// </summary>
    /// <typeparam name="T">Type of the object to clone</typeparam>
    /// <param name="cloneable">Object to clone</param>
    /// <returns>Typed copy of the object</returns>
    public static T Clone<T>(this ICloneable cloneable) where T : class
    {
        return (T)cloneable.Clone();
    }

    /// <summary>
    /// Creates a type-safe copy of an ISource object
    /// </summary>
    /// <typeparam name="T">Type of Item in the source</typeparam>
    /// <param name="source">Source to clone</param>
    /// <returns>Typed copy of the source</returns>
    public static ISource<T> CloneSource<T>(this ISource<T> source) where T : Item
    {
        return (ISource<T>)((ICloneable)source).Clone();
    }
}