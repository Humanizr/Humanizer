## StringHumanizeExtensions Class

Contains extension methods for humanizing string values\.

```csharp
public static class StringHumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → StringHumanizeExtensions
### Methods

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring)'></a>

## StringHumanizeExtensions\.Humanize\(this string\) Method

Transforms a string into a human\-readable format by intelligently handling PascalCase, camelCase,
underscored\_strings, and dash\-separated\-strings, converting them into space\-separated text with
appropriate capitalization\.

```csharp
public static string Humanize(this string input);
```
#### Parameters

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be humanized\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A humanized version of the input string with spaces inserted between words and appropriate
capitalization\. Preserves all\-uppercase acronyms unchanged\.

### Example

```csharp
"PascalCaseInputString".Humanize() => "Pascal case input string"
"Underscored_input_String_is_turned_INTO_sentence".Humanize() => "Underscored input String is turned INTO sentence"
"dash-separated-string".Humanize() => "Dash separated string"
"HTML".Humanize() => "HTML"
"camelCaseText".Humanize() => "Camel case text"
```

### Remarks
The method applies several rules in order:
\- If the entire input is uppercase \(an acronym\), it returns unchanged
\- Handles freestanding underscores/dashes \(e\.g\., "some \_ string"\)
\- Splits on underscores and dashes
\- Breaks up PascalCase and camelCase text
The first letter of the result is always capitalized\.

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring,Humanizer.LetterCasing)'></a>

## StringHumanizeExtensions\.Humanize\(this string, LetterCasing\) Method

Transforms a string into a human\-readable format and applies the specified letter casing\.

```csharp
public static string Humanize(this string input, Humanizer.LetterCasing casing);
```
#### Parameters

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring,Humanizer.LetterCasing).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be humanized\. Must not be null\.

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring,Humanizer.LetterCasing).casing'></a>

`casing` [LetterCasing](Humanizer.LetterCasing.md 'Humanizer\.LetterCasing')

The desired letter casing to apply to the humanized result\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A humanized version of the input string with the specified casing applied\.

### Example

```csharp
"PascalCaseInputString".Humanize(LetterCasing.AllCaps) => "PASCAL CASE INPUT STRING"
"PascalCaseInputString".Humanize(LetterCasing.LowerCase) => "pascal case input string"
"PascalCaseInputString".Humanize(LetterCasing.Title) => "Pascal Case Input String"
```

### Remarks
This is a convenience method that combines [Humanize\(this string\)](Humanizer.StringHumanizeExtensions.md#Humanizer.StringHumanizeExtensions.Humanize(thisstring) 'Humanizer\.StringHumanizeExtensions\.Humanize\(this string\)') with [ApplyCase\(this string, LetterCasing\)](Humanizer.CasingExtensions.md#Humanizer.CasingExtensions.ApplyCase(thisstring,Humanizer.LetterCasing) 'Humanizer\.CasingExtensions\.ApplyCase\(this string, Humanizer\.LetterCasing\)')\.