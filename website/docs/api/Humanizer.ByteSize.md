## ByteSize Struct

```csharp
public struct ByteSize : System.IComparable<Humanizer.ByteSize>, System.IEquatable<Humanizer.ByteSize>, System.IComparable, System.IFormattable
```

Implements [System\.IComparable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1')[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1'), [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1'), [System\.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable 'System\.IComparable'), [System\.IFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iformattable 'System\.IFormattable')
### Constructors

<a name='Humanizer.ByteSize.ByteSize(double)'></a>

## ByteSize\(double\) Constructor

```csharp
public ByteSize(double byteSize);
```
#### Parameters

<a name='Humanizer.ByteSize.ByteSize(double).byteSize'></a>

`byteSize` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')
### Fields

<a name='Humanizer.ByteSize.Bit'></a>

## ByteSize\.Bit Field

```csharp
public const string Bit = "bit";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.BitsInByte'></a>

## ByteSize\.BitsInByte Field

```csharp
public const long BitsInByte = 8;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BitSymbol'></a>

## ByteSize\.BitSymbol Field

```csharp
public const string BitSymbol = "b";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Byte'></a>

## ByteSize\.Byte Field

```csharp
public const string Byte = "byte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.BytesInExabyte'></a>

## ByteSize\.BytesInExabyte Field

```csharp
public const long BytesInExabyte = 1000000000000000000;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInGibibyte'></a>

## ByteSize\.BytesInGibibyte Field

The number of bytes in a gibibyte, equivalent to the established gigabyte factor\.

```csharp
public const long BytesInGibibyte = 1073741824;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInGigabyte'></a>

## ByteSize\.BytesInGigabyte Field

```csharp
public const long BytesInGigabyte = 1073741824;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInKibibyte'></a>

## ByteSize\.BytesInKibibyte Field

The number of bytes in a kibibyte, equivalent to the established kilobyte factor\.

```csharp
public const long BytesInKibibyte = 1024;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInKilobyte'></a>

## ByteSize\.BytesInKilobyte Field

```csharp
public const long BytesInKilobyte = 1024;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInMebibyte'></a>

## ByteSize\.BytesInMebibyte Field

The number of bytes in a mebibyte, equivalent to the established megabyte factor\.

```csharp
public const long BytesInMebibyte = 1048576;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInMegabyte'></a>

## ByteSize\.BytesInMegabyte Field

```csharp
public const long BytesInMegabyte = 1048576;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInPebibyte'></a>

## ByteSize\.BytesInPebibyte Field

```csharp
public const long BytesInPebibyte = 1125899906842624;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInPetabyte'></a>

## ByteSize\.BytesInPetabyte Field

```csharp
public const long BytesInPetabyte = 1000000000000000;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInTebibyte'></a>

## ByteSize\.BytesInTebibyte Field

The number of bytes in a tebibyte, equivalent to the established terabyte factor\.

```csharp
public const long BytesInTebibyte = 1099511627776;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInTerabyte'></a>

## ByteSize\.BytesInTerabyte Field

```csharp
public const long BytesInTerabyte = 1099511627776;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.ByteSymbol'></a>

## ByteSize\.ByteSymbol Field

```csharp
public const string ByteSymbol = "B";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Exabyte'></a>

## ByteSize\.Exabyte Field

```csharp
public const string Exabyte = "exabyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ExabyteSymbol'></a>

## ByteSize\.ExabyteSymbol Field

```csharp
public const string ExabyteSymbol = "EB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Gibibyte'></a>

## ByteSize\.Gibibyte Field

The name of a gibibyte\.

```csharp
public const string Gibibyte = "gibibyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.GibibyteSymbol'></a>

## ByteSize\.GibibyteSymbol Field

The symbol for a gibibyte\.

```csharp
public const string GibibyteSymbol = "GiB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Gigabyte'></a>

## ByteSize\.Gigabyte Field

```csharp
public const string Gigabyte = "gigabyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.GigabyteSymbol'></a>

## ByteSize\.GigabyteSymbol Field

```csharp
public const string GigabyteSymbol = "GB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Kibibyte'></a>

## ByteSize\.Kibibyte Field

The name of a kibibyte\.

```csharp
public const string Kibibyte = "kibibyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.KibibyteSymbol'></a>

## ByteSize\.KibibyteSymbol Field

The symbol for a kibibyte\.

```csharp
public const string KibibyteSymbol = "KiB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Kilobyte'></a>

## ByteSize\.Kilobyte Field

```csharp
public const string Kilobyte = "kilobyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.KilobyteSymbol'></a>

## ByteSize\.KilobyteSymbol Field

```csharp
public const string KilobyteSymbol = "KB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.MaxValue'></a>

## ByteSize\.MaxValue Field

```csharp
public static readonly ByteSize MaxValue;
```

#### Field Value
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.Mebibyte'></a>

## ByteSize\.Mebibyte Field

The name of a mebibyte\.

```csharp
public const string Mebibyte = "mebibyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.MebibyteSymbol'></a>

## ByteSize\.MebibyteSymbol Field

The symbol for a mebibyte\.

```csharp
public const string MebibyteSymbol = "MiB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Megabyte'></a>

## ByteSize\.Megabyte Field

```csharp
public const string Megabyte = "megabyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.MegabyteSymbol'></a>

## ByteSize\.MegabyteSymbol Field

```csharp
public const string MegabyteSymbol = "MB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.MinValue'></a>

## ByteSize\.MinValue Field

```csharp
public static readonly ByteSize MinValue;
```

#### Field Value
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.Pebibyte'></a>

## ByteSize\.Pebibyte Field

```csharp
public const string Pebibyte = "pebibyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.PebibyteSymbol'></a>

## ByteSize\.PebibyteSymbol Field

```csharp
public const string PebibyteSymbol = "PiB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Petabyte'></a>

## ByteSize\.Petabyte Field

```csharp
public const string Petabyte = "petabyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.PetabyteSymbol'></a>

## ByteSize\.PetabyteSymbol Field

```csharp
public const string PetabyteSymbol = "PB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Tebibyte'></a>

## ByteSize\.Tebibyte Field

The name of a tebibyte\.

```csharp
public const string Tebibyte = "tebibyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TebibyteSymbol'></a>

## ByteSize\.TebibyteSymbol Field

The symbol for a tebibyte\.

```csharp
public const string TebibyteSymbol = "TiB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Terabyte'></a>

## ByteSize\.Terabyte Field

```csharp
public const string Terabyte = "terabyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TerabyteSymbol'></a>

## ByteSize\.TerabyteSymbol Field

```csharp
public const string TerabyteSymbol = "TB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Properties

<a name='Humanizer.ByteSize.Bits'></a>

## ByteSize\.Bits Property

```csharp
public readonly long Bits { get; }
```

#### Property Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.Bytes'></a>

## ByteSize\.Bytes Property

```csharp
public readonly double Bytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Exabytes'></a>

## ByteSize\.Exabytes Property

```csharp
public readonly double Exabytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Gibibytes'></a>

## ByteSize\.Gibibytes Property

Gets the size in gibibytes\.

```csharp
public readonly double Gibibytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Gigabytes'></a>

## ByteSize\.Gigabytes Property

```csharp
public readonly double Gigabytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Kibibytes'></a>

## ByteSize\.Kibibytes Property

Gets the size in kibibytes\.

```csharp
public readonly double Kibibytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Kilobytes'></a>

## ByteSize\.Kilobytes Property

```csharp
public readonly double Kilobytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.LargestWholeNumberFullWord'></a>

## ByteSize\.LargestWholeNumberFullWord Property

```csharp
public readonly string LargestWholeNumberFullWord { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.LargestWholeNumberSymbol'></a>

## ByteSize\.LargestWholeNumberSymbol Property

```csharp
public readonly string LargestWholeNumberSymbol { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.LargestWholeNumberValue'></a>

## ByteSize\.LargestWholeNumberValue Property

```csharp
public readonly double LargestWholeNumberValue { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Mebibytes'></a>

## ByteSize\.Mebibytes Property

Gets the size in mebibytes\.

```csharp
public readonly double Mebibytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Megabytes'></a>

## ByteSize\.Megabytes Property

```csharp
public readonly double Megabytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Pebibytes'></a>

## ByteSize\.Pebibytes Property

```csharp
public readonly double Pebibytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Petabytes'></a>

## ByteSize\.Petabytes Property

```csharp
public readonly double Petabytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Tebibytes'></a>

## ByteSize\.Tebibytes Property

Gets the size in tebibytes\.

```csharp
public readonly double Tebibytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Terabytes'></a>

## ByteSize\.Terabytes Property

```csharp
public readonly double Terabytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')
### Methods

<a name='Humanizer.ByteSize.Add(Humanizer.ByteSize)'></a>

## ByteSize\.Add\(ByteSize\) Method

```csharp
public readonly Humanizer.ByteSize Add(Humanizer.ByteSize bs);
```
#### Parameters

<a name='Humanizer.ByteSize.Add(Humanizer.ByteSize).bs'></a>

`bs` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddBits(long)'></a>

## ByteSize\.AddBits\(long\) Method

```csharp
public readonly Humanizer.ByteSize AddBits(long value);
```
#### Parameters

<a name='Humanizer.ByteSize.AddBits(long).value'></a>

`value` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddBytes(double)'></a>

## ByteSize\.AddBytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddBytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.AddBytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddExabytes(double)'></a>

## ByteSize\.AddExabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddExabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.AddExabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddGigabytes(double)'></a>

## ByteSize\.AddGigabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddGigabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.AddGigabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddKilobytes(double)'></a>

## ByteSize\.AddKilobytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddKilobytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.AddKilobytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddMegabytes(double)'></a>

## ByteSize\.AddMegabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddMegabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.AddMegabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddPebibytes(double)'></a>

## ByteSize\.AddPebibytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddPebibytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.AddPebibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddPetabytes(double)'></a>

## ByteSize\.AddPetabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddPetabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.AddPetabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddTerabytes(double)'></a>

## ByteSize\.AddTerabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddTerabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.AddTerabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.CompareTo(Humanizer.ByteSize)'></a>

## ByteSize\.CompareTo\(ByteSize\) Method

```csharp
public readonly int CompareTo(Humanizer.ByteSize other);
```
#### Parameters

<a name='Humanizer.ByteSize.CompareTo(Humanizer.ByteSize).other'></a>

`other` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.ByteSize.CompareTo(object)'></a>

## ByteSize\.CompareTo\(object\) Method

```csharp
public readonly int CompareTo(object? obj);
```
#### Parameters

<a name='Humanizer.ByteSize.CompareTo(object).obj'></a>

`obj` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

Implements [CompareTo\(object\)](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable.compareto#system-icomparable-compareto(system-object) 'System\.IComparable\.CompareTo\(System\.Object\)')

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.ByteSize.Equals(Humanizer.ByteSize)'></a>

## ByteSize\.Equals\(ByteSize\) Method

```csharp
public readonly bool Equals(Humanizer.ByteSize value);
```
#### Parameters

<a name='Humanizer.ByteSize.Equals(Humanizer.ByteSize).value'></a>

`value` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.Equals(object)'></a>

## ByteSize\.Equals\(object\) Method

```csharp
public override readonly bool Equals(object? value);
```
#### Parameters

<a name='Humanizer.ByteSize.Equals(object).value'></a>

`value` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.FromBits(long)'></a>

## ByteSize\.FromBits\(long\) Method

```csharp
public static Humanizer.ByteSize FromBits(long value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromBits(long).value'></a>

`value` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromBytes(double)'></a>

## ByteSize\.FromBytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromBytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromBytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromExabytes(double)'></a>

## ByteSize\.FromExabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromExabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromExabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromGibibytes(double)'></a>

## ByteSize\.FromGibibytes\(double\) Method

Creates a byte size from a number of gibibytes\.

```csharp
public static Humanizer.ByteSize FromGibibytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromGibibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromGigabytes(double)'></a>

## ByteSize\.FromGigabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromGigabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromGigabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromKibibytes(double)'></a>

## ByteSize\.FromKibibytes\(double\) Method

Creates a byte size from a number of kibibytes\.

```csharp
public static Humanizer.ByteSize FromKibibytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromKibibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromKilobytes(double)'></a>

## ByteSize\.FromKilobytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromKilobytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromKilobytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromMebibytes(double)'></a>

## ByteSize\.FromMebibytes\(double\) Method

Creates a byte size from a number of mebibytes\.

```csharp
public static Humanizer.ByteSize FromMebibytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromMebibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromMegabytes(double)'></a>

## ByteSize\.FromMegabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromMegabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromMegabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromPebibytes(double)'></a>

## ByteSize\.FromPebibytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromPebibytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromPebibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromPetabytes(double)'></a>

## ByteSize\.FromPetabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromPetabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromPetabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromTebibytes(double)'></a>

## ByteSize\.FromTebibytes\(double\) Method

Creates a byte size from a number of tebibytes\.

```csharp
public static Humanizer.ByteSize FromTebibytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromTebibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromTerabytes(double)'></a>

## ByteSize\.FromTerabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromTerabytes(double value);
```
#### Parameters

<a name='Humanizer.ByteSize.FromTerabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.GetHashCode()'></a>

## ByteSize\.GetHashCode\(\) Method

```csharp
public override readonly int GetHashCode();
```

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.ByteSize.GetLargestWholeNumberFullWord(System.IFormatProvider)'></a>

## ByteSize\.GetLargestWholeNumberFullWord\(IFormatProvider\) Method

```csharp
public readonly string GetLargestWholeNumberFullWord(System.IFormatProvider? provider=null);
```
#### Parameters

<a name='Humanizer.ByteSize.GetLargestWholeNumberFullWord(System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.GetLargestWholeNumberSymbol(System.IFormatProvider)'></a>

## ByteSize\.GetLargestWholeNumberSymbol\(IFormatProvider\) Method

```csharp
public readonly string GetLargestWholeNumberSymbol(System.IFormatProvider? provider=null);
```
#### Parameters

<a name='Humanizer.ByteSize.GetLargestWholeNumberSymbol(System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Parse(string)'></a>

## ByteSize\.Parse\(string\) Method

```csharp
public static Humanizer.ByteSize Parse(string s);
```
#### Parameters

<a name='Humanizer.ByteSize.Parse(string).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.Parse(string,System.IFormatProvider)'></a>

## ByteSize\.Parse\(string, IFormatProvider\) Method

```csharp
public static Humanizer.ByteSize Parse(string s, System.IFormatProvider? formatProvider);
```
#### Parameters

<a name='Humanizer.ByteSize.Parse(string,System.IFormatProvider).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Parse(string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.Subtract(Humanizer.ByteSize)'></a>

## ByteSize\.Subtract\(ByteSize\) Method

```csharp
public readonly Humanizer.ByteSize Subtract(Humanizer.ByteSize bs);
```
#### Parameters

<a name='Humanizer.ByteSize.Subtract(Humanizer.ByteSize).bs'></a>

`bs` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider)'></a>

## ByteSize\.ToFullWords\(string, IFormatProvider\) Method

Converts the value of the current ByteSize object to a string with
full words\. The metric prefix symbol \(bit, byte, kilo, mega, giga,
tera, peta, exa\) used is the largest metric prefix such that the corresponding
value is greater than or equal to one\.

```csharp
public readonly string ToFullWords(string? format=null, System.IFormatProvider? provider=null);
```
#### Parameters

<a name='Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString()'></a>

## ByteSize\.ToString\(\) Method

Converts the value of the current ByteSize object to a string\.
The metric prefix symbol \(bit, byte, kilo, mega, giga, tera, peta, exa\) used is
the largest metric prefix such that the corresponding value is greater
 than or equal to one\.

```csharp
public override readonly string ToString();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString(string)'></a>

## ByteSize\.ToString\(string\) Method

```csharp
public readonly string ToString(string? format);
```
#### Parameters

<a name='Humanizer.ByteSize.ToString(string).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString(string,System.IFormatProvider)'></a>

## ByteSize\.ToString\(string, IFormatProvider\) Method

```csharp
public readonly string ToString(string? format, System.IFormatProvider? provider);
```
#### Parameters

<a name='Humanizer.ByteSize.ToString(string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString(string,System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

Implements [ToString\(string, IFormatProvider\)](https://learn.microsoft.com/en-us/dotnet/api/system.iformattable.tostring#system-iformattable-tostring(system-string-system-iformatprovider) 'System\.IFormattable\.ToString\(System\.String,System\.IFormatProvider\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString(System.IFormatProvider)'></a>

## ByteSize\.ToString\(IFormatProvider\) Method

```csharp
public readonly string ToString(System.IFormatProvider? provider);
```
#### Parameters

<a name='Humanizer.ByteSize.ToString(System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TryParse(string,Humanizer.ByteSize)'></a>

## ByteSize\.TryParse\(string, ByteSize\) Method

```csharp
public static bool TryParse(string? s, out Humanizer.ByteSize result);
```
#### Parameters

<a name='Humanizer.ByteSize.TryParse(string,Humanizer.ByteSize).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TryParse(string,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.ByteSize)'></a>

## ByteSize\.TryParse\(string, IFormatProvider, ByteSize\) Method

```csharp
public static bool TryParse(string? s, System.IFormatProvider? formatProvider, out Humanizer.ByteSize result);
```
#### Parameters

<a name='Humanizer.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.ByteSize).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.ByteSize).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

<a name='Humanizer.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,Humanizer.ByteSize)'></a>

## ByteSize\.TryParse\(ReadOnlySpan\<char\>, ByteSize\) Method

```csharp
public static bool TryParse(System.ReadOnlySpan<char> s, out Humanizer.ByteSize result);
```
#### Parameters

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,Humanizer.ByteSize).s'></a>

`s` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,System.IFormatProvider,Humanizer.ByteSize)'></a>

## ByteSize\.TryParse\(ReadOnlySpan\<char\>, IFormatProvider, ByteSize\) Method

```csharp
public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? formatProvider, out Humanizer.ByteSize result);
```
#### Parameters

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,System.IFormatProvider,Humanizer.ByteSize).s'></a>

`s` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,System.IFormatProvider,Humanizer.ByteSize).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,System.IFormatProvider,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')
### Operators

