## NumberToWordsExtension Class

Converts numbers to localized words and ordinals\.
The output is culture\-aware, including locale\-specific high\-range scale names and
English\-family differences such as `en`, `en-GB`, and `en-IN`\.

```csharp
public static class NumberToWordsExtension
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → NumberToWordsExtension
### Methods

<a name='Humanizer.NumberToWordsExtension.ToIndianWords(thisint,Humanizer.IndianScaleStyle)'></a>

## NumberToWordsExtension\.ToIndianWords\(this int, IndianScaleStyle\) Method

Converts the given value to Indian English cardinal words using the selected large\-number vocabulary\.

```csharp
public static string ToIndianWords(this int number, Humanizer.IndianScaleStyle scaleStyle=Humanizer.IndianScaleStyle.NamedScales);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToIndianWords(thisint,Humanizer.IndianScaleStyle).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToIndianWords(thisint,Humanizer.IndianScaleStyle).scaleStyle'></a>

`scaleStyle` [IndianScaleStyle](Humanizer.IndianScaleStyle.md 'Humanizer\.IndianScaleStyle')

The Indian large\-number vocabulary to use\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The Indian English cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToIndianWords(thisint,Humanizer.IndianScaleStyle).number 'Humanizer\.NumberToWordsExtension\.ToIndianWords\(this int, Humanizer\.IndianScaleStyle\)\.number')\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[scaleStyle](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToIndianWords(thisint,Humanizer.IndianScaleStyle).scaleStyle 'Humanizer\.NumberToWordsExtension\.ToIndianWords\(this int, Humanizer\.IndianScaleStyle\)\.scaleStyle') is not a defined value\.

<a name='Humanizer.NumberToWordsExtension.ToIndianWords(thislong,Humanizer.IndianScaleStyle)'></a>

## NumberToWordsExtension\.ToIndianWords\(this long, IndianScaleStyle\) Method

Converts the given value to Indian English cardinal words using the selected large\-number vocabulary\.

```csharp
public static string ToIndianWords(this long number, Humanizer.IndianScaleStyle scaleStyle=Humanizer.IndianScaleStyle.NamedScales);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToIndianWords(thislong,Humanizer.IndianScaleStyle).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToIndianWords(thislong,Humanizer.IndianScaleStyle).scaleStyle'></a>

`scaleStyle` [IndianScaleStyle](Humanizer.IndianScaleStyle.md 'Humanizer\.IndianScaleStyle')

The Indian large\-number vocabulary to use\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The Indian English cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToIndianWords(thislong,Humanizer.IndianScaleStyle).number 'Humanizer\.NumberToWordsExtension\.ToIndianWords\(this long, Humanizer\.IndianScaleStyle\)\.number')\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[scaleStyle](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToIndianWords(thislong,Humanizer.IndianScaleStyle).scaleStyle 'Humanizer\.NumberToWordsExtension\.ToIndianWords\(this long, Humanizer\.IndianScaleStyle\)\.scaleStyle') is not a defined value\.

### Remarks
[NamedScales](Humanizer.IndianScaleStyle.md#Humanizer.IndianScaleStyle.NamedScales 'Humanizer\.IndianScaleStyle\.NamedScales') uses the named\-scale vocabulary of the `en-IN` culture\.
            [CroreBased](Humanizer.IndianScaleStyle.md#Humanizer.IndianScaleStyle.CroreBased 'Humanizer\.IndianScaleStyle\.CroreBased') uses common crore\-based expressions without changing
            the configured converter or the behavior of other locales\.

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToOrdinalWords\(this int, GrammaticalGender, WordForm, CultureInfo\) Method

Converts a number to ordinal words supporting locale's specific variations\.

```csharp
public static string ToOrdinalWords(this int number, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to ordinal words

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm,System.Globalization.CultureInfo).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number converted into ordinal words

### Example
In Spanish:

```csharp
3.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Normal) -> "tercero"
3.ToOrdinalWords(GrammaticalGender.Masculine, WordForm.Abbreviation) -> "tercer"
3.ToOrdinalWords(GrammaticalGender.Feminine, WordForm.Normal) -> "tercera"
3.ToOrdinalWords(GrammaticalGender.Feminine, WordForm.Abbreviation) -> "tercera"
```

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToOrdinalWords\(this int, GrammaticalGender, CultureInfo\) Method

for Brazilian Portuguese locale
1\.ToOrdinalWords\(GrammaticalGender\.Masculine\) \-\> "primeiro"
1\.ToOrdinalWords\(GrammaticalGender\.Feminine\) \-\> "primeira"

```csharp
public static string ToOrdinalWords(this int number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture=null);
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

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.WordForm,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToOrdinalWords\(this int, WordForm, CultureInfo\) Method

Converts a number to ordinal words supporting locale's specific variations\.

```csharp
public static string ToOrdinalWords(this int number, Humanizer.WordForm wordForm, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.WordForm,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to ordinal words

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.WordForm,System.Globalization.CultureInfo).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,Humanizer.WordForm,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number converted into ordinal words

### Example
In Spanish:

```csharp
1.ToOrdinalWords(WordForm.Normal) -> "primero" // As in "He llegado el primero".
3.ToOrdinalWords(WordForm.Abbreviation) -> "tercer" // As in "Vivo en el tercer piso"
```

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToOrdinalWords\(this int, CultureInfo\) Method

1\.ToOrdinalWords\(\) \-\> "first"

