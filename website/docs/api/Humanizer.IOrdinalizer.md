## IOrdinalizer Interface

Localizes the ordinal form of a number\.

```csharp
public interface IOrdinalizer
```

Derived  
↳ [ILongOrdinalizer](Humanizer.ILongOrdinalizer.md 'Humanizer\.ILongOrdinalizer')
### Methods

<a name='Humanizer.IOrdinalizer.Convert(int,string)'></a>

## IOrdinalizer\.Convert\(int, string\) Method

Ordinalizes the number using the default grammatical form\.

```csharp
string Convert(int number, string numberString);
```
#### Parameters

<a name='Humanizer.IOrdinalizer.Convert(int,string).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The numeric value being ordinalized\.

<a name='Humanizer.IOrdinalizer.Convert(int,string).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The cardinal representation of the number\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The ordinalized text\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender)'></a>

## IOrdinalizer\.Convert\(int, string, GrammaticalGender\) Method

Ordinalizes the number using the provided grammatical gender\.

```csharp
string Convert(int number, string numberString, Humanizer.GrammaticalGender gender);
```
#### Parameters

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The numeric value being ordinalized\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The cardinal representation of the number\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale requires one\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The ordinalized text\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm)'></a>

## IOrdinalizer\.Convert\(int, string, GrammaticalGender, WordForm\) Method

Ordinalizes the number using the provided grammatical gender and word form\.

```csharp
string Convert(int number, string numberString, Humanizer.GrammaticalGender gender, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The numeric value being ordinalized\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The cardinal representation of the number\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm).gender'></a>

`gender` [GrammaticalGender](Humanizer.GrammaticalGender.md 'Humanizer\.GrammaticalGender')

The grammatical gender to use when the locale requires one\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.GrammaticalGender,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The word form to use when the locale distinguishes abbreviations from full words\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The ordinalized text\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.WordForm)'></a>

## IOrdinalizer\.Convert\(int, string, WordForm\) Method

Ordinalizes the number using a locale\-specific word form\.

```csharp
string Convert(int number, string numberString, Humanizer.WordForm wordForm);
```
#### Parameters

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.WordForm).number'></a>

`number` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The numeric value being ordinalized\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.WordForm).numberString'></a>

`numberString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The cardinal representation of the number\.

<a name='Humanizer.IOrdinalizer.Convert(int,string,Humanizer.WordForm).wordForm'></a>

`wordForm` [WordForm](Humanizer.WordForm.md 'Humanizer\.WordForm')

The word form to use when the locale distinguishes abbreviations from full words\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The ordinalized text\.