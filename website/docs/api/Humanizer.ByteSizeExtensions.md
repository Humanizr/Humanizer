---
title: 'Humanizer.ByteSizeExtensions'
sidebar_label: 'Humanizer.ByteSizeExtensions'
description: 'API reference for Humanizer.ByteSizeExtensions.'
---
## ByteSizeExtensions Class

Provides extension methods for ByteSize

```csharp
public static class ByteSizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → ByteSizeExtensions
- *Methods*
  - **[Bits\(this byte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bits(thisbyte) 'Humanizer\.ByteSizeExtensions\.Bits\(this byte\)')**
  - **[Bits\(this int\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bits(thisint) 'Humanizer\.ByteSizeExtensions\.Bits\(this int\)')**
  - **[Bits\(this long\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bits(thislong) 'Humanizer\.ByteSizeExtensions\.Bits\(this long\)')**
  - **[Bits\(this sbyte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bits(thissbyte) 'Humanizer\.ByteSizeExtensions\.Bits\(this sbyte\)')**
  - **[Bits\(this short\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bits(thisshort) 'Humanizer\.ByteSizeExtensions\.Bits\(this short\)')**
  - **[Bits\(this uint\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bits(thisuint) 'Humanizer\.ByteSizeExtensions\.Bits\(this uint\)')**
  - **[Bits\(this ushort\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bits(thisushort) 'Humanizer\.ByteSizeExtensions\.Bits\(this ushort\)')**
  - **[Bytes\(this byte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bytes(thisbyte) 'Humanizer\.ByteSizeExtensions\.Bytes\(this byte\)')**
  - **[Bytes\(this double\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bytes(thisdouble) 'Humanizer\.ByteSizeExtensions\.Bytes\(this double\)')**
  - **[Bytes\(this int\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bytes(thisint) 'Humanizer\.ByteSizeExtensions\.Bytes\(this int\)')**
  - **[Bytes\(this long\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bytes(thislong) 'Humanizer\.ByteSizeExtensions\.Bytes\(this long\)')**
  - **[Bytes\(this sbyte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bytes(thissbyte) 'Humanizer\.ByteSizeExtensions\.Bytes\(this sbyte\)')**
  - **[Bytes\(this short\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bytes(thisshort) 'Humanizer\.ByteSizeExtensions\.Bytes\(this short\)')**
  - **[Bytes\(this uint\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bytes(thisuint) 'Humanizer\.ByteSizeExtensions\.Bytes\(this uint\)')**
  - **[Bytes\(this ushort\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Bytes(thisushort) 'Humanizer\.ByteSizeExtensions\.Bytes\(this ushort\)')**
  - **[Exabytes\(this byte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Exabytes(thisbyte) 'Humanizer\.ByteSizeExtensions\.Exabytes\(this byte\)')**
  - **[Exabytes\(this double\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Exabytes(thisdouble) 'Humanizer\.ByteSizeExtensions\.Exabytes\(this double\)')**
  - **[Exabytes\(this int\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Exabytes(thisint) 'Humanizer\.ByteSizeExtensions\.Exabytes\(this int\)')**
  - **[Exabytes\(this long\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Exabytes(thislong) 'Humanizer\.ByteSizeExtensions\.Exabytes\(this long\)')**
  - **[Exabytes\(this sbyte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Exabytes(thissbyte) 'Humanizer\.ByteSizeExtensions\.Exabytes\(this sbyte\)')**
  - **[Exabytes\(this short\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Exabytes(thisshort) 'Humanizer\.ByteSizeExtensions\.Exabytes\(this short\)')**
  - **[Exabytes\(this uint\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Exabytes(thisuint) 'Humanizer\.ByteSizeExtensions\.Exabytes\(this uint\)')**
  - **[Exabytes\(this ushort\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Exabytes(thisushort) 'Humanizer\.ByteSizeExtensions\.Exabytes\(this ushort\)')**
  - **[Gigabytes\(this byte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Gigabytes(thisbyte) 'Humanizer\.ByteSizeExtensions\.Gigabytes\(this byte\)')**
  - **[Gigabytes\(this double\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Gigabytes(thisdouble) 'Humanizer\.ByteSizeExtensions\.Gigabytes\(this double\)')**
  - **[Gigabytes\(this int\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Gigabytes(thisint) 'Humanizer\.ByteSizeExtensions\.Gigabytes\(this int\)')**
  - **[Gigabytes\(this long\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Gigabytes(thislong) 'Humanizer\.ByteSizeExtensions\.Gigabytes\(this long\)')**
  - **[Gigabytes\(this sbyte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Gigabytes(thissbyte) 'Humanizer\.ByteSizeExtensions\.Gigabytes\(this sbyte\)')**
  - **[Gigabytes\(this short\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Gigabytes(thisshort) 'Humanizer\.ByteSizeExtensions\.Gigabytes\(this short\)')**
  - **[Gigabytes\(this uint\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Gigabytes(thisuint) 'Humanizer\.ByteSizeExtensions\.Gigabytes\(this uint\)')**
  - **[Gigabytes\(this ushort\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Gigabytes(thisushort) 'Humanizer\.ByteSizeExtensions\.Gigabytes\(this ushort\)')**
  - **[Humanize\(this ByteSize, string\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,string) 'Humanizer\.ByteSizeExtensions\.Humanize\(this Humanizer\.ByteSize, string\)')**
  - **[Humanize\(this ByteSize, string, IFormatProvider\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,string,System.IFormatProvider) 'Humanizer\.ByteSizeExtensions\.Humanize\(this Humanizer\.ByteSize, string, System\.IFormatProvider\)')**
  - **[Humanize\(this ByteSize, IFormatProvider\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,System.IFormatProvider) 'Humanizer\.ByteSizeExtensions\.Humanize\(this Humanizer\.ByteSize, System\.IFormatProvider\)')**
  - **[HumanizeComposite\(this ByteSize, int, IFormatProvider, string, bool\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeComposite(thisHumanizer.ByteSize,int,System.IFormatProvider,string,bool) 'Humanizer\.ByteSizeExtensions\.HumanizeComposite\(this Humanizer\.ByteSize, int, System\.IFormatProvider, string, bool\)')**
  - **[HumanizeCompositeWithUnitSystem\(this ByteSize, ByteSizeUnitSystem, int, IFormatProvider, string, bool\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool) 'Humanizer\.ByteSizeExtensions\.HumanizeCompositeWithUnitSystem\(this Humanizer\.ByteSize, Humanizer\.ByteSizeUnitSystem, int, System\.IFormatProvider, string, bool\)')**
  - **[HumanizeWithUnitSystem\(this ByteSize, ByteSizeUnitSystem, string, IFormatProvider\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider) 'Humanizer\.ByteSizeExtensions\.HumanizeWithUnitSystem\(this Humanizer\.ByteSize, Humanizer\.ByteSizeUnitSystem, string, System\.IFormatProvider\)')**
  - **[Kilobytes\(this byte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Kilobytes(thisbyte) 'Humanizer\.ByteSizeExtensions\.Kilobytes\(this byte\)')**
  - **[Kilobytes\(this double\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Kilobytes(thisdouble) 'Humanizer\.ByteSizeExtensions\.Kilobytes\(this double\)')**
  - **[Kilobytes\(this int\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Kilobytes(thisint) 'Humanizer\.ByteSizeExtensions\.Kilobytes\(this int\)')**
  - **[Kilobytes\(this long\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Kilobytes(thislong) 'Humanizer\.ByteSizeExtensions\.Kilobytes\(this long\)')**
  - **[Kilobytes\(this sbyte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Kilobytes(thissbyte) 'Humanizer\.ByteSizeExtensions\.Kilobytes\(this sbyte\)')**
  - **[Kilobytes\(this short\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Kilobytes(thisshort) 'Humanizer\.ByteSizeExtensions\.Kilobytes\(this short\)')**
  - **[Kilobytes\(this uint\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Kilobytes(thisuint) 'Humanizer\.ByteSizeExtensions\.Kilobytes\(this uint\)')**
  - **[Kilobytes\(this ushort\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Kilobytes(thisushort) 'Humanizer\.ByteSizeExtensions\.Kilobytes\(this ushort\)')**
  - **[Megabytes\(this byte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Megabytes(thisbyte) 'Humanizer\.ByteSizeExtensions\.Megabytes\(this byte\)')**
  - **[Megabytes\(this double\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Megabytes(thisdouble) 'Humanizer\.ByteSizeExtensions\.Megabytes\(this double\)')**
  - **[Megabytes\(this int\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Megabytes(thisint) 'Humanizer\.ByteSizeExtensions\.Megabytes\(this int\)')**
  - **[Megabytes\(this long\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Megabytes(thislong) 'Humanizer\.ByteSizeExtensions\.Megabytes\(this long\)')**
  - **[Megabytes\(this sbyte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Megabytes(thissbyte) 'Humanizer\.ByteSizeExtensions\.Megabytes\(this sbyte\)')**
  - **[Megabytes\(this short\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Megabytes(thisshort) 'Humanizer\.ByteSizeExtensions\.Megabytes\(this short\)')**
  - **[Megabytes\(this uint\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Megabytes(thisuint) 'Humanizer\.ByteSizeExtensions\.Megabytes\(this uint\)')**
  - **[Megabytes\(this ushort\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Megabytes(thisushort) 'Humanizer\.ByteSizeExtensions\.Megabytes\(this ushort\)')**
  - **[Pebibytes\(this byte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Pebibytes(thisbyte) 'Humanizer\.ByteSizeExtensions\.Pebibytes\(this byte\)')**
  - **[Pebibytes\(this double\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Pebibytes(thisdouble) 'Humanizer\.ByteSizeExtensions\.Pebibytes\(this double\)')**
  - **[Pebibytes\(this int\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Pebibytes(thisint) 'Humanizer\.ByteSizeExtensions\.Pebibytes\(this int\)')**
  - **[Pebibytes\(this long\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Pebibytes(thislong) 'Humanizer\.ByteSizeExtensions\.Pebibytes\(this long\)')**
  - **[Pebibytes\(this sbyte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Pebibytes(thissbyte) 'Humanizer\.ByteSizeExtensions\.Pebibytes\(this sbyte\)')**
  - **[Pebibytes\(this short\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Pebibytes(thisshort) 'Humanizer\.ByteSizeExtensions\.Pebibytes\(this short\)')**
  - **[Pebibytes\(this uint\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Pebibytes(thisuint) 'Humanizer\.ByteSizeExtensions\.Pebibytes\(this uint\)')**
  - **[Pebibytes\(this ushort\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Pebibytes(thisushort) 'Humanizer\.ByteSizeExtensions\.Pebibytes\(this ushort\)')**
  - **[Per\(this ByteSize, TimeSpan\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Per(thisHumanizer.ByteSize,System.TimeSpan) 'Humanizer\.ByteSizeExtensions\.Per\(this Humanizer\.ByteSize, System\.TimeSpan\)')**
  - **[Petabytes\(this byte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Petabytes(thisbyte) 'Humanizer\.ByteSizeExtensions\.Petabytes\(this byte\)')**
  - **[Petabytes\(this double\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Petabytes(thisdouble) 'Humanizer\.ByteSizeExtensions\.Petabytes\(this double\)')**
  - **[Petabytes\(this int\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Petabytes(thisint) 'Humanizer\.ByteSizeExtensions\.Petabytes\(this int\)')**
  - **[Petabytes\(this long\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Petabytes(thislong) 'Humanizer\.ByteSizeExtensions\.Petabytes\(this long\)')**
  - **[Petabytes\(this sbyte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Petabytes(thissbyte) 'Humanizer\.ByteSizeExtensions\.Petabytes\(this sbyte\)')**
  - **[Petabytes\(this short\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Petabytes(thisshort) 'Humanizer\.ByteSizeExtensions\.Petabytes\(this short\)')**
  - **[Petabytes\(this uint\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Petabytes(thisuint) 'Humanizer\.ByteSizeExtensions\.Petabytes\(this uint\)')**
  - **[Petabytes\(this ushort\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Petabytes(thisushort) 'Humanizer\.ByteSizeExtensions\.Petabytes\(this ushort\)')**
  - **[Terabytes\(this byte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Terabytes(thisbyte) 'Humanizer\.ByteSizeExtensions\.Terabytes\(this byte\)')**
  - **[Terabytes\(this double\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Terabytes(thisdouble) 'Humanizer\.ByteSizeExtensions\.Terabytes\(this double\)')**
  - **[Terabytes\(this int\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Terabytes(thisint) 'Humanizer\.ByteSizeExtensions\.Terabytes\(this int\)')**
  - **[Terabytes\(this long\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Terabytes(thislong) 'Humanizer\.ByteSizeExtensions\.Terabytes\(this long\)')**
  - **[Terabytes\(this sbyte\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Terabytes(thissbyte) 'Humanizer\.ByteSizeExtensions\.Terabytes\(this sbyte\)')**
  - **[Terabytes\(this short\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Terabytes(thisshort) 'Humanizer\.ByteSizeExtensions\.Terabytes\(this short\)')**
  - **[Terabytes\(this uint\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Terabytes(thisuint) 'Humanizer\.ByteSizeExtensions\.Terabytes\(this uint\)')**
  - **[Terabytes\(this ushort\)](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.Terabytes(thisushort) 'Humanizer\.ByteSizeExtensions\.Terabytes\(this ushort\)')**
### Methods

<a name='Humanizer.ByteSizeExtensions.Bits(thisbyte)'></a>

#### ByteSizeExtensions\.Bits\(this byte\) Method

Considers input as bits

```csharp
public static Humanizer.ByteSize Bits(this byte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bits(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bits(thisint)'></a>

#### ByteSizeExtensions\.Bits\(this int\) Method

Considers input as bits

```csharp
public static Humanizer.ByteSize Bits(this int input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bits(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bits(thislong)'></a>

#### ByteSizeExtensions\.Bits\(this long\) Method

Considers input as bits

```csharp
public static Humanizer.ByteSize Bits(this long input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bits(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bits(thissbyte)'></a>

#### ByteSizeExtensions\.Bits\(this sbyte\) Method

Considers input as bits

```csharp
public static Humanizer.ByteSize Bits(this sbyte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bits(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bits(thisshort)'></a>

#### ByteSizeExtensions\.Bits\(this short\) Method

Considers input as bits

```csharp
public static Humanizer.ByteSize Bits(this short input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bits(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bits(thisuint)'></a>

#### ByteSizeExtensions\.Bits\(this uint\) Method

Considers input as bits

```csharp
public static Humanizer.ByteSize Bits(this uint input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bits(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bits(thisushort)'></a>

#### ByteSizeExtensions\.Bits\(this ushort\) Method

Considers input as bits

```csharp
public static Humanizer.ByteSize Bits(this ushort input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bits(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bytes(thisbyte)'></a>

#### ByteSizeExtensions\.Bytes\(this byte\) Method

Considers input as bytes

```csharp
public static Humanizer.ByteSize Bytes(this byte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bytes(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bytes(thisdouble)'></a>

#### ByteSizeExtensions\.Bytes\(this double\) Method

Considers input as bytes

```csharp
public static Humanizer.ByteSize Bytes(this double input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bytes(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bytes(thisint)'></a>

#### ByteSizeExtensions\.Bytes\(this int\) Method

Considers input as bytes

```csharp
public static Humanizer.ByteSize Bytes(this int input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bytes(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bytes(thislong)'></a>

#### ByteSizeExtensions\.Bytes\(this long\) Method

Considers input as bytes

```csharp
public static Humanizer.ByteSize Bytes(this long input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bytes(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bytes(thissbyte)'></a>

#### ByteSizeExtensions\.Bytes\(this sbyte\) Method

Considers input as bytes

```csharp
public static Humanizer.ByteSize Bytes(this sbyte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bytes(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bytes(thisshort)'></a>

#### ByteSizeExtensions\.Bytes\(this short\) Method

Considers input as bytes

```csharp
public static Humanizer.ByteSize Bytes(this short input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bytes(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bytes(thisuint)'></a>

#### ByteSizeExtensions\.Bytes\(this uint\) Method

Considers input as bytes

```csharp
public static Humanizer.ByteSize Bytes(this uint input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bytes(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Bytes(thisushort)'></a>

#### ByteSizeExtensions\.Bytes\(this ushort\) Method

Considers input as bytes

```csharp
public static Humanizer.ByteSize Bytes(this ushort input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Bytes(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisbyte)'></a>

#### ByteSizeExtensions\.Exabytes\(this byte\) Method

Considers input as exabytes

```csharp
public static Humanizer.ByteSize Exabytes(this byte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisdouble)'></a>

#### ByteSizeExtensions\.Exabytes\(this double\) Method

Considers input as exabytes

```csharp
public static Humanizer.ByteSize Exabytes(this double input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisint)'></a>

#### ByteSizeExtensions\.Exabytes\(this int\) Method

Considers input as exabytes

```csharp
public static Humanizer.ByteSize Exabytes(this int input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Exabytes(thislong)'></a>

#### ByteSizeExtensions\.Exabytes\(this long\) Method

Considers input as exabytes

```csharp
public static Humanizer.ByteSize Exabytes(this long input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Exabytes(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Exabytes(thissbyte)'></a>

#### ByteSizeExtensions\.Exabytes\(this sbyte\) Method

Considers input as exabytes

```csharp
public static Humanizer.ByteSize Exabytes(this sbyte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Exabytes(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisshort)'></a>

#### ByteSizeExtensions\.Exabytes\(this short\) Method

Considers input as exabytes

```csharp
public static Humanizer.ByteSize Exabytes(this short input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisuint)'></a>

#### ByteSizeExtensions\.Exabytes\(this uint\) Method

Considers input as exabytes

```csharp
public static Humanizer.ByteSize Exabytes(this uint input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisushort)'></a>

#### ByteSizeExtensions\.Exabytes\(this ushort\) Method

Considers input as exabytes

```csharp
public static Humanizer.ByteSize Exabytes(this ushort input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Exabytes(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisbyte)'></a>

#### ByteSizeExtensions\.Gigabytes\(this byte\) Method

Considers input as gigabytes

```csharp
public static Humanizer.ByteSize Gigabytes(this byte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisdouble)'></a>

#### ByteSizeExtensions\.Gigabytes\(this double\) Method

Considers input as gigabytes

```csharp
public static Humanizer.ByteSize Gigabytes(this double input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisint)'></a>

#### ByteSizeExtensions\.Gigabytes\(this int\) Method

Considers input as gigabytes

```csharp
public static Humanizer.ByteSize Gigabytes(this int input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thislong)'></a>

#### ByteSizeExtensions\.Gigabytes\(this long\) Method

Considers input as gigabytes

```csharp
public static Humanizer.ByteSize Gigabytes(this long input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thissbyte)'></a>

#### ByteSizeExtensions\.Gigabytes\(this sbyte\) Method

Considers input as gigabytes

```csharp
public static Humanizer.ByteSize Gigabytes(this sbyte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisshort)'></a>

#### ByteSizeExtensions\.Gigabytes\(this short\) Method

Considers input as gigabytes

```csharp
public static Humanizer.ByteSize Gigabytes(this short input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisuint)'></a>

#### ByteSizeExtensions\.Gigabytes\(this uint\) Method

Considers input as gigabytes

```csharp
public static Humanizer.ByteSize Gigabytes(this uint input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisushort)'></a>

#### ByteSizeExtensions\.Gigabytes\(this ushort\) Method

Considers input as gigabytes

```csharp
public static Humanizer.ByteSize Gigabytes(this ushort input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Gigabytes(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,string)'></a>

#### ByteSizeExtensions\.Humanize\(this ByteSize, string\) Method

Turns a byte quantity into human readable form, eg 2 GB

```csharp
public static string Humanize(this Humanizer.ByteSize input, string? format=null);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,string).input'></a>

`input` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,string).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string format to use

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,string,System.IFormatProvider)'></a>

#### ByteSizeExtensions\.Humanize\(this ByteSize, string, IFormatProvider\) Method

Turns a byte quantity into human readable form, eg 2 GB

```csharp
public static string Humanize(this Humanizer.ByteSize input, string? format, System.IFormatProvider? formatProvider);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,string,System.IFormatProvider).input'></a>

`input` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string format to use

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The format provider to use

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,System.IFormatProvider)'></a>

#### ByteSizeExtensions\.Humanize\(this ByteSize, IFormatProvider\) Method

Turns a byte quantity into human readable form, eg 2 GB

```csharp
public static string Humanize(this Humanizer.ByteSize input, System.IFormatProvider formatProvider);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,System.IFormatProvider).input'></a>

`input` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Humanize(thisHumanizer.ByteSize,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The format provider to use

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSizeExtensions.HumanizeComposite(thisHumanizer.ByteSize,int,System.IFormatProvider,string,bool)'></a>

#### ByteSizeExtensions\.HumanizeComposite\(this ByteSize, int, IFormatProvider, string, bool\) Method

Turns a byte quantity into a composite human\-readable form using descending units, e\.g\. 10 KB 2 B\.

```csharp
public static string HumanizeComposite(this Humanizer.ByteSize input, int precision=2, System.IFormatProvider? formatProvider=null, string separator=" ", bool toWords=false);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.HumanizeComposite(thisHumanizer.ByteSize,int,System.IFormatProvider,string,bool).input'></a>

`input` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

The byte quantity to humanize\.

<a name='Humanizer.ByteSizeExtensions.HumanizeComposite(thisHumanizer.ByteSize,int,System.IFormatProvider,string,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of non\-zero parts to return\.

<a name='Humanizer.ByteSizeExtensions.HumanizeComposite(thisHumanizer.ByteSize,int,System.IFormatProvider,string,bool).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The format provider to use\. If null, the current culture is used\.

<a name='Humanizer.ByteSizeExtensions.HumanizeComposite(thisHumanizer.ByteSize,int,System.IFormatProvider,string,bool).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator to use between parts\.

<a name='Humanizer.ByteSizeExtensions.HumanizeComposite(thisHumanizer.ByteSize,int,System.IFormatProvider,string,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Uses unit words instead of symbols if true\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The composite byte quantity\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[precision](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeComposite(thisHumanizer.ByteSize,int,System.IFormatProvider,string,bool).precision 'Humanizer\.ByteSizeExtensions\.HumanizeComposite\(this Humanizer\.ByteSize, int, System\.IFormatProvider, string, bool\)\.precision') is less than one\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[separator](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeComposite(thisHumanizer.ByteSize,int,System.IFormatProvider,string,bool).separator 'Humanizer\.ByteSizeExtensions\.HumanizeComposite\(this Humanizer\.ByteSize, int, System\.IFormatProvider, string, bool\)\.separator') is null\.

<a name='Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool)'></a>

#### ByteSizeExtensions\.HumanizeCompositeWithUnitSystem\(this ByteSize, ByteSizeUnitSystem, int, IFormatProvider, string, bool\) Method

Turns a byte quantity into composite human\-readable form using one explicit unit system\.

```csharp
public static string HumanizeCompositeWithUnitSystem(this Humanizer.ByteSize input, Humanizer.ByteSizeUnitSystem unitSystem, int precision=2, System.IFormatProvider? formatProvider=null, string separator=" ", bool toWords=false);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool).input'></a>

`input` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

The byte quantity to humanize\.

<a name='Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool).unitSystem'></a>

`unitSystem` [ByteSizeUnitSystem](Humanizer.ByteSizeUnitSystem.md 'Humanizer\.ByteSizeUnitSystem')

The unit system to use\.

<a name='Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool).precision'></a>

`precision` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The maximum number of non\-zero parts to return\.

<a name='Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The provider used to format each numeric part and select localized unit words\.

<a name='Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The separator to place between parts\.

<a name='Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool).toWords'></a>

`toWords` [System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

Uses localized unit words instead of canonical symbols when [true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The composite humanized byte quantity\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[unitSystem](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool).unitSystem 'Humanizer\.ByteSizeExtensions\.HumanizeCompositeWithUnitSystem\(this Humanizer\.ByteSize, Humanizer\.ByteSizeUnitSystem, int, System\.IFormatProvider, string, bool\)\.unitSystem') is not defined or [precision](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool).precision 'Humanizer\.ByteSizeExtensions\.HumanizeCompositeWithUnitSystem\(this Humanizer\.ByteSize, Humanizer\.ByteSizeUnitSystem, int, System\.IFormatProvider, string, bool\)\.precision') is less than one\.

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[separator](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeCompositeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,int,System.IFormatProvider,string,bool).separator 'Humanizer\.ByteSizeExtensions\.HumanizeCompositeWithUnitSystem\(this Humanizer\.ByteSize, Humanizer\.ByteSizeUnitSystem, int, System\.IFormatProvider, string, bool\)\.separator') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

<a name='Humanizer.ByteSizeExtensions.HumanizeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider)'></a>

#### ByteSizeExtensions\.HumanizeWithUnitSystem\(this ByteSize, ByteSizeUnitSystem, string, IFormatProvider\) Method

Turns a byte quantity into human\-readable form using an explicit unit system\.

```csharp
public static string HumanizeWithUnitSystem(this Humanizer.ByteSize input, Humanizer.ByteSizeUnitSystem unitSystem, string? format=null, System.IFormatProvider? formatProvider=null);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.HumanizeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).input'></a>

`input` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

The byte quantity to humanize\.

<a name='Humanizer.ByteSizeExtensions.HumanizeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).unitSystem'></a>

`unitSystem` [ByteSizeUnitSystem](Humanizer.ByteSizeUnitSystem.md 'Humanizer\.ByteSizeUnitSystem')

The unit system to use\.

<a name='Humanizer.ByteSizeExtensions.HumanizeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The numeric format and optional unit token\. For [DecimalSi](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.DecimalSi 'Humanizer\.ByteSizeUnitSystem\.DecimalSi') and
[BinaryIec](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.BinaryIec 'Humanizer\.ByteSizeUnitSystem\.BinaryIec'), SI/IEC\-prefixed unit tokens are matched case\-insensitively,
while `b` and `B` remain case\-sensitive; output uses canonical symbol casing\.
[Legacy](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.Legacy 'Humanizer\.ByteSizeUnitSystem\.Legacy') preserves established matching behavior\.

<a name='Humanizer.ByteSizeExtensions.HumanizeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The provider used to format the numeric value\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The humanized byte quantity\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[unitSystem](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).unitSystem 'Humanizer\.ByteSizeExtensions\.HumanizeWithUnitSystem\(this Humanizer\.ByteSize, Humanizer\.ByteSizeUnitSystem, string, System\.IFormatProvider\)\.unitSystem') is not defined\.

[System\.FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception 'System\.FormatException')  
[format](Humanizer.ByteSizeExtensions.md#Humanizer.ByteSizeExtensions.HumanizeWithUnitSystem(thisHumanizer.ByteSize,Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).format 'Humanizer\.ByteSizeExtensions\.HumanizeWithUnitSystem\(this Humanizer\.ByteSize, Humanizer\.ByteSizeUnitSystem, string, System\.IFormatProvider\)\.format') is invalid, or selects a token not supported by the selected non\-legacy system\.

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisbyte)'></a>

#### ByteSizeExtensions\.Kilobytes\(this byte\) Method

Considers input as kilobytes

```csharp
public static Humanizer.ByteSize Kilobytes(this byte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisdouble)'></a>

#### ByteSizeExtensions\.Kilobytes\(this double\) Method

Considers input as kilobytes

```csharp
public static Humanizer.ByteSize Kilobytes(this double input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisint)'></a>

#### ByteSizeExtensions\.Kilobytes\(this int\) Method

Considers input as kilobytes

```csharp
public static Humanizer.ByteSize Kilobytes(this int input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thislong)'></a>

#### ByteSizeExtensions\.Kilobytes\(this long\) Method

Considers input as kilobytes

```csharp
public static Humanizer.ByteSize Kilobytes(this long input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thissbyte)'></a>

#### ByteSizeExtensions\.Kilobytes\(this sbyte\) Method

Considers input as kilobytes

```csharp
public static Humanizer.ByteSize Kilobytes(this sbyte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisshort)'></a>

#### ByteSizeExtensions\.Kilobytes\(this short\) Method

Considers input as kilobytes

```csharp
public static Humanizer.ByteSize Kilobytes(this short input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisuint)'></a>

#### ByteSizeExtensions\.Kilobytes\(this uint\) Method

Considers input as kilobytes

```csharp
public static Humanizer.ByteSize Kilobytes(this uint input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisushort)'></a>

#### ByteSizeExtensions\.Kilobytes\(this ushort\) Method

Considers input as kilobytes

```csharp
public static Humanizer.ByteSize Kilobytes(this ushort input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Kilobytes(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisbyte)'></a>

#### ByteSizeExtensions\.Megabytes\(this byte\) Method

Considers input as megabytes

```csharp
public static Humanizer.ByteSize Megabytes(this byte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisdouble)'></a>

#### ByteSizeExtensions\.Megabytes\(this double\) Method

Considers input as megabytes

```csharp
public static Humanizer.ByteSize Megabytes(this double input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisint)'></a>

#### ByteSizeExtensions\.Megabytes\(this int\) Method

Considers input as megabytes

```csharp
public static Humanizer.ByteSize Megabytes(this int input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Megabytes(thislong)'></a>

#### ByteSizeExtensions\.Megabytes\(this long\) Method

Considers input as megabytes

```csharp
public static Humanizer.ByteSize Megabytes(this long input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Megabytes(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Megabytes(thissbyte)'></a>

#### ByteSizeExtensions\.Megabytes\(this sbyte\) Method

Considers input as megabytes

```csharp
public static Humanizer.ByteSize Megabytes(this sbyte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Megabytes(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisshort)'></a>

#### ByteSizeExtensions\.Megabytes\(this short\) Method

Considers input as megabytes

```csharp
public static Humanizer.ByteSize Megabytes(this short input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisuint)'></a>

#### ByteSizeExtensions\.Megabytes\(this uint\) Method

Considers input as megabytes

```csharp
public static Humanizer.ByteSize Megabytes(this uint input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisushort)'></a>

#### ByteSizeExtensions\.Megabytes\(this ushort\) Method

Considers input as megabytes

```csharp
public static Humanizer.ByteSize Megabytes(this ushort input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Megabytes(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisbyte)'></a>

#### ByteSizeExtensions\.Pebibytes\(this byte\) Method

Considers input as pebibytes

```csharp
public static Humanizer.ByteSize Pebibytes(this byte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisdouble)'></a>

#### ByteSizeExtensions\.Pebibytes\(this double\) Method

Considers input as pebibytes

```csharp
public static Humanizer.ByteSize Pebibytes(this double input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisint)'></a>

#### ByteSizeExtensions\.Pebibytes\(this int\) Method

Considers input as pebibytes

```csharp
public static Humanizer.ByteSize Pebibytes(this int input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thislong)'></a>

#### ByteSizeExtensions\.Pebibytes\(this long\) Method

Considers input as pebibytes

```csharp
public static Humanizer.ByteSize Pebibytes(this long input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thissbyte)'></a>

#### ByteSizeExtensions\.Pebibytes\(this sbyte\) Method

Considers input as pebibytes

```csharp
public static Humanizer.ByteSize Pebibytes(this sbyte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisshort)'></a>

#### ByteSizeExtensions\.Pebibytes\(this short\) Method

Considers input as pebibytes

```csharp
public static Humanizer.ByteSize Pebibytes(this short input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisuint)'></a>

#### ByteSizeExtensions\.Pebibytes\(this uint\) Method

Considers input as pebibytes

```csharp
public static Humanizer.ByteSize Pebibytes(this uint input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisushort)'></a>

#### ByteSizeExtensions\.Pebibytes\(this ushort\) Method

Considers input as pebibytes

```csharp
public static Humanizer.ByteSize Pebibytes(this ushort input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Pebibytes(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Per(thisHumanizer.ByteSize,System.TimeSpan)'></a>

#### ByteSizeExtensions\.Per\(this ByteSize, TimeSpan\) Method

Turns a quantity of bytes in a given interval into a rate that can be manipulated

```csharp
public static Humanizer.ByteRate Per(this Humanizer.ByteSize size, System.TimeSpan interval);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Per(thisHumanizer.ByteSize,System.TimeSpan).size'></a>

`size` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

Quantity of bytes

<a name='Humanizer.ByteSizeExtensions.Per(thisHumanizer.ByteSize,System.TimeSpan).interval'></a>

`interval` [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan')

Interval to create rate for

##### Returns
[ByteRate](Humanizer.ByteRate.md 'Humanizer\.ByteRate')

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisbyte)'></a>

#### ByteSizeExtensions\.Petabytes\(this byte\) Method

Considers input as petabytes

```csharp
public static Humanizer.ByteSize Petabytes(this byte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisdouble)'></a>

#### ByteSizeExtensions\.Petabytes\(this double\) Method

Considers input as petabytes

```csharp
public static Humanizer.ByteSize Petabytes(this double input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisint)'></a>

#### ByteSizeExtensions\.Petabytes\(this int\) Method

Considers input as petabytes

```csharp
public static Humanizer.ByteSize Petabytes(this int input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Petabytes(thislong)'></a>

#### ByteSizeExtensions\.Petabytes\(this long\) Method

Considers input as petabytes

```csharp
public static Humanizer.ByteSize Petabytes(this long input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Petabytes(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Petabytes(thissbyte)'></a>

#### ByteSizeExtensions\.Petabytes\(this sbyte\) Method

Considers input as petabytes

```csharp
public static Humanizer.ByteSize Petabytes(this sbyte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Petabytes(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisshort)'></a>

#### ByteSizeExtensions\.Petabytes\(this short\) Method

Considers input as petabytes

```csharp
public static Humanizer.ByteSize Petabytes(this short input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisuint)'></a>

#### ByteSizeExtensions\.Petabytes\(this uint\) Method

Considers input as petabytes

```csharp
public static Humanizer.ByteSize Petabytes(this uint input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisushort)'></a>

#### ByteSizeExtensions\.Petabytes\(this ushort\) Method

Considers input as petabytes

```csharp
public static Humanizer.ByteSize Petabytes(this ushort input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Petabytes(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisbyte)'></a>

#### ByteSizeExtensions\.Terabytes\(this byte\) Method

Considers input as terabytes

```csharp
public static Humanizer.ByteSize Terabytes(this byte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisbyte).input'></a>

`input` [System\.Byte](https://learn.microsoft.com/en-us/dotnet/api/system.byte 'System\.Byte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisdouble)'></a>

#### ByteSizeExtensions\.Terabytes\(this double\) Method

Considers input as terabytes

```csharp
public static Humanizer.ByteSize Terabytes(this double input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisint)'></a>

#### ByteSizeExtensions\.Terabytes\(this int\) Method

Considers input as terabytes

```csharp
public static Humanizer.ByteSize Terabytes(this int input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Terabytes(thislong)'></a>

#### ByteSizeExtensions\.Terabytes\(this long\) Method

Considers input as terabytes

```csharp
public static Humanizer.ByteSize Terabytes(this long input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Terabytes(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Terabytes(thissbyte)'></a>

#### ByteSizeExtensions\.Terabytes\(this sbyte\) Method

Considers input as terabytes

```csharp
public static Humanizer.ByteSize Terabytes(this sbyte input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Terabytes(thissbyte).input'></a>

`input` [System\.SByte](https://learn.microsoft.com/en-us/dotnet/api/system.sbyte 'System\.SByte')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisshort)'></a>

#### ByteSizeExtensions\.Terabytes\(this short\) Method

Considers input as terabytes

```csharp
public static Humanizer.ByteSize Terabytes(this short input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisshort).input'></a>

`input` [System\.Int16](https://learn.microsoft.com/en-us/dotnet/api/system.int16 'System\.Int16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisuint)'></a>

#### ByteSizeExtensions\.Terabytes\(this uint\) Method

Considers input as terabytes

```csharp
public static Humanizer.ByteSize Terabytes(this uint input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisushort)'></a>

#### ByteSizeExtensions\.Terabytes\(this ushort\) Method

Considers input as terabytes

```csharp
public static Humanizer.ByteSize Terabytes(this ushort input);
```
##### Parameters

<a name='Humanizer.ByteSizeExtensions.Terabytes(thisushort).input'></a>

`input` [System\.UInt16](https://learn.microsoft.com/en-us/dotnet/api/system.uint16 'System\.UInt16')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')
