## OrdinalizeExtensions Class

Ordinalize extensions

```csharp
public static class OrdinalizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → OrdinalizeExtensions
### Methods

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint)'></a>

## OrdinalizeExtensions\.Ordinalize\(this int\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this int number);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender)'></a>

## OrdinalizeExtensions\.Ordinalize\(this int, GrammaticalGender\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.
Gender for Brazilian Portuguese locale
1\.Ordinalize\(GrammaticalGender\.Masculine\) \-\> "1º"
1\.Ordinalize\(GrammaticalGender\.Feminine\) \-\> "1ª"

```csharp
public static string Ordinalize(this int number, Humanizer.GrammaticalGender gender);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## OrdinalizeExtensions\.Ordinalize\(this int, GrammaticalGender, CultureInfo\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.
Gender for Brazilian Portuguese locale
1\.Ordinalize\(GrammaticalGender\.Masculine\) \-\> "1º"
1\.Ordinalize\(GrammaticalGender\.Feminine\) \-\> "1ª"

```csharp
public static string Ordinalize(this int number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo)'></a>

## OrdinalizeExtensions\.Ordinalize\(this int, CultureInfo\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this int number, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring)'></a>

## OrdinalizeExtensions\.Ordinalize\(this string\) Method

Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this string numberString);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender)'></a>

## OrdinalizeExtensions\.Ordinalize\(this string, GrammaticalGender\) Method

Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.
Gender for Brazilian Portuguese locale
"1"\.Ordinalize\(GrammaticalGender\.Masculine\) \-\> "1º"
"1"\.Ordinalize\(GrammaticalGender\.Feminine\) \-\> "1ª"

```csharp
public static string Ordinalize(this string numberString, Humanizer.GrammaticalGender gender);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## OrdinalizeExtensions\.Ordinalize\(this string, GrammaticalGender, CultureInfo\) Method

Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.
Gender for Brazilian Portuguese locale
"1"\.Ordinalize\(GrammaticalGender\.Masculine\) \-\> "1º"
"1"\.Ordinalize\(GrammaticalGender\.Feminine\) \-\> "1ª"

```csharp
public static string Ordinalize(this string numberString, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo)'></a>

## OrdinalizeExtensions\.Ordinalize\(this string, CultureInfo\) Method

Turns a number into an ordinal string used to denote the position in an ordered sequence such as 1st, 2nd, 3rd, 4th\.

```csharp
public static string Ordinalize(this string numberString, System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')