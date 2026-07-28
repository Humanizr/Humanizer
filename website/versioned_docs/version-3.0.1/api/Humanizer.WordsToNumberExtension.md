## WordsToNumberExtension Class

Transform humanized string to number; e\.g\. one =\> 1

```csharp
public static class WordsToNumberExtension
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → WordsToNumberExtension
### Methods

<a name='Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo)'></a>

## WordsToNumberExtension\.ToNumber\(this string, CultureInfo\) Method

Converts a spelled\-out number string to its integer representation\.

```csharp
public static int ToNumber(this string words, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The spelled\-out number \(e\.g\., "three hundred twenty\-one", "forty\-two"\)\.
Must not be null\.

<a name='Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for parsing\. Different cultures may have different word representations
for numbers \(e\.g\., "twenty" in English vs\. "vingt" in French\)\.

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
The integer value represented by the words\.

#### Exceptions

[System\.FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception 'System\.FormatException')  
Thrown when the input contains unrecognized words or cannot be parsed as a number\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [words](Humanizer.WordsToNumberExtension.md#Humanizer.WordsToNumberExtension.ToNumber(thisstring,System.Globalization.CultureInfo).words 'Humanizer\.WordsToNumberExtension\.ToNumber\(this string, System\.Globalization\.CultureInfo\)\.words') is null\.

### Example

```csharp
// English (en-US)
"three hundred twenty-one".ToNumber(new CultureInfo("en-US")) => 321
"forty-two".ToNumber(new CultureInfo("en-US")) => 42
"one thousand".ToNumber(new CultureInfo("en-US")) => 1000

// Invalid input throws exception
"xyz".ToNumber(new CultureInfo("en-US")) => throws FormatException
```

### Remarks
This method strictly parses the input and throws an exception if any word is not recognized\.
For a non\-throwing version, use [TryToNumber\(this string, int, CultureInfo\)](Humanizer.WordsToNumberExtension.md#Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo) 'Humanizer\.WordsToNumberExtension\.TryToNumber\(this string, int, System\.Globalization\.CultureInfo\)')\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo)'></a>

## WordsToNumberExtension\.TryToNumber\(this string, int, CultureInfo\) Method

Attempts to convert a spelled\-out number string to its integer representation without throwing exceptions\.

```csharp
public static bool TryToNumber(this string words, out int parsedNumber, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The spelled\-out number \(e\.g\., "forty\-two", "one hundred"\)\.
Must not be null\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo).parsedNumber'></a>

`parsedNumber` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

When this method returns, contains the integer value represented by the words if the conversion succeeded,
or 0 if the conversion failed\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for parsing\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the conversion was successful; otherwise, `false`\.

### Example

```csharp
// Successful conversion
"forty-two".TryToNumber(out int result, new CultureInfo("en-US")) => returns true, result = 42

// Failed conversion
"xyz".TryToNumber(out int result, new CultureInfo("en-US")) => returns false, result = 0
```

### Remarks
This is the recommended method for parsing when you're not sure if the input is valid\.
It does not throw exceptions on invalid input\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo,string)'></a>

## WordsToNumberExtension\.TryToNumber\(this string, int, CultureInfo, string\) Method

Attempts to convert a spelled\-out number string to its integer representation and provides
the first unrecognized word if the conversion fails\.

```csharp
public static bool TryToNumber(this string words, out int parsedNumber, System.Globalization.CultureInfo culture, out string? unrecognizedWord);
```
#### Parameters

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo,string).words'></a>

`words` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The spelled\-out number \(e\.g\., "one thousand one"\)\.
Must not be null\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo,string).parsedNumber'></a>

`parsedNumber` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

When this method returns, contains the integer value represented by the words if the conversion succeeded,
or 0 if the conversion failed\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use for parsing\.

<a name='Humanizer.WordsToNumberExtension.TryToNumber(thisstring,int,System.Globalization.CultureInfo,string).unrecognizedWord'></a>

`unrecognizedWord` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

When this method returns `false`, contains the first unrecognized word found in the input\.
When this method returns `true`, this parameter is set to `null`\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
`true` if the conversion was successful; otherwise, `false`\.

### Example

```csharp
// Successful conversion
"one thousand".TryToNumber(out int result, new CultureInfo("en-US"), out string? badWord) 
  => returns true, result = 1000, badWord = null

// Failed conversion with unrecognized word
"one xyz three".TryToNumber(out int result, new CultureInfo("en-US"), out string? badWord)
  => returns false, result = 0, badWord = "xyz"
```

### Remarks
This overload is useful for debugging or providing detailed error messages to users,
as it identifies which specific word caused the parsing failure\.