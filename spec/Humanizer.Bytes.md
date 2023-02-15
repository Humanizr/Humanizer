## `ByteRate`

Class to hold a ByteSize and a measurement interval, for the purpose of calculating the rate of transfer
```csharp
public class Humanizer.Bytes.ByteRate

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `TimeSpan` | Interval | Interval that bytes were transferred in | 
| `ByteSize` | Size | Quantity of bytes | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(`TimeUnit` timeUnit = Second) | Calculate rate for the quantity of bytes and interval defined by this instance | 
| `String` | Humanize(`String` format, `TimeUnit` timeUnit = Second, `CultureInfo` culture = null) | Calculate rate for the quantity of bytes and interval defined by this instance | 


## `ByteSize`

Represents a byte size value.
```csharp
public struct Humanizer.Bytes.ByteSize
    : IComparable<ByteSize>, IEquatable<ByteSize>, IComparable, IFormattable

```

Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `Int64` | Bits |  | 
| `Double` | Bytes |  | 
| `Double` | Gigabytes |  | 
| `Double` | Kilobytes |  | 
| `String` | LargestWholeNumberFullWord |  | 
| `String` | LargestWholeNumberSymbol |  | 
| `Double` | LargestWholeNumberValue |  | 
| `Double` | Megabytes |  | 
| `Double` | Terabytes |  | 


Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `ByteSize` | Add(`ByteSize` bs) |  | 
| `ByteSize` | AddBits(`Int64` value) |  | 
| `ByteSize` | AddBytes(`Double` value) |  | 
| `ByteSize` | AddGigabytes(`Double` value) |  | 
| `ByteSize` | AddKilobytes(`Double` value) |  | 
| `ByteSize` | AddMegabytes(`Double` value) |  | 
| `ByteSize` | AddTerabytes(`Double` value) |  | 
| `Int32` | CompareTo(`Object` obj) |  | 
| `Int32` | CompareTo(`ByteSize` other) |  | 
| `Boolean` | Equals(`Object` value) |  | 
| `Boolean` | Equals(`ByteSize` value) |  | 
| `Int32` | GetHashCode() |  | 
| `String` | GetLargestWholeNumberFullWord(`IFormatProvider` provider = null) |  | 
| `String` | GetLargestWholeNumberSymbol(`IFormatProvider` provider = null) |  | 
| `ByteSize` | Subtract(`ByteSize` bs) |  | 
| `String` | ToFullWords(`String` format = null, `IFormatProvider` provider = null) | Converts the value of the current ByteSize object to a string with  full words. The metric prefix symbol (bit, byte, kilo, mega, giga,  tera) used is the largest metric prefix such that the corresponding  value is greater than or equal to one. | 
| `String` | ToString() | Converts the value of the current ByteSize object to a string.  The metric prefix symbol (bit, byte, kilo, mega, giga, tera) used is  the largest metric prefix such that the corresponding value is greater  than or equal to one. | 
| `String` | ToString(`IFormatProvider` provider) | Converts the value of the current ByteSize object to a string.  The metric prefix symbol (bit, byte, kilo, mega, giga, tera) used is  the largest metric prefix such that the corresponding value is greater  than or equal to one. | 
| `String` | ToString(`String` format) | Converts the value of the current ByteSize object to a string.  The metric prefix symbol (bit, byte, kilo, mega, giga, tera) used is  the largest metric prefix such that the corresponding value is greater  than or equal to one. | 
| `String` | ToString(`String` format, `IFormatProvider` provider) | Converts the value of the current ByteSize object to a string.  The metric prefix symbol (bit, byte, kilo, mega, giga, tera) used is  the largest metric prefix such that the corresponding value is greater  than or equal to one. | 


Static Fields

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Bit |  | 
| `Int64` | BitsInByte |  | 
| `String` | BitSymbol |  | 
| `String` | Byte |  | 
| `Int64` | BytesInGigabyte |  | 
| `Int64` | BytesInKilobyte |  | 
| `Int64` | BytesInMegabyte |  | 
| `Int64` | BytesInTerabyte |  | 
| `String` | ByteSymbol |  | 
| `String` | Gigabyte |  | 
| `String` | GigabyteSymbol |  | 
| `String` | Kilobyte |  | 
| `String` | KilobyteSymbol |  | 
| `ByteSize` | MaxValue |  | 
| `String` | Megabyte |  | 
| `String` | MegabyteSymbol |  | 
| `ByteSize` | MinValue |  | 
| `String` | Terabyte |  | 
| `String` | TerabyteSymbol |  | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `Boolean` | <ToString>g__has|62_0(`String` s, `<>c__DisplayClass62_0&`  = null) |  | 
| `String` | <ToString>g__output|62_1(`Double` n, `<>c__DisplayClass62_0&`  = null) |  | 
| `ByteSize` | FromBits(`Int64` value) |  | 
| `ByteSize` | FromBytes(`Double` value) |  | 
| `ByteSize` | FromGigabytes(`Double` value) |  | 
| `ByteSize` | FromKilobytes(`Double` value) |  | 
| `ByteSize` | FromMegabytes(`Double` value) |  | 
| `ByteSize` | FromTerabytes(`Double` value) |  | 
| `ByteSize` | Parse(`String` s) |  | 
| `ByteSize` | Parse(`String` s, `IFormatProvider` formatProvider) |  | 
| `Boolean` | TryParse(`String` s, `ByteSize&` result) |  | 
| `Boolean` | TryParse(`String` s, `IFormatProvider` formatProvider, `ByteSize&` result) |  | 


