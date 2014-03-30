using System;
using Xunit;

namespace Humanizer.Tests
{
    public sealed class FactExcludingTeamCityAttribute : FactAttribute
    {
        public FactExcludingTeamCityAttribute()
        {
            if (Environment.GetEnvironmentVariable("TEAMCITY_PROJECT_NAME") != null)
            {
                Skip = "Sorry, I am told not to run on TeamCity!";
            }
        }
    }
}
