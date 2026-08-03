---
title: 'Humanizer.NumberToTimeSpanExtensions'
sidebar_label: 'Humanizer.NumberToTimeSpanExtensions'
description: 'API reference for Humanizer.NumberToTimeSpanExtensions.'
---
## NumberToTimeSpanExtensions Class

Provides extension methods for converting numeric values to [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') instances,
enabling fluent and readable time duration creation \(e\.g\., 5\.Seconds\(\), 3\.Hours\(\), 2\.Weeks\(\)\)\.

```csharp
public static class NumberToTimeSpanExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → NumberToTimeSpanExtensions

### Remarks
These extensions make it easy to create TimeSpan values in a more natural, readable way:
\- Instead of TimeSpan\.FromHours\(3\), you can write 3\.Hours\(\)
\- Instead of TimeSpan\.FromMinutes\(30\), you can write 30\.Minutes\(\)
\- Supports all numeric types: byte, sbyte, short, ushort, int, uint, long, ulong, and double
\- Weeks are converted to days \(1 week = 7 days\)
- *Methods*
  - **[Days\(this byte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this byte\)')**
  - **[Days\(this double\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisdouble) 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this double\)')**
  - **[Days\(this int\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisint) 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this int\)')**
  - **[Days\(this long\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thislong) 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this long\)')**
  - **[Days\(this sbyte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thissbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this sbyte\)')**
  - **[Days\(this short\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisshort) 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this short\)')**
  - **[Days\(this uint\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisuint) 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this uint\)')**
  - **[Days\(this ulong\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisulong) 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this ulong\)')**
  - **[Days\(this ushort\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisushort) 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this ushort\)')**
  - **[Hours\(this byte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this byte\)')**
  - **[Hours\(this double\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisdouble) 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this double\)')**
  - **[Hours\(this int\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisint) 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this int\)')**
  - **[Hours\(this long\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thislong) 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this long\)')**
  - **[Hours\(this sbyte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thissbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this sbyte\)')**
  - **[Hours\(this short\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisshort) 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this short\)')**
  - **[Hours\(this uint\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisuint) 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this uint\)')**
  - **[Hours\(this ulong\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisulong) 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this ulong\)')**
  - **[Hours\(this ushort\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisushort) 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this ushort\)')**
  - **[Milliseconds\(this byte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this byte\)')**
  - **[Milliseconds\(this double\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisdouble) 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this double\)')**
  - **[Milliseconds\(this int\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisint) 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this int\)')**
  - **[Milliseconds\(this long\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thislong) 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this long\)')**
  - **[Milliseconds\(this sbyte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thissbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this sbyte\)')**
  - **[Milliseconds\(this short\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisshort) 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this short\)')**
  - **[Milliseconds\(this uint\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisuint) 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this uint\)')**
  - **[Milliseconds\(this ulong\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisulong) 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this ulong\)')**
  - **[Milliseconds\(this ushort\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisushort) 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this ushort\)')**
  - **[Minutes\(this byte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this byte\)')**
  - **[Minutes\(this double\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisdouble) 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this double\)')**
  - **[Minutes\(this int\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisint) 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this int\)')**
  - **[Minutes\(this long\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thislong) 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this long\)')**
  - **[Minutes\(this sbyte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thissbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this sbyte\)')**
  - **[Minutes\(this short\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisshort) 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this short\)')**
  - **[Minutes\(this uint\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisuint) 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this uint\)')**
  - **[Minutes\(this ulong\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisulong) 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this ulong\)')**
  - **[Minutes\(this ushort\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisushort) 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this ushort\)')**
  - **[Seconds\(this byte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this byte\)')**
  - **[Seconds\(this double\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisdouble) 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this double\)')**
  - **[Seconds\(this int\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisint) 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this int\)')**
  - **[Seconds\(this long\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thislong) 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this long\)')**
  - **[Seconds\(this sbyte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thissbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this sbyte\)')**
  - **[Seconds\(this short\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisshort) 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this short\)')**
  - **[Seconds\(this uint\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisuint) 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this uint\)')**
  - **[Seconds\(this ulong\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisulong) 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this ulong\)')**
  - **[Seconds\(this ushort\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisushort) 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this ushort\)')**
  - **[Weeks\(this byte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this byte\)')**
  - **[Weeks\(this double\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisdouble) 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this double\)')**
  - **[Weeks\(this int\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisint) 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this int\)')**
  - **[Weeks\(this long\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thislong) 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this long\)')**
  - **[Weeks\(this sbyte\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thissbyte) 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this sbyte\)')**
  - **[Weeks\(this short\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisshort) 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this short\)')**
  - **[Weeks\(this uint\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisuint) 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this uint\)')**
  - **[Weeks\(this ulong\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisulong) 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this ulong\)')**
  - **[Weeks\(this ushort\)](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisushort) 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this ushort\)')**
### Methods

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisbyte)'></a>

#### NumberToTimeSpanExtensions\.Days\(this byte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of days\.

```csharp
public static System.TimeSpan Days(this byte days);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisbyte).days'></a>

