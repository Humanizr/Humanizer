## TimeSpanDehumanizeExtensions Class

Contains extension methods for parsing invariant duration text into a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')\.

```csharp
public static class TimeSpanDehumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TimeSpanDehumanizeExtensions
### Methods

<a name='Humanizer.TimeSpanDehumanizeExtensions.DehumanizeTimeSpan(thisstring)'></a>

## TimeSpanDehumanizeExtensions\.DehumanizeTimeSpan\(this string\) Method

Parses a standard invariant [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') value or compact duration tokens using
`ms`, `s`, `m`, `h`, `d`, and `w`\.

```csharp
public static System.TimeSpan DehumanizeTimeSpan(this string input);
```
#### Parameters

<a name='Humanizer.TimeSpanDehumanizeExtensions.DehumanizeTimeSpan(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The duration text to parse\.

#### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
The parsed duration\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [input](Humanizer.TimeSpanDehumanizeExtensions.md#Humanizer.TimeSpanDehumanizeExtensions.DehumanizeTimeSpan(thisstring).input 'Humanizer\.TimeSpanDehumanizeExtensions\.DehumanizeTimeSpan\(this string\)\.input') is null\.

[System\.FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception 'System\.FormatException')  
Thrown when [input](Humanizer.TimeSpanDehumanizeExtensions.md#Humanizer.TimeSpanDehumanizeExtensions.DehumanizeTimeSpan(thisstring).input 'Humanizer\.TimeSpanDehumanizeExtensions\.DehumanizeTimeSpan\(this string\)\.input') is not a supported duration\.

### Remarks
Compact tokens are culture\-invariant, may be separated by whitespace, and may have one leading sign\.
Units may repeat and appear in any order; their values are added\. Each token must resolve to whole ticks\.
A week is seven days\.
Colon\-separated values use the standard invariant [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') interpretation\.

<a name='Humanizer.TimeSpanDehumanizeExtensions.TryDehumanizeTimeSpan(thisstring,System.TimeSpan)'></a>

## TimeSpanDehumanizeExtensions\.TryDehumanizeTimeSpan\(this string, TimeSpan\) Method

Tries to parse a standard invariant [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') value or compact duration tokens using
`ms`, `s`, `m`, `h`, `d`, and `w`\.

```csharp
public static bool TryDehumanizeTimeSpan(this string? input, out System.TimeSpan result);
```
#### Parameters

<a name='Humanizer.TimeSpanDehumanizeExtensions.TryDehumanizeTimeSpan(thisstring,System.TimeSpan).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The duration text to parse\.

<a name='Humanizer.TimeSpanDehumanizeExtensions.TryDehumanizeTimeSpan(thisstring,System.TimeSpan).result'></a>

`result` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

The parsed duration, or [System\.TimeSpan\.Zero](https://learn.microsoft.com/en-us/dotnet/api/system.timespan.zero 'System\.TimeSpan\.Zero') when parsing fails\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when parsing succeeds; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

### Remarks
Compact tokens are culture\-invariant, may be separated by whitespace, and may have one leading sign\.
Units may repeat and appear in any order; their values are added\. Each token must resolve to whole ticks\.
A week is seven days\.
Colon\-separated values use the standard invariant [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') interpretation\.