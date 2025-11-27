using Warehouse.Models;
namespace Warehouse.Controller;

public class PredicateBuilder<T> where T : Item
{
    private List<Predicate<T>> predicates = new();

    // Add a predicate
    public PredicateBuilder<T> Add(Predicate<T> predicate)
    {
        if (predicate != null)
            predicates.Add(predicate);
        return this;
    }

    public Predicate<T> Build()
    {
        return item => predicates.All(p => p(item));
    }
}