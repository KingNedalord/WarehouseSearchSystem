using Controller.Interfaces;
using Models;
using Services.Interfaces;

namespace Controller.Commands;

public class AdminCommands : ICommand
{
    private readonly IAdminService _adminService;
    private readonly IUserService _userService;

    /// <summary>
    /// Key-value list of options when choosing Size
    /// </summary>
    private static readonly Dictionary<string, Size> SizeMap = new()
    {
        { "1", Size.Xs }, { "2", Size.S }, { "3", Size.M },
        { "4", Size.L },  { "5", Size.Xl }, { "6", Size.Xxl }
    };

    /// <summary>
    /// Key-value list of options when choosing Gender
    /// </summary>
    private static readonly Dictionary<string, Gender> GenderMap = new()
    {
        { "1", Gender.Male }, { "2", Gender.Female }, { "3", Gender.Unisex }
    };

    /// <summary>
    /// Key-value list of options when choosing ClothingType
    /// </summary>
    private static readonly Dictionary<string, ClothingType> ClothingTypeMap = new()
    {
        { "1", ClothingType.Top }, { "2", ClothingType.Bottom }, { "3", ClothingType.FullBody },
        { "4", ClothingType.Outerwear},  { "5", ClothingType.Accessory }
    };

    /// <summary>
    /// Key-value list of options when choosing FootwearType
    /// </summary>
    private static readonly Dictionary<string, FootwearType> FootwearTypeMap = new()
    {
        { "1", FootwearType.Casual }, { "2", FootwearType.Formal }, { "3", FootwearType.Special },
    };

    public AdminCommands(IAdminService adminService, IUserService userService)
    {
        this._adminService = adminService;
        this._userService = userService;
    }

    /// <inheritdoc/>>
    public async Task<Response> ExecuteAsync(Request request)
    {
        string command = request.Command.ToLower();
        return command switch
        {
            "add" => request.Target switch
            {
                TargetType.Clothing  => await this.AddItemAsync<Clothing>(),
                TargetType.Footwear  => await this.AddItemAsync<Footwear>(),
                _ => Response.Fail("Unknown target"),
            },
            "update" => request.Target switch
            {
                TargetType.Clothing  => await this.UpdateItemAsync<Clothing>(request.Parameters),
                TargetType.Footwear  => await this.UpdateItemAsync<Footwear>(request.Parameters),
                _ => Response.Fail("Unknown target"),
            },
            "delete" => request.Target switch
            {
                TargetType.Clothing  => await this.DeleteAsync<Clothing>(request.Parameters),
                TargetType.Footwear  => await this.DeleteAsync<Footwear>(request.Parameters),
                _ => Response.Fail("Unknown target"),
            },
            _ => Response.WrongCommand(command),
        };
    }

    /// <summary>
    /// Deletes item from data base
    /// </summary>
    /// <param name="parameters">List of parameters(we need only id)</param>
    /// <returns>Response indicating result of operation</returns>
    /// <exception cref="ArgumentException">Throws when parameter is empty or can't be parsed</exception>
    private async Task<Response> DeleteAsync<T>(string[] parameters) where T : Item
    {
        if (!TryParseId(parameters, out var id, out var errorResponse))
        {
            return errorResponse!;
        }

        await this._adminService.DeleteAsync<T>(id);
        return Response.Ok($"Deleted {typeof(T).Name} with id: {id}");
    }

    /// <summary>
    /// Updates item in data base
    /// </summary>
    /// <param name="parameters">List of parameters(we need only id)</param>
    /// <returns>Response indicating result of operation</returns>
    private async Task<Response> UpdateItemAsync<T>(string[] parameters) where T: Item
    {
        if (!TryParseId(parameters, out var id, out var errorResponse))
        {
            return errorResponse!;
        }

        var item = (await this._userService
                .FilterByAsync<T>(c => c.Id == id))
            .FirstOrDefault();

        if (item == null)
        {
            return Response.Fail("No items found");
        }

        var updated = this.UpdateForm(item);
        await this._adminService.UpdateAsync(updated);
        return Response.Ok($"Updated {typeof(T).Name} with id: {id}", [updated]);
    }

    /// <summary>
    /// Adds an item to database
    /// </summary>
    /// <returns>Response indicating result of operation</returns>
    private async Task<Response> AddItemAsync<T>() where T : Item
    {
        var item = this.AddForm<T>();
        await this._adminService.AddAsync(item);
        return Response.Ok($"Added {typeof(T).Name} with id: {item.Id}", [item]);
    }

