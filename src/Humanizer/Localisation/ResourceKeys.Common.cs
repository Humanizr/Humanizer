namespace Humanizer
{
    /// <summary>
    ///
    /// </summary>
    public partial class ResourceKeys
    {
        const string Single = "Single";
        const string Multiple = "Multiple";

        static void ValidateRange(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
        }
    }
}
