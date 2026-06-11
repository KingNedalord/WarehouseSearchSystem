using Models;

namespace Controller.Helpers.Parsers;

public static class SizeParser
{
    private const string SizeText = "size";

    /// <summary>
    /// Parses size from string filter
    /// </summary>
    public static Size Parse(string sizeFilter)
    {
        if (sizeFilter.StartsWith(SizeText))
        {
            sizeFilter = sizeFilter[SizeText.Length..];
        }

        sizeFilter = sizeFilter.Split('=')[1].Trim();

        if (!Enum.TryParse<Size>(sizeFilter, ignoreCase: true, out var size))
        {
            var validValues = string.Join(", ", Enum.GetNames<Size>());
            throw new ArgumentException($"Invalid size: '{sizeFilter}'. Valid values are: {validValues}");
        }

        return size;
    }
}