`days` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of days\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [days](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisbyte).days 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this byte\)\.days') days\.

##### Example

```csharp
((byte)2).Days() => TimeSpan.FromDays(2)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisdouble)'></a>

#### NumberToTimeSpanExtensions\.Days\(this double\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of days\.

```csharp
public static System.TimeSpan Days(this double days);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisdouble).days'></a>

`days` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of days\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [days](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisdouble).days 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this double\)\.days') days\.

##### Example

```csharp
2.Days() => TimeSpan representing 2 days  
7.Days() => TimeSpan representing 1 week
1.5.Days() => TimeSpan representing 1 day and 12 hours
```

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisint)'></a>

#### NumberToTimeSpanExtensions\.Days\(this int\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of days\.

```csharp
public static System.TimeSpan Days(this int days);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisint).days'></a>

`days` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of days\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [days](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisint).days 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this int\)\.days') days\.

##### Example

```csharp
2.Days() => TimeSpan.FromDays(2)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thislong)'></a>

#### NumberToTimeSpanExtensions\.Days\(this long\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of days\.

```csharp
public static System.TimeSpan Days(this long days);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thislong).days'></a>

`days` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number of days\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [days](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thislong).days 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this long\)\.days') days\.

##### Example

```csharp
2L.Days() => TimeSpan.FromDays(2)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thissbyte)'></a>

#### NumberToTimeSpanExtensions\.Days\(this sbyte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of days\.

```csharp
public static System.TimeSpan Days(this sbyte days);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thissbyte).days'></a>

`days` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

The number of days\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [days](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thissbyte).days 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this sbyte\)\.days') days\.

##### Example

```csharp
((sbyte)2).Days() => TimeSpan.FromDays(2)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisshort)'></a>

#### NumberToTimeSpanExtensions\.Days\(this short\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of days\.

```csharp
public static System.TimeSpan Days(this short days);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisshort).days'></a>

`days` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

The number of days\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [days](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisshort).days 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this short\)\.days') days\.

##### Example

```csharp
((short)2).Days() => TimeSpan.FromDays(2)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisuint)'></a>

#### NumberToTimeSpanExtensions\.Days\(this uint\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of days\.

```csharp
public static System.TimeSpan Days(this uint days);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisuint).days'></a>

`days` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The number of days\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [days](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisuint).days 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this uint\)\.days') days\.

##### Example

```csharp
2U.Days() => TimeSpan.FromDays(2)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisulong)'></a>

#### NumberToTimeSpanExtensions\.Days\(this ulong\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of days\.

```csharp
public static System.TimeSpan Days(this ulong days);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisulong).days'></a>

`days` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The number of days\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [days](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisulong).days 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this ulong\)\.days') days\.

##### Example

```csharp
2UL.Days() => TimeSpan.FromDays(2)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisushort)'></a>

#### NumberToTimeSpanExtensions\.Days\(this ushort\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of days\.