<a name='Humanizer.ByteSize.op_Addition(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

## ByteSize\.operator \+\(ByteSize, ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator +(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
#### Parameters

<a name='Humanizer.ByteSize.op_Addition(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Addition(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Decrement(Humanizer.ByteSize)'></a>

## ByteSize\.operator \-\-\(ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator --(Humanizer.ByteSize b);
```
#### Parameters

<a name='Humanizer.ByteSize.op_Decrement(Humanizer.ByteSize).b'></a>

`b` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Equality(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

## ByteSize\.operator ==\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator ==(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
#### Parameters

<a name='Humanizer.ByteSize.op_Equality(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Equality(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_GreaterThan(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

## ByteSize\.operator \>\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator >(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
#### Parameters

<a name='Humanizer.ByteSize.op_GreaterThan(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_GreaterThan(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_GreaterThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

## ByteSize\.operator \>=\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator >=(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
#### Parameters

<a name='Humanizer.ByteSize.op_GreaterThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_GreaterThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_Increment(Humanizer.ByteSize)'></a>

## ByteSize\.operator \+\+\(ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator ++(Humanizer.ByteSize b);
```
#### Parameters

<a name='Humanizer.ByteSize.op_Increment(Humanizer.ByteSize).b'></a>

`b` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Inequality(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

## ByteSize\.operator \!=\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator !=(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
#### Parameters

<a name='Humanizer.ByteSize.op_Inequality(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Inequality(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_LessThan(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

## ByteSize\.operator \<\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator <(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
#### Parameters

<a name='Humanizer.ByteSize.op_LessThan(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_LessThan(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_LessThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

## ByteSize\.operator \<=\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator <=(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
#### Parameters

<a name='Humanizer.ByteSize.op_LessThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_LessThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_Subtraction(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

## ByteSize\.operator \-\(ByteSize, ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator -(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
#### Parameters

<a name='Humanizer.ByteSize.op_Subtraction(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Subtraction(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_UnaryNegation(Humanizer.ByteSize)'></a>

## ByteSize\.operator \-\(ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator -(Humanizer.ByteSize b);
```
#### Parameters

<a name='Humanizer.ByteSize.op_UnaryNegation(Humanizer.ByteSize).b'></a>

`b` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

#### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')