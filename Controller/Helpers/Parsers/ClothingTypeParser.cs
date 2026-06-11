using Models;

namespace Controller.Helpers.Parsers;

public static class ClothingTypeParser
{
    private const string TypeText = "type";

    /// <summary>
    /// Parses clothing type from string filter
    /// </summary>
    public static ClothingType Parse(string typeFilter)
    {
        if (typeFilter.StartsWith(TypeText))
        {
            typeFilter = typeFilter[TypeText.Length..];
        }

        typeFilter = typeFilter.Split('=')[1].Trim();

        if (!Enum.TryParse<ClothingType>(typeFilter, ignoreCase: true, out var type))
        {
            var validValues = string.Join(", ", Enum.GetNames<ClothingType>());
            throw new ArgumentException($"Invalid type: '{typeFilter}'. Valid values are: {validValues}");
        }

        return type;
    }
}