```csharp
public static System.TimeSpan Days(this ushort days);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Days(thisushort).days'></a>

`days` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

The number of days\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [days](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Days(thisushort).days 'Humanizer\.NumberToTimeSpanExtensions\.Days\(this ushort\)\.days') days\.

##### Example

```csharp
((ushort)2).Days() => TimeSpan.FromDays(2)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisbyte)'></a>

#### NumberToTimeSpanExtensions\.Hours\(this byte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of hours\.

```csharp
public static System.TimeSpan Hours(this byte hours);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisbyte).hours'></a>

`hours` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of hours\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [hours](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisbyte).hours 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this byte\)\.hours') hours\.

##### Example

```csharp
((byte)3).Hours() => TimeSpan.FromHours(3)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisdouble)'></a>

#### NumberToTimeSpanExtensions\.Hours\(this double\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of hours\.

```csharp
public static System.TimeSpan Hours(this double hours);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisdouble).hours'></a>

`hours` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of hours\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [hours](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisdouble).hours 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this double\)\.hours') hours\.

##### Example

```csharp
3.Hours() => TimeSpan representing 3 hours
24.Hours() => TimeSpan representing 1 day
1.5.Hours() => TimeSpan representing 1 hour and 30 minutes
```

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisint)'></a>

#### NumberToTimeSpanExtensions\.Hours\(this int\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of hours\.

```csharp
public static System.TimeSpan Hours(this int hours);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisint).hours'></a>

`hours` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of hours\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [hours](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisint).hours 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this int\)\.hours') hours\.

##### Example

```csharp
3.Hours() => TimeSpan.FromHours(3)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thislong)'></a>

#### NumberToTimeSpanExtensions\.Hours\(this long\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of hours\.

```csharp
public static System.TimeSpan Hours(this long hours);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thislong).hours'></a>

`hours` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number of hours\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [hours](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thislong).hours 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this long\)\.hours') hours\.

##### Example

```csharp
3L.Hours() => TimeSpan.FromHours(3)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thissbyte)'></a>

#### NumberToTimeSpanExtensions\.Hours\(this sbyte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of hours\.

```csharp
public static System.TimeSpan Hours(this sbyte hours);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thissbyte).hours'></a>

`hours` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

The number of hours\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [hours](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thissbyte).hours 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this sbyte\)\.hours') hours\.

##### Example

```csharp
((sbyte)3).Hours() => TimeSpan.FromHours(3)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisshort)'></a>

#### NumberToTimeSpanExtensions\.Hours\(this short\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of hours\.

```csharp
public static System.TimeSpan Hours(this short hours);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisshort).hours'></a>

`hours` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

The number of hours\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [hours](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisshort).hours 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this short\)\.hours') hours\.

##### Example

```csharp
((short)3).Hours() => TimeSpan.FromHours(3)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisuint)'></a>

#### NumberToTimeSpanExtensions\.Hours\(this uint\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of hours\.

```csharp
public static System.TimeSpan Hours(this uint hours);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisuint).hours'></a>

`hours` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The number of hours\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [hours](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisuint).hours 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this uint\)\.hours') hours\.

##### Example

```csharp
3U.Hours() => TimeSpan.FromHours(3)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisulong)'></a>

#### NumberToTimeSpanExtensions\.Hours\(this ulong\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of hours\.

```csharp
public static System.TimeSpan Hours(this ulong hours);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisulong).hours'></a>

`hours` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The number of hours\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [hours](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisulong).hours 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this ulong\)\.hours') hours\.

##### Example

```csharp
3UL.Hours() => TimeSpan.FromHours(3)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisushort)'></a>

#### NumberToTimeSpanExtensions\.Hours\(this ushort\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of hours\.

```csharp
public static System.TimeSpan Hours(this ushort hours);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Hours(thisushort).hours'></a>

`hours` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

The number of hours\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [hours](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Hours(thisushort).hours 'Humanizer\.NumberToTimeSpanExtensions\.Hours\(this ushort\)\.hours') hours\.

##### Example

```csharp
((ushort)3).Hours() => TimeSpan.FromHours(3)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisbyte)'></a>

#### NumberToTimeSpanExtensions\.Milliseconds\(this byte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of milliseconds\.

```csharp
public static System.TimeSpan Milliseconds(this byte ms);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisbyte).ms'></a>

