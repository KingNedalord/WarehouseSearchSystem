using Models;

namespace Controller.Helpers;

/// <summary>
/// Helps to create predicate(filter)
/// </summary>
/// <typeparam name="T"></typeparam>
public class PredicateBuilder<T> where T : Item
{
    private readonly List<Predicate<T>> _predicates = [];

    /// <summary>
    /// Adds new condition into filter.
    /// </summary>
    /// <param name="predicate"></param>
    public void Add(Predicate<T>? predicate)
    {
        if (predicate != null)
        {
            this._predicates.Add(predicate);
        }
    }

    /// <summary>
    /// Builds final predicate with all filters
    /// </summary>
    /// <returns>Predicate filled with filters</returns>
    public Predicate<T> Build()
    {
        return item => this._predicates.All(p => p(item));
    }
}
