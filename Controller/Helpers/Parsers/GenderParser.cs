using Models;

namespace Controller.Helpers.Parsers;

public static class GenderParser
{
    private const string GenderText = "gender";

    /// <summary>
    /// Parses gender from string filter
    /// </summary>
    public static Gender Parse(string genderFilter)
    {
        if (genderFilter.StartsWith(GenderText))
        {
            genderFilter = genderFilter[GenderText.Length..];
        }

        genderFilter = genderFilter.Split('=')[1].Trim();
        if (!Enum.TryParse<Gender>(genderFilter, ignoreCase: true, out var gender))
        {
            var validValues = string.Join(", ", Enum.GetNames<Gender>());
            throw new ArgumentException($"Invalid gender: '{genderFilter}'. Valid values are: {validValues}");
        }

        return gender;
    }
}