`ms` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of milliseconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [ms](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisbyte).ms 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this byte\)\.ms') milliseconds\.

##### Example

```csharp
500.Milliseconds() => TimeSpan representing 500 milliseconds
1000.Milliseconds() => TimeSpan representing 1 second
```

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisdouble)'></a>

#### NumberToTimeSpanExtensions\.Milliseconds\(this double\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of milliseconds\.

```csharp
public static System.TimeSpan Milliseconds(this double ms);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisdouble).ms'></a>

`ms` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of milliseconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [ms](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisdouble).ms 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this double\)\.ms') milliseconds\.

##### Example

```csharp
500.0.Milliseconds() => TimeSpan.FromMilliseconds(500)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisint)'></a>

#### NumberToTimeSpanExtensions\.Milliseconds\(this int\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of milliseconds\.

```csharp
public static System.TimeSpan Milliseconds(this int ms);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisint).ms'></a>

`ms` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of milliseconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [ms](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisint).ms 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this int\)\.ms') milliseconds\.

##### Example

```csharp
500.Milliseconds() => TimeSpan.FromMilliseconds(500)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thislong)'></a>

#### NumberToTimeSpanExtensions\.Milliseconds\(this long\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of milliseconds\.

```csharp
public static System.TimeSpan Milliseconds(this long ms);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thislong).ms'></a>

`ms` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number of milliseconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [ms](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thislong).ms 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this long\)\.ms') milliseconds\.

##### Example

```csharp
500L.Milliseconds() => TimeSpan.FromMilliseconds(500)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thissbyte)'></a>

#### NumberToTimeSpanExtensions\.Milliseconds\(this sbyte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of milliseconds\.

```csharp
public static System.TimeSpan Milliseconds(this sbyte ms);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thissbyte).ms'></a>

`ms` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

The number of milliseconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [ms](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thissbyte).ms 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this sbyte\)\.ms') milliseconds\.

##### Example

```csharp
((sbyte)500).Milliseconds() => TimeSpan.FromMilliseconds(500)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisshort)'></a>

#### NumberToTimeSpanExtensions\.Milliseconds\(this short\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of milliseconds\.

```csharp
public static System.TimeSpan Milliseconds(this short ms);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisshort).ms'></a>

`ms` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

The number of milliseconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [ms](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisshort).ms 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this short\)\.ms') milliseconds\.

##### Example

```csharp
((short)500).Milliseconds() => TimeSpan.FromMilliseconds(500)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisuint)'></a>

#### NumberToTimeSpanExtensions\.Milliseconds\(this uint\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of milliseconds\.

```csharp
public static System.TimeSpan Milliseconds(this uint ms);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisuint).ms'></a>

`ms` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The number of milliseconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [ms](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisuint).ms 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this uint\)\.ms') milliseconds\.

##### Example

```csharp
500U.Milliseconds() => TimeSpan.FromMilliseconds(500)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisulong)'></a>

#### NumberToTimeSpanExtensions\.Milliseconds\(this ulong\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of milliseconds\.

```csharp
public static System.TimeSpan Milliseconds(this ulong ms);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisulong).ms'></a>

`ms` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The number of milliseconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [ms](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisulong).ms 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this ulong\)\.ms') milliseconds\.

##### Example

```csharp
500UL.Milliseconds() => TimeSpan.FromMilliseconds(500)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisushort)'></a>

#### NumberToTimeSpanExtensions\.Milliseconds\(this ushort\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of milliseconds\.

```csharp
public static System.TimeSpan Milliseconds(this ushort ms);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisushort).ms'></a>

`ms` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

The number of milliseconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [ms](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Milliseconds(thisushort).ms 'Humanizer\.NumberToTimeSpanExtensions\.Milliseconds\(this ushort\)\.ms') milliseconds\.

##### Example

```csharp
((ushort)500).Milliseconds() => TimeSpan.FromMilliseconds(500)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisbyte)'></a>

#### NumberToTimeSpanExtensions\.Minutes\(this byte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of minutes\.

