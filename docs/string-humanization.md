# String Humanization

String humanization transforms computerized strings (like class names, method names, or property names) into human-readable text. This is particularly useful when displaying programming identifiers to end users.

## Overview

The `Humanize` extension method intelligently handles:
- **PascalCase**: `PascalCaseString` → `Pascal case string`
- **camelCase**: `camelCaseString` → `Camel case string`
- **Underscores**: `underscored_string` → `Underscored string`
- **Dashes**: `dash-separated-string` → `Dash separated string`

## Basic Usage

```csharp
using Humanizer;

"PascalCaseInputStringIsTurnedIntoSentence".Humanize() 
    // => "Pascal case input string is turned into sentence"

"Underscored_input_string_is_turned_into_sentence".Humanize() 
    // => "Underscored input string is turned into sentence"

"dash-separated-string".Humanize() 
    // => "Dash separated string"
```

## Acronym Handling

Strings containing only uppercase letters are treated as acronyms and left unchanged:

```csharp
"HTML".Humanize() // => "HTML"
"HUMANIZER".Humanize() // => "HUMANIZER"
```

To force humanization of all-caps strings, use the `Transform` method:

```csharp
"HUMANIZER".Transform(To.LowerCase, To.TitleCase) // => "Humanizer"
```

## Letter Casing

Control the output casing:

```csharp
"CanReturnTitleCase".Humanize(LetterCasing.Title) 
    // => "Can Return Title Case"

"CanReturnLowerCase".Humanize(LetterCasing.LowerCase) 
    // => "can return lower case"

"CanHumanizeIntoUpperCase".Humanize(LetterCasing.AllCaps) 
    // => "CAN HUMANIZE INTO UPPER CASE"

"some string".Humanize(LetterCasing.Sentence) 
    // => "Some string"
```

Available casing options:
- `LetterCasing.Title` - Capitalizes the first letter of each word
- `LetterCasing.Sentence` - Capitalizes only the first letter
- `LetterCasing.AllCaps` - All uppercase
- `LetterCasing.LowerCase` - All lowercase

## How It Works

The humanization process applies several rules in order:

1. If the entire input is uppercase (an acronym), it returns unchanged
2. Handles freestanding underscores/dashes (e.g., "some _ string")
3. Splits on underscores and dashes
4. Breaks up PascalCase and camelCase text
5. Capitalizes the first letter of the result

## Real-World Examples

### Display Test Method Names

```csharp
// In a testing framework
public void ItShouldCalculateTheTotalPrice()
{
    // Test implementation
}

// Display to user:
nameof(ItShouldCalculateTheTotalPrice).Humanize()
// => "It should calculate the total price"
```

### Display Property Names in UI

```csharp
public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}

// Generate form labels automatically
nameof(User.FirstName).Humanize() // => "First name"
nameof(User.DateOfBirth).Humanize() // => "Date of birth"
```

### Convert API Field Names

```csharp
// API returns: "account_status", "last_login_date"
var fields = new[] { "account_status", "last_login_date" };

foreach (var field in fields)
{
    Console.WriteLine(field.Humanize());
}
// Output:
// Account status
// Last login date
```

## Version 3.0 Behavioral Change

In version 3.0, `Humanize` and `Titleize` now preserve input strings that contain no recognized letters (e.g., special characters, unrecognized Unicode scripts) instead of returning an empty string:

```csharp
// Before v3.0: returned ""
// v3.0 and later: returns "@@"
"@@".Humanize() // => "@@"

// Cyrillic and other Unicode scripts are preserved
"Майк".Titleize() // => "Майк"
```

## Related Topics

- [String Dehumanization](string-dehumanization.md) - Convert back to PascalCase
- [String Transformations](string-transformations.md) - Custom transformations
- [Inflector Methods](inflector-methods.md) - Titleize, Pascalize, Camelize, etc.
