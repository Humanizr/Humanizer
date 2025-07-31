using System.Globalization;
using System.Reflection;
using System.Resources;
using Xunit;

namespace Humanizer.Tests;

/// <summary>
/// Tests to ensure all language resource files have complete translations
/// </summary>
public class ResourceCompletenessTests
{
    [Fact]
    public void AllLanguageResourceFiles_ShouldHaveCompleteTranslations()
    {
        // Get the main resource set
        var resourceManager = new ResourceManager("Humanizer.Properties.Resources", typeof(Resources).Assembly);
        var mainResourceSet = resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
        
        // Get all keys from the main resource set
        var mainKeys = new HashSet<string>();
        foreach (DictionaryEntry entry in mainResourceSet!)
        {
            if (entry.Key is string key)
            {
                mainKeys.Add(key);
            }
        }
        
        Assert.True(mainKeys.Count > 0, "Main resource set should contain entries");
        
        // Get all cultures that have resource files
        var cultures = GetAvailableCultures();
        
        foreach (var culture in cultures)
        {
            try
            {
                var cultureResourceSet = resourceManager.GetResourceSet(culture, true, false);
                if (cultureResourceSet != null)
                {
                    var cultureKeys = new HashSet<string>();
                    foreach (DictionaryEntry entry in cultureResourceSet)
                    {
                        if (entry.Key is string key)
                        {
                            cultureKeys.Add(key);
                        }
                    }
                    
                    // Check that all main keys exist in this culture
                    var missingKeys = mainKeys.Except(cultureKeys).ToList();
                    
                    Assert.True(missingKeys.Count == 0,
                        $"Culture '{culture.Name}' is missing {missingKeys.Count} translations: {string.Join(", ", missingKeys.Take(5))}{(missingKeys.Count > 5 ? "..." : "")}");
                    
                    // Verify the culture has at least as many keys as main
                    Assert.True(cultureKeys.Count >= mainKeys.Count,
                        $"Culture '{culture.Name}' has fewer keys ({cultureKeys.Count}) than main resource set ({mainKeys.Count})");
                }
            }
            catch (Exception ex)
            {
                Assert.True(false, $"Failed to load resources for culture '{culture.Name}': {ex.Message}");
            }
        }
    }
    
    [Fact]
    public void AllResourceKeys_ShouldExistInMainResourceFile()
    {
        // This test ensures that the main resource file contains all expected keys
        var expectedKeys = new[]
        {
            "DateHumanize_SingleSecondAgo",
            "DateHumanize_MultipleSecondsAgo", 
            "DateHumanize_SingleMinuteAgo",
            "DateHumanize_MultipleMinutesAgo",
            "TimeSpanHumanize_SingleSecond",
            "TimeSpanHumanize_MultipleSeconds",
            "DataUnit_Bit",
            "DataUnit_Byte",
            "DataUnit_Kilobyte",
            "DataUnit_Megabyte",
            "DataUnit_Gigabyte",
            "DataUnit_Terabyte"
        };
        
        var resourceManager = new ResourceManager("Humanizer.Properties.Resources", typeof(Resources).Assembly);
        
        foreach (var key in expectedKeys)
        {
            var value = resourceManager.GetString(key);
            Assert.False(string.IsNullOrEmpty(value), $"Key '{key}' should have a non-empty value in main resource file");
        }
    }
    
    [Theory]
    [InlineData("fr")]
    [InlineData("es")] 
    [InlineData("de")]
    [InlineData("ru")]
    [InlineData("ja")]
    [InlineData("zh-CN")]
    public void SpecificCultures_ShouldHaveCompleteTranslations(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        var resourceManager = new ResourceManager("Humanizer.Properties.Resources", typeof(Resources).Assembly);
        
        // Get main resource count
        var mainResourceSet = resourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
        var mainKeyCount = mainResourceSet!.Cast<DictionaryEntry>().Count();
        
        // Get culture resource count
        var cultureResourceSet = resourceManager.GetResourceSet(culture, true, false);
        Assert.NotNull(cultureResourceSet);
        
        var cultureKeyCount = cultureResourceSet.Cast<DictionaryEntry>().Count();
        
        Assert.Equal(mainKeyCount, cultureKeyCount);
    }
    
    private static List<CultureInfo> GetAvailableCultures()
    {
        var cultures = new List<CultureInfo>();
        var assembly = typeof(Resources).Assembly;
        
        // Find all embedded resource files for cultures
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(name => name.Contains("Humanizer.Properties.Resources.") && name.EndsWith(".resources"))
            .ToList();
        
        foreach (var resourceName in resourceNames)
        {
            // Extract culture from resource name like "Humanizer.Properties.Resources.fr.resources"
            var parts = resourceName.Split('.');
            if (parts.Length >= 4)
            {
                var culturePart = parts[parts.Length - 2]; // Get the part before ".resources"
                
                try
                {
                    if (culturePart != "Resources") // Skip the main resource file
                    {
                        var culture = new CultureInfo(culturePart);
                        cultures.Add(culture);
                    }
                }
                catch (CultureNotFoundException)
                {
                    // Some resource names might not be valid culture names, skip them
                }
            }
        }
        
        return cultures.Distinct().ToList();
    }
}