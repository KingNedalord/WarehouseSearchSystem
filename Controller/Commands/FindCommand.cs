using Controller.Helpers;
using Controller.Helpers.Parsers;
using Controller.Interfaces;
using Models;
using Services;
using Services.Interfaces;

namespace Controller.Commands;

/// <summary>
/// Command for finding items based on type and criteria
/// </summary>
public class FindCommand : ICommand
{
    private readonly IUserService _service;

    private const string PriceText = "price";
    private const string SizeText = "size";
    private const string GenderText = "gender";
    private const string TypeText = "type";

    /// <summary>
    /// Creates a new FindCommand with injected service
    /// </summary>
    public FindCommand(IUserService service)
    {
        this._service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <inheritdoc/>
    public async Task<Response> ExecuteAsync(Request request)
    {
        try
        {
            switch (request.Target)
            {
                case TargetType.All:
                {
                    var predicate = this.GetItemsPredicate(request.Parameters);
                    var allRes = await this._service.FilterAllByAsync(predicate);
                    return Response.Ok(GetMessage(allRes), allRes);
                }
                case TargetType.Clothing:
                {
                    var predicate = this.GetClothingPredicate(request.Parameters);
                    var res = await this._service.FilterByAsync(predicate);
                    return Response.Ok(GetMessage(res), res);
                }
                case TargetType.Footwear:
                {
                    var predicate = this.GetFootwearPredicate(request.Parameters);
                    var res = await this._service.FilterByAsync(predicate);
                    return Response.Ok(GetMessage(res), res);
                }
                default:
                    return Response.Fail("Command pattern: find <target> <parameters>\n" +
                                               "Where target is one of: all, clothing, footwear\n" +
                                               "And parameters are in format: price=min;max ..");
            }
        }
        catch (ServiceException ex)
        {
            return Response.Fail($"Error during search: {ex.Message}");
        }
        catch (ArgumentException ex)
        {
            return Response.Fail($"Invalid parameters: {ex.Message}");
        }
    }

    /// <summary>
    /// Extracts the target type from parameters
    /// </summary>
    private static string GetMessage<T>(List<T> resultList)
    {
        int count = resultList.Count;
        return count == 0 ? "No result found" : $"Found: {count} items";
    }

    /// <summary>
    /// Builds a predicate for filtering items based on parameters
    /// </summary>
    private Predicate<Item> GetItemsPredicate(string[] parameters)
    {
        var predicate = new PredicateBuilder<Item>();
        foreach (var parameter in parameters)
        {
            if (parameter.StartsWith(PriceText))
            {
                var range = PriceRangeParser.Parse(parameter);
                predicate.Add(c => c.Price >= range.Min && c.Price <= range.Max);
            }
            else if (parameter.StartsWith(SizeText))
            {
                var size = SizeParser.Parse(parameter);
                predicate.Add(c => c.Size == size);
            }
            else if (parameter.StartsWith(GenderText))
            {
                var gender = GenderParser.Parse(parameter);
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
            if (parameter.StartsWith(PriceText))
            {
                var range = PriceRangeParser.Parse(parameter);
                predicate.Add(c => c.Price >= range.Min && c.Price <= range.Max);
            }
            else if (parameter.StartsWith(SizeText))
            {
                var size = SizeParser.Parse(parameter);
                predicate.Add(c => c.Size == size);
            }
            else if (parameter.StartsWith(GenderText))
            {
                var gender = GenderParser.Parse(parameter);
                predicate.Add(c => c.Gender == gender);
            }
            else if (parameter.StartsWith(TypeText))
            {
                var type = ClothingTypeParser.Parse(parameter);
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
            if (parameter.StartsWith(PriceText))
            {
                var range = PriceRangeParser.Parse(parameter);
                predicate.Add(c => c.Price >= range.Min && c.Price <= range.Max);
            }
            else if (parameter.StartsWith(SizeText))
            {
                var size = SizeParser.Parse(parameter);
                predicate.Add(c => c.Size == size);
            }
            else if (parameter.StartsWith(GenderText))
            {
                var gender = GenderParser.Parse(parameter);
                predicate.Add(c => c.Gender == gender);
            }
            else if (parameter.StartsWith(TypeText))
            {
                var type = FootwearTypeParser.Parse(parameter);
                predicate.Add(c => c.FootwearType == type);
            }
        }

        return predicate.Build();
    }
}
