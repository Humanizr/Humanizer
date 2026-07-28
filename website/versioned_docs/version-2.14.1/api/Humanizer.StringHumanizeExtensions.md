## StringHumanizeExtensions Class

Contains extension methods for humanizing string values\.

```csharp
public static class StringHumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → StringHumanizeExtensions
### Methods

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring)'></a>

## StringHumanizeExtensions\.Humanize\(this string\) Method

Humanizes the input string; e\.g\. Underscored\_input\_String\_is\_turned\_INTO\_sentence \-\> 'Underscored input String is turned INTO sentence'

```csharp
public static string Humanize(this string input);
```
#### Parameters

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be humanized

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring,Humanizer.LetterCasing)'></a>

## StringHumanizeExtensions\.Humanize\(this string, LetterCasing\) Method

Humanized the input string based on the provided casing

```csharp
public static string Humanize(this string input, Humanizer.LetterCasing casing);
```
#### Parameters

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring,Humanizer.LetterCasing).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be humanized

<a name='Humanizer.StringHumanizeExtensions.Humanize(thisstring,Humanizer.LetterCasing).casing'></a>

`casing` [LetterCasing](Humanizer.LetterCasing.md 'Humanizer\.LetterCasing')

The desired casing for the output

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')