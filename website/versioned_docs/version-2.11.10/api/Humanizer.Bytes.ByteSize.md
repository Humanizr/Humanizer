## ByteSize Struct

Represents a byte size value\.

```csharp
public struct ByteSize : System.IComparable<Humanizer.Bytes.ByteSize>, System.IEquatable<Humanizer.Bytes.ByteSize>, System.IComparable, System.IFormattable
```

Implements [System\.IComparable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1')[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1'), [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1'), [System\.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable 'System\.IComparable'), [System\.IFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iformattable 'System\.IFormattable')
### Constructors

<a name='Humanizer.Bytes.ByteSize.ByteSize(double)'></a>

## ByteSize\(double\) Constructor

```csharp
public ByteSize(double byteSize);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.ByteSize(double).byteSize'></a>

`byteSize` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')
### Fields

<a name='Humanizer.Bytes.ByteSize.Bit'></a>

## ByteSize\.Bit Field

```csharp
public const string Bit = "bit";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.BitsInByte'></a>

## ByteSize\.BitsInByte Field

```csharp
public const long BitsInByte = 8;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Bytes.ByteSize.BitSymbol'></a>

## ByteSize\.BitSymbol Field

```csharp
public const string BitSymbol = "b";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.Byte'></a>

## ByteSize\.Byte Field

```csharp
public const string Byte = "byte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.BytesInGigabyte'></a>

## ByteSize\.BytesInGigabyte Field

```csharp
public const long BytesInGigabyte = 1073741824;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Bytes.ByteSize.BytesInKilobyte'></a>

## ByteSize\.BytesInKilobyte Field

```csharp
public const long BytesInKilobyte = 1024;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Bytes.ByteSize.BytesInMegabyte'></a>

## ByteSize\.BytesInMegabyte Field

```csharp
public const long BytesInMegabyte = 1048576;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Bytes.ByteSize.BytesInTerabyte'></a>

## ByteSize\.BytesInTerabyte Field

```csharp
public const long BytesInTerabyte = 1099511627776;
```

#### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Bytes.ByteSize.ByteSymbol'></a>

## ByteSize\.ByteSymbol Field

```csharp
public const string ByteSymbol = "B";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.Gigabyte'></a>

## ByteSize\.Gigabyte Field

```csharp
public const string Gigabyte = "gigabyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.GigabyteSymbol'></a>

## ByteSize\.GigabyteSymbol Field

```csharp
public const string GigabyteSymbol = "GB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.Kilobyte'></a>

## ByteSize\.Kilobyte Field

```csharp
public const string Kilobyte = "kilobyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.KilobyteSymbol'></a>

## ByteSize\.KilobyteSymbol Field

```csharp
public const string KilobyteSymbol = "KB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.MaxValue'></a>

## ByteSize\.MaxValue Field

```csharp
public static readonly ByteSize MaxValue;
```

#### Field Value
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.Megabyte'></a>

## ByteSize\.Megabyte Field

```csharp
public const string Megabyte = "megabyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.MegabyteSymbol'></a>

## ByteSize\.MegabyteSymbol Field

```csharp
public const string MegabyteSymbol = "MB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.MinValue'></a>

## ByteSize\.MinValue Field

```csharp
public static readonly ByteSize MinValue;
```

#### Field Value
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.Terabyte'></a>

## ByteSize\.Terabyte Field

```csharp
public const string Terabyte = "terabyte";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.TerabyteSymbol'></a>

## ByteSize\.TerabyteSymbol Field

```csharp
public const string TerabyteSymbol = "TB";
```

#### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Properties

<a name='Humanizer.Bytes.ByteSize.Bits'></a>

## ByteSize\.Bits Property

```csharp
public long Bits { get; }
```

#### Property Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.Bytes.ByteSize.Bytes'></a>

## ByteSize\.Bytes Property

```csharp
public double Bytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.Bytes.ByteSize.Gigabytes'></a>

## ByteSize\.Gigabytes Property

```csharp
public double Gigabytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.Bytes.ByteSize.Kilobytes'></a>

## ByteSize\.Kilobytes Property

```csharp
public double Kilobytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.Bytes.ByteSize.LargestWholeNumberFullWord'></a>

## ByteSize\.LargestWholeNumberFullWord Property

```csharp
public string LargestWholeNumberFullWord { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.LargestWholeNumberSymbol'></a>

## ByteSize\.LargestWholeNumberSymbol Property

```csharp
public string LargestWholeNumberSymbol { get; }
```

#### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.LargestWholeNumberValue'></a>

## ByteSize\.LargestWholeNumberValue Property

```csharp
public double LargestWholeNumberValue { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.Bytes.ByteSize.Megabytes'></a>

## ByteSize\.Megabytes Property

```csharp
public double Megabytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.Bytes.ByteSize.Terabytes'></a>

## ByteSize\.Terabytes Property

```csharp
public double Terabytes { get; }
```

#### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')
### Methods

<a name='Humanizer.Bytes.ByteSize.Add(Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.Add\(ByteSize\) Method

```csharp
public Humanizer.Bytes.ByteSize Add(Humanizer.Bytes.ByteSize bs);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.Add(Humanizer.Bytes.ByteSize).bs'></a>

`bs` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.AddBits(long)'></a>

## ByteSize\.AddBits\(long\) Method

```csharp
public Humanizer.Bytes.ByteSize AddBits(long value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.AddBits(long).value'></a>

`value` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.AddBytes(double)'></a>

## ByteSize\.AddBytes\(double\) Method

```csharp
public Humanizer.Bytes.ByteSize AddBytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.AddBytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.AddGigabytes(double)'></a>

## ByteSize\.AddGigabytes\(double\) Method

```csharp
public Humanizer.Bytes.ByteSize AddGigabytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.AddGigabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.AddKilobytes(double)'></a>

## ByteSize\.AddKilobytes\(double\) Method

```csharp
public Humanizer.Bytes.ByteSize AddKilobytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.AddKilobytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.AddMegabytes(double)'></a>

## ByteSize\.AddMegabytes\(double\) Method

```csharp
public Humanizer.Bytes.ByteSize AddMegabytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.AddMegabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.AddTerabytes(double)'></a>

## ByteSize\.AddTerabytes\(double\) Method

```csharp
public Humanizer.Bytes.ByteSize AddTerabytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.AddTerabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.CompareTo(Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.CompareTo\(ByteSize\) Method

```csharp
public int CompareTo(Humanizer.Bytes.ByteSize other);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.CompareTo(Humanizer.Bytes.ByteSize).other'></a>

`other` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.Bytes.ByteSize.CompareTo(object)'></a>

## ByteSize\.CompareTo\(object\) Method

```csharp
public int CompareTo(object obj);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.CompareTo(object).obj'></a>

`obj` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

Implements [CompareTo\(object\)](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable.compareto#system-icomparable-compareto(system-object) 'System\.IComparable\.CompareTo\(System\.Object\)')

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.Bytes.ByteSize.Equals(Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.Equals\(ByteSize\) Method

```csharp
public bool Equals(Humanizer.Bytes.ByteSize value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.Equals(Humanizer.Bytes.ByteSize).value'></a>

`value` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Bytes.ByteSize.Equals(object)'></a>

## ByteSize\.Equals\(object\) Method

```csharp
public override bool Equals(object value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.Equals(object).value'></a>

`value` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Bytes.ByteSize.FromBits(long)'></a>

## ByteSize\.FromBits\(long\) Method

```csharp
public static Humanizer.Bytes.ByteSize FromBits(long value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.FromBits(long).value'></a>

`value` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.FromBytes(double)'></a>

## ByteSize\.FromBytes\(double\) Method

```csharp
public static Humanizer.Bytes.ByteSize FromBytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.FromBytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.FromGigabytes(double)'></a>

## ByteSize\.FromGigabytes\(double\) Method

```csharp
public static Humanizer.Bytes.ByteSize FromGigabytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.FromGigabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.FromKilobytes(double)'></a>

## ByteSize\.FromKilobytes\(double\) Method

```csharp
public static Humanizer.Bytes.ByteSize FromKilobytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.FromKilobytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.FromMegabytes(double)'></a>

## ByteSize\.FromMegabytes\(double\) Method

```csharp
public static Humanizer.Bytes.ByteSize FromMegabytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.FromMegabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.FromTerabytes(double)'></a>

## ByteSize\.FromTerabytes\(double\) Method

```csharp
public static Humanizer.Bytes.ByteSize FromTerabytes(double value);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.FromTerabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.GetHashCode()'></a>

## ByteSize\.GetHashCode\(\) Method

```csharp
public override int GetHashCode();
```

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.Bytes.ByteSize.GetLargestWholeNumberFullWord(System.IFormatProvider)'></a>

## ByteSize\.GetLargestWholeNumberFullWord\(IFormatProvider\) Method

```csharp
public string GetLargestWholeNumberFullWord(System.IFormatProvider provider=null);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.GetLargestWholeNumberFullWord(System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.GetLargestWholeNumberSymbol(System.IFormatProvider)'></a>

## ByteSize\.GetLargestWholeNumberSymbol\(IFormatProvider\) Method

```csharp
public string GetLargestWholeNumberSymbol(System.IFormatProvider provider=null);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.GetLargestWholeNumberSymbol(System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.Parse(string)'></a>

## ByteSize\.Parse\(string\) Method

```csharp
public static Humanizer.Bytes.ByteSize Parse(string s);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.Parse(string).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.Parse(string,System.IFormatProvider)'></a>

## ByteSize\.Parse\(string, IFormatProvider\) Method

```csharp
public static Humanizer.Bytes.ByteSize Parse(string s, System.IFormatProvider formatProvider);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.Parse(string,System.IFormatProvider).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.Parse(string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.Subtract(Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.Subtract\(ByteSize\) Method

```csharp
public Humanizer.Bytes.ByteSize Subtract(Humanizer.Bytes.ByteSize bs);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.Subtract(Humanizer.Bytes.ByteSize).bs'></a>

`bs` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.ToFullWords(string,System.IFormatProvider)'></a>

## ByteSize\.ToFullWords\(string, IFormatProvider\) Method

Converts the value of the current ByteSize object to a string with 
full words\. The metric prefix symbol \(bit, byte, kilo, mega, giga, 
tera\) used is the largest metric prefix such that the corresponding 
value is greater than or equal to one\.

```csharp
public string ToFullWords(string format=null, System.IFormatProvider provider=null);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.ToFullWords(string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.ToFullWords(string,System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.ToString()'></a>

## ByteSize\.ToString\(\) Method

Converts the value of the current ByteSize object to a string\.
The metric prefix symbol \(bit, byte, kilo, mega, giga, tera\) used is
the largest metric prefix such that the corresponding value is greater
 than or equal to one\.

```csharp
public override string ToString();
```

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.ToString(string)'></a>

## ByteSize\.ToString\(string\) Method

```csharp
public string ToString(string format);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.ToString(string).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.ToString(string,System.IFormatProvider)'></a>

## ByteSize\.ToString\(string, IFormatProvider\) Method

```csharp
public string ToString(string format, System.IFormatProvider provider);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.ToString(string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.ToString(string,System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

Implements [ToString\(string, IFormatProvider\)](https://learn.microsoft.com/en-us/dotnet/api/system.iformattable.tostring#system-iformattable-tostring(system-string-system-iformatprovider) 'System\.IFormattable\.ToString\(System\.String,System\.IFormatProvider\)')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.ToString(System.IFormatProvider)'></a>

## ByteSize\.ToString\(IFormatProvider\) Method

```csharp
public string ToString(System.IFormatProvider provider);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.ToString(System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.TryParse(string,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.TryParse\(string, ByteSize\) Method

```csharp
public static bool TryParse(string s, out Humanizer.Bytes.ByteSize result);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.TryParse(string,Humanizer.Bytes.ByteSize).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.TryParse(string,Humanizer.Bytes.ByteSize).result'></a>

`result` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Bytes.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.TryParse\(string, IFormatProvider, ByteSize\) Method

```csharp
public static bool TryParse(string s, System.IFormatProvider formatProvider, out Humanizer.Bytes.ByteSize result);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.Bytes.ByteSize).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.Bytes.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.Bytes.ByteSize).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

<a name='Humanizer.Bytes.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.Bytes.ByteSize).result'></a>

`result` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')
### Operators

<a name='Humanizer.Bytes.ByteSize.op_Addition(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \+\(ByteSize, ByteSize\) Operator

```csharp
public static Humanizer.Bytes.ByteSize operator +(Humanizer.Bytes.ByteSize b1, Humanizer.Bytes.ByteSize b2);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_Addition(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_Addition(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_Decrement(Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \-\-\(ByteSize\) Operator

```csharp
public static Humanizer.Bytes.ByteSize operator --(Humanizer.Bytes.ByteSize b);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_Decrement(Humanizer.Bytes.ByteSize).b'></a>

`b` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_Equality(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator ==\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator ==(Humanizer.Bytes.ByteSize b1, Humanizer.Bytes.ByteSize b2);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_Equality(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_Equality(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Bytes.ByteSize.op_GreaterThan(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \>\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator >(Humanizer.Bytes.ByteSize b1, Humanizer.Bytes.ByteSize b2);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_GreaterThan(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_GreaterThan(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Bytes.ByteSize.op_GreaterThanOrEqual(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \>=\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator >=(Humanizer.Bytes.ByteSize b1, Humanizer.Bytes.ByteSize b2);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_GreaterThanOrEqual(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_GreaterThanOrEqual(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Bytes.ByteSize.op_Increment(Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \+\+\(ByteSize\) Operator

```csharp
public static Humanizer.Bytes.ByteSize operator ++(Humanizer.Bytes.ByteSize b);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_Increment(Humanizer.Bytes.ByteSize).b'></a>

`b` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_Inequality(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \!=\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator !=(Humanizer.Bytes.ByteSize b1, Humanizer.Bytes.ByteSize b2);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_Inequality(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_Inequality(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Bytes.ByteSize.op_LessThan(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \<\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator <(Humanizer.Bytes.ByteSize b1, Humanizer.Bytes.ByteSize b2);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_LessThan(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_LessThan(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Bytes.ByteSize.op_LessThanOrEqual(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \<=\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator <=(Humanizer.Bytes.ByteSize b1, Humanizer.Bytes.ByteSize b2);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_LessThanOrEqual(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_LessThanOrEqual(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.Bytes.ByteSize.op_Subtraction(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \-\(ByteSize, ByteSize\) Operator

```csharp
public static Humanizer.Bytes.ByteSize operator -(Humanizer.Bytes.ByteSize b1, Humanizer.Bytes.ByteSize b2);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_Subtraction(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_Subtraction(Humanizer.Bytes.ByteSize,Humanizer.Bytes.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

<a name='Humanizer.Bytes.ByteSize.op_UnaryNegation(Humanizer.Bytes.ByteSize)'></a>

## ByteSize\.operator \-\(ByteSize\) Operator

```csharp
public static Humanizer.Bytes.ByteSize operator -(Humanizer.Bytes.ByteSize b);
```
#### Parameters

<a name='Humanizer.Bytes.ByteSize.op_UnaryNegation(Humanizer.Bytes.ByteSize).b'></a>

`b` [ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')

#### Returns
[ByteSize](Humanizer.Bytes.ByteSize.md 'Humanizer\.Bytes\.ByteSize')