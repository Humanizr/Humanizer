namespace Humanizer;

public partial class ResourceKeys
{
    static void ValidateRange(int count)
    {
        if (count < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(count));
        }
    }
}