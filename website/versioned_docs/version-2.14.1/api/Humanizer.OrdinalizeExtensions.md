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

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

## OrdinalizeExtensions\.Ordinalize\(this int, GrammaticalGender, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided

```csharp
public static string Ordinalize(this int number, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

### Example
In Spanish:

```csharp
1.Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
1.Ordinalize(GrammaticalGender.Masculine, WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
1.Ordinalize(GrammaticalGender.Feminine, WordForm.Normal) -> 1.ª // As in "Es 1ª vez que hago esto"
```

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

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

## OrdinalizeExtensions\.Ordinalize\(this int, GrammaticalGender, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided

```csharp
public static string Ordinalize(this int number, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo culture, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

### Example
In Spanish:

```csharp
1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
1.Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
1.Ordinalize(GrammaticalGender.Feminine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.WordForm)'></a>

## OrdinalizeExtensions\.Ordinalize\(this int, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this int number, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

### Example
In Spanish:

```csharp
1.Ordinalize(WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
1.Ordinalize(WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

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

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

## OrdinalizeExtensions\.Ordinalize\(this int, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this int number, System.Globalization.CultureInfo culture, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisint,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

### Example
In Spanish:

```csharp
1.Ordinalize(new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
1.Ordinalize(new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

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

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

## OrdinalizeExtensions\.Ordinalize\(this string, GrammaticalGender, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided

```csharp
public static string Ordinalize(this string numberString, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

### Example
In Spanish:

```csharp
"1".Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
"1".Ordinalize(GrammaticalGender.Masculine, WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
"1".Ordinalize(GrammaticalGender.Feminine, WordForm.Normal) -> 1.ª // As in "Es 1ª vez que hago esto"
```

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

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

## OrdinalizeExtensions\.Ordinalize\(this string, GrammaticalGender, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific
locale's variations using the grammatical gender provided

```csharp
public static string Ordinalize(this string numberString, Humanizer.GrammaticalGender gender, System.Globalization.CultureInfo culture, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use for output words

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.GrammaticalGender,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

### Example
In Spanish:

```csharp
"1".Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
"1".Ordinalize(GrammaticalGender.Masculine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
"1".Ordinalize(GrammaticalGender.Feminine, new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.WordForm)'></a>

## OrdinalizeExtensions\.Ordinalize\(this string, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this string numberString, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number, in string, to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

### Example
In Spanish:

```csharp
"1".Ordinalize(WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
"1".Ordinalize(WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```

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

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo,Humanizer.WordForm)'></a>

## OrdinalizeExtensions\.Ordinalize\(this string, CultureInfo, WordForm\) Method

Turns a number into an ordinal number used to denote the position in an ordered sequence supporting specific locale's variations\.

```csharp
public static string Ordinalize(this string numberString, System.Globalization.CultureInfo culture, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The number to be ordinalized

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo,Humanizer.WordForm).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

Culture to use\. If null, current thread's UI culture is used\.

<a name='Humanizer.OrdinalizeExtensions.Ordinalize(thisstring,System.Globalization.CultureInfo,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

Form of the word, i\.e\. abbreviation

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The number ordinalized

### Example
In Spanish:

```csharp
"1".Ordinalize(new CultureInfo("es-ES"),WordForm.Abbreviation) -> 1.er // As in "Vivo en el 1.er piso"
"1".Ordinalize(new CultureInfo("es-ES"), WordForm.Normal) -> 1.º // As in "Fui el 1º de mi promoción"
```