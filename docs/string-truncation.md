# String Truncation

Humanizer provides intelligent string truncation with multiple strategies to handle different use cases.

## Overview

Truncation is useful when you need to limit string length for display purposes while maintaining readability. Humanizer offers several truncation strategies and uses the `…` character (one character) instead of `"..."` (three characters) to maximize visible text.

## Basic Usage

```csharp
using Humanizer;

"Long text to truncate".Truncate(10) 
    // => "Long text…"

"Long text to truncate".Truncate(10, "---") 
    // => "Long te---"
```

## Truncation Strategies

### Fixed Length (Default)

Truncates to a specific total length including the truncation indicator:

```csharp
"Long text to truncate".Truncate(10, Truncator.FixedLength) 
    // => "Long text…"

"Long text to truncate".Truncate(10, "---", Truncator.FixedLength) 
    // => "Long te---"
```

### Fixed Number of Characters

Truncates to a specific number of alphanumeric characters:

```csharp
"Long text to truncate".Truncate(6, Truncator.FixedNumberOfCharacters) 
    // => "Long t…"

"Long text to truncate".Truncate(6, "---", Truncator.FixedNumberOfCharacters) 
    // => "Lon---"
```

### Fixed Number of Words

Truncates to a specific number of words:

```csharp
"Long text to truncate".Truncate(2, Truncator.FixedNumberOfWords) 
    // => "Long text…"

"Long text to truncate".Truncate(2, "---", Truncator.FixedNumberOfWords) 
    // => "Long text---"
```

### Dynamic Length - Preserve Words

Similar to fixed length but attempts to preserve whole words:

```csharp
"Long text to truncate".Truncate(10, Truncator.DynamicLengthAndPreserveWords) 
    // => "Long text…"

"Long text to truncate".Truncate(10, "---", Truncator.DynamicLengthAndPreserveWords) 
    // => "Long---"
```

### Dynamic Characters - Preserve Words

Similar to fixed number of characters but attempts to preserve whole words:

```csharp
"Long text to truncate".Truncate(6, Truncator.DynamicNumberOfCharactersAndPreserveWords) 
    // => "Long…"

"Long text to truncate".Truncate(6, "---", Truncator.DynamicNumberOfCharactersAndPreserveWords) 
    // => "---"
```

## Truncation Direction

By default, truncation happens from the right (end) of the string. You can truncate from the left (beginning) using `TruncateFrom.Left`:

```csharp
"Long text to truncate".Truncate(10, Truncator.FixedLength, TruncateFrom.Left) 
    // => "… truncate"

"Long text to truncate".Truncate(10, "---", Truncator.FixedLength, TruncateFrom.Left) 
    // => "---runcate"

"Long text to truncate".Truncate(2, Truncator.FixedNumberOfWords, TruncateFrom.Left) 
    // => "…to truncate"
```

## Custom Truncation Indicators

You can use any string as a truncation indicator:

```csharp
"Long text to truncate".Truncate(15, " [more]") 
    // => "Long [more]"

"Long text to truncate".Truncate(15, "...") 
    // => "Long text..."
```

## Custom Truncators

Implement the `ITruncator` interface to create custom truncation logic:

```csharp
public interface ITruncator
{
    string Truncate(string value, int length, string truncationString, TruncateFrom truncateFrom = TruncateFrom.Right);
}
```

Example custom truncator:

```csharp
public class SentenceTruncator : ITruncator
{
    public string Truncate(string value, int length, string truncationString, TruncateFrom truncateFrom)
    {
        // Custom logic to truncate at sentence boundaries
        // Implementation details...
    }
}

// Usage
"First sentence. Second sentence. Third sentence."
    .Truncate(30, new SentenceTruncator());
```

## Real-World Examples

### Truncate Article Previews

```csharp
var article = "This is a very long article that needs to be truncated for the preview.";
var preview = article.Truncate(50, Truncator.FixedNumberOfWords);
// => "This is a very long article that needs to be…"
```

### Truncate File Names

```csharp
var fileName = "Very_Long_File_Name_That_Needs_Truncation.pdf";
var displayName = fileName.Truncate(20, "…", Truncator.DynamicLengthAndPreserveWords);
// => "Very_Long_File_Name…"
```

### Truncate User Comments

```csharp
var comment = "This is a user comment that might be too long to display in a notification.";
var notification = comment.Truncate(40);
// => "This is a user comment that might be…"
```

## Best Practices

1. **Choose the right strategy**: 
   - Use `FixedLength` for consistent visual length
   - Use `FixedNumberOfWords` when word boundaries matter
   - Use dynamic strategies when readability is more important than exact length

2. **Consider the truncation indicator**:
   - Use `…` (single character) for space efficiency
   - Use `"..."` if your audience expects it
   - Use `" [more]"` or similar for clarity

3. **Test with your actual content**: Different text may truncate differently depending on word boundaries

## Related Topics

- [String Humanization](string-humanization.md)
- [String Transformations](string-transformations.md)
