## NumberToWordsExtension Class

Transform a number into words; e\.g\. 1 =\> one

```csharp
public static class NumberToWordsExtension
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → NumberToWordsExtension
### Methods

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToOrdinalWords\(this int, GrammaticalGender, CultureInfo\) Method

for Brazilian Portuguese locale
1\.ToOrdinalWords\(GrammaticalGender\.Masculine\) \-\> "primeiro"
1\.ToOrdinalWords\(GrammaticalGender\.Feminine\) \-\> "primeira"

```csharp
public static string ToOrdinalWords(this int number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToOrdinalWords\(this int, CultureInfo\) Method

1\.ToOrdinalWords\(\) \-\> "first"

```csharp
public static string ToOrdinalWords(this int number, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to ordinal words

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NumberToWordsExtension.ToTuple(thisint,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToTuple\(this int, CultureInfo\) Method

1\.ToTuple\(\) \-\> "single"

```csharp
public static string ToTuple(this int number, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToTuple(thisint,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to tuple

<a name='Humanizer.NumberToWordsExtension.ToTuple(thisint,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, bool, CultureInfo\) Method

3501\.ToWords\(false\) \-\> "three thousand five hundred one"

```csharp
public static string ToWords(this int number, bool addAnd, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

To add 'and' before the last number\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, GrammaticalGender, CultureInfo\) Method

For locales that support gender\-specific forms

```csharp
public static string ToWords(this int number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
Russian:

```csharp
1.ToWords(GrammaticalGender.Masculine) -> "один"
1.ToWords(GrammaticalGender.Feminine) -> "одна"
```
Hebrew:

```csharp
1.ToWords(GrammaticalGender.Masculine) -> "אחד"
1.ToWords(GrammaticalGender.Feminine) -> "אחת"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, CultureInfo\) Method

3501\.ToWords\(\) \-\> "three thousand five hundred and one"

```csharp
public static string ToWords(this int number, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this long, GrammaticalGender, CultureInfo\) Method

For locales that support gender\-specific forms

```csharp
public static string ToWords(this long number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

### Example
Russian:

```csharp
1.ToWords(GrammaticalGender.Masculine) -> "один"
1.ToWords(GrammaticalGender.Feminine) -> "одна"
```
Hebrew:

```csharp
1.ToWords(GrammaticalGender.Masculine) -> "אחד"
1.ToWords(GrammaticalGender.Feminine) -> "אחת"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool)'></a>

## NumberToWordsExtension\.ToWords\(this long, CultureInfo, bool\) Method

3501\.ToWords\(\) \-\> "three thousand five hundred and one"

```csharp
public static string ToWords(this long number, System.Globalization.CultureInfo culture=null, bool addAnd=true);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether "and" should be included or not\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')