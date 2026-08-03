---
title: 'Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator'
sidebar_label: 'Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator'
description: 'API reference for Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.'
---
## DynamicNumberOfCharactersAndPreserveWordsTruncator Class

Truncate a string to a fixed number of letters or digits,
preserving whole words by never cutting a word in half\.
If a complete word \(plus the delimiter, if any\) cannot fit, then only the delimiter is returned\.
When truncating from the left, the delimiter is prepended if a complete word can be preserved;
otherwise, only the delimiter is returned\.
The allowed count is computed by counting only letters/digits\.

```csharp
public class DynamicNumberOfCharactersAndPreserveWordsTruncator : Humanizer.ITruncator
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DynamicNumberOfCharactersAndPreserveWordsTruncator

Implements [ITruncator](Humanizer.ITruncator.md 'Humanizer\.ITruncator')
- *Constructors*
  - **[DynamicNumberOfCharactersAndPreserveWordsTruncator\(\)](Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.md#Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.DynamicNumberOfCharactersAndPreserveWordsTruncator())**
- *Methods*
  - **[Truncate\(string, int, string, TruncateFrom\)](Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.md#Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.Truncate(string,int,string,Humanizer.TruncateFrom) 'Humanizer\.DynamicNumberOfCharactersAndPreserveWordsTruncator\.Truncate\(string, int, string, Humanizer\.TruncateFrom\)')**
### Constructors

<a name='Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.DynamicNumberOfCharactersAndPreserveWordsTruncator()'></a>

#### DynamicNumberOfCharactersAndPreserveWordsTruncator\(\) Constructor

Initializes a new instance of the DynamicNumberOfCharactersAndPreserveWordsTruncator class.

```csharp
public DynamicNumberOfCharactersAndPreserveWordsTruncator();
```
### Methods

<a name='Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.Truncate(string,int,string,Humanizer.TruncateFrom)'></a>

#### DynamicNumberOfCharactersAndPreserveWordsTruncator\.Truncate\(string, int, string, TruncateFrom\) Method

```csharp
public string? Truncate(string? value, int totalLength, string? delimiter, Humanizer.TruncateFrom truncateFrom=Humanizer.TruncateFrom.Right);
```
##### Parameters

<a name='Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.Truncate(string,int,string,Humanizer.TruncateFrom).value'></a>

`value` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.Truncate(string,int,string,Humanizer.TruncateFrom).totalLength'></a>

`totalLength` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.Truncate(string,int,string,Humanizer.TruncateFrom).delimiter'></a>

`delimiter` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.DynamicNumberOfCharactersAndPreserveWordsTruncator.Truncate(string,int,string,Humanizer.TruncateFrom).truncateFrom'></a>

`truncateFrom` [TruncateFrom](Humanizer.TruncateFrom.md 'Humanizer\.TruncateFrom')

Implements [Truncate\(string, int, string, TruncateFrom\)](Humanizer.ITruncator.md#Humanizer.ITruncator.Truncate(string,int,string,Humanizer.TruncateFrom) 'Humanizer\.ITruncator\.Truncate\(string, int, string, Humanizer\.TruncateFrom\)')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
