---
title: 'Humanizer.TupleizeExtensions'
sidebar_label: 'Humanizer.TupleizeExtensions'
description: 'API reference for Humanizer.TupleizeExtensions.'
---
## TupleizeExtensions Class

Convert int to named tuple strings \(1 \-\> 'single', 2\-\> 'double' etc\.\)\.
Only values 1\-10, 100, and 1000 have specific names\. All others will return 'n\-tuple'\.

```csharp
public static class TupleizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TupleizeExtensions
- *Methods*
  - **[Tupleize\(this int\)](Humanizer.TupleizeExtensions.md#Humanizer.TupleizeExtensions.Tupleize(thisint) 'Humanizer\.TupleizeExtensions\.Tupleize\(this int\)')**
### Methods

<a name='Humanizer.TupleizeExtensions.Tupleize(thisint)'></a>

#### TupleizeExtensions\.Tupleize\(this int\) Method

Converts an integer to its corresponding tuple name \(e\.g\., 'single', 'double', 'triple'\)\.

```csharp
public static string Tupleize(this int input);
```
##### Parameters

<a name='Humanizer.TupleizeExtensions.Tupleize(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The integer value to convert to a tuple name\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string representing the tuple name:
\- 1 returns "single"
\- 2 returns "double"
\- 3 returns "triple"
\- 4 returns "quadruple"
\- 5 returns "quintuple"
\- 6 returns "sextuple"
\- 7 returns "septuple"
\- 8 returns "octuple"
\- 9 returns "nonuple"
\- 10 returns "decuple"
\- 100 returns "centuple"
\- 1000 returns "milluple"
\- Any other value returns "\{value\}\-tuple" \(e\.g\., "42\-tuple"\)

##### Example

```csharp
1.Tupleize() => "single"
2.Tupleize() => "double"
3.Tupleize() => "triple"
10.Tupleize() => "decuple"
100.Tupleize() => "centuple"
42.Tupleize() => "42-tuple"
(-5).Tupleize() => "-5-tuple"
```

##### Remarks
Only values 1\-10, 100, and 1000 have specific named tuples\. All other values return 
a generic n\-tuple format\. Negative values and zero will return in the format "\{value\}\-tuple"\.
