## DateToOrdinalWordsExtensions Class

Humanizes DateTime into human readable sentence

```csharp
public static class DateToOrdinalWordsExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DateToOrdinalWordsExtensions
### Methods

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateOnly)'></a>

## DateToOrdinalWordsExtensions\.ToOrdinalWords\(this DateOnly\) Method

Converts a [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly') to its ordinal words representation \(e\.g\., "1st of January, 2023"\)\.

```csharp
public static string ToOrdinalWords(this System.DateOnly input);
```
#### Parameters

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateOnly).input'></a>

`input` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

The date to be converted to ordinal words\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string containing the date expressed in ordinal words format, culture\-specific\.
For English: "1st of January, 2023", "22nd of December, 2020", etc\.

### Example

```csharp
new DateOnly(2023, 1, 1).ToOrdinalWords() => "1st of January, 2023" (in en-US culture)
new DateOnly(2020, 12, 22).ToOrdinalWords() => "22nd of December, 2020" (in en-US culture)
```

### Remarks
The format and style of ordinal words depends on the current culture\.
Uses the configured date\-only\-to\-ordinal\-words converter for conversion\.
This method is available only on \.NET 6\.0 and later\.

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateOnly,Humanizer.GrammaticalCase)'></a>

## DateToOrdinalWordsExtensions\.ToOrdinalWords\(this DateOnly, GrammaticalCase\) Method

Converts a [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly') to its ordinal words representation using the specified grammatical case\.

```csharp
public static string ToOrdinalWords(this System.DateOnly input, Humanizer.GrammaticalCase grammaticalCase);
```
#### Parameters

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateOnly,Humanizer.GrammaticalCase).input'></a>

`input` [System\.DateOnly](https://learn.microsoft.com/en-us/dotnet/api/system.dateonly 'System\.DateOnly')

The date to be converted to ordinal words\.

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateOnly,Humanizer.GrammaticalCase).grammaticalCase'></a>

`grammaticalCase` [GrammaticalCase](Humanizer.GrammaticalCase.md 'Humanizer\.GrammaticalCase')

The grammatical case to use for the output words \(e\.g\., Nominative, Genitive, etc\.\)\.
This is particularly important for languages with case systems like Russian, Polish, etc\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string containing the date expressed in ordinal words format in the specified grammatical case\.

### Example

```csharp
// In Russian culture:
date.ToOrdinalWords(GrammaticalCase.Nominative) => different form than
date.ToOrdinalWords(GrammaticalCase.Genitive)
```

### Remarks
The grammatical case parameter is primarily used by languages that have case systems\.
For languages without grammatical cases \(like English\), this parameter has no effect\.
This method is available only on \.NET 6\.0 and later\.

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateTime)'></a>

## DateToOrdinalWordsExtensions\.ToOrdinalWords\(this DateTime\) Method

Converts a [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime') to its ordinal words representation \(e\.g\., "1st of January, 2023"\)\.

```csharp
public static string ToOrdinalWords(this System.DateTime input);
```
#### Parameters

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateTime).input'></a>

`input` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The date to be converted to ordinal words\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string containing the date expressed in ordinal words format, culture\-specific\.
For English: "1st of January, 2023", "22nd of December, 2020", etc\.

### Example

```csharp
new DateTime(2023, 1, 1).ToOrdinalWords() => "1st of January, 2023" (in en-US culture)
new DateTime(2020, 12, 22).ToOrdinalWords() => "22nd of December, 2020" (in en-US culture)
```

### Remarks
The format and style of ordinal words depends on the current culture\.
Uses the configured date\-to\-ordinal\-words converter for conversion\.

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateTime,Humanizer.GrammaticalCase)'></a>

## DateToOrdinalWordsExtensions\.ToOrdinalWords\(this DateTime, GrammaticalCase\) Method

Converts a [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime') to its ordinal words representation using the specified grammatical case\.

```csharp
public static string ToOrdinalWords(this System.DateTime input, Humanizer.GrammaticalCase grammaticalCase);
```
#### Parameters

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateTime,Humanizer.GrammaticalCase).input'></a>

`input` [System\.DateTime](https://learn.microsoft.com/en-us/dotnet/api/system.datetime 'System\.DateTime')

The date to be converted to ordinal words\.

<a name='Humanizer.DateToOrdinalWordsExtensions.ToOrdinalWords(thisSystem.DateTime,Humanizer.GrammaticalCase).grammaticalCase'></a>

`grammaticalCase` [GrammaticalCase](Humanizer.GrammaticalCase.md 'Humanizer\.GrammaticalCase')

The grammatical case to use for the output words \(e\.g\., Nominative, Genitive, etc\.\)\.
This is particularly important for languages with case systems like Russian, Polish, etc\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string containing the date expressed in ordinal words format in the specified grammatical case\.

### Example

```csharp
// In Russian culture:
date.ToOrdinalWords(GrammaticalCase.Nominative) => different form than
date.ToOrdinalWords(GrammaticalCase.Genitive)
```

### Remarks
The grammatical case parameter is primarily used by languages that have case systems\.
For languages without grammatical cases \(like English\), this parameter has no effect\.