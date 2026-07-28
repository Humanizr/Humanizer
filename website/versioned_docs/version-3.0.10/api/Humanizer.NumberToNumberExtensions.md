## NumberToNumberExtensions Class

Number to Number extensions

```csharp
public static class NumberToNumberExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → NumberToNumberExtensions
### Methods

<a name='Humanizer.NumberToNumberExtensions.Billions(thisdouble)'></a>

## NumberToNumberExtensions\.Billions\(this double\) Method

Multiplies a double by 1,000,000,000 \(one billion in short scale\), providing a more readable way to express billions in code\.

```csharp
public static double Billions(this double input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Billions(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The double value to multiply by 1,000,000,000\.

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')  
The input value multiplied by 1,000,000,000\.

### Example

```csharp
1.0.Billions() => 1000000000.0
```

### Remarks
Uses the short scale definition where 1 billion = 1,000,000,000 \(10^9\)\.

<a name='Humanizer.NumberToNumberExtensions.Billions(thisint)'></a>

## NumberToNumberExtensions\.Billions\(this int\) Method

Multiplies an integer by 1,000,000,000 \(one billion in short scale\), providing a more readable way to express billions in code\.

```csharp
public static int Billions(this int input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Billions(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The integer value to multiply by 1,000,000,000\.

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
The input value multiplied by 1,000,000,000\.

### Example

```csharp
1.Billions() => 1000000000
2.Billions() => 2000000000
```

### Remarks
Uses the short scale definition where 1 billion = 1,000,000,000 \(10^9\)\.

<a name='Humanizer.NumberToNumberExtensions.Billions(thislong)'></a>

## NumberToNumberExtensions\.Billions\(this long\) Method

Multiplies a long integer by 1,000,000,000 \(one billion in short scale\), providing a more readable way to express billions in code\.

```csharp
public static long Billions(this long input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Billions(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The long integer value to multiply by 1,000,000,000\.

#### Returns
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')  
The input value multiplied by 1,000,000,000\.

### Example

```csharp
1L.Billions() => 1000000000L
```

### Remarks
Uses the short scale definition where 1 billion = 1,000,000,000 \(10^9\)\.

<a name='Humanizer.NumberToNumberExtensions.Billions(thisuint)'></a>

## NumberToNumberExtensions\.Billions\(this uint\) Method

Multiplies an unsigned integer by 1,000,000,000 \(one billion in short scale\), providing a more readable way to express billions in code\.

```csharp
public static uint Billions(this uint input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Billions(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The unsigned integer value to multiply by 1,000,000,000\.

#### Returns
[System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')  
The input value multiplied by 1,000,000,000\.

### Example

```csharp
1U.Billions() => 1000000000U
```

### Remarks
Uses the short scale definition where 1 billion = 1,000,000,000 \(10^9\)\.

<a name='Humanizer.NumberToNumberExtensions.Billions(thisulong)'></a>

## NumberToNumberExtensions\.Billions\(this ulong\) Method

Multiplies an unsigned long integer by 1,000,000,000 \(one billion in short scale\), providing a more readable way to express billions in code\.

```csharp
public static ulong Billions(this ulong input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Billions(thisulong).input'></a>

`input` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The unsigned long integer value to multiply by 1,000,000,000\.

#### Returns
[System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')  
The input value multiplied by 1,000,000,000\.

### Example

```csharp
1UL.Billions() => 1000000000UL
```

### Remarks
Uses the short scale definition where 1 billion = 1,000,000,000 \(10^9\)\.

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thisdouble)'></a>

## NumberToNumberExtensions\.Hundreds\(this double\) Method

Multiplies a double by 100, providing a more readable way to express hundreds in code\.

```csharp
public static double Hundreds(this double input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The double value to multiply by 100\.

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')  
The input value multiplied by 100\.

### Example

```csharp
4.0.Hundreds() => 400.0
```

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thisint)'></a>

## NumberToNumberExtensions\.Hundreds\(this int\) Method

Multiplies an integer by 100, providing a more readable way to express hundreds in code\.

```csharp
public static int Hundreds(this int input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The integer value to multiply by 100\.

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
The input value multiplied by 100\.

### Example

```csharp
4.Hundreds() => 400
2.Hundreds() => 200
```

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thislong)'></a>

## NumberToNumberExtensions\.Hundreds\(this long\) Method

Multiplies a long integer by 100, providing a more readable way to express hundreds in code\.

```csharp
public static long Hundreds(this long input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The long integer value to multiply by 100\.

#### Returns
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')  
The input value multiplied by 100\.

### Example

```csharp
4L.Hundreds() => 400L
```

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thisuint)'></a>

## NumberToNumberExtensions\.Hundreds\(this uint\) Method

Multiplies an unsigned integer by 100, providing a more readable way to express hundreds in code\.

```csharp
public static uint Hundreds(this uint input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The unsigned integer value to multiply by 100\.

#### Returns
[System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')  
The input value multiplied by 100\.

### Example

```csharp
4U.Hundreds() => 400U
```

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thisulong)'></a>

## NumberToNumberExtensions\.Hundreds\(this ulong\) Method

Multiplies an unsigned long integer by 100, providing a more readable way to express hundreds in code\.

```csharp
public static ulong Hundreds(this ulong input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Hundreds(thisulong).input'></a>

`input` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The unsigned long integer value to multiply by 100\.

#### Returns
[System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')  
The input value multiplied by 100\.

### Example

```csharp
4UL.Hundreds() => 400UL
```

<a name='Humanizer.NumberToNumberExtensions.Millions(thisdouble)'></a>

## NumberToNumberExtensions\.Millions\(this double\) Method

Multiplies a double by 1,000,000, providing a more readable way to express millions in code\.

```csharp
public static double Millions(this double input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Millions(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The double value to multiply by 1,000,000\.

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')  
The input value multiplied by 1,000,000\.

### Example

```csharp
2.0.Millions() => 2000000.0
```

<a name='Humanizer.NumberToNumberExtensions.Millions(thisint)'></a>

## NumberToNumberExtensions\.Millions\(this int\) Method

Multiplies an integer by 1,000,000, providing a more readable way to express millions in code\.

```csharp
public static int Millions(this int input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Millions(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The integer value to multiply by 1,000,000\.

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
The input value multiplied by 1,000,000\.

### Example

```csharp
2.Millions() => 2000000
5.Millions() => 5000000
```

<a name='Humanizer.NumberToNumberExtensions.Millions(thislong)'></a>

## NumberToNumberExtensions\.Millions\(this long\) Method

Multiplies a long integer by 1,000,000, providing a more readable way to express millions in code\.

```csharp
public static long Millions(this long input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Millions(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The long integer value to multiply by 1,000,000\.

#### Returns
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')  
The input value multiplied by 1,000,000\.

### Example

```csharp
2L.Millions() => 2000000L
```

<a name='Humanizer.NumberToNumberExtensions.Millions(thisuint)'></a>

## NumberToNumberExtensions\.Millions\(this uint\) Method

Multiplies an unsigned integer by 1,000,000, providing a more readable way to express millions in code\.

```csharp
public static uint Millions(this uint input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Millions(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The unsigned integer value to multiply by 1,000,000\.

#### Returns
[System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')  
The input value multiplied by 1,000,000\.

### Example

```csharp
2U.Millions() => 2000000U
```

<a name='Humanizer.NumberToNumberExtensions.Millions(thisulong)'></a>

## NumberToNumberExtensions\.Millions\(this ulong\) Method

Multiplies an unsigned long integer by 1,000,000, providing a more readable way to express millions in code\.

```csharp
public static ulong Millions(this ulong input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Millions(thisulong).input'></a>

`input` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The unsigned long integer value to multiply by 1,000,000\.

#### Returns
[System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')  
The input value multiplied by 1,000,000\.

### Example

```csharp
2UL.Millions() => 2000000UL
```

<a name='Humanizer.NumberToNumberExtensions.Tens(thisdouble)'></a>

## NumberToNumberExtensions\.Tens\(this double\) Method

Multiplies a double by 10, providing a more readable way to express multiples of ten in code\.

```csharp
public static double Tens(this double input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Tens(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The double value to multiply by 10\.

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')  
The input value multiplied by 10\.

### Example

```csharp
5.5.Tens() => 55.0
```

<a name='Humanizer.NumberToNumberExtensions.Tens(thisint)'></a>

## NumberToNumberExtensions\.Tens\(this int\) Method

Multiplies an integer by 10, providing a more readable way to express multiples of ten in code\.

```csharp
public static int Tens(this int input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Tens(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The integer value to multiply by 10\.

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
The input value multiplied by 10\.

### Example

```csharp
5.Tens() => 50
3.Tens() => 30
10.Tens() => 100
```

<a name='Humanizer.NumberToNumberExtensions.Tens(thislong)'></a>

## NumberToNumberExtensions\.Tens\(this long\) Method

Multiplies a long integer by 10, providing a more readable way to express multiples of ten in code\.

```csharp
public static long Tens(this long input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Tens(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The long integer value to multiply by 10\.

#### Returns
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')  
The input value multiplied by 10\.

### Example

```csharp
5L.Tens() => 50L
```

<a name='Humanizer.NumberToNumberExtensions.Tens(thisuint)'></a>

## NumberToNumberExtensions\.Tens\(this uint\) Method

Multiplies an unsigned integer by 10, providing a more readable way to express multiples of ten in code\.

```csharp
public static uint Tens(this uint input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Tens(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The unsigned integer value to multiply by 10\.

#### Returns
[System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')  
The input value multiplied by 10\.

### Example

```csharp
5U.Tens() => 50U
```

<a name='Humanizer.NumberToNumberExtensions.Tens(thisulong)'></a>

## NumberToNumberExtensions\.Tens\(this ulong\) Method

Multiplies an unsigned long integer by 10, providing a more readable way to express multiples of ten in code\.

```csharp
public static ulong Tens(this ulong input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Tens(thisulong).input'></a>

`input` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The unsigned long integer value to multiply by 10\.

#### Returns
[System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')  
The input value multiplied by 10\.

### Example

```csharp
5UL.Tens() => 50UL
```

<a name='Humanizer.NumberToNumberExtensions.Thousands(thisdouble)'></a>

## NumberToNumberExtensions\.Thousands\(this double\) Method

Multiplies a double by 1000, providing a more readable way to express thousands in code\.

```csharp
public static double Thousands(this double input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Thousands(thisdouble).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The double value to multiply by 1000\.

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')  
The input value multiplied by 1000\.

### Example

```csharp
3.0.Thousands() => 3000.0
```

<a name='Humanizer.NumberToNumberExtensions.Thousands(thisint)'></a>

## NumberToNumberExtensions\.Thousands\(this int\) Method

Multiplies an integer by 1000, providing a more readable way to express thousands in code\.

```csharp
public static int Thousands(this int input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Thousands(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The integer value to multiply by 1000\.

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
The input value multiplied by 1000\.

### Example

```csharp
3.Thousands() => 3000
10.Thousands() => 10000
```

<a name='Humanizer.NumberToNumberExtensions.Thousands(thislong)'></a>

## NumberToNumberExtensions\.Thousands\(this long\) Method

Multiplies a long integer by 1000, providing a more readable way to express thousands in code\.

```csharp
public static long Thousands(this long input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Thousands(thislong).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

The long integer value to multiply by 1000\.

#### Returns
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')  
The input value multiplied by 1000\.

### Example

```csharp
3L.Thousands() => 3000L
```

<a name='Humanizer.NumberToNumberExtensions.Thousands(thisuint)'></a>

## NumberToNumberExtensions\.Thousands\(this uint\) Method

Multiplies an unsigned integer by 1000, providing a more readable way to express thousands in code\.

```csharp
public static uint Thousands(this uint input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Thousands(thisuint).input'></a>

`input` [System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')

The unsigned integer value to multiply by 1000\.

#### Returns
[System\.UInt32](https://learn.microsoft.com/en-us/dotnet/api/system.uint32 'System\.UInt32')  
The input value multiplied by 1000\.

### Example

```csharp
3U.Thousands() => 3000U
```

<a name='Humanizer.NumberToNumberExtensions.Thousands(thisulong)'></a>

## NumberToNumberExtensions\.Thousands\(this ulong\) Method

Multiplies an unsigned long integer by 1000, providing a more readable way to express thousands in code\.

```csharp
public static ulong Thousands(this ulong input);
```
#### Parameters

<a name='Humanizer.NumberToNumberExtensions.Thousands(thisulong).input'></a>

`input` [System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')

The unsigned long integer value to multiply by 1000\.

#### Returns
[System\.UInt64](https://learn.microsoft.com/en-us/dotnet/api/system.uint64 'System\.UInt64')  
The input value multiplied by 1000\.

### Example

```csharp
3UL.Thousands() => 3000UL
```