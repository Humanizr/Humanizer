using Humanizer.Configuration;
using Xunit;
namespace Humanizer.Tests.Extensions.Localisation
{
    public class ResourcesTests
    {
        [Fact]
        public void CanGetCultureSpecificTranslations()
        {
            using (new AmbientCulture("ro"))
            {
                var format = Resources.GetResource("DateHumanize__years_ago_above_20");
                Assert.Equal("acum {0} de ani", format);
            }
        }
    }
}
