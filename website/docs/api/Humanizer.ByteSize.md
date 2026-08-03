---
title: 'Humanizer.ByteSize'
sidebar_label: 'Humanizer.ByteSize'
description: 'API reference for Humanizer.ByteSize.'
---
## ByteSize Struct

```csharp
public struct ByteSize : System.IComparable<Humanizer.ByteSize>, System.IEquatable<Humanizer.ByteSize>, System.IComparable, System.IFormattable
```

Implements [System\.IComparable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1')[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable-1 'System\.IComparable\`1'), [System\.IEquatable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1')[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.iequatable-1 'System\.IEquatable\`1'), [System\.IComparable](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable 'System\.IComparable'), [System\.IFormattable](https://learn.microsoft.com/en-us/dotnet/api/system.iformattable 'System\.IFormattable')
- *Constructors*
  - **[ByteSize\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.ByteSize(double) 'Humanizer\.ByteSize\.ByteSize\(double\)')**
- *Fields*
  - **[Bit](Humanizer.ByteSize.md#Humanizer.ByteSize.Bit 'Humanizer\.ByteSize\.Bit')**
  - **[BitsInByte](Humanizer.ByteSize.md#Humanizer.ByteSize.BitsInByte 'Humanizer\.ByteSize\.BitsInByte')**
  - **[BitSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.BitSymbol 'Humanizer\.ByteSize\.BitSymbol')**
  - **[Byte](Humanizer.ByteSize.md#Humanizer.ByteSize.Byte 'Humanizer\.ByteSize\.Byte')**
  - **[BytesInDecimalExabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInDecimalExabyte 'Humanizer\.ByteSize\.BytesInDecimalExabyte')**
  - **[BytesInDecimalGigabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInDecimalGigabyte 'Humanizer\.ByteSize\.BytesInDecimalGigabyte')**
  - **[BytesInDecimalKilobyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInDecimalKilobyte 'Humanizer\.ByteSize\.BytesInDecimalKilobyte')**
  - **[BytesInDecimalMegabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInDecimalMegabyte 'Humanizer\.ByteSize\.BytesInDecimalMegabyte')**
  - **[BytesInDecimalPetabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInDecimalPetabyte 'Humanizer\.ByteSize\.BytesInDecimalPetabyte')**
  - **[BytesInDecimalTerabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInDecimalTerabyte 'Humanizer\.ByteSize\.BytesInDecimalTerabyte')**
  - **[BytesInExabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInExabyte 'Humanizer\.ByteSize\.BytesInExabyte')**
  - **[BytesInGibibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInGibibyte 'Humanizer\.ByteSize\.BytesInGibibyte')**
  - **[BytesInGigabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInGigabyte 'Humanizer\.ByteSize\.BytesInGigabyte')**
  - **[BytesInKibibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInKibibyte 'Humanizer\.ByteSize\.BytesInKibibyte')**
  - **[BytesInKilobyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInKilobyte 'Humanizer\.ByteSize\.BytesInKilobyte')**
  - **[BytesInMebibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInMebibyte 'Humanizer\.ByteSize\.BytesInMebibyte')**
  - **[BytesInMegabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInMegabyte 'Humanizer\.ByteSize\.BytesInMegabyte')**
  - **[BytesInPebibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInPebibyte 'Humanizer\.ByteSize\.BytesInPebibyte')**
  - **[BytesInPetabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInPetabyte 'Humanizer\.ByteSize\.BytesInPetabyte')**
  - **[BytesInTebibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInTebibyte 'Humanizer\.ByteSize\.BytesInTebibyte')**
  - **[BytesInTerabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.BytesInTerabyte 'Humanizer\.ByteSize\.BytesInTerabyte')**
  - **[ByteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.ByteSymbol 'Humanizer\.ByteSize\.ByteSymbol')**
  - **[Exabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Exabyte 'Humanizer\.ByteSize\.Exabyte')**
  - **[ExabyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.ExabyteSymbol 'Humanizer\.ByteSize\.ExabyteSymbol')**
  - **[Gibibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Gibibyte 'Humanizer\.ByteSize\.Gibibyte')**
  - **[GibibyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.GibibyteSymbol 'Humanizer\.ByteSize\.GibibyteSymbol')**
  - **[Gigabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Gigabyte 'Humanizer\.ByteSize\.Gigabyte')**
  - **[GigabyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.GigabyteSymbol 'Humanizer\.ByteSize\.GigabyteSymbol')**
  - **[Kibibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Kibibyte 'Humanizer\.ByteSize\.Kibibyte')**
  - **[KibibyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.KibibyteSymbol 'Humanizer\.ByteSize\.KibibyteSymbol')**
  - **[Kilobyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Kilobyte 'Humanizer\.ByteSize\.Kilobyte')**
  - **[KilobyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.KilobyteSymbol 'Humanizer\.ByteSize\.KilobyteSymbol')**
  - **[MaxValue](Humanizer.ByteSize.md#Humanizer.ByteSize.MaxValue 'Humanizer\.ByteSize\.MaxValue')**
  - **[Mebibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Mebibyte 'Humanizer\.ByteSize\.Mebibyte')**
  - **[MebibyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.MebibyteSymbol 'Humanizer\.ByteSize\.MebibyteSymbol')**
  - **[Megabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Megabyte 'Humanizer\.ByteSize\.Megabyte')**
  - **[MegabyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.MegabyteSymbol 'Humanizer\.ByteSize\.MegabyteSymbol')**
  - **[MinValue](Humanizer.ByteSize.md#Humanizer.ByteSize.MinValue 'Humanizer\.ByteSize\.MinValue')**
  - **[Pebibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Pebibyte 'Humanizer\.ByteSize\.Pebibyte')**
  - **[PebibyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.PebibyteSymbol 'Humanizer\.ByteSize\.PebibyteSymbol')**
  - **[Petabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Petabyte 'Humanizer\.ByteSize\.Petabyte')**
  - **[PetabyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.PetabyteSymbol 'Humanizer\.ByteSize\.PetabyteSymbol')**
  - **[Tebibyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Tebibyte 'Humanizer\.ByteSize\.Tebibyte')**
  - **[TebibyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.TebibyteSymbol 'Humanizer\.ByteSize\.TebibyteSymbol')**
  - **[Terabyte](Humanizer.ByteSize.md#Humanizer.ByteSize.Terabyte 'Humanizer\.ByteSize\.Terabyte')**
  - **[TerabyteSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.TerabyteSymbol 'Humanizer\.ByteSize\.TerabyteSymbol')**
- *Properties*
  - **[Bits](Humanizer.ByteSize.md#Humanizer.ByteSize.Bits 'Humanizer\.ByteSize\.Bits')**
  - **[Bytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Bytes 'Humanizer\.ByteSize\.Bytes')**
  - **[DecimalExabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.DecimalExabytes 'Humanizer\.ByteSize\.DecimalExabytes')**
  - **[DecimalGigabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.DecimalGigabytes 'Humanizer\.ByteSize\.DecimalGigabytes')**
  - **[DecimalKilobytes](Humanizer.ByteSize.md#Humanizer.ByteSize.DecimalKilobytes 'Humanizer\.ByteSize\.DecimalKilobytes')**
  - **[DecimalMegabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.DecimalMegabytes 'Humanizer\.ByteSize\.DecimalMegabytes')**
  - **[DecimalPetabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.DecimalPetabytes 'Humanizer\.ByteSize\.DecimalPetabytes')**
  - **[DecimalTerabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.DecimalTerabytes 'Humanizer\.ByteSize\.DecimalTerabytes')**
  - **[Exabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Exabytes 'Humanizer\.ByteSize\.Exabytes')**
  - **[Gibibytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Gibibytes 'Humanizer\.ByteSize\.Gibibytes')**
  - **[Gigabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Gigabytes 'Humanizer\.ByteSize\.Gigabytes')**
  - **[Kibibytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Kibibytes 'Humanizer\.ByteSize\.Kibibytes')**
  - **[Kilobytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Kilobytes 'Humanizer\.ByteSize\.Kilobytes')**
  - **[LargestWholeNumberFullWord](Humanizer.ByteSize.md#Humanizer.ByteSize.LargestWholeNumberFullWord 'Humanizer\.ByteSize\.LargestWholeNumberFullWord')**
  - **[LargestWholeNumberSymbol](Humanizer.ByteSize.md#Humanizer.ByteSize.LargestWholeNumberSymbol 'Humanizer\.ByteSize\.LargestWholeNumberSymbol')**
  - **[LargestWholeNumberValue](Humanizer.ByteSize.md#Humanizer.ByteSize.LargestWholeNumberValue 'Humanizer\.ByteSize\.LargestWholeNumberValue')**
  - **[Mebibytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Mebibytes 'Humanizer\.ByteSize\.Mebibytes')**
  - **[Megabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Megabytes 'Humanizer\.ByteSize\.Megabytes')**
  - **[Pebibytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Pebibytes 'Humanizer\.ByteSize\.Pebibytes')**
  - **[Petabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Petabytes 'Humanizer\.ByteSize\.Petabytes')**
  - **[Tebibytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Tebibytes 'Humanizer\.ByteSize\.Tebibytes')**
  - **[Terabytes](Humanizer.ByteSize.md#Humanizer.ByteSize.Terabytes 'Humanizer\.ByteSize\.Terabytes')**
- *Methods*
  - **[Add\(ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.Add(Humanizer.ByteSize) 'Humanizer\.ByteSize\.Add\(Humanizer\.ByteSize\)')**
  - **[AddBits\(long\)](Humanizer.ByteSize.md#Humanizer.ByteSize.AddBits(long) 'Humanizer\.ByteSize\.AddBits\(long\)')**
  - **[AddBytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.AddBytes(double) 'Humanizer\.ByteSize\.AddBytes\(double\)')**
  - **[AddExabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.AddExabytes(double) 'Humanizer\.ByteSize\.AddExabytes\(double\)')**
  - **[AddGigabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.AddGigabytes(double) 'Humanizer\.ByteSize\.AddGigabytes\(double\)')**
  - **[AddKilobytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.AddKilobytes(double) 'Humanizer\.ByteSize\.AddKilobytes\(double\)')**
  - **[AddMegabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.AddMegabytes(double) 'Humanizer\.ByteSize\.AddMegabytes\(double\)')**
  - **[AddPebibytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.AddPebibytes(double) 'Humanizer\.ByteSize\.AddPebibytes\(double\)')**
  - **[AddPetabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.AddPetabytes(double) 'Humanizer\.ByteSize\.AddPetabytes\(double\)')**
  - **[AddTerabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.AddTerabytes(double) 'Humanizer\.ByteSize\.AddTerabytes\(double\)')**
  - **[CompareTo\(ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.CompareTo(Humanizer.ByteSize) 'Humanizer\.ByteSize\.CompareTo\(Humanizer\.ByteSize\)')**
  - **[CompareTo\(object\)](Humanizer.ByteSize.md#Humanizer.ByteSize.CompareTo(object) 'Humanizer\.ByteSize\.CompareTo\(object\)')**
  - **[Equals\(ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.Equals(Humanizer.ByteSize) 'Humanizer\.ByteSize\.Equals\(Humanizer\.ByteSize\)')**
  - **[Equals\(object\)](Humanizer.ByteSize.md#Humanizer.ByteSize.Equals(object) 'Humanizer\.ByteSize\.Equals\(object\)')**
  - **[Format\(ByteSizeUnitSystem, string, IFormatProvider\)](Humanizer.ByteSize.md#Humanizer.ByteSize.Format(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider) 'Humanizer\.ByteSize\.Format\(Humanizer\.ByteSizeUnitSystem, string, System\.IFormatProvider\)')**
  - **[FormatFullWords\(ByteSizeUnitSystem, string, IFormatProvider\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FormatFullWords(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider) 'Humanizer\.ByteSize\.FormatFullWords\(Humanizer\.ByteSizeUnitSystem, string, System\.IFormatProvider\)')**
  - **[FromBits\(long\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromBits(long) 'Humanizer\.ByteSize\.FromBits\(long\)')**
  - **[FromBytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromBytes(double) 'Humanizer\.ByteSize\.FromBytes\(double\)')**
  - **[FromDecimalExabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalExabytes(double) 'Humanizer\.ByteSize\.FromDecimalExabytes\(double\)')**
  - **[FromDecimalGigabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalGigabytes(double) 'Humanizer\.ByteSize\.FromDecimalGigabytes\(double\)')**
  - **[FromDecimalKilobytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalKilobytes(double) 'Humanizer\.ByteSize\.FromDecimalKilobytes\(double\)')**
  - **[FromDecimalMegabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalMegabytes(double) 'Humanizer\.ByteSize\.FromDecimalMegabytes\(double\)')**
  - **[FromDecimalPetabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalPetabytes(double) 'Humanizer\.ByteSize\.FromDecimalPetabytes\(double\)')**
  - **[FromDecimalTerabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalTerabytes(double) 'Humanizer\.ByteSize\.FromDecimalTerabytes\(double\)')**
  - **[FromExabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromExabytes(double) 'Humanizer\.ByteSize\.FromExabytes\(double\)')**
  - **[FromGibibytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromGibibytes(double) 'Humanizer\.ByteSize\.FromGibibytes\(double\)')**
  - **[FromGigabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromGigabytes(double) 'Humanizer\.ByteSize\.FromGigabytes\(double\)')**
  - **[FromKibibytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromKibibytes(double) 'Humanizer\.ByteSize\.FromKibibytes\(double\)')**
  - **[FromKilobytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromKilobytes(double) 'Humanizer\.ByteSize\.FromKilobytes\(double\)')**
  - **[FromMebibytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromMebibytes(double) 'Humanizer\.ByteSize\.FromMebibytes\(double\)')**
  - **[FromMegabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromMegabytes(double) 'Humanizer\.ByteSize\.FromMegabytes\(double\)')**
  - **[FromPebibytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromPebibytes(double) 'Humanizer\.ByteSize\.FromPebibytes\(double\)')**
  - **[FromPetabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromPetabytes(double) 'Humanizer\.ByteSize\.FromPetabytes\(double\)')**
  - **[FromTebibytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromTebibytes(double) 'Humanizer\.ByteSize\.FromTebibytes\(double\)')**
  - **[FromTerabytes\(double\)](Humanizer.ByteSize.md#Humanizer.ByteSize.FromTerabytes(double) 'Humanizer\.ByteSize\.FromTerabytes\(double\)')**
  - **[GetHashCode\(\)](Humanizer.ByteSize.md#Humanizer.ByteSize.GetHashCode() 'Humanizer\.ByteSize\.GetHashCode\(\)')**
  - **[GetLargestWholeNumberFullWord\(IFormatProvider\)](Humanizer.ByteSize.md#Humanizer.ByteSize.GetLargestWholeNumberFullWord(System.IFormatProvider) 'Humanizer\.ByteSize\.GetLargestWholeNumberFullWord\(System\.IFormatProvider\)')**
  - **[GetLargestWholeNumberSymbol\(IFormatProvider\)](Humanizer.ByteSize.md#Humanizer.ByteSize.GetLargestWholeNumberSymbol(System.IFormatProvider) 'Humanizer\.ByteSize\.GetLargestWholeNumberSymbol\(System\.IFormatProvider\)')**
  - **[Parse\(string\)](Humanizer.ByteSize.md#Humanizer.ByteSize.Parse(string) 'Humanizer\.ByteSize\.Parse\(string\)')**
  - **[Parse\(string, IFormatProvider\)](Humanizer.ByteSize.md#Humanizer.ByteSize.Parse(string,System.IFormatProvider) 'Humanizer\.ByteSize\.Parse\(string, System\.IFormatProvider\)')**
  - **[ParseWithUnitSystem\(string, ByteSizeUnitSystem, IFormatProvider\)](Humanizer.ByteSize.md#Humanizer.ByteSize.ParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider) 'Humanizer\.ByteSize\.ParseWithUnitSystem\(string, Humanizer\.ByteSizeUnitSystem, System\.IFormatProvider\)')**
  - **[Subtract\(ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.Subtract(Humanizer.ByteSize) 'Humanizer\.ByteSize\.Subtract\(Humanizer\.ByteSize\)')**
  - **[ToFullWords\(string, IFormatProvider\)](Humanizer.ByteSize.md#Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider) 'Humanizer\.ByteSize\.ToFullWords\(string, System\.IFormatProvider\)')**
  - **[ToString\(\)](Humanizer.ByteSize.md#Humanizer.ByteSize.ToString() 'Humanizer\.ByteSize\.ToString\(\)')**
  - **[ToString\(string\)](Humanizer.ByteSize.md#Humanizer.ByteSize.ToString(string) 'Humanizer\.ByteSize\.ToString\(string\)')**
  - **[ToString\(string, IFormatProvider\)](Humanizer.ByteSize.md#Humanizer.ByteSize.ToString(string,System.IFormatProvider) 'Humanizer\.ByteSize\.ToString\(string, System\.IFormatProvider\)')**
  - **[ToString\(IFormatProvider\)](Humanizer.ByteSize.md#Humanizer.ByteSize.ToString(System.IFormatProvider) 'Humanizer\.ByteSize\.ToString\(System\.IFormatProvider\)')**
  - **[TryParse\(string, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParse(string,Humanizer.ByteSize) 'Humanizer\.ByteSize\.TryParse\(string, Humanizer\.ByteSize\)')**
  - **[TryParse\(string, IFormatProvider, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.ByteSize) 'Humanizer\.ByteSize\.TryParse\(string, System\.IFormatProvider, Humanizer\.ByteSize\)')**
  - **[TryParse\(ReadOnlySpan&lt;char&gt;, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,Humanizer.ByteSize) 'Humanizer\.ByteSize\.TryParse\(System\.ReadOnlySpan\<char\>, Humanizer\.ByteSize\)')**
  - **[TryParse\(ReadOnlySpan&lt;char&gt;, IFormatProvider, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,System.IFormatProvider,Humanizer.ByteSize) 'Humanizer\.ByteSize\.TryParse\(System\.ReadOnlySpan\<char\>, System\.IFormatProvider, Humanizer\.ByteSize\)')**
  - **[TryParseSpanWithUnitSystem\(ReadOnlySpan&lt;char&gt;, ByteSizeUnitSystem, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize) 'Humanizer\.ByteSize\.TryParseSpanWithUnitSystem\(System\.ReadOnlySpan\<char\>, Humanizer\.ByteSizeUnitSystem, Humanizer\.ByteSize\)')**
  - **[TryParseSpanWithUnitSystem\(ReadOnlySpan&lt;char&gt;, ByteSizeUnitSystem, IFormatProvider, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize) 'Humanizer\.ByteSize\.TryParseSpanWithUnitSystem\(System\.ReadOnlySpan\<char\>, Humanizer\.ByteSizeUnitSystem, System\.IFormatProvider, Humanizer\.ByteSize\)')**
  - **[TryParseWithUnitSystem\(string, ByteSizeUnitSystem, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize) 'Humanizer\.ByteSize\.TryParseWithUnitSystem\(string, Humanizer\.ByteSizeUnitSystem, Humanizer\.ByteSize\)')**
  - **[TryParseWithUnitSystem\(string, ByteSizeUnitSystem, IFormatProvider, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize) 'Humanizer\.ByteSize\.TryParseWithUnitSystem\(string, Humanizer\.ByteSizeUnitSystem, System\.IFormatProvider, Humanizer\.ByteSize\)')**
- *Operators*
  - **[operator \+\(ByteSize, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_Addition(Humanizer.ByteSize,Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_Addition\(Humanizer\.ByteSize, Humanizer\.ByteSize\)')**
  - **[operator \-\-\(ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_Decrement(Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_Decrement\(Humanizer\.ByteSize\)')**
  - **[operator ==\(ByteSize, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_Equality(Humanizer.ByteSize,Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_Equality\(Humanizer\.ByteSize, Humanizer\.ByteSize\)')**
  - **[operator &gt;\(ByteSize, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_GreaterThan(Humanizer.ByteSize,Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_GreaterThan\(Humanizer\.ByteSize, Humanizer\.ByteSize\)')**
  - **[operator &gt;=\(ByteSize, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_GreaterThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_GreaterThanOrEqual\(Humanizer\.ByteSize, Humanizer\.ByteSize\)')**
  - **[operator \+\+\(ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_Increment(Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_Increment\(Humanizer\.ByteSize\)')**
  - **[operator \!=\(ByteSize, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_Inequality(Humanizer.ByteSize,Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_Inequality\(Humanizer\.ByteSize, Humanizer\.ByteSize\)')**
  - **[operator &lt;\(ByteSize, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_LessThan(Humanizer.ByteSize,Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_LessThan\(Humanizer\.ByteSize, Humanizer\.ByteSize\)')**
  - **[operator &lt;=\(ByteSize, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_LessThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_LessThanOrEqual\(Humanizer\.ByteSize, Humanizer\.ByteSize\)')**
  - **[operator \-\(ByteSize, ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_Subtraction(Humanizer.ByteSize,Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_Subtraction\(Humanizer\.ByteSize, Humanizer\.ByteSize\)')**
  - **[operator \-\(ByteSize\)](Humanizer.ByteSize.md#Humanizer.ByteSize.op_UnaryNegation(Humanizer.ByteSize) 'Humanizer\.ByteSize\.op\_UnaryNegation\(Humanizer\.ByteSize\)')**
### Constructors

<a name='Humanizer.ByteSize.ByteSize(double)'></a>

#### ByteSize\(double\) Constructor

```csharp
public ByteSize(double byteSize);
```
##### Parameters

<a name='Humanizer.ByteSize.ByteSize(double).byteSize'></a>

`byteSize` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')
### Fields

<a name='Humanizer.ByteSize.Bit'></a>

#### ByteSize\.Bit Field

```csharp
public const string Bit = "bit";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.BitsInByte'></a>

#### ByteSize\.BitsInByte Field

```csharp
public const long BitsInByte = 8;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BitSymbol'></a>

#### ByteSize\.BitSymbol Field

```csharp
public const string BitSymbol = "b";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Byte'></a>

#### ByteSize\.Byte Field

```csharp
public const string Byte = "byte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.BytesInDecimalExabyte'></a>

#### ByteSize\.BytesInDecimalExabyte Field

Gets the number of bytes in one decimal SI exabyte\.

```csharp
public const long BytesInDecimalExabyte = 1000000000000000000;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInDecimalGigabyte'></a>

#### ByteSize\.BytesInDecimalGigabyte Field

Gets the number of bytes in one decimal SI gigabyte\.

```csharp
public const long BytesInDecimalGigabyte = 1000000000;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInDecimalKilobyte'></a>

#### ByteSize\.BytesInDecimalKilobyte Field

Gets the number of bytes in one decimal SI kilobyte\.

```csharp
public const long BytesInDecimalKilobyte = 1000;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInDecimalMegabyte'></a>

#### ByteSize\.BytesInDecimalMegabyte Field

Gets the number of bytes in one decimal SI megabyte\.

```csharp
public const long BytesInDecimalMegabyte = 1000000;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInDecimalPetabyte'></a>

#### ByteSize\.BytesInDecimalPetabyte Field

Gets the number of bytes in one decimal SI petabyte\.

```csharp
public const long BytesInDecimalPetabyte = 1000000000000000;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInDecimalTerabyte'></a>

#### ByteSize\.BytesInDecimalTerabyte Field

Gets the number of bytes in one decimal SI terabyte\.

```csharp
public const long BytesInDecimalTerabyte = 1000000000000;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInExabyte'></a>

#### ByteSize\.BytesInExabyte Field

```csharp
public const long BytesInExabyte = 1000000000000000000;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInGibibyte'></a>

#### ByteSize\.BytesInGibibyte Field

The number of bytes in a gibibyte, equivalent to the established gigabyte factor\.

```csharp
public const long BytesInGibibyte = 1073741824;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInGigabyte'></a>

#### ByteSize\.BytesInGigabyte Field

```csharp
public const long BytesInGigabyte = 1073741824;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInKibibyte'></a>

#### ByteSize\.BytesInKibibyte Field

The number of bytes in a kibibyte, equivalent to the established kilobyte factor\.

```csharp
public const long BytesInKibibyte = 1024;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInKilobyte'></a>

#### ByteSize\.BytesInKilobyte Field

```csharp
public const long BytesInKilobyte = 1024;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInMebibyte'></a>

#### ByteSize\.BytesInMebibyte Field

The number of bytes in a mebibyte, equivalent to the established megabyte factor\.

```csharp
public const long BytesInMebibyte = 1048576;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInMegabyte'></a>

#### ByteSize\.BytesInMegabyte Field

```csharp
public const long BytesInMegabyte = 1048576;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInPebibyte'></a>

#### ByteSize\.BytesInPebibyte Field

```csharp
public const long BytesInPebibyte = 1125899906842624;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInPetabyte'></a>

#### ByteSize\.BytesInPetabyte Field

```csharp
public const long BytesInPetabyte = 1000000000000000;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInTebibyte'></a>

#### ByteSize\.BytesInTebibyte Field

The number of bytes in a tebibyte, equivalent to the established terabyte factor\.

```csharp
public const long BytesInTebibyte = 1099511627776;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.BytesInTerabyte'></a>

#### ByteSize\.BytesInTerabyte Field

```csharp
public const long BytesInTerabyte = 1099511627776;
```

##### Field Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.ByteSymbol'></a>

#### ByteSize\.ByteSymbol Field

```csharp
public const string ByteSymbol = "B";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Exabyte'></a>

#### ByteSize\.Exabyte Field

```csharp
public const string Exabyte = "exabyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ExabyteSymbol'></a>

#### ByteSize\.ExabyteSymbol Field

```csharp
public const string ExabyteSymbol = "EB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Gibibyte'></a>

#### ByteSize\.Gibibyte Field

The name of a gibibyte\.

```csharp
public const string Gibibyte = "gibibyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.GibibyteSymbol'></a>

#### ByteSize\.GibibyteSymbol Field

The symbol for a gibibyte\.

```csharp
public const string GibibyteSymbol = "GiB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Gigabyte'></a>

#### ByteSize\.Gigabyte Field

```csharp
public const string Gigabyte = "gigabyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.GigabyteSymbol'></a>

#### ByteSize\.GigabyteSymbol Field

```csharp
public const string GigabyteSymbol = "GB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Kibibyte'></a>

#### ByteSize\.Kibibyte Field

The name of a kibibyte\.

```csharp
public const string Kibibyte = "kibibyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.KibibyteSymbol'></a>

#### ByteSize\.KibibyteSymbol Field

The symbol for a kibibyte\.

```csharp
public const string KibibyteSymbol = "KiB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Kilobyte'></a>

#### ByteSize\.Kilobyte Field

```csharp
public const string Kilobyte = "kilobyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.KilobyteSymbol'></a>

#### ByteSize\.KilobyteSymbol Field

```csharp
public const string KilobyteSymbol = "KB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.MaxValue'></a>

#### ByteSize\.MaxValue Field

```csharp
public static readonly ByteSize MaxValue;
```

##### Field Value
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.Mebibyte'></a>

#### ByteSize\.Mebibyte Field

The name of a mebibyte\.

```csharp
public const string Mebibyte = "mebibyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.MebibyteSymbol'></a>

#### ByteSize\.MebibyteSymbol Field

The symbol for a mebibyte\.

```csharp
public const string MebibyteSymbol = "MiB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Megabyte'></a>

#### ByteSize\.Megabyte Field

```csharp
public const string Megabyte = "megabyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.MegabyteSymbol'></a>

#### ByteSize\.MegabyteSymbol Field

```csharp
public const string MegabyteSymbol = "MB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.MinValue'></a>

#### ByteSize\.MinValue Field

```csharp
public static readonly ByteSize MinValue;
```

##### Field Value
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.Pebibyte'></a>

#### ByteSize\.Pebibyte Field

```csharp
public const string Pebibyte = "pebibyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.PebibyteSymbol'></a>

#### ByteSize\.PebibyteSymbol Field

```csharp
public const string PebibyteSymbol = "PiB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Petabyte'></a>

#### ByteSize\.Petabyte Field

```csharp
public const string Petabyte = "petabyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.PetabyteSymbol'></a>

#### ByteSize\.PetabyteSymbol Field

```csharp
public const string PetabyteSymbol = "PB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Tebibyte'></a>

#### ByteSize\.Tebibyte Field

The name of a tebibyte\.

```csharp
public const string Tebibyte = "tebibyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TebibyteSymbol'></a>

#### ByteSize\.TebibyteSymbol Field

The symbol for a tebibyte\.

```csharp
public const string TebibyteSymbol = "TiB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Terabyte'></a>

#### ByteSize\.Terabyte Field

```csharp
public const string Terabyte = "terabyte";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TerabyteSymbol'></a>

#### ByteSize\.TerabyteSymbol Field

```csharp
public const string TerabyteSymbol = "TB";
```

##### Field Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')
### Properties

<a name='Humanizer.ByteSize.Bits'></a>

#### ByteSize\.Bits Property

```csharp
public readonly long Bits { get; }
```

##### Property Value
[System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

<a name='Humanizer.ByteSize.Bytes'></a>

#### ByteSize\.Bytes Property

```csharp
public readonly double Bytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.DecimalExabytes'></a>

#### ByteSize\.DecimalExabytes Property

Gets this value expressed in decimal SI exabytes\.

```csharp
public readonly double DecimalExabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.DecimalGigabytes'></a>

#### ByteSize\.DecimalGigabytes Property

Gets this value expressed in decimal SI gigabytes\.

```csharp
public readonly double DecimalGigabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.DecimalKilobytes'></a>

#### ByteSize\.DecimalKilobytes Property

Gets this value expressed in decimal SI kilobytes\.

```csharp
public readonly double DecimalKilobytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.DecimalMegabytes'></a>

#### ByteSize\.DecimalMegabytes Property

Gets this value expressed in decimal SI megabytes\.

```csharp
public readonly double DecimalMegabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.DecimalPetabytes'></a>

#### ByteSize\.DecimalPetabytes Property

Gets this value expressed in decimal SI petabytes\.

```csharp
public readonly double DecimalPetabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.DecimalTerabytes'></a>

#### ByteSize\.DecimalTerabytes Property

Gets this value expressed in decimal SI terabytes\.

```csharp
public readonly double DecimalTerabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Exabytes'></a>

#### ByteSize\.Exabytes Property

```csharp
public readonly double Exabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Gibibytes'></a>

#### ByteSize\.Gibibytes Property

Gets the size in gibibytes\.

```csharp
public readonly double Gibibytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Gigabytes'></a>

#### ByteSize\.Gigabytes Property

```csharp
public readonly double Gigabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Kibibytes'></a>

#### ByteSize\.Kibibytes Property

Gets the size in kibibytes\.

```csharp
public readonly double Kibibytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Kilobytes'></a>

#### ByteSize\.Kilobytes Property

```csharp
public readonly double Kilobytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.LargestWholeNumberFullWord'></a>

#### ByteSize\.LargestWholeNumberFullWord Property

```csharp
public readonly string LargestWholeNumberFullWord { get; }
```

##### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.LargestWholeNumberSymbol'></a>

#### ByteSize\.LargestWholeNumberSymbol Property

```csharp
public readonly string LargestWholeNumberSymbol { get; }
```

##### Property Value
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.LargestWholeNumberValue'></a>

#### ByteSize\.LargestWholeNumberValue Property

```csharp
public readonly double LargestWholeNumberValue { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Mebibytes'></a>

#### ByteSize\.Mebibytes Property

Gets the size in mebibytes\.

```csharp
public readonly double Mebibytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Megabytes'></a>

#### ByteSize\.Megabytes Property

```csharp
public readonly double Megabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Pebibytes'></a>

#### ByteSize\.Pebibytes Property

```csharp
public readonly double Pebibytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Petabytes'></a>

#### ByteSize\.Petabytes Property

```csharp
public readonly double Petabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Tebibytes'></a>

#### ByteSize\.Tebibytes Property

Gets the size in tebibytes\.

```csharp
public readonly double Tebibytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

<a name='Humanizer.ByteSize.Terabytes'></a>

#### ByteSize\.Terabytes Property

```csharp
public readonly double Terabytes { get; }
```

##### Property Value
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')
### Methods

<a name='Humanizer.ByteSize.Add(Humanizer.ByteSize)'></a>

#### ByteSize\.Add\(ByteSize\) Method

```csharp
public readonly Humanizer.ByteSize Add(Humanizer.ByteSize bs);
```
##### Parameters

<a name='Humanizer.ByteSize.Add(Humanizer.ByteSize).bs'></a>

`bs` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddBits(long)'></a>

#### ByteSize\.AddBits\(long\) Method

```csharp
public readonly Humanizer.ByteSize AddBits(long value);
```
##### Parameters

<a name='Humanizer.ByteSize.AddBits(long).value'></a>

`value` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddBytes(double)'></a>

#### ByteSize\.AddBytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddBytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.AddBytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddExabytes(double)'></a>

#### ByteSize\.AddExabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddExabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.AddExabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddGigabytes(double)'></a>

#### ByteSize\.AddGigabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddGigabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.AddGigabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddKilobytes(double)'></a>

#### ByteSize\.AddKilobytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddKilobytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.AddKilobytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddMegabytes(double)'></a>

#### ByteSize\.AddMegabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddMegabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.AddMegabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddPebibytes(double)'></a>

#### ByteSize\.AddPebibytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddPebibytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.AddPebibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddPetabytes(double)'></a>

#### ByteSize\.AddPetabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddPetabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.AddPetabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.AddTerabytes(double)'></a>

#### ByteSize\.AddTerabytes\(double\) Method

```csharp
public readonly Humanizer.ByteSize AddTerabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.AddTerabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.CompareTo(Humanizer.ByteSize)'></a>

#### ByteSize\.CompareTo\(ByteSize\) Method

```csharp
public readonly int CompareTo(Humanizer.ByteSize other);
```
##### Parameters

<a name='Humanizer.ByteSize.CompareTo(Humanizer.ByteSize).other'></a>

`other` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.ByteSize.CompareTo(object)'></a>

#### ByteSize\.CompareTo\(object\) Method

```csharp
public readonly int CompareTo(object? obj);
```
##### Parameters

<a name='Humanizer.ByteSize.CompareTo(object).obj'></a>

`obj` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

Implements [CompareTo\(object\)](https://learn.microsoft.com/en-us/dotnet/api/system.icomparable.compareto#system-icomparable-compareto(system-object) 'System\.IComparable\.CompareTo\(System\.Object\)')

##### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.ByteSize.Equals(Humanizer.ByteSize)'></a>

#### ByteSize\.Equals\(ByteSize\) Method

```csharp
public readonly bool Equals(Humanizer.ByteSize value);
```
##### Parameters

<a name='Humanizer.ByteSize.Equals(Humanizer.ByteSize).value'></a>

`value` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.Equals(object)'></a>

#### ByteSize\.Equals\(object\) Method

```csharp
public override readonly bool Equals(object? value);
```
##### Parameters

<a name='Humanizer.ByteSize.Equals(object).value'></a>

`value` [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.Format(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider)'></a>

#### ByteSize\.Format\(ByteSizeUnitSystem, string, IFormatProvider\) Method

Formats this value with an explicitly selected unit system\.

```csharp
public readonly string Format(Humanizer.ByteSizeUnitSystem unitSystem, string? format=null, System.IFormatProvider? formatProvider=null);
```
##### Parameters

<a name='Humanizer.ByteSize.Format(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).unitSystem'></a>

`unitSystem` [ByteSizeUnitSystem](Humanizer.ByteSizeUnitSystem.md 'Humanizer\.ByteSizeUnitSystem')

The unit system to use\.

<a name='Humanizer.ByteSize.Format(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The numeric format and optional unit token\. For [DecimalSi](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.DecimalSi 'Humanizer\.ByteSizeUnitSystem\.DecimalSi') and
[BinaryIec](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.BinaryIec 'Humanizer\.ByteSizeUnitSystem\.BinaryIec'), SI/IEC\-prefixed unit tokens are matched case\-insensitively,
while `b` and `B` remain case\-sensitive; output uses canonical symbol casing\.
[Legacy](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.Legacy 'Humanizer\.ByteSizeUnitSystem\.Legacy') preserves established matching behavior\. At most one distinct
unescaped, unquoted unit token is permitted, although the same token may be repeated across numeric format
sections\.

<a name='Humanizer.ByteSize.Format(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The provider used to format the numeric value\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The formatted byte size\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[unitSystem](Humanizer.ByteSize.md#Humanizer.ByteSize.Format(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).unitSystem 'Humanizer\.ByteSize\.Format\(Humanizer\.ByteSizeUnitSystem, string, System\.IFormatProvider\)\.unitSystem') is not defined\.

[System\.FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception 'System\.FormatException')  
[format](Humanizer.ByteSize.md#Humanizer.ByteSize.Format(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).format 'Humanizer\.ByteSize\.Format\(Humanizer\.ByteSizeUnitSystem, string, System\.IFormatProvider\)\.format') is invalid, contains mixed unit tokens, or selects a token not supported by the
            selected non\-legacy system\.

<a name='Humanizer.ByteSize.FormatFullWords(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider)'></a>

#### ByteSize\.FormatFullWords\(ByteSizeUnitSystem, string, IFormatProvider\) Method

Formats this value with localized unit words from an explicitly selected unit system\.

```csharp
public readonly string FormatFullWords(Humanizer.ByteSizeUnitSystem unitSystem, string? format=null, System.IFormatProvider? formatProvider=null);
```
##### Parameters

<a name='Humanizer.ByteSize.FormatFullWords(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).unitSystem'></a>

`unitSystem` [ByteSizeUnitSystem](Humanizer.ByteSizeUnitSystem.md 'Humanizer\.ByteSizeUnitSystem')

The unit system to use\.

<a name='Humanizer.ByteSize.FormatFullWords(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The numeric format and optional unit token\. For [DecimalSi](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.DecimalSi 'Humanizer\.ByteSizeUnitSystem\.DecimalSi') and
[BinaryIec](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.BinaryIec 'Humanizer\.ByteSizeUnitSystem\.BinaryIec'), SI/IEC\-prefixed unit tokens are matched case\-insensitively,
while `b` and `B` remain case\-sensitive; localized words replace the selected token\.
[Legacy](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.Legacy 'Humanizer\.ByteSizeUnitSystem\.Legacy') preserves established matching behavior\. At most one distinct
unescaped, unquoted unit token is permitted, although the same token may be repeated across numeric format
sections\.

<a name='Humanizer.ByteSize.FormatFullWords(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The provider used to format the numeric value and select localized unit words\.

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The formatted byte size using localized unit words\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[unitSystem](Humanizer.ByteSize.md#Humanizer.ByteSize.FormatFullWords(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).unitSystem 'Humanizer\.ByteSize\.FormatFullWords\(Humanizer\.ByteSizeUnitSystem, string, System\.IFormatProvider\)\.unitSystem') is not defined\.

[System\.FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception 'System\.FormatException')  
[format](Humanizer.ByteSize.md#Humanizer.ByteSize.FormatFullWords(Humanizer.ByteSizeUnitSystem,string,System.IFormatProvider).format 'Humanizer\.ByteSize\.FormatFullWords\(Humanizer\.ByteSizeUnitSystem, string, System\.IFormatProvider\)\.format') is invalid, contains mixed unit tokens, or selects a token not supported by the
            selected non\-legacy system\.

<a name='Humanizer.ByteSize.FromBits(long)'></a>

#### ByteSize\.FromBits\(long\) Method

```csharp
public static Humanizer.ByteSize FromBits(long value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromBits(long).value'></a>

`value` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromBytes(double)'></a>

#### ByteSize\.FromBytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromBytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromBytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromDecimalExabytes(double)'></a>

#### ByteSize\.FromDecimalExabytes\(double\) Method

Creates a byte size from a number of decimal SI exabytes\.

```csharp
public static Humanizer.ByteSize FromDecimalExabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromDecimalExabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of decimal SI exabytes\.

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')  
The equivalent byte size\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[value](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalExabytes(double).value 'Humanizer\.ByteSize\.FromDecimalExabytes\(double\)\.value') is not finite or is outside the range supported by [Bits](Humanizer.ByteSize.md#Humanizer.ByteSize.Bits 'Humanizer\.ByteSize\.Bits')\.

<a name='Humanizer.ByteSize.FromDecimalGigabytes(double)'></a>

#### ByteSize\.FromDecimalGigabytes\(double\) Method

Creates a byte size from a number of decimal SI gigabytes\.

```csharp
public static Humanizer.ByteSize FromDecimalGigabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromDecimalGigabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of decimal SI gigabytes\.

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')  
The equivalent byte size\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[value](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalGigabytes(double).value 'Humanizer\.ByteSize\.FromDecimalGigabytes\(double\)\.value') is not finite or is outside the range supported by [Bits](Humanizer.ByteSize.md#Humanizer.ByteSize.Bits 'Humanizer\.ByteSize\.Bits')\.

<a name='Humanizer.ByteSize.FromDecimalKilobytes(double)'></a>

#### ByteSize\.FromDecimalKilobytes\(double\) Method

Creates a byte size from a number of decimal SI kilobytes\.

```csharp
public static Humanizer.ByteSize FromDecimalKilobytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromDecimalKilobytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of decimal SI kilobytes\.

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')  
The equivalent byte size\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[value](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalKilobytes(double).value 'Humanizer\.ByteSize\.FromDecimalKilobytes\(double\)\.value') is not finite or is outside the range supported by [Bits](Humanizer.ByteSize.md#Humanizer.ByteSize.Bits 'Humanizer\.ByteSize\.Bits')\.

<a name='Humanizer.ByteSize.FromDecimalMegabytes(double)'></a>

#### ByteSize\.FromDecimalMegabytes\(double\) Method

Creates a byte size from a number of decimal SI megabytes\.

```csharp
public static Humanizer.ByteSize FromDecimalMegabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromDecimalMegabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of decimal SI megabytes\.

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')  
The equivalent byte size\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[value](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalMegabytes(double).value 'Humanizer\.ByteSize\.FromDecimalMegabytes\(double\)\.value') is not finite or is outside the range supported by [Bits](Humanizer.ByteSize.md#Humanizer.ByteSize.Bits 'Humanizer\.ByteSize\.Bits')\.

<a name='Humanizer.ByteSize.FromDecimalPetabytes(double)'></a>

#### ByteSize\.FromDecimalPetabytes\(double\) Method

Creates a byte size from a number of decimal SI petabytes\.

```csharp
public static Humanizer.ByteSize FromDecimalPetabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromDecimalPetabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of decimal SI petabytes\.

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')  
The equivalent byte size\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[value](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalPetabytes(double).value 'Humanizer\.ByteSize\.FromDecimalPetabytes\(double\)\.value') is not finite or is outside the range supported by [Bits](Humanizer.ByteSize.md#Humanizer.ByteSize.Bits 'Humanizer\.ByteSize\.Bits')\.

<a name='Humanizer.ByteSize.FromDecimalTerabytes(double)'></a>

#### ByteSize\.FromDecimalTerabytes\(double\) Method

Creates a byte size from a number of decimal SI terabytes\.

```csharp
public static Humanizer.ByteSize FromDecimalTerabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromDecimalTerabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

The number of decimal SI terabytes\.

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')  
The equivalent byte size\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[value](Humanizer.ByteSize.md#Humanizer.ByteSize.FromDecimalTerabytes(double).value 'Humanizer\.ByteSize\.FromDecimalTerabytes\(double\)\.value') is not finite or is outside the range supported by [Bits](Humanizer.ByteSize.md#Humanizer.ByteSize.Bits 'Humanizer\.ByteSize\.Bits')\.

<a name='Humanizer.ByteSize.FromExabytes(double)'></a>

#### ByteSize\.FromExabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromExabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromExabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromGibibytes(double)'></a>

#### ByteSize\.FromGibibytes\(double\) Method

Creates a byte size from a number of gibibytes\.

```csharp
public static Humanizer.ByteSize FromGibibytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromGibibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromGigabytes(double)'></a>

#### ByteSize\.FromGigabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromGigabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromGigabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromKibibytes(double)'></a>

#### ByteSize\.FromKibibytes\(double\) Method

Creates a byte size from a number of kibibytes\.

```csharp
public static Humanizer.ByteSize FromKibibytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromKibibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromKilobytes(double)'></a>

#### ByteSize\.FromKilobytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromKilobytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromKilobytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromMebibytes(double)'></a>

#### ByteSize\.FromMebibytes\(double\) Method

Creates a byte size from a number of mebibytes\.

```csharp
public static Humanizer.ByteSize FromMebibytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromMebibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromMegabytes(double)'></a>

#### ByteSize\.FromMegabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromMegabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromMegabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromPebibytes(double)'></a>

#### ByteSize\.FromPebibytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromPebibytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromPebibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromPetabytes(double)'></a>

#### ByteSize\.FromPetabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromPetabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromPetabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromTebibytes(double)'></a>

#### ByteSize\.FromTebibytes\(double\) Method

Creates a byte size from a number of tebibytes\.

```csharp
public static Humanizer.ByteSize FromTebibytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromTebibytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.FromTerabytes(double)'></a>

#### ByteSize\.FromTerabytes\(double\) Method

```csharp
public static Humanizer.ByteSize FromTerabytes(double value);
```
##### Parameters

<a name='Humanizer.ByteSize.FromTerabytes(double).value'></a>

`value` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.GetHashCode()'></a>

#### ByteSize\.GetHashCode\(\) Method

```csharp
public override readonly int GetHashCode();
```

##### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

<a name='Humanizer.ByteSize.GetLargestWholeNumberFullWord(System.IFormatProvider)'></a>

#### ByteSize\.GetLargestWholeNumberFullWord\(IFormatProvider\) Method

```csharp
public readonly string GetLargestWholeNumberFullWord(System.IFormatProvider? provider=null);
```
##### Parameters

<a name='Humanizer.ByteSize.GetLargestWholeNumberFullWord(System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.GetLargestWholeNumberSymbol(System.IFormatProvider)'></a>

#### ByteSize\.GetLargestWholeNumberSymbol\(IFormatProvider\) Method

```csharp
public readonly string GetLargestWholeNumberSymbol(System.IFormatProvider? provider=null);
```
##### Parameters

<a name='Humanizer.ByteSize.GetLargestWholeNumberSymbol(System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Parse(string)'></a>

#### ByteSize\.Parse\(string\) Method

```csharp
public static Humanizer.ByteSize Parse(string s);
```
##### Parameters

<a name='Humanizer.ByteSize.Parse(string).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.Parse(string,System.IFormatProvider)'></a>

#### ByteSize\.Parse\(string, IFormatProvider\) Method

```csharp
public static Humanizer.ByteSize Parse(string s, System.IFormatProvider? formatProvider);
```
##### Parameters

<a name='Humanizer.ByteSize.Parse(string,System.IFormatProvider).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.Parse(string,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.ParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider)'></a>

#### ByteSize\.ParseWithUnitSystem\(string, ByteSizeUnitSystem, IFormatProvider\) Method

Parses a byte size using only the tokens defined by the selected unit system\.

```csharp
public static Humanizer.ByteSize ParseWithUnitSystem(string s, Humanizer.ByteSizeUnitSystem unitSystem, System.IFormatProvider? formatProvider=null);
```
##### Parameters

<a name='Humanizer.ByteSize.ParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to parse\.

<a name='Humanizer.ByteSize.ParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider).unitSystem'></a>

`unitSystem` [ByteSizeUnitSystem](Humanizer.ByteSizeUnitSystem.md 'Humanizer\.ByteSizeUnitSystem')

The unit system whose tokens are accepted\.

<a name='Humanizer.ByteSize.ParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The provider used to parse the numeric value\.

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')  
The parsed byte size\.

##### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
[s](Humanizer.ByteSize.md#Humanizer.ByteSize.ParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider).s 'Humanizer\.ByteSize\.ParseWithUnitSystem\(string, Humanizer\.ByteSizeUnitSystem, System\.IFormatProvider\)\.s') is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[unitSystem](Humanizer.ByteSize.md#Humanizer.ByteSize.ParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider).unitSystem 'Humanizer\.ByteSize\.ParseWithUnitSystem\(string, Humanizer\.ByteSizeUnitSystem, System\.IFormatProvider\)\.unitSystem') is not defined\.

[System\.FormatException](https://learn.microsoft.com/en-us/dotnet/api/system.formatexception 'System\.FormatException')  
[s](Humanizer.ByteSize.md#Humanizer.ByteSize.ParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider).s 'Humanizer\.ByteSize\.ParseWithUnitSystem\(string, Humanizer\.ByteSizeUnitSystem, System\.IFormatProvider\)\.s') is not valid for the selected unit system\.

##### Remarks
For [DecimalSi](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.DecimalSi 'Humanizer\.ByteSizeUnitSystem\.DecimalSi') and [BinaryIec](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.BinaryIec 'Humanizer\.ByteSizeUnitSystem\.BinaryIec'), SI/IEC\-prefixed
unit tokens are matched case\-insensitively, while `b` and `B` remain case\-sensitive\.
Legacy parsing preserves the established behavior\.

<a name='Humanizer.ByteSize.Subtract(Humanizer.ByteSize)'></a>

#### ByteSize\.Subtract\(ByteSize\) Method

```csharp
public readonly Humanizer.ByteSize Subtract(Humanizer.ByteSize bs);
```
##### Parameters

<a name='Humanizer.ByteSize.Subtract(Humanizer.ByteSize).bs'></a>

`bs` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider)'></a>

#### ByteSize\.ToFullWords\(string, IFormatProvider\) Method

Converts the value of the current ByteSize object to a string with
full words\. The metric prefix symbol \(bit, byte, kilo, mega, giga,
tera, peta, exa\) used is the largest metric prefix such that the corresponding
value is greater than or equal to one\.

```csharp
public readonly string ToFullWords(string? format=null, System.IFormatProvider? provider=null);
```
##### Parameters

<a name='Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToFullWords(string,System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString()'></a>

#### ByteSize\.ToString\(\) Method

Converts the value of the current ByteSize object to a string\.
The metric prefix symbol \(bit, byte, kilo, mega, giga, tera, peta, exa\) used is
the largest metric prefix such that the corresponding value is greater
 than or equal to one\.

```csharp
public override readonly string ToString();
```

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString(string)'></a>

#### ByteSize\.ToString\(string\) Method

```csharp
public readonly string ToString(string? format);
```
##### Parameters

<a name='Humanizer.ByteSize.ToString(string).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString(string,System.IFormatProvider)'></a>

#### ByteSize\.ToString\(string, IFormatProvider\) Method

```csharp
public readonly string ToString(string? format, System.IFormatProvider? provider);
```
##### Parameters

<a name='Humanizer.ByteSize.ToString(string,System.IFormatProvider).format'></a>

`format` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString(string,System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

Implements [ToString\(string, IFormatProvider\)](https://learn.microsoft.com/en-us/dotnet/api/system.iformattable.tostring#system-iformattable-tostring(system-string-system-iformatprovider) 'System\.IFormattable\.ToString\(System\.String,System\.IFormatProvider\)')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.ToString(System.IFormatProvider)'></a>

#### ByteSize\.ToString\(IFormatProvider\) Method

```csharp
public readonly string ToString(System.IFormatProvider? provider);
```
##### Parameters

<a name='Humanizer.ByteSize.ToString(System.IFormatProvider).provider'></a>

`provider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TryParse(string,Humanizer.ByteSize)'></a>

#### ByteSize\.TryParse\(string, ByteSize\) Method

```csharp
public static bool TryParse(string? s, out Humanizer.ByteSize result);
```
##### Parameters

<a name='Humanizer.ByteSize.TryParse(string,Humanizer.ByteSize).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TryParse(string,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.ByteSize)'></a>

#### ByteSize\.TryParse\(string, IFormatProvider, ByteSize\) Method

```csharp
public static bool TryParse(string? s, System.IFormatProvider? formatProvider, out Humanizer.ByteSize result);
```
##### Parameters

<a name='Humanizer.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.ByteSize).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.ByteSize).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

<a name='Humanizer.ByteSize.TryParse(string,System.IFormatProvider,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,Humanizer.ByteSize)'></a>

#### ByteSize\.TryParse\(ReadOnlySpan\<char\>, ByteSize\) Method

```csharp
public static bool TryParse(System.ReadOnlySpan<char> s, out Humanizer.ByteSize result);
```
##### Parameters

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,Humanizer.ByteSize).s'></a>

`s` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,System.IFormatProvider,Humanizer.ByteSize)'></a>

#### ByteSize\.TryParse\(ReadOnlySpan\<char\>, IFormatProvider, ByteSize\) Method

```csharp
public static bool TryParse(System.ReadOnlySpan<char> s, System.IFormatProvider? formatProvider, out Humanizer.ByteSize result);
```
##### Parameters

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,System.IFormatProvider,Humanizer.ByteSize).s'></a>

`s` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,System.IFormatProvider,Humanizer.ByteSize).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

<a name='Humanizer.ByteSize.TryParse(System.ReadOnlySpan_char_,System.IFormatProvider,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize)'></a>

#### ByteSize\.TryParseSpanWithUnitSystem\(ReadOnlySpan\<char\>, ByteSizeUnitSystem, ByteSize\) Method

Attempts to parse a byte\-size character span using only the tokens defined by the selected unit system\.

```csharp
public static bool TryParseSpanWithUnitSystem(System.ReadOnlySpan<char> s, Humanizer.ByteSizeUnitSystem unitSystem, out Humanizer.ByteSize result);
```
##### Parameters

<a name='Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize).s'></a>

`s` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

The character span to parse\.

<a name='Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize).unitSystem'></a>

`unitSystem` [ByteSizeUnitSystem](Humanizer.ByteSizeUnitSystem.md 'Humanizer\.ByteSizeUnitSystem')

The unit system whose tokens are accepted\.

<a name='Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

When this method returns, contains the parsed byte size if parsing succeeded; otherwise, the default value\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') if parsing succeeded; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[unitSystem](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize).unitSystem 'Humanizer\.ByteSize\.TryParseSpanWithUnitSystem\(System\.ReadOnlySpan\<char\>, Humanizer\.ByteSizeUnitSystem, Humanizer\.ByteSize\)\.unitSystem') is not defined\.

##### Remarks
For [DecimalSi](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.DecimalSi 'Humanizer\.ByteSizeUnitSystem\.DecimalSi') and [BinaryIec](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.BinaryIec 'Humanizer\.ByteSizeUnitSystem\.BinaryIec'), SI/IEC\-prefixed
unit tokens are matched case\-insensitively, while `b` and `B` remain case\-sensitive\.
Legacy parsing preserves the established behavior\.

<a name='Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize)'></a>

#### ByteSize\.TryParseSpanWithUnitSystem\(ReadOnlySpan\<char\>, ByteSizeUnitSystem, IFormatProvider, ByteSize\) Method

Attempts to parse a byte\-size character span using only the tokens defined by the selected unit system\.

```csharp
public static bool TryParseSpanWithUnitSystem(System.ReadOnlySpan<char> s, Humanizer.ByteSizeUnitSystem unitSystem, System.IFormatProvider? formatProvider, out Humanizer.ByteSize result);
```
##### Parameters

<a name='Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).s'></a>

`s` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

The character span to parse\.

<a name='Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).unitSystem'></a>

`unitSystem` [ByteSizeUnitSystem](Humanizer.ByteSizeUnitSystem.md 'Humanizer\.ByteSizeUnitSystem')

The unit system whose tokens are accepted\.

<a name='Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The provider used to parse the numeric value\.

<a name='Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

When this method returns, contains the parsed byte size if parsing succeeded; otherwise, the default value\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') if parsing succeeded; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[unitSystem](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParseSpanWithUnitSystem(System.ReadOnlySpan_char_,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).unitSystem 'Humanizer\.ByteSize\.TryParseSpanWithUnitSystem\(System\.ReadOnlySpan\<char\>, Humanizer\.ByteSizeUnitSystem, System\.IFormatProvider, Humanizer\.ByteSize\)\.unitSystem') is not defined\.

##### Remarks
For [DecimalSi](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.DecimalSi 'Humanizer\.ByteSizeUnitSystem\.DecimalSi') and [BinaryIec](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.BinaryIec 'Humanizer\.ByteSizeUnitSystem\.BinaryIec'), SI/IEC\-prefixed
unit tokens are matched case\-insensitively, while `b` and `B` remain case\-sensitive\.
Legacy parsing preserves the established behavior\.

<a name='Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize)'></a>

#### ByteSize\.TryParseWithUnitSystem\(string, ByteSizeUnitSystem, ByteSize\) Method

Attempts to parse a byte size using only the tokens defined by the selected unit system\.

```csharp
public static bool TryParseWithUnitSystem(string? s, Humanizer.ByteSizeUnitSystem unitSystem, out Humanizer.ByteSize result);
```
##### Parameters

<a name='Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to parse\.

<a name='Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize).unitSystem'></a>

`unitSystem` [ByteSizeUnitSystem](Humanizer.ByteSizeUnitSystem.md 'Humanizer\.ByteSizeUnitSystem')

The unit system whose tokens are accepted\.

<a name='Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

When this method returns, contains the parsed byte size if parsing succeeded; otherwise, the default value\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') if parsing succeeded; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[unitSystem](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,Humanizer.ByteSize).unitSystem 'Humanizer\.ByteSize\.TryParseWithUnitSystem\(string, Humanizer\.ByteSizeUnitSystem, Humanizer\.ByteSize\)\.unitSystem') is not defined\.

##### Remarks
For [DecimalSi](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.DecimalSi 'Humanizer\.ByteSizeUnitSystem\.DecimalSi') and [BinaryIec](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.BinaryIec 'Humanizer\.ByteSizeUnitSystem\.BinaryIec'), SI/IEC\-prefixed
unit tokens are matched case\-insensitively, while `b` and `B` remain case\-sensitive\.
Legacy parsing preserves the established behavior\.

<a name='Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize)'></a>

#### ByteSize\.TryParseWithUnitSystem\(string, ByteSizeUnitSystem, IFormatProvider, ByteSize\) Method

Attempts to parse a byte size using only the tokens defined by the selected unit system\.

```csharp
public static bool TryParseWithUnitSystem(string? s, Humanizer.ByteSizeUnitSystem unitSystem, System.IFormatProvider? formatProvider, out Humanizer.ByteSize result);
```
##### Parameters

<a name='Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).s'></a>

`s` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The text to parse\.

<a name='Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).unitSystem'></a>

`unitSystem` [ByteSizeUnitSystem](Humanizer.ByteSizeUnitSystem.md 'Humanizer\.ByteSizeUnitSystem')

The unit system whose tokens are accepted\.

<a name='Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).formatProvider'></a>

`formatProvider` [System\.IFormatProvider](https://learn.microsoft.com/en-us/dotnet/api/system.iformatprovider 'System\.IFormatProvider')

The provider used to parse the numeric value\.

<a name='Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).result'></a>

`result` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

When this method returns, contains the parsed byte size if parsing succeeded; otherwise, the default value\.

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') if parsing succeeded; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

##### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
[unitSystem](Humanizer.ByteSize.md#Humanizer.ByteSize.TryParseWithUnitSystem(string,Humanizer.ByteSizeUnitSystem,System.IFormatProvider,Humanizer.ByteSize).unitSystem 'Humanizer\.ByteSize\.TryParseWithUnitSystem\(string, Humanizer\.ByteSizeUnitSystem, System\.IFormatProvider, Humanizer\.ByteSize\)\.unitSystem') is not defined\.

##### Remarks
For [DecimalSi](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.DecimalSi 'Humanizer\.ByteSizeUnitSystem\.DecimalSi') and [BinaryIec](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.BinaryIec 'Humanizer\.ByteSizeUnitSystem\.BinaryIec'), SI/IEC\-prefixed
unit tokens are matched case\-insensitively, while `b` and `B` remain case\-sensitive\.
Legacy parsing preserves the established behavior\.
### Operators

<a name='Humanizer.ByteSize.op_Addition(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

#### ByteSize\.operator \+\(ByteSize, ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator +(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
##### Parameters

<a name='Humanizer.ByteSize.op_Addition(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Addition(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Decrement(Humanizer.ByteSize)'></a>

#### ByteSize\.operator \-\-\(ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator --(Humanizer.ByteSize b);
```
##### Parameters

<a name='Humanizer.ByteSize.op_Decrement(Humanizer.ByteSize).b'></a>

`b` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Equality(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

#### ByteSize\.operator ==\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator ==(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
##### Parameters

<a name='Humanizer.ByteSize.op_Equality(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Equality(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_GreaterThan(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

#### ByteSize\.operator \>\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator >(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
##### Parameters

<a name='Humanizer.ByteSize.op_GreaterThan(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_GreaterThan(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_GreaterThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

#### ByteSize\.operator \>=\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator >=(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
##### Parameters

<a name='Humanizer.ByteSize.op_GreaterThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_GreaterThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_Increment(Humanizer.ByteSize)'></a>

#### ByteSize\.operator \+\+\(ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator ++(Humanizer.ByteSize b);
```
##### Parameters

<a name='Humanizer.ByteSize.op_Increment(Humanizer.ByteSize).b'></a>

`b` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Inequality(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

#### ByteSize\.operator \!=\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator !=(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
##### Parameters

<a name='Humanizer.ByteSize.op_Inequality(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Inequality(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_LessThan(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

#### ByteSize\.operator \<\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator <(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
##### Parameters

<a name='Humanizer.ByteSize.op_LessThan(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_LessThan(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_LessThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

#### ByteSize\.operator \<=\(ByteSize, ByteSize\) Operator

```csharp
public static bool operator <=(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
##### Parameters

<a name='Humanizer.ByteSize.op_LessThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_LessThanOrEqual(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')

<a name='Humanizer.ByteSize.op_Subtraction(Humanizer.ByteSize,Humanizer.ByteSize)'></a>

#### ByteSize\.operator \-\(ByteSize, ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator -(Humanizer.ByteSize b1, Humanizer.ByteSize b2);
```
##### Parameters

<a name='Humanizer.ByteSize.op_Subtraction(Humanizer.ByteSize,Humanizer.ByteSize).b1'></a>

`b1` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_Subtraction(Humanizer.ByteSize,Humanizer.ByteSize).b2'></a>

`b2` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

<a name='Humanizer.ByteSize.op_UnaryNegation(Humanizer.ByteSize)'></a>

#### ByteSize\.operator \-\(ByteSize\) Operator

```csharp
public static Humanizer.ByteSize operator -(Humanizer.ByteSize b);
```
##### Parameters

<a name='Humanizer.ByteSize.op_UnaryNegation(Humanizer.ByteSize).b'></a>

`b` [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')

##### Returns
[ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize')
