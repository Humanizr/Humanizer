## HeadingExtensions Class

Contains extensions to transform a number indicating a heading into the 
textual representation of the heading\.

```csharp
public static class HeadingExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → HeadingExtensions
### Methods

<a name='Humanizer.HeadingExtensions.FromAbbreviatedHeading(thisstring)'></a>

## HeadingExtensions\.FromAbbreviatedHeading\(this string\) Method

Returns a heading based on the short textual representation of the heading\.

```csharp
public static double FromAbbreviatedHeading(this string heading);
```
#### Parameters

<a name='Humanizer.HeadingExtensions.FromAbbreviatedHeading(thisstring).heading'></a>

`heading` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The short textual representation of a heading

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')  
The heading\. \-1 if the heading could not be parsed\.

<a name='Humanizer.HeadingExtensions.FromAbbreviatedHeading(thisstring,System.Globalization.CultureInfo)'></a>

## HeadingExtensions\.FromAbbreviatedHeading\(this string, CultureInfo\) Method

Returns a heading based on the short textual representation of the heading\.

```csharp
public static double FromAbbreviatedHeading(this string heading, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.HeadingExtensions.FromAbbreviatedHeading(thisstring,System.Globalization.CultureInfo).heading'></a>

`heading` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The short textual representation of a heading

<a name='Humanizer.HeadingExtensions.FromAbbreviatedHeading(thisstring,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture of the heading

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')  
The heading\. \-1 if the heading could not be parsed\.

<a name='Humanizer.HeadingExtensions.FromHeadingArrow(thischar)'></a>

## HeadingExtensions\.FromHeadingArrow\(this char\) Method

Returns a heading based on the heading arrow\.

```csharp
public static double FromHeadingArrow(this char heading);
```
#### Parameters

<a name='Humanizer.HeadingExtensions.FromHeadingArrow(thischar).heading'></a>

`heading` [System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.HeadingExtensions.FromHeadingArrow(thisstring)'></a>

## HeadingExtensions\.FromHeadingArrow\(this string\) Method

Returns a heading based on the heading arrow\.

```csharp
public static double FromHeadingArrow(this string heading);
```
#### Parameters

<a name='Humanizer.HeadingExtensions.FromHeadingArrow(thisstring).heading'></a>

`heading` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.HeadingExtensions.ToHeading(thisdouble,Humanizer.HeadingStyle,System.Globalization.CultureInfo)'></a>

## HeadingExtensions\.ToHeading\(this double, HeadingStyle, CultureInfo\) Method

Returns a textual representation of the heading\.

This representation has a maximum deviation of 11\.25 degrees\.

```csharp
public static string ToHeading(this double heading, Humanizer.HeadingStyle style=Humanizer.HeadingStyle.Abbreviated, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.HeadingExtensions.ToHeading(thisdouble,Humanizer.HeadingStyle,System.Globalization.CultureInfo).heading'></a>

`heading` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The heading value

<a name='Humanizer.HeadingExtensions.ToHeading(thisdouble,Humanizer.HeadingStyle,System.Globalization.CultureInfo).style'></a>

`style` [HeadingStyle](Humanizer.HeadingStyle.md 'Humanizer\.HeadingStyle')

Whether to return a short result or not\. [HeadingStyle](Humanizer.HeadingStyle.md 'Humanizer\.HeadingStyle')

<a name='Humanizer.HeadingExtensions.ToHeading(thisdouble,Humanizer.HeadingStyle,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to return the textual representation in

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A textual representation of the heading

<a name='Humanizer.HeadingExtensions.ToHeadingArrow(thisdouble)'></a>

## HeadingExtensions\.ToHeadingArrow\(this double\) Method

Returns a char arrow indicating the heading\.

This representation has a maximum deviation of 22\.5 degrees\.

```csharp
public static char ToHeadingArrow(this double heading);
```
#### Parameters

<a name='Humanizer.HeadingExtensions.ToHeadingArrow(thisdouble).heading'></a>

`heading` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')  
The heading arrow\.