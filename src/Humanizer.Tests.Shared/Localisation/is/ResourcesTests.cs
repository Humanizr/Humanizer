using System.Globalization;

using Humanizer.Localisation;

using Xunit;

namespace Humanizer.Tests.Localisation.@is
{
    public class ResourcesTests
    {
        [Fact]
        [UseCulture("is")]
        public void GetCultureSpecificTranslationsWithImplicitCulture()
        {
            var format = Resources.GetResource("DateHumanize_MultipleYearsAgo");
            Assert.Equal("fyrir {0} árum", format);
        }

        [Fact]
        public void GetCultureSpecificTranslationsWithExplicitCulture()
        {
            var format = Resources.GetResource("DateHumanize_SingleYearAgo", new CultureInfo("is"));
            Assert.Equal("fyrir einu ári", format);
        }
    }
}
