using System;
using System.Globalization;
using Xunit;

namespace Humanizer.Tests
{
    /// <summary>
    /// Tests to ensure culture parameter is respected, especially in environments 
    /// like Blazor WebAssembly where ResourceManager might have issues
    /// </summary>
    public class BlazorWebAssemblyCultureTests
    {
        [Theory]
        [InlineData("de-DE")]
        [InlineData("fr-FR")]
        [InlineData("es-ES")]
        [InlineData("it-IT")]
        [InlineData("pt-BR")]
        public void DateTimeHumanize_ShouldRespectCultureParameter_AndNotReturnEnglish(string cultureCode)
        {
            // Arrange
            var culture = new CultureInfo(cultureCode);
            var date = DateTime.UtcNow.AddHours(-2);
            
            // Act - this should use the specified culture, not fall back to English
            var result = date.Humanize(culture: culture);
            
            // Assert - in the original issue, this always returned English regardless of culture
            // The fix ensures we get localized results, not English fallbacks
            Assert.DoesNotContain("hours ago", result); // Should not be English
            Assert.DoesNotContain("hour ago", result);  // Should not be English
            Assert.DoesNotContain("2 hours ago", result); // Should not be English
            Assert.DoesNotContain("an hour ago", result); // Should not be English
        }

        [Theory]
        [InlineData("de-DE", "fünf")]
        [InlineData("fr-FR", "cinq")]
        [InlineData("es-ES", "cinco")]
        [InlineData("it-IT", "cinque")]
        [InlineData("pt-BR", "cinco")]
        public void NumberToWords_WorksCorrectlyWithCulture_ForComparison(string cultureCode, string expected)
        {
            // Arrange
            var culture = new CultureInfo(cultureCode);
            
            // Act - this reportedly works correctly according to the issue
            var result = 5.ToWords(culture: culture);
            
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("de-DE")]
        [InlineData("fr-FR")]
        [InlineData("es-ES")]
        public void ResourceManager_ShouldLoadCultureSpecificResources(string cultureCode)
        {
            // Arrange
            var culture = new CultureInfo(cultureCode);
            
            // Act - directly test the resource loading mechanism that was problematic
            var resourceExists = Resources.TryGetResource("DateHumanize_MultipleHoursAgo", culture, out var result);
            
            // Assert
            Assert.True(resourceExists, $"Resource should exist for culture {cultureCode}");
            Assert.NotNull(result);
            Assert.NotEqual("{0} hours ago", result); // Should not be English template
        }

        [Fact]
        public void DateTimeHumanize_WithNullCulture_ShouldUseCurrentUICulture()
        {
            // Arrange
            var date = DateTime.UtcNow.AddHours(-1);
            
            // Act
            var result = date.Humanize(culture: null);
            
            // Assert - should not throw and should return something meaningful
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void DateTimeHumanize_WithInvariantCulture_ShouldReturnEnglish()
        {
            // Arrange
            var date = DateTime.UtcNow.AddHours(-1);
            
            // Act
            var result = date.Humanize(culture: CultureInfo.InvariantCulture);
            
            // Assert - invariant culture should return English
            Assert.NotNull(result);
            Assert.Contains("hour ago", result);
        }

        [Fact]
        public void ResourcesGetResource_ShouldHandleBlazorWebAssemblyScenarios()
        {
            // Arrange
            var germanCulture = new CultureInfo("de-DE");
            
            // Act - this is the core method that was failing in Blazor WebAssembly
            var result = Resources.GetResource("DateHumanize_SingleHourAgo", germanCulture);
            
            // Assert
            Assert.NotNull(result);
            Assert.NotEqual("an hour ago", result); // Should not be English
            Assert.Contains("Stunde", result); // Should contain German word for hour
        }
    }
}