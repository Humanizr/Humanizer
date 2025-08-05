using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Humanizer.Tests.Localisation;

public class ResourceCompletenessTests
{
    [Fact]
    public void AllLanguageResourceFilesShouldBeComplete()
    {
        // Get all resource entries from the main resource file
        var mainResourceManager = new ResourceManager("Humanizer.Properties.Resources", typeof(Resources).Assembly);
        var mainResourceSet = mainResourceManager.GetResourceSet(CultureInfo.InvariantCulture, true, true);
        
        var expectedKeys = new HashSet<string>();
        foreach (DictionaryEntry entry in mainResourceSet!)
        {
            if (entry.Key is string key && !IsExampleEntry(key))
            {
                expectedKeys.Add(key);
            }
        }
        
        // Get all supported cultures
        var assembly = typeof(Resources).Assembly;
        var supportedCultures = GetSupportedCultures(assembly);
        
        Assert.True(supportedCultures.Count >= 52, $"Expected at least 52 supported cultures, found {supportedCultures.Count}");
        
        // Verify each culture has all resource entries
        var incompleteLanguages = new List<string>();
        
        foreach (var culture in supportedCultures)
        {
            var cultureResourceSet = mainResourceManager.GetResourceSet(culture, true, false);
            if (cultureResourceSet == null) continue;
            
            var actualKeys = new HashSet<string>();
            foreach (DictionaryEntry entry in cultureResourceSet)
            {
                if (entry.Key is string key && !IsExampleEntry(key))
                {
                    actualKeys.Add(key);
                }
            }
            
            var missingKeys = expectedKeys.Except(actualKeys).ToList();
            if (missingKeys.Count > 0)
            {
                incompleteLanguages.Add($"{culture.Name}: missing {missingKeys.Count} keys ({string.Join(", ", missingKeys.Take(5))}{(missingKeys.Count > 5 ? "..." : "")})");
            }
        }
        
        Assert.Empty(incompleteLanguages);
    }
    
    [Theory]
    [InlineData("ru")]
    [InlineData("es")]
    [InlineData("fr")]
    [InlineData("de")]
    [InlineData("bg")]
    [InlineData("pt")]
    [InlineData("hu")]
    [InlineData("zh-CN")]
    [InlineData("ja")]
    [InlineData("ko-KR")]
    public void SpecificLanguageResourcesShouldHaveKeyTranslations(string cultureName)
    {
        var culture = new CultureInfo(cultureName);
        
        // Test specific key translations that should be properly localized
        var testKeys = new[]
        {
            "TimeSpanHumanize_Age",
            "N",
            "S", 
            "E",
            "W"
        };
        
        foreach (var key in testKeys)
        {
            var value = Resources.GetResource(key, culture);
            Assert.NotNull(value);
            Assert.NotEmpty(value);
            
            // Verify it's not the default example values
            Assert.DoesNotContain("this is my long string", value);
        }
    }
    
    [Fact]
    public void ResourceManagerCanLoadAllLanguages()
    {
        var assembly = typeof(Resources).Assembly;
        var supportedCultures = GetSupportedCultures(assembly);
        
        foreach (var culture in supportedCultures)
        {
            var resourceManager = new ResourceManager("Humanizer.Properties.Resources", assembly);
            var resourceSet = resourceManager.GetResourceSet(culture, true, false);
            
            Assert.NotNull(resourceSet);
            
            // Test that we can retrieve at least one resource
            var testResource = resourceSet.GetString("TimeSpanHumanize_Age");
            Assert.NotNull(testResource);
        }
    }
    
    private static bool IsExampleEntry(string key)
    {
        // Skip the example entries from the resource template
        return key is "Name1" or "Color1" or "Bitmap1" or "Icon1";
    }
    
    private static List<CultureInfo> GetSupportedCultures(Assembly assembly)
    {
        var cultures = new List<CultureInfo>();
        var resourceNames = assembly.GetManifestResourceNames()
            .Where(name => name.StartsWith("Humanizer.Properties.Resources.") && name.EndsWith(".resources"))
            .ToList();
        
        foreach (var resourceName in resourceNames)
        {
            // Extract culture from resource name like "Humanizer.Properties.Resources.bg.resources"
            var culturePart = resourceName
                .Replace("Humanizer.Properties.Resources.", "")
                .Replace(".resources", "");
            
            if (!string.IsNullOrEmpty(culturePart))
            {
                try
                {
                    var culture = new CultureInfo(culturePart);
                    cultures.Add(culture);
                }
                catch (CultureNotFoundException)
                {
                    // Skip invalid culture names
                }
            }
        }
        
        return cultures;
    }
}