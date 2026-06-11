using Models;

namespace Controller.Helpers.Parsers;

public static class FootwearTypeParser
{
    private const string TypeText = "type";

    /// <summary>
    /// Parses footwear type from string filter
    /// </summary>
    public static FootwearType Parse(string typeFilter)
    {
        if (typeFilter.StartsWith(TypeText))
        {
            typeFilter = typeFilter[TypeText.Length..];
        }

        typeFilter = typeFilter.Split('=')[1].Trim();

        if (!Enum.TryParse<FootwearType>(typeFilter, ignoreCase: true, out var type))
        {
            var validValues = string.Join(", ", Enum.GetNames<FootwearType>());
            throw new ArgumentException($"Invalid type: '{typeFilter}'. Valid values are: {validValues}");
        }

        return type;
    }
}
