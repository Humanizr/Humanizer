using System.Globalization;
using Humanizer.Localisation;
using Xunit;

namespace Humanizer.Tests.Localisation
{
    public class ResourcesTests
    {
        [Fact]
        public void CanGetCultureSpecificTranslationsWithImplicitCulture()
        {
            using (new AmbientCulture("ro"))
            {
                var format = Resources.GetResource("DateHumanize_MultipleYearsAgo_Above20");
                Assert.Equal("acum {0} de ani", format);
            }
        }

        [Fact]
        public void CanGetCultureSpecificTranslationsWithExplicitCulture()
        {
            var format = Resources.GetResource("DateHumanize_MultipleYearsAgo_Above20", new CultureInfo("ro"));
            Assert.Equal("acum {0} de ani", format);
        }
    }
}
