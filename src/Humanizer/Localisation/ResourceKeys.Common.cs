﻿using System;

namespace Humanizer.Localisation
{
    /// <summary>
    /// 
    /// </summary>
    public partial class ResourceKeys
    {
        private const string Single = "Single";
        private const string Multiple = "Multiple";
        private const string Abbreviate = "Abb";

        private static void ValidateRange(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
        }
    }
}
