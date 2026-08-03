---
title: 'Humanizer.NoMatchFoundException'
sidebar_label: 'Humanizer.NoMatchFoundException'
description: 'API reference for Humanizer.NoMatchFoundException.'
---
## NoMatchFoundException Class

This is thrown on String\.DehumanizeTo enum when the provided string cannot be mapped to the target enum

```csharp
public class NoMatchFoundException : System.Exception
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → [System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception') → NoMatchFoundException
- *Constructors*
  - **[NoMatchFoundException\(\)](Humanizer.NoMatchFoundException.md#Humanizer.NoMatchFoundException.NoMatchFoundException() 'Humanizer\.NoMatchFoundException\.NoMatchFoundException\(\)')**
  - **[NoMatchFoundException\(string\)](Humanizer.NoMatchFoundException.md#Humanizer.NoMatchFoundException.NoMatchFoundException(string) 'Humanizer\.NoMatchFoundException\.NoMatchFoundException\(string\)')**
  - **[NoMatchFoundException\(string, Exception\)](Humanizer.NoMatchFoundException.md#Humanizer.NoMatchFoundException.NoMatchFoundException(string,System.Exception) 'Humanizer\.NoMatchFoundException\.NoMatchFoundException\(string, System\.Exception\)')**
### Constructors

<a name='Humanizer.NoMatchFoundException.NoMatchFoundException()'></a>

#### NoMatchFoundException\(\) Constructor

```csharp
public NoMatchFoundException();
```

<a name='Humanizer.NoMatchFoundException.NoMatchFoundException(string)'></a>

#### NoMatchFoundException\(string\) Constructor

```csharp
public NoMatchFoundException(string message);
```
##### Parameters

<a name='Humanizer.NoMatchFoundException.NoMatchFoundException(string).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NoMatchFoundException.NoMatchFoundException(string,System.Exception)'></a>

#### NoMatchFoundException\(string, Exception\) Constructor

```csharp
public NoMatchFoundException(string message, System.Exception inner);
```
##### Parameters

<a name='Humanizer.NoMatchFoundException.NoMatchFoundException(string,System.Exception).message'></a>

`message` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.NoMatchFoundException.NoMatchFoundException(string,System.Exception).inner'></a>

`inner` [System\.Exception](https://learn.microsoft.com/en-us/dotnet/api/system.exception 'System\.Exception')