```csharp
public static System.TimeSpan Minutes(this byte minutes);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisbyte).minutes'></a>

`minutes` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of minutes\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [minutes](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisbyte).minutes 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this byte\)\.minutes') minutes\.

##### Example

```csharp
((byte)30).Minutes() => TimeSpan.FromMinutes(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisdouble)'></a>

#### NumberToTimeSpanExtensions\.Minutes\(this double\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of minutes\.

```csharp
public static System.TimeSpan Minutes(this double minutes);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisdouble).minutes'></a>

`minutes` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of minutes\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [minutes](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisdouble).minutes 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this double\)\.minutes') minutes\.

##### Example

```csharp
30.Minutes() => TimeSpan representing 30 minutes
90.Minutes() => TimeSpan representing 1 hour and 30 minutes
```

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisint)'></a>

#### NumberToTimeSpanExtensions\.Minutes\(this int\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of minutes\.

```csharp
public static System.TimeSpan Minutes(this int minutes);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisint).minutes'></a>

`minutes` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of minutes\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [minutes](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisint).minutes 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this int\)\.minutes') minutes\.

##### Example

```csharp
30.Minutes() => TimeSpan.FromMinutes(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thislong)'></a>

#### NumberToTimeSpanExtensions\.Minutes\(this long\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of minutes\.

```csharp
public static System.TimeSpan Minutes(this long minutes);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thislong).minutes'></a>

`minutes` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number of minutes\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [minutes](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thislong).minutes 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this long\)\.minutes') minutes\.

##### Example

```csharp
30L.Minutes() => TimeSpan.FromMinutes(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thissbyte)'></a>

#### NumberToTimeSpanExtensions\.Minutes\(this sbyte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of minutes\.

```csharp
public static System.TimeSpan Minutes(this sbyte minutes);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thissbyte).minutes'></a>

`minutes` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

The number of minutes\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [minutes](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thissbyte).minutes 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this sbyte\)\.minutes') minutes\.

##### Example

```csharp
((sbyte)30).Minutes() => TimeSpan.FromMinutes(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisshort)'></a>

#### NumberToTimeSpanExtensions\.Minutes\(this short\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of minutes\.

```csharp
public static System.TimeSpan Minutes(this short minutes);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisshort).minutes'></a>

`minutes` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

The number of minutes\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [minutes](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisshort).minutes 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this short\)\.minutes') minutes\.

##### Example

```csharp
((short)30).Minutes() => TimeSpan.FromMinutes(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisuint)'></a>

#### NumberToTimeSpanExtensions\.Minutes\(this uint\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of minutes\.

```csharp
public static System.TimeSpan Minutes(this uint minutes);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisuint).minutes'></a>

`minutes` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The number of minutes\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [minutes](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisuint).minutes 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this uint\)\.minutes') minutes\.

##### Example

```csharp
30U.Minutes() => TimeSpan.FromMinutes(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisulong)'></a>

#### NumberToTimeSpanExtensions\.Minutes\(this ulong\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of minutes\.

```csharp
public static System.TimeSpan Minutes(this ulong minutes);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisulong).minutes'></a>

`minutes` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The number of minutes\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [minutes](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisulong).minutes 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this ulong\)\.minutes') minutes\.

##### Example

```csharp
30UL.Minutes() => TimeSpan.FromMinutes(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisushort)'></a>

#### NumberToTimeSpanExtensions\.Minutes\(this ushort\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of minutes\.

```csharp
public static System.TimeSpan Minutes(this ushort minutes);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Minutes(thisushort).minutes'></a>

`minutes` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

The number of minutes\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [minutes](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Minutes(thisushort).minutes 'Humanizer\.NumberToTimeSpanExtensions\.Minutes\(this ushort\)\.minutes') minutes\.

##### Example

```csharp
((ushort)30).Minutes() => TimeSpan.FromMinutes(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisbyte)'></a>

#### NumberToTimeSpanExtensions\.Seconds\(this byte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of seconds\.

```csharp
public static System.TimeSpan Seconds(this byte seconds);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisbyte).seconds'></a>

