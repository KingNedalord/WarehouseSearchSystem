using Warehouse.Models;
using Warehouse.Services;
namespace Warehouse.Controller;
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
    public FindCommand(IServiceFactory serviceFactory)
    {
        service = (serviceFactory ?? ServiceFactory.Instance).CreateService();
    }

    /// <summary>
    /// Creates a new FindCommand with injected service
    /// </summary>
    public FindCommand(IItemService service)
    {
        this.service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Executes the find command to retrieve items based on parameters
    /// </summary>
    public Response Execute(Request request)
    {
        try
        {
            // var target = GetTarget(request.Parameters);
            var target = request.Target.ToLower();
            if (request.Parameters.Length == 0)
            {
                if (target.StartsWith("all"))
                {
                    var res = service.FindAllItems();
                    return new Response(true, "all without predicate", res);
                }
                else if (target.StartsWith("cloth"))
                {
                    var res = service.FindAll<Clothing>();
                    return new Response(true, "clothing without predicate", res);
                }
                else if (target.StartsWith("foot"))
                {
                    var res = service.FindAll<Footwear>();
                    return new Response(true, "footwear without predicate", res);
                }

            }

            if (target.StartsWith("all"))
            {
                var predicate = GetItemsPredicate(request.Parameters);
                service.FindAllItems();
                var res = service.FilterAllBy(predicate);
                return new Response(true, "all with parameters", res);
            }
            else if (target.StartsWith("cloth"))
            {
                var predicate = GetClothingPredicate(request.Parameters);
                var res = service.FilterBy(predicate);
                return new Response(true, "clothing with parameters", res);
            }
            else if (target.StartsWith("foot"))
            {
                var predicate = GetFootwearPredicate(request.Parameters);
                var res = service.FilterBy(predicate);
                return new Response(true, "footwear with parameters", res);
            }
            return new Response(false, "Command pattern: find <target> <parameters>\n" +
                "Where target is one of: all, clothing, footwear\n" +
                "And parameters are in format: price=min;max ..");
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
    /// Extracts the target type from parameters
    /// </summary>
    private static string GetTarget(string[] parameters)
    {
        if (parameters.Length == 0)
            return "";

        return parameters[0].ToLower();
    }
    /// <summary>
    /// Builds a predicate for filtering items based on parameters
    /// </summary>
    private Predicate<Item> GetItemsPredicate(string[] parameters)
    {
        var predicate = new PredicateBuilder<Item>();
        foreach (var parameter in parameters)
        {
            if (parameter.StartsWith("price"))
            {
                Range<decimal> range = ParsePriceRange(parameter);
                predicate.Add(c => c.Price >= range.Min && c.Price <= range.Max);
            }
            else if (parameter.StartsWith("size"))
            {
                Size size = ParseSize(parameter);
                predicate.Add(c => c.Size == size);
            }
            else if (parameter.StartsWith("gender"))
            {
                Gender gender = ParsGender(parameter);
                predicate.Add(c => c.Gender == gender);
            }
        }

        return predicate.Build();
    }

    /// <summary>
    /// Builds a predicate for filtering clothing based on parameters
    /// /// </summary>
    private Predicate<Clothing> GetClothingPredicate(string[] parameters)
    {
        var predicate = new PredicateBuilder<Clothing>();
        foreach (var parameter in parameters)
        {
            if (parameter.StartsWith("price"))
            {
                Range<decimal> range = ParsePriceRange(parameter);
                predicate.Add(c => c.Price >= range.Min && c.Price <= range.Max);
            }
            else if (parameter.StartsWith("size"))
            {
                Size size = ParseSize(parameter);
                predicate.Add(c => c.Size == size);
            }
            else if (parameter.StartsWith("gender"))
            {
                Gender gender = ParsGender(parameter);
                predicate.Add(c => c.Gender == gender);
            }
            else if (parameter.StartsWith("type"))
            {
                var type = ParseClothingType(parameter);
                predicate.Add(c => c.ClothingType == type);
            }
        }

        return predicate.Build();
    }

    /// <summary>
    /// Builds a predicate for filtering footwear based on parameters
    /// /// </summary>
    private Predicate<Footwear> GetFootwearPredicate(string[] parameters)
    {
        var predicate = new PredicateBuilder<Footwear>();
        foreach (var parameter in parameters)
        {
            if (parameter.StartsWith("price"))
            {
                Range<decimal> range = ParsePriceRange(parameter);
                predicate.Add(c => c.Price >= range.Min && c.Price <= range.Max);
            }
            else if (parameter.StartsWith("size"))
            {
                Size size = ParseSize(parameter);
                predicate.Add(c => c.Size == size);
            }
            else if (parameter.StartsWith("gender"))
            {
                Gender gender = ParsGender(parameter);
                predicate.Add(c => c.Gender == gender);
            }
            else if (parameter.StartsWith("type"))
            {
                var type = ParseFootwearType(parameter);
                predicate.Add(c => c.FootwearType == type);
            }
        }

        return predicate.Build();
    }



    /// <summary>
    /// Parses price range from string filter
    /// </summary>
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

    /// <summary>
    /// Parses size from string filter
    /// </summary>
    private Size ParseSize(string sizeFilter)
    {
        if (sizeFilter.StartsWith("size="))
            sizeFilter = sizeFilter.Substring(5); // Remove "size=" prefix

        sizeFilter = sizeFilter.Trim();

        if (!Enum.TryParse<Size>(sizeFilter, out var size))
        {
            var validValues = string.Join(", ", Enum.GetNames<Size>());
            throw new ArgumentException($"Invalid size: '{sizeFilter}'. Valid values are: {validValues}");
        }

        return size;
    }

    /// <summary>
    /// Parses gender from string filter
    /// </summary>
    private Gender ParsGender(string genderFilter)
    {
        if (genderFilter.StartsWith("gender="))
            genderFilter = genderFilter.Substring(7);

        genderFilter = genderFilter.Trim();

        if (!Enum.TryParse<Gender>(genderFilter, out var gender))
        {
            var validValues = string.Join(", ", Enum.GetNames<Gender>());
            throw new ArgumentException($"Invalid gender: '{genderFilter}'. Valid values are: {validValues}");
        }

        return gender;
    }

    /// <summary>
    /// Parses clothing type from string filter
    /// </summary>
    private ClothingType ParseClothingType(string typeFilter)
    {
        if (typeFilter.StartsWith("type="))
            typeFilter = typeFilter.Substring(5);

        typeFilter = typeFilter.Trim();

        if (!Enum.TryParse<ClothingType>(typeFilter, out var type))
        {
            var validValues = string.Join(", ", Enum.GetNames<ClothingType>());
            throw new ArgumentException($"Invalid type: '{typeFilter}'. Valid values are: {validValues}");
        }

        return type;
    }

    /// <summary>
    /// Parses footwear type from string filter
    /// </summary>
    private FootwearType ParseFootwearType(string typeFilter)
    {
        if (typeFilter.StartsWith("type="))
            typeFilter = typeFilter.Substring(5);

        typeFilter = typeFilter.Trim();

        if (!Enum.TryParse<FootwearType>(typeFilter, out var type))
        {
            var validValues = string.Join(", ", Enum.GetNames<FootwearType>());
            throw new ArgumentException($"Invalid type: '{typeFilter}'. Valid values are: {validValues}");
        }

        return type;
    }
}