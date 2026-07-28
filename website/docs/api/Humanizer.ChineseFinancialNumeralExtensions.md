## ChineseFinancialNumeralExtensions Class

Contains extension methods for converting integers to Chinese financial characters\.

```csharp
public static class ChineseFinancialNumeralExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ChineseFinancialNumeralExtensions
### Methods

<a name='Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thisint,System.Globalization.CultureInfo)'></a>

## ChineseFinancialNumeralExtensions\.ToChineseFinancialCharacters\(this int, CultureInfo\) Method

Converts the given value to Chinese financial characters\.

```csharp
public static string ToChineseFinancialCharacters(this int number, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thisint,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The value to convert\.

<a name='Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thisint,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

A culture in the `zh-Hans` or `zh-Hant` hierarchy, which selects simplified or
traditional financial characters\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The value written with Chinese financial digits and units\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](Humanizer.ChineseFinancialNumeralExtensions.md#Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thisint,System.Globalization.CultureInfo).culture 'Humanizer\.ChineseFinancialNumeralExtensions\.ToChineseFinancialCharacters\(this int, System\.Globalization\.CultureInfo\)\.culture') is `null`\.

[System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException')  
Thrown when [culture](Humanizer.ChineseFinancialNumeralExtensions.md#Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thisint,System.Globalization.CultureInfo).culture 'Humanizer\.ChineseFinancialNumeralExtensions\.ToChineseFinancialCharacters\(this int, System\.Globalization\.CultureInfo\)\.culture') does not resolve to simplified or traditional Chinese\.

<a name='Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thislong,System.Globalization.CultureInfo)'></a>

## ChineseFinancialNumeralExtensions\.ToChineseFinancialCharacters\(this long, CultureInfo\) Method

Converts the given value to Chinese financial characters\.

```csharp
public static string ToChineseFinancialCharacters(this long number, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thislong,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The value to convert\.

<a name='Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thislong,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

A culture in the `zh-Hans` or `zh-Hant` hierarchy, which selects simplified or
traditional financial characters\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The value written with Chinese financial digits and units\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](Humanizer.ChineseFinancialNumeralExtensions.md#Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thislong,System.Globalization.CultureInfo).culture 'Humanizer\.ChineseFinancialNumeralExtensions\.ToChineseFinancialCharacters\(this long, System\.Globalization\.CultureInfo\)\.culture') is `null`\.

[System\.NotSupportedException](https://learn.microsoft.com/en-us/dotnet/api/system.notsupportedexception 'System\.NotSupportedException')  
Thrown when [culture](Humanizer.ChineseFinancialNumeralExtensions.md#Humanizer.ChineseFinancialNumeralExtensions.ToChineseFinancialCharacters(thislong,System.Globalization.CultureInfo).culture 'Humanizer\.ChineseFinancialNumeralExtensions\.ToChineseFinancialCharacters\(this long, System\.Globalization\.CultureInfo\)\.culture') does not resolve to simplified or traditional Chinese\.

### Remarks
Supports the full [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64') range\. This method does not add currency names or units
such as yuan, jiao, or fen\.