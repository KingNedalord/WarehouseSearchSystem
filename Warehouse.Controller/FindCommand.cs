using Warehouse.Models;
using Warehouse.Services;

/// <summary>
/// Command for finding items based on type and criteria
/// </summary>
public class FindCommand : ICommand
{
    private readonly IItemService service;

    /// <summary>
    /// Creates a new FindCommand with default service factory
    /// </summary>
    public FindCommand() : this(ServiceFactory.Instance)
    {
    }

    /// <summary>
    /// Creates a new FindCommand with injected service factory
    /// </summary>
    /// <param name="serviceFactory">Service factory to use</param>
    public FindCommand(IServiceFactory serviceFactory)
    {
        service = (serviceFactory ?? ServiceFactory.Instance).CreateService();
    }

    /// <summary>
    /// Creates a new FindCommand with injected service
    /// </summary>
    /// <param name="service">Item service to use</param>
    public FindCommand(IItemService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Executes the find command to retrieve items based on parameters
    /// </summary>
    /// <param name="request">Request containing command parameters</param>
    /// <returns>Response containing found items</returns>
    public Response Execute(Request request)
    {
        try
        {
            if (request.Parameters.Length == 0)
            {
                return new Response(false, "Usage: find <clothings|footwears|all> [price=min;max]");
            }

            var target = request.Parameters[0].ToLower();
            List<Item> items = new();

            switch (target)
            {
                case "clothings":
                    var clothings = service.FindClothings();
                    foreach (var clothing in clothings)
                    {
                        items.Add(clothing);
                    }
                    break;

                case "footwears":
                    var footwears = service.FindFootwears();
                    foreach (var footwear in footwears)
                    {
                        items.Add(footwear);
                    }
                    break;

                case "all":
                    string? priceFilter = null;
                    for (int i = 1; i < request.Parameters.Length; i++)
                    {
                        if (request.Parameters[i].StartsWith("price="))
                        {
                            priceFilter = request.Parameters[i];
                            break;
                        }
                    }
                    if (priceFilter != null)
                    {
                        var range = ParsePriceRange(priceFilter);
                        items.AddRange(service.FindByPrice(range));
                    }
                    else
                    {
                        var allCloting = service.FindClothings();
                        var allFootwears = service.FindFootwears();
                        foreach (var clothing in allCloting)
                        {
                            items.Add(clothing);
                        }
                        foreach (var footwear in allFootwears)
                        {
                            items.Add(footwear);
                        }
                    }
                    break;

                default:
                    return new Response(false, $"Unknown target: {target}. Use 'clothings', 'footwears', or 'all'");
            }

            return new Response(true, $"Found {items.Count} item(s):", items);
        }
        catch (ServiceException ex)
        {
            return new Response(false, $"Error during search: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return new Response(false, $"Invalid parameters: {ex.Message}");
        }
    }

    /// <summary>
    /// Parses price range from string filter
    /// </summary>
    /// <param name="priceFilter">Price filter string in format "min;max"</param>
    /// <returns>Price range</returns>
    /// <exception cref="ArgumentException">When price filter format is invalid</exception>
    private Range<decimal> ParsePriceRange(string priceFilter)
    {
        if (priceFilter.StartsWith("price="))
            priceFilter = priceFilter.Substring(6);

        var parts = priceFilter.Split(';');
        if (parts.Length != 2)
        {
            throw new ArgumentException("Price filter must be in format 'min;max'");
        }

        if (!decimal.TryParse(parts[0], out var minPrice) || minPrice < 0)
        {
            throw new ArgumentException($"Invalid minimum price: {parts[0]}");
        }

        if (!decimal.TryParse(parts[1], out var maxPrice) || maxPrice < 0)
        {
            throw new ArgumentException($"Invalid maximum price: {parts[1]}");
        }

        if (minPrice > maxPrice)
        {
            throw new ArgumentException("Minimum price cannot be greater than maximum price");
        }

        return new Range<decimal>(minPrice, maxPrice);
    }
}