namespace Humanizer;

public partial class ResourceKeys
{
    static void ValidateRange(int count)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(count, nameof(count));
    }
}