# String Transformations

String transformations provide a flexible, extensible way to change the casing or format of strings. Unlike the `LetterCasing` enum which limits you to built-in options, the `IStringTransformer` interface lets you implement domain-specific transformations and compose them together.

## Overview

The `Transform` extension method applies one or more transformers to a string, in order. Humanizer ships with four built-in transformers on the `To` class:

- **`To.TitleCase`** - Capitalizes the first letter of each word
- **`To.SentenceCase`** - Capitalizes only the first letter
- **`To.UpperCase`** - All uppercase
- **`To.LowerCase`** - All lowercase

## Basic Usage

```csharp
using Humanizer;

"Sentence casing".Transform(To.LowerCase)    // => "sentence casing"
"Sentence casing".Transform(To.SentenceCase) // => "Sentence casing"
"Sentence casing".Transform(To.TitleCase)    // => "Sentence Casing"
"Sentence casing".Transform(To.UpperCase)    // => "SENTENCE CASING"
```

## Title Case

Title case capitalizes the first letter of each word while preserving fully uppercase words (treated as acronyms):

```csharp
"a great movie".Transform(To.TitleCase)
    // => "A Great Movie"

"honors UPPER case".Transform(To.TitleCase)
    // => "Honors UPPER Case"

"INvalid caSEs arE corrected".Transform(To.TitleCase)
    // => "Invalid Cases Are Corrected"

"apostrophe's aren't capitalized".Transform(To.TitleCase)
    // => "Apostrophe's Aren't Capitalized"
```

## Sentence Case

Sentence case capitalizes only the first letter and leaves everything else unchanged:

```csharp
"lower case statement".Transform(To.SentenceCase)
    // => "Lower case statement"

"honors UPPER case".Transform(To.SentenceCase)
    // => "Honors UPPER case"
```

## Chaining Transformers

You can pass multiple transformers to `Transform`. They are applied in order from left to right:

```csharp
"HUMANIZER".Transform(To.LowerCase, To.TitleCase)
    // => "Humanizer"
```

## Culture-Aware Transformations

All built-in transformers implement `ICulturedStringTransformer`, so you can pass a specific `CultureInfo` for locale-sensitive casing rules:

```csharp
"istanbul".Transform(new CultureInfo("tr-TR"), To.TitleCase)
    // => "İstanbul" (Turkish dotted capital I)
```

## Custom Transformers

Implement `IStringTransformer` for simple transformations or `ICulturedStringTransformer` for culture-aware ones:

```csharp
public interface IStringTransformer
{
    string Transform(string input);
}

public interface ICulturedStringTransformer : IStringTransformer
{
    string Transform(string input, CultureInfo culture);
}
```

For example, a transformer that reverses text:

```csharp
public class ReverseTransformer : IStringTransformer
{
    public string Transform(string input)
    {
        var chars = input.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }
}

// Usage:
"hello".Transform(new ReverseTransformer()) // => "olleh"
```

## Related Topics

- [String Humanization](string-humanization.md) - Convert PascalCase/camelCase to readable text
- [String Dehumanization](string-dehumanization.md) - Convert back to PascalCase
- [Inflector Methods](inflector-methods.md) - Pascalize, Camelize, Kebaberize, etc.
