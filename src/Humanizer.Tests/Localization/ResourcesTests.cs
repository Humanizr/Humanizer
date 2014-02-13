﻿using Humanizer.Localization;
using Xunit;

namespace Humanizer.Tests.Localization
{
    public class ResourcesTests
    {
        [Fact]
        public void CanGetCultureSpecificTranslations()
        {
            using (new AmbientCulture("ro"))
            {
                var format = Resources.GetResource("DateHumanize_MultipleYearsAgo_Above20");
                Assert.Equal("acum {0} de ani", format);
            }
        }
    }
}
