# Extensibility

Humanizer is designed to be extensible, allowing you to customize and extend its behavior to meet your specific needs.

## Custom String Transformers

Implement `IStringTransformer` to create custom string transformations:

```csharp
public interface IStringTransformer
{
    string Transform(string input);
}
```

### Example: Custom Title Case

```csharp
public class MyTitleCase : IStringTransformer
{
    private readonly HashSet<string> _articlesAndPrepositions = new()
    {
        "a", "an", "the", "and", "but", "or", "for", "nor", "on", "at", "to", "from", "by"
    };

    public string Transform(string input)
    {
        var words = input.Split(' ');
        for (int i = 0; i < words.Length; i++)
        {
            // Always capitalize first word
            if (i == 0 || !_articlesAndPrepositions.Contains(words[i].ToLower()))
            {
                words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
            }
        }
        return string.Join(" ", words);
    }
}

// Usage
"the quick brown fox".Transform(new MyTitleCase());
// => "The Quick Brown Fox"
```

The advantage over the built-in `LetterCasing` enum is complete control over the transformation logic.

## Custom Truncators

Implement `ITruncator` to create custom truncation strategies:

```csharp
public interface ITruncator
{
    string Truncate(string value, int length, string truncationString, 
                   TruncateFrom truncateFrom = TruncateFrom.Right);
}
```

### Example: Sentence Boundary Truncator

```csharp
public class SentenceTruncator : ITruncator
{
    public string Truncate(string value, int length, string truncationString, 
                          TruncateFrom truncateFrom)
    {
        if (value.Length <= length)
            return value;

        // Find the last sentence boundary before the length limit
        var truncated = value.Substring(0, length);
        var lastPeriod = truncated.LastIndexOf('.');
        
        if (lastPeriod > 0)
        {
            return truncated.Substring(0, lastPeriod + 1);
        }

        // Fall back to word boundary
        var lastSpace = truncated.LastIndexOf(' ');
        if (lastSpace > 0)
        {
            return truncated.Substring(0, lastSpace) + truncationString;
        }

        return truncated + truncationString;
    }
}

// Usage
"First sentence. Second sentence. Third sentence."
    .Truncate(30, "...", new SentenceTruncator());
// => "First sentence."
```

## Custom Vocabularies

Extend the pluralization/singularization vocabulary:

```csharp
// Add irregular word
Vocabularies.Default.AddIrregular("person", "people");

// Add uncountable word
Vocabularies.Default.AddUncountable("equipment");

// Add plural rule
Vocabularies.Default.AddPlural("(quiz)$", "$1zes");

// Add singular rule
Vocabularies.Default.AddSingular("(vert|ind)ices$", "$1ex");
```

### Match Ending Control

```csharp
// Match only "person" (not "salesperson")
Vocabularies.Default.AddIrregular("person", "people", matchEnding: false);

// Match "person" and "salesperson"
Vocabularies.Default.AddIrregular("person", "people", matchEnding: true);
```

## Custom Number to Words Converters

Implement language-specific number to words conversion:

```csharp
public interface INumberToWordsConverter
{
    string Convert(long number);
    string Convert(long number, GrammaticalGender gender);
    string ConvertToOrdinal(int number);
    string ConvertToOrdinal(int number, GrammaticalGender gender);
}
```

Register your converter:

```csharp
Configurator.NumberToWordsConverters.Register("my-culture", 
    new MyNumberToWordsConverter());
```

## Custom Formatters

Implement custom date/time/number formatting:

```csharp
public interface IFormatter
{
    string DateHumanize(DateTime value, DateTime? comparisonBase, 
                       CultureInfo culture);
    string TimeSpanHumanize(TimeSpan timeSpan, int precision, 
                           CultureInfo culture);
    // ... other methods
}
```

Register your formatter:

```csharp
Configurator.Formatters.Register("my-culture", new MyFormatter());
```

## Configuration

Customize Humanizer's behavior globally:

```csharp
// Custom enum description property name
Configurator.EnumDescriptionPropertyLocator = p => p.Name == "Info";

// Custom collection separator
Configurator.CollectionFormatters.Register("my-culture", 
    new MyCollectionFormatter());
```

## Best Practices

1. **Implement interfaces rather than extending classes** - This ensures compatibility with future versions

2. **Register custom components at application startup** - Do this once rather than on every use

3. **Thread safety** - Make custom implementations thread-safe if they'll be used concurrently

4. **Test thoroughly** - Custom implementations should have comprehensive test coverage

5. **Consider culture** - If your custom component is culture-specific, register it appropriately

## Related Topics

- [Custom Vocabularies](custom-vocabularies.md) - Detailed pluralization customization
- [Localization](localization.md) - Multi-language support
- [Configuration](configuration.md) - Global configuration options