```csharp
public static string ToOrdinalWords(this int number, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to ordinal words

<a name='Humanizer.NumberToWordsExtension.ToOrdinalWords(thisint,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NumberToWordsExtension.ToTuple(thisint,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToTuple\(this int, CultureInfo\) Method

1\.ToTuple\(\) \-\> "single"

```csharp
public static string ToTuple(this int number, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToTuple(thisint,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to tuple

<a name='Humanizer.NumberToWordsExtension.ToTuple(thisint,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, bool, WordForm, CultureInfo\) Method

Converts the given value to localized cardinal words with an explicit conjunction choice
and requested word form\.

```csharp
public static string ToWords(this int number, bool addAnd, Humanizer.WordForm wordForm, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether to include the culture's conjunction before the terminal group\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The requested word form\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo).number 'Humanizer\.NumberToWordsExtension\.ToWords\(this int, bool, Humanizer\.WordForm, System\.Globalization\.CultureInfo\)\.number')\.

### Example
In Spanish, numbers ended in 1 changes its form depending on their position in the sentence\.

```csharp
21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, bool, CultureInfo\) Method

Converts the given value to localized cardinal words with an explicit conjunction choice\.

```csharp
public static string ToWords(this int number, bool addAnd, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether to include the culture's conjunction before the terminal group\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo).number 'Humanizer\.NumberToWordsExtension\.ToWords\(this int, bool, System\.Globalization\.CultureInfo\)\.number')\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, GrammaticalGender, CultureInfo\) Method

Converts the given value to localized cardinal words using grammatical gender where supported\.

```csharp
public static string ToWords(this int number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale supports gendered forms\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number 'Humanizer\.NumberToWordsExtension\.ToWords\(this int, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo\)\.number')\.

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

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, WordForm, GrammaticalGender, CultureInfo\) Method

Converts the given value to localized cardinal words using both word form and grammatical gender\.

```csharp
public static string ToWords(this int number, Humanizer.WordForm wordForm, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The requested word form\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale supports gendered forms\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number 'Humanizer\.NumberToWordsExtension\.ToWords\(this int, Humanizer\.WordForm, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo\)\.number')\.

### Example
In Spanish, numbers ended in 1 change its form depending on their position in the sentence\.

```csharp
21.ToWords(WordForm.Normal, GrammaticalGender.Masculine) -> veintiuno // as in "Mi número favorito es el veintiuno".
21.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine) -> veintiún // as in "En total, conté veintiún coches"
21.ToWords(WordForm.Normal, GrammaticalGender.Feminine) -> veintiuna // as in "veintiuna personas"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, WordForm, CultureInfo\) Method

Converts the given value to localized cardinal words using both word form and grammatical gender\.

```csharp
public static string ToWords(this int number, Humanizer.WordForm wordForm, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,System.Globalization.CultureInfo).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number converted to words

### Example
In Spanish, numbers ended in 1 change its form depending on their position in the sentence\.

```csharp
21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, CultureInfo\) Method

Converts the given value to localized cardinal words\.

```csharp
public static string ToWords(this int number, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToWords(thisint,System.Globalization.CultureInfo).number 'Humanizer\.NumberToWordsExtension\.ToWords\(this int, System\.Globalization\.CultureInfo\)\.number')\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this long, GrammaticalGender, CultureInfo\) Method

Converts the given value to localized cardinal words using grammatical gender where supported\.

```csharp
public static string ToWords(this long number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale supports gendered forms\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number 'Humanizer\.NumberToWordsExtension\.ToWords\(this long, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo\)\.number')\.

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

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this long, WordForm, GrammaticalGender, CultureInfo\) Method

Converts the given value to localized cardinal words using both word form and grammatical gender\.

```csharp
public static string ToWords(this long number, Humanizer.WordForm wordForm, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The requested word form\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale supports gendered forms\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number 'Humanizer\.NumberToWordsExtension\.ToWords\(this long, Humanizer\.WordForm, Humanizer\.GrammaticalGender, System\.Globalization\.CultureInfo\)\.number')\.

### Example
In Spanish, numbers ended in 1 changes its form depending on their position in the sentence\.

```csharp
21.ToWords(WordForm.Normal, GrammaticalGender.Masculine) -> veintiuno // as in "Mi número favorito es el veintiuno".
21.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine) -> veintiún // as in "En total, conté veintiún coches"
21.ToWords(WordForm.Normal, GrammaticalGender.Feminine) -> veintiuna // as in "veintiuna personas"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool)'></a>

## NumberToWordsExtension\.ToWords\(this long, WordForm, CultureInfo, bool\) Method

Converts the given value to localized cardinal words using the requested word form\.

```csharp
public static string ToWords(this long number, Humanizer.WordForm wordForm, System.Globalization.CultureInfo? culture=null, bool addAnd=false);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The requested word form\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether to include the culture's conjunction before the terminal group\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool).number 'Humanizer\.NumberToWordsExtension\.ToWords\(this long, Humanizer\.WordForm, System\.Globalization\.CultureInfo, bool\)\.number')\.

### Example
In Spanish, numbers ended in 1 changes its form depending on their position in the sentence\.

```csharp
21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool)'></a>

## NumberToWordsExtension\.ToWords\(this long, CultureInfo, bool\) Method

Converts the given value to localized cardinal words using the culture's default conjunction policy\.

```csharp
public static string ToWords(this long number, System.Globalization.CultureInfo? culture=null, bool addAnd=true);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The value to convert\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to use\. If `null`, the current culture is used\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Whether to include the culture's conjunction before the terminal group\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized cardinal words for [number](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool).number 'Humanizer\.NumberToWordsExtension\.ToWords\(this long, System\.Globalization\.CultureInfo, bool\)\.number')\.