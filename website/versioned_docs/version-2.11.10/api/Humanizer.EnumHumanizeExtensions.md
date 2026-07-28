## EnumHumanizeExtensions Class

Contains extension methods for humanizing Enums

```csharp
public static class EnumHumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → EnumHumanizeExtensions
### Methods

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum)'></a>

## EnumHumanizeExtensions\.Humanize\(this Enum\) Method

Turns an enum member into a human readable string; e\.g\. AnonymousUser \-\> Anonymous user\. It also honors DescriptionAttribute data annotation

```csharp
public static string Humanize(this System.Enum input);
```
#### Parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum).input'></a>

`input` [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')

The enum member to be humanized

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing)'></a>

## EnumHumanizeExtensions\.Humanize\(this Enum, LetterCasing\) Method

Turns an enum member into a human readable string with the provided casing; e\.g\. AnonymousUser with Title casing \-\> Anonymous User\. It also honors DescriptionAttribute data annotation

```csharp
public static string Humanize(this System.Enum input, Humanizer.LetterCasing casing);
```
#### Parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing).input'></a>

`input` [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')

The enum member to be humanized

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing).casing'></a>

`casing` [LetterCasing](Humanizer.LetterCasing.md 'Humanizer\.LetterCasing')

The casing to use for humanizing the enum member

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')