`seconds` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of seconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [seconds](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisbyte).seconds 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this byte\)\.seconds') seconds\.

##### Example

```csharp
((byte)30).Seconds() => TimeSpan.FromSeconds(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisdouble)'></a>

#### NumberToTimeSpanExtensions\.Seconds\(this double\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of seconds\.

```csharp
public static System.TimeSpan Seconds(this double seconds);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisdouble).seconds'></a>

`seconds` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of seconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [seconds](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisdouble).seconds 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this double\)\.seconds') seconds\.

##### Example

```csharp
30.Seconds() => TimeSpan representing 30 seconds
90.Seconds() => TimeSpan representing 1 minute and 30 seconds
```

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisint)'></a>

#### NumberToTimeSpanExtensions\.Seconds\(this int\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of seconds\.

```csharp
public static System.TimeSpan Seconds(this int seconds);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisint).seconds'></a>

`seconds` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of seconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [seconds](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisint).seconds 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this int\)\.seconds') seconds\.

##### Example

```csharp
30.Seconds() => TimeSpan.FromSeconds(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thislong)'></a>

#### NumberToTimeSpanExtensions\.Seconds\(this long\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of seconds\.

```csharp
public static System.TimeSpan Seconds(this long seconds);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thislong).seconds'></a>

`seconds` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number of seconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [seconds](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thislong).seconds 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this long\)\.seconds') seconds\.

##### Example

```csharp
30L.Seconds() => TimeSpan.FromSeconds(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thissbyte)'></a>

#### NumberToTimeSpanExtensions\.Seconds\(this sbyte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of seconds\.

```csharp
public static System.TimeSpan Seconds(this sbyte seconds);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thissbyte).seconds'></a>

`seconds` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

The number of seconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [seconds](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thissbyte).seconds 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this sbyte\)\.seconds') seconds\.

##### Example

```csharp
((sbyte)30).Seconds() => TimeSpan.FromSeconds(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisshort)'></a>

#### NumberToTimeSpanExtensions\.Seconds\(this short\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of seconds\.

```csharp
public static System.TimeSpan Seconds(this short seconds);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisshort).seconds'></a>

`seconds` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

The number of seconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [seconds](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisshort).seconds 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this short\)\.seconds') seconds\.

##### Example

```csharp
((short)30).Seconds() => TimeSpan.FromSeconds(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisuint)'></a>

#### NumberToTimeSpanExtensions\.Seconds\(this uint\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of seconds\.

```csharp
public static System.TimeSpan Seconds(this uint seconds);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisuint).seconds'></a>

`seconds` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The number of seconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [seconds](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisuint).seconds 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this uint\)\.seconds') seconds\.

##### Example

```csharp
30U.Seconds() => TimeSpan.FromSeconds(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisulong)'></a>

#### NumberToTimeSpanExtensions\.Seconds\(this ulong\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of seconds\.

```csharp
public static System.TimeSpan Seconds(this ulong seconds);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisulong).seconds'></a>

`seconds` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The number of seconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [seconds](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisulong).seconds 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this ulong\)\.seconds') seconds\.

##### Example

```csharp
30UL.Seconds() => TimeSpan.FromSeconds(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisushort)'></a>

#### NumberToTimeSpanExtensions\.Seconds\(this ushort\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of seconds\.

```csharp
public static System.TimeSpan Seconds(this ushort seconds);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Seconds(thisushort).seconds'></a>

`seconds` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

The number of seconds\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [seconds](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Seconds(thisushort).seconds 'Humanizer\.NumberToTimeSpanExtensions\.Seconds\(this ushort\)\.seconds') seconds\.

##### Example

```csharp
((ushort)30).Seconds() => TimeSpan.FromSeconds(30)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisbyte)'></a>

#### NumberToTimeSpanExtensions\.Weeks\(this byte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of weeks\.

```csharp
public static System.TimeSpan Weeks(this byte input);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

The number of weeks\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [input](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisbyte).input 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this byte\)\.input') weeks \(converted to days: 1 week = 7 days\)\.

##### Example

```csharp
((byte)2).Weeks() => new TimeSpan(14, 0, 0, 0)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisdouble)'></a>

