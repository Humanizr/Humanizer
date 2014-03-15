using System;

namespace Humanizer.Localisation.DynamicResourceKeys
{
    public partial class ResourceKeys
    {
        private const string Single = "Single";
        private const string Multiple = "Multiple";

        private static void ValidateRange(int count)
        {
            if (count < 1) throw new ArgumentOutOfRangeException("count");
        }
    }
}
