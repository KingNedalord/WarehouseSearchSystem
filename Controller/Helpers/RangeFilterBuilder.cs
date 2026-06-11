namespace Controller.Helpers;

/// <summary>
/// Class representation of price range
/// </summary>
/// <typeparam name="T"></typeparam>
public class Range<T>(T min, T max)
    where T : IComparable<T>
{
    public T Min { get; } = min;

    public T Max { get; } = max;
}