#### NumberToTimeSpanExtensions\.Weeks\(this double\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of weeks\.

```csharp
public static System.TimeSpan Weeks(this double input);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of weeks\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [input](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisdouble).input 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this double\)\.input') weeks \(converted to days: 1 week = 7 days\)\.

##### Example

```csharp
2.Weeks() => TimeSpan representing 14 days
1.Weeks() => TimeSpan representing 7 days
0.5.Weeks() => TimeSpan representing 3.5 days
```

##### Remarks
Since [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') doesn't have a native concept of weeks, this method converts
weeks to days \(multiplying by 7\)\.

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisint)'></a>

#### NumberToTimeSpanExtensions\.Weeks\(this int\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of weeks\.

```csharp
public static System.TimeSpan Weeks(this int input);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The number of weeks\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [input](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisint).input 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this int\)\.input') weeks \(converted to days: 1 week = 7 days\)\.

##### Example

```csharp
2.Weeks() => new TimeSpan(14, 0, 0, 0)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thislong)'></a>

#### NumberToTimeSpanExtensions\.Weeks\(this long\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of weeks\.

```csharp
public static System.TimeSpan Weeks(this long input);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The number of weeks\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [input](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thislong).input 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this long\)\.input') weeks \(converted to days: 1 week = 7 days\)\.

##### Example

```csharp
2L.Weeks() => new TimeSpan(14, 0, 0, 0)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thissbyte)'></a>

#### NumberToTimeSpanExtensions\.Weeks\(this sbyte\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of weeks\.

```csharp
public static System.TimeSpan Weeks(this sbyte input);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

The number of weeks\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [input](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thissbyte).input 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this sbyte\)\.input') weeks \(converted to days: 1 week = 7 days\)\.

##### Example

```csharp
((sbyte)2).Weeks() => new TimeSpan(14, 0, 0, 0)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisshort)'></a>

#### NumberToTimeSpanExtensions\.Weeks\(this short\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of weeks\.

```csharp
public static System.TimeSpan Weeks(this short input);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

The number of weeks\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [input](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisshort).input 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this short\)\.input') weeks \(converted to days: 1 week = 7 days\)\.

##### Example

```csharp
((short)2).Weeks() => new TimeSpan(14, 0, 0, 0)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisuint)'></a>

#### NumberToTimeSpanExtensions\.Weeks\(this uint\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of weeks\.

```csharp
public static System.TimeSpan Weeks(this uint input);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The number of weeks\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [input](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisuint).input 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this uint\)\.input') weeks \(converted to days: 1 week = 7 days\)\.

##### Example

```csharp
2U.Weeks() => new TimeSpan(14, 0, 0, 0)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisulong)'></a>

#### NumberToTimeSpanExtensions\.Weeks\(this ulong\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of weeks\.

```csharp
public static System.TimeSpan Weeks(this ulong input);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisulong).input'></a>

`input` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The number of weeks\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [input](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisulong).input 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this ulong\)\.input') weeks \(converted to days: 1 week = 7 days\)\.

##### Example

```csharp
2UL.Weeks() => new TimeSpan(14, 0, 0, 0)
```

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisushort)'></a>

#### NumberToTimeSpanExtensions\.Weeks\(this ushort\) Method

Creates a [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing the specified number of weeks\.

```csharp
public static System.TimeSpan Weeks(this ushort input);
```
##### Parameters

<a name='Humanizer.NumberToTimeSpanExtensions.Weeks(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

The number of weeks\.

##### Returns
[System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')  
A [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') representing [input](Humanizer.NumberToTimeSpanExtensions.md#Humanizer.NumberToTimeSpanExtensions.Weeks(thisushort).input 'Humanizer\.NumberToTimeSpanExtensions\.Weeks\(this ushort\)\.input') weeks \(converted to days: 1 week = 7 days\)\.

##### Example

```csharp
((ushort)2).Weeks() => new TimeSpan(14, 0, 0, 0)
```