    /// <summary>
    /// Form to fill after add command started execution
    /// </summary>
    /// <typeparam name="T">Type indicating return instance, either Clothing or Footwear</typeparam>
    /// <returns>Either Clothing or Footwear</returns>
    private T AddForm<T>() where T : Item
    {
        Console.WriteLine($"=== Add {typeof(T).Name} ===");
        var name = PromptString("Name: ");
        var size = GetSize()!.Value;
        var gender = GetGender()!.Value;
        var price = PromptValue<decimal>("Price: ", decimal.TryParse)!.Value;
        var quantity = PromptValue<int>("Quantity: ", int.TryParse)!.Value;

        if (typeof(T) == typeof(Clothing))
        {
            var type = GetClothingType();
            return (T)(Item)new Clothing(name, size, gender, price, quantity, type);
        }
        else
        {
            var type = GetFootwearType();
            return (T)(Item)new Footwear(name, size, gender, price, quantity, type);
        }
    }

    /// <summary>
    /// Form to fill after update command started execution
    /// </summary>
    /// <param name="item">Items which is being updated</param>
    /// <typeparam name="T">Type indicating return instance, either Clothing or Footwear</typeparam>
    /// <returns>Either Clothing or Footwear</returns>
    private T UpdateForm<T>(T item) where T : Item
    {
        Console.WriteLine($"=== Update {typeof(T).Name} ===");
        Console.WriteLine("Hit enter to keep current value");

        var name = PromptString($"Name [{item.Name}]: ", item.Name);
        var size = GetSize(item.Size.ToString(),true) ?? item.Size;
        var gender = GetGender(item.Gender.ToString(),true) ?? item.Gender;
        var price = PromptValue<decimal>($"Price [{item.Price}]: ", decimal.TryParse, true) ?? item.Price;
        var quantity = PromptValue<int>($"Quantity [{item.Quantity}]: ", int.TryParse, true) ?? item.Quantity;

        return typeof(T) == typeof(Clothing)
            ? (T)(Item)new Clothing(name, size, gender, price, quantity, GetClothingType()) { Id = item.Id }
            : (T)(Item)new Footwear(name, size, gender, price, quantity, GetFootwearType()) { Id = item.Id };
    }

    private static T? PromptEnum<T>(string label, Dictionary<string, T> map, bool update = false)
        where T : struct
    {
        Console.WriteLine(label);
        foreach (var (key, val) in map)
        {
            Console.WriteLine($"{key} - {val}");
        }

        Console.Write("Choice: ");

        var input = Console.ReadLine();

        if (update && string.IsNullOrEmpty(input))
        {
            return null;
        }

        if (map.TryGetValue(input ?? "", out var result))
        {
            return result;
        }

        Console.WriteLine("Invalid choice");
        return PromptEnum(label, map, update);
    }

    private static T? PromptValue<T>(string label, TryParseDelegate<T> tryParse, bool update = false)
        where T : struct, IComparable<T>
    {
        Console.Write(label);
        var input = Console.ReadLine();

        if (update && string.IsNullOrEmpty(input))
        {
            return null;
        }

        if (tryParse(input ?? "", out var result))
        {
            if (result.CompareTo(default) > 0)
            {
                return result;
            }

            Console.WriteLine($"{label} cannot be less than 1");
            return PromptValue(label, tryParse, update);

        }

        Console.WriteLine($"Invalid {label}");
        return PromptValue(label, tryParse, update);
    }

    private delegate bool TryParseDelegate<T>(string input, out T result);

    private static string PromptString(string label, string? fallback = null)
    {
        Console.Write(label);
        var input = Console.ReadLine();
        return string.IsNullOrWhiteSpace(input) ? fallback ?? "" : input;
    }

    private static Size? GetSize(string? oldValue = null, bool update = false) =>
        PromptEnum($"Size: {oldValue}", SizeMap, update);

    private static Gender? GetGender(string? oldValue = null, bool update = false) =>
        PromptEnum($"Gender: {oldValue}", GenderMap, update);

    private static ClothingType GetClothingType() =>
        PromptEnum("Clothing Type:", ClothingTypeMap)!.Value;

    private static FootwearType GetFootwearType() =>
        PromptEnum("Footwear Type:", FootwearTypeMap)!.Value;

    private static bool TryParseId(string[] parameters, out int id, out Response? errorResponse)
    {
        id = 0;
        if (parameters.Length == 0)
        {
            errorResponse = Response.Fail("ID is required");
            return false;
        }
        if (!int.TryParse(parameters[0], out id))
        {
            errorResponse = Response.Fail("Parameter must be an integer");
            return false;
        }

        errorResponse = null;
        return true;
    }
}
