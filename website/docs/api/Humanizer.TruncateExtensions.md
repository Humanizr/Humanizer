---
title: 'Humanizer.TruncateExtensions'
sidebar_label: 'Humanizer.TruncateExtensions'
description: 'API reference for Humanizer.TruncateExtensions.'
---
## TruncateExtensions Class

Allow strings to be truncated

```csharp
public static class TruncateExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TruncateExtensions
- *Methods*
  - **[Truncate\(this string, int\)](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int) 'Humanizer\.TruncateExtensions\.Truncate\(this string, int\)')**
  - **[Truncate\(this string, int, ITruncator, TruncateFrom\)](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom) 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, Humanizer\.ITruncator, Humanizer\.TruncateFrom\)')**
  - **[Truncate\(this string, int, string, ITruncator, TruncateFrom\)](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom) 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, string, Humanizer\.ITruncator, Humanizer\.TruncateFrom\)')**
  - **[Truncate\(this string, int, string, TruncateFrom\)](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom) 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, string, Humanizer\.TruncateFrom\)')**
### Methods

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int)'></a>

#### TruncateExtensions\.Truncate\(this string, int\) Method

Truncates a string to a specified maximum length using the default truncation string \("…"\) and
fixed\-length truncator\.

```csharp
public static string? Truncate(this string? input, int length);
```
##### Parameters

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be truncated\. Can be null\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int).length'></a>

`length` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum length of the result string, including the truncation indicator\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The truncated string if its length exceeds [length](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int).length 'Humanizer\.TruncateExtensions\.Truncate\(this string, int\)\.length'), otherwise the original string\.
Returns null if [input](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int).input 'Humanizer\.TruncateExtensions\.Truncate\(this string, int\)\.input') is null\.

##### Example

```csharp
"This is a long string".Truncate(10) => "This is a…"
"Short".Truncate(10) => "Short"
null.Truncate(10) => null
```

##### Remarks
The default truncation indicator is "…" \(ellipsis\), and truncation occurs from the right side of the string\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom)'></a>

#### TruncateExtensions\.Truncate\(this string, int, ITruncator, TruncateFrom\) Method

Truncates a string to a specified maximum length using a custom truncator and truncation direction\.

```csharp
public static string? Truncate(this string? input, int length, Humanizer.ITruncator truncator, Humanizer.TruncateFrom from=Humanizer.TruncateFrom.Right);
```
##### Parameters

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be truncated\. Can be null\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).length'></a>

`length` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum length of the result string, including the truncation indicator\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).truncator'></a>

`truncator` [ITruncator](Humanizer.ITruncator.md 'Humanizer\.ITruncator')

The [ITruncator](Humanizer.ITruncator.md 'Humanizer\.ITruncator') implementation to use for truncation logic\.
Must not be null\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).from'></a>

`from` [TruncateFrom](Humanizer.TruncateFrom.md 'Humanizer\.TruncateFrom')

Specifies from which side of the string to truncate\. Default is [Right](Humanizer.TruncateFrom.md#Humanizer.TruncateFrom.Right 'Humanizer\.TruncateFrom\.Right')\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The truncated string if its length exceeds [length](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).length 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, Humanizer\.ITruncator, Humanizer\.TruncateFrom\)\.length'), otherwise the original string\.
Returns null if [input](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).input 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, Humanizer\.ITruncator, Humanizer\.TruncateFrom\)\.input') is null\.

##### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [truncator](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,Humanizer.ITruncator,Humanizer.TruncateFrom).truncator 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, Humanizer\.ITruncator, Humanizer\.TruncateFrom\)\.truncator') is null\.

##### Example

```csharp
"This is a long string".Truncate(10, Truncator.FixedLength, TruncateFrom.Left) => "…ng string"
"This is a long string".Truncate(10, Truncator.FixedNumberOfWords) => "This is…"
```

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom)'></a>

#### TruncateExtensions\.Truncate\(this string, int, string, ITruncator, TruncateFrom\) Method

Truncates a string to a specified maximum length using a custom truncation string, truncator, and direction\.

```csharp
public static string? Truncate(this string? input, int length, string? truncationString, Humanizer.ITruncator truncator, Humanizer.TruncateFrom from=Humanizer.TruncateFrom.Right);
```
##### Parameters

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be truncated\. Can be null\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).length'></a>

`length` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum length of the result string, including the truncation indicator\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).truncationString'></a>

`truncationString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to use as the truncation indicator \(e\.g\., "\.\.\.", "…", or any custom string\)\.
Can be null or empty\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).truncator'></a>

`truncator` [ITruncator](Humanizer.ITruncator.md 'Humanizer\.ITruncator')

The [ITruncator](Humanizer.ITruncator.md 'Humanizer\.ITruncator') implementation to use for truncation logic\.
Must not be null\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).from'></a>

`from` [TruncateFrom](Humanizer.TruncateFrom.md 'Humanizer\.TruncateFrom')

Specifies from which side of the string to truncate\. Default is [Right](Humanizer.TruncateFrom.md#Humanizer.TruncateFrom.Right 'Humanizer\.TruncateFrom\.Right')\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The truncated string if its length exceeds [length](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).length 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, string, Humanizer\.ITruncator, Humanizer\.TruncateFrom\)\.length'), otherwise the original string\.
Returns null if [input](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).input 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, string, Humanizer\.ITruncator, Humanizer\.TruncateFrom\)\.input') is null\.

##### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [truncator](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.ITruncator,Humanizer.TruncateFrom).truncator 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, string, Humanizer\.ITruncator, Humanizer\.TruncateFrom\)\.truncator') is null\.

##### Example

```csharp
"This is a long string".Truncate(10, "...", Truncator.FixedLength, TruncateFrom.Right) => "This is..."
"This is a long string".Truncate(10, "…", Truncator.FixedNumberOfWords, TruncateFrom.Left) => "… string"
```

##### Remarks
This is the most flexible truncation method, allowing full customization of the truncation behavior\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom)'></a>

#### TruncateExtensions\.Truncate\(this string, int, string, TruncateFrom\) Method

Truncates a string to a specified maximum length using a custom truncation string and fixed\-length truncator\.

```csharp
public static string? Truncate(this string? input, int length, string? truncationString, Humanizer.TruncateFrom from=Humanizer.TruncateFrom.Right);
```
##### Parameters

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be truncated\. Can be null\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).length'></a>

`length` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum length of the result string, including the truncation indicator\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).truncationString'></a>

`truncationString` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to use as the truncation indicator \(e\.g\., "\.\.\.", "…", or any custom string\)\.
Can be null or empty\.

<a name='Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).from'></a>

`from` [TruncateFrom](Humanizer.TruncateFrom.md 'Humanizer\.TruncateFrom')

Specifies from which side of the string to truncate\. Default is [Right](Humanizer.TruncateFrom.md#Humanizer.TruncateFrom.Right 'Humanizer\.TruncateFrom\.Right')\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The truncated string if its length exceeds [length](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).length 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, string, Humanizer\.TruncateFrom\)\.length'), otherwise the original string\.
Returns null if [input](Humanizer.TruncateExtensions.md#Humanizer.TruncateExtensions.Truncate(thisstring,int,string,Humanizer.TruncateFrom).input 'Humanizer\.TruncateExtensions\.Truncate\(this string, int, string, Humanizer\.TruncateFrom\)\.input') is null\.

##### Example

```csharp
"This is a long string".Truncate(10, "...") => "This is..."
"This is a long string".Truncate(15, "--") => "This is a lo--"
"This is a long string".Truncate(10, "...", TruncateFrom.Left) => "...string"
```
