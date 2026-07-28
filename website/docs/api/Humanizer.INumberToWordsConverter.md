## INumberToWordsConverter Interface

Converts numbers into locale\-specific words, ordinals, and tuple names\.

```csharp
public interface INumberToWordsConverter
```
### Methods

<a name='Humanizer.INumberToWordsConverter.Convert(long)'></a>

## INumberToWordsConverter\.Convert\(long\) Method

Converts the number using the locale's default grammatical gender\.

```csharp
string Convert(long number);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.Convert(long).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to convert\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.Convert(long).number 'Humanizer\.INumberToWordsConverter\.Convert\(long\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,bool)'></a>

## INumberToWordsConverter\.Convert\(long, bool\) Method

Converts the number using the locale's default grammatical gender and optionally inserts the locale\-specific conjunction\.

```csharp
string Convert(long number, bool addAnd);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.Convert(long,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to convert\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to insert the locale\-specific conjunction in compound numbers; otherwise, `false`\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.Convert(long,bool).number 'Humanizer\.INumberToWordsConverter\.Convert\(long, bool\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,bool,Humanizer.WordForm)'></a>

## INumberToWordsConverter\.Convert\(long, bool, WordForm\) Method

Converts the number using the locale's default grammatical gender, the specified word form, and optional conjunction handling\.

```csharp
string Convert(long number, bool addAnd, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.Convert(long,bool,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to convert\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,bool,Humanizer.WordForm).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to insert the locale\-specific conjunction in compound numbers; otherwise, `false`\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,bool,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The grammatical or morphological word form to use\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.Convert(long,bool,Humanizer.WordForm).number 'Humanizer\.INumberToWordsConverter\.Convert\(long, bool, Humanizer\.WordForm\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.GrammaticalGender,bool)'></a>

## INumberToWordsConverter\.Convert\(long, GrammaticalGender, bool\) Method

Converts the number using the specified grammatical gender\.

```csharp
string Convert(long number, Humanizer.GrammaticalGender gender, bool addAnd=true);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.GrammaticalGender,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to convert\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.GrammaticalGender,bool).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale distinguishes gendered forms\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.GrammaticalGender,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to insert the locale\-specific conjunction in compound numbers; otherwise, `false`\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.Convert(long,Humanizer.GrammaticalGender,bool).number 'Humanizer\.INumberToWordsConverter\.Convert\(long, Humanizer\.GrammaticalGender, bool\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm)'></a>

## INumberToWordsConverter\.Convert\(long, WordForm\) Method

Converts the number using the locale's default grammatical gender and the specified word form\.

```csharp
string Convert(long number, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to convert\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The grammatical or morphological word form to use\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm).number 'Humanizer\.INumberToWordsConverter\.Convert\(long, Humanizer\.WordForm\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool)'></a>

## INumberToWordsConverter\.Convert\(long, WordForm, GrammaticalGender, bool\) Method

Converts the number using the specified grammatical gender and word form\.

```csharp
string Convert(long number, Humanizer.WordForm wordForm, Humanizer.GrammaticalGender gender, bool addAnd=true);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool).number'></a>

`number` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number to convert\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The grammatical or morphological word form to use\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale distinguishes gendered forms\.

<a name='Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool).addAnd'></a>

`addAnd` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

`true` to insert the locale\-specific conjunction in compound numbers; otherwise, `false`\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.Convert(long,Humanizer.WordForm,Humanizer.GrammaticalGender,bool).number 'Humanizer\.INumberToWordsConverter\.Convert\(long, Humanizer\.WordForm, Humanizer\.GrammaticalGender, bool\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int)'></a>

## INumberToWordsConverter\.ConvertToOrdinal\(int\) Method

Converts the number to an ordinal string using the locale's default grammatical gender\.

```csharp
string ConvertToOrdinal(int number);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to convert\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized ordinal words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.ConvertToOrdinal(int).number 'Humanizer\.INumberToWordsConverter\.ConvertToOrdinal\(int\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender)'></a>

## INumberToWordsConverter\.ConvertToOrdinal\(int, GrammaticalGender\) Method

Converts the number to an ordinal string using the specified grammatical gender\.

```csharp
string ConvertToOrdinal(int number, Humanizer.GrammaticalGender gender);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to convert\.

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale distinguishes gendered forms\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized ordinal words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender).number 'Humanizer\.INumberToWordsConverter\.ConvertToOrdinal\(int, Humanizer\.GrammaticalGender\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

## INumberToWordsConverter\.ConvertToOrdinal\(int, GrammaticalGender, WordForm\) Method

Converts the number to an ordinal string using the specified grammatical gender and word form\.

```csharp
string ConvertToOrdinal(int number, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to convert\.

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale distinguishes gendered forms\.

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The grammatical or morphological word form to use\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized ordinal words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.GrammaticalGender,Humanizer.WordForm).number 'Humanizer\.INumberToWordsConverter\.ConvertToOrdinal\(int, Humanizer\.GrammaticalGender, Humanizer\.WordForm\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.WordForm)'></a>

## INumberToWordsConverter\.ConvertToOrdinal\(int, WordForm\) Method

Converts the number to an ordinal string using the locale's default grammatical gender and the specified word form\.

```csharp
string ConvertToOrdinal(int number, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to convert\.

<a name='Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The grammatical or morphological word form to use\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized ordinal words for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.ConvertToOrdinal(int,Humanizer.WordForm).number 'Humanizer\.INumberToWordsConverter\.ConvertToOrdinal\(int, Humanizer\.WordForm\)\.number')\.

<a name='Humanizer.INumberToWordsConverter.ConvertToTuple(int)'></a>

## INumberToWordsConverter\.ConvertToTuple\(int\) Method

Converts the integer to a locale\-specific named tuple such as `single` or `double`\.

```csharp
string ConvertToTuple(int number);
```
#### Parameters

<a name='Humanizer.INumberToWordsConverter.ConvertToTuple(int).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number to convert\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The localized tuple name for [number](Humanizer.INumberToWordsConverter.md#Humanizer.INumberToWordsConverter.ConvertToTuple(int).number 'Humanizer\.INumberToWordsConverter\.ConvertToTuple\(int\)\.number') when the locale defines one; otherwise, a numeric fallback\.