public class Range<T> where T : IComparable<T>
{
    public T Min { get; }
    public T Max { get; }

    public Range(T min, T max)
    {
        Min = min;
        Max = max;
    }
}
