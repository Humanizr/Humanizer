---
title: 'Humanizer.IStringTransformer'
sidebar_label: 'Humanizer.IStringTransformer'
description: 'API reference for Humanizer.IStringTransformer.'
---
## IStringTransformer Interface

Can transform a string

```csharp
public interface IStringTransformer
```

Derived  
↳ [ICulturedStringTransformer](Humanizer.ICulturedStringTransformer.md 'Humanizer\.ICulturedStringTransformer')
- *Methods*
  - **[Transform\(string\)](Humanizer.IStringTransformer.md#Humanizer.IStringTransformer.Transform(string) 'Humanizer\.IStringTransformer\.Transform\(string\)')**
### Methods

<a name='Humanizer.IStringTransformer.Transform(string)'></a>

#### IStringTransformer\.Transform\(string\) Method

Transform the input

```csharp
string Transform(string input);
```
##### Parameters

<a name='Humanizer.IStringTransformer.Transform(string).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

String to be transformed

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
