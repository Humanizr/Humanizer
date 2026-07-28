## NumberToWordsExtension Class

Transform a number into words; e\.g\. 1 =\> one

```csharp
public static class NumberToWordsExtension
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → NumberToWordsExtension
### Methods

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

Culture to use\. If null, current thread's UI culture is used\.

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

Culture to use\. If null, current thread's UI culture is used\.

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

Culture to use\. If null, current thread's UI culture is used\.

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

Culture to use\. If null, current thread's UI culture is used\.

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

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, bool, WordForm, CultureInfo\) Method

Converts a number to words supporting specific word variations of some locales\.

```csharp
public static string ToWords(this int number, bool addAnd, Humanizer.WordForm wordForm, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

To add 'and' before the last number

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,Humanizer.WordForm,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number converted to words

### Example
In Spanish, numbers ended in 1 changes its form depending on their position in the sentence\.

```csharp
21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,bool,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, bool, CultureInfo\) Method

3501\.ToWords\(false\) \-\> "three thousand five hundred one"

```csharp
public static string ToWords(this int number, bool addAnd, System.Globalization.CultureInfo? culture=null);
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
public static string ToWords(this int number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture=null);
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

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, WordForm, GrammaticalGender, CultureInfo\) Method

Converts a number to words supporting specific word variations, including grammatical gender, of some locales\.

```csharp
public static string ToWords(this int number, Humanizer.WordForm wordForm, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number converted to words

### Example
In Spanish, numbers ended in 1 change its form depending on their position in the sentence\.

```csharp
21.ToWords(WordForm.Normal, GrammaticalGender.Masculine) -> veintiuno // as in "Mi número favorito es el veintiuno".
21.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine) -> veintiún // as in "En total, conté veintiún coches"
21.ToWords(WordForm.Normal, GrammaticalGender.Feminine) -> veintiuna // as in "veintiuna personas"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thisint,Humanizer.WordForm,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this int, WordForm, CultureInfo\) Method

Converts a number to words supporting specific word variations, including grammatical gender, of some locales\.

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

Culture to use\. If null, current thread's UI culture is used\.

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

3501\.ToWords\(\) \-\> "three thousand five hundred and one"

```csharp
public static string ToWords(this int number, System.Globalization.CultureInfo? culture=null);
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
public static string ToWords(this long number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture=null);
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

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo)'></a>

## NumberToWordsExtension\.ToWords\(this long, WordForm, GrammaticalGender, CultureInfo\) Method

Converts a number to words supporting specific word variations, including grammatical gender, of some locales\.

```csharp
public static string ToWords(this long number, Humanizer.WordForm wordForm, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,Humanizer.GrammaticalGender,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number converted to words

### Example
In Spanish, numbers ended in 1 changes its form depending on their position in the sentence\.

```csharp
21.ToWords(WordForm.Normal, GrammaticalGender.Masculine) -> veintiuno // as in "Mi número favorito es el veintiuno".
21.ToWords(WordForm.Abbreviation, GrammaticalGender.Masculine) -> veintiún // as in "En total, conté veintiún coches"
21.ToWords(WordForm.Normal, GrammaticalGender.Feminine) -> veintiuna // as in "veintiuna personas"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool)'></a>

## NumberToWordsExtension\.ToWords\(this long, WordForm, CultureInfo, bool\) Method

Converts a number to words supporting specific word variations of some locales\.

```csharp
public static string ToWords(this long number, Humanizer.WordForm wordForm, System.Globalization.CultureInfo? culture=null, bool addAnd=false);
```
#### Parameters

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

Number to be turned to words

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,Humanizer.WordForm,System.Globalization.CultureInfo,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

To add 'and' before the last number

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number converted to words

### Example
In Spanish, numbers ended in 1 changes its form depending on their position in the sentence\.

```csharp
21.ToWords(WordForm.Normal) -> veintiuno // as in "Mi número favorito es el veintiuno".
21.ToWords(WordForm.Abbreviation) -> veintiún // as in "En total, conté veintiún coches"
```

<a name='Humanizer.NumberToWordsExtension.ToWords(thislong,System.Globalization.CultureInfo,bool)'></a>

## NumberToWordsExtension\.ToWords\(this long, CultureInfo, bool\) Method

3501\.ToWords\(\) \-\> "three thousand five hundred and one"

```csharp
public static string ToWords(this long number, System.Globalization.CultureInfo? culture=null, bool addAnd=true);
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