namespace Controller.Helpers.Parsers;

public static class PriceRangeParser
{
    private const string PriceText = "price";

    /// <summary>
    /// Parses price range from string filter
    /// </summary>
    public static Range<decimal> Parse(string priceFilter)
    {
        if (priceFilter.StartsWith(PriceText))
        {
            priceFilter = priceFilter[PriceText.Length..];
        }

        priceFilter = priceFilter.Split('=')[1].Trim();
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
