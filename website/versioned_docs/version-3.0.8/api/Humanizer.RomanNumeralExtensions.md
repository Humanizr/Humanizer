## RomanNumeralExtensions Class

Contains extension methods for changing a number to Roman representation \(ToRoman\) and from Roman representation back to the number \(FromRoman\)

```csharp
public static class RomanNumeralExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → RomanNumeralExtensions
### Methods

<a name='Humanizer.RomanNumeralExtensions.FromRoman(System.ReadOnlySpan_char_)'></a>

## RomanNumeralExtensions\.FromRoman\(ReadOnlySpan\<char\>\) Method

Converts a Roman numeral character span to its integer representation\.

```csharp
public static int FromRoman(System.ReadOnlySpan<char> input);
```
#### Parameters

<a name='Humanizer.RomanNumeralExtensions.FromRoman(System.ReadOnlySpan_char_).input'></a>

`input` [System\.ReadOnlySpan&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')[System\.Char](https://learn.microsoft.com/en-us/dotnet/api/system.char 'System\.Char')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.readonlyspan-1 'System\.ReadOnlySpan\`1')

The Roman numeral character span to convert\. Must not be empty\.

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
The integer value represented by the Roman numeral\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [input](Humanizer.RomanNumeralExtensions.md#Humanizer.RomanNumeralExtensions.FromRoman(System.ReadOnlySpan_char_).input 'Humanizer\.RomanNumeralExtensions\.FromRoman\(System\.ReadOnlySpan\<char\>\)\.input') is empty \(after trimming\) or contains an invalid Roman numeral format\.

### Example

```csharp
"XIV".AsSpan().FromRoman() => 14
"MCMXC".AsSpan().FromRoman() => 1990
```

### Remarks
This is a memory\-efficient overload that works with character spans to avoid string allocations\.
Valid Roman numerals use the characters M, D, C, L, X, V, and I \(case\-insensitive\)\.
Supports subtractive notation \(e\.g\., IV = 4, IX = 9\)\.

<a name='Humanizer.RomanNumeralExtensions.FromRoman(thisstring)'></a>

## RomanNumeralExtensions\.FromRoman\(this string\) Method

Converts a Roman numeral string to its integer representation\.

```csharp
public static int FromRoman(this string input);
```
#### Parameters

<a name='Humanizer.RomanNumeralExtensions.FromRoman(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The Roman numeral string to convert \(e\.g\., "XIV", "MCMXC"\)\. Must not be null\.

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
The integer value represented by the Roman numeral\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [input](Humanizer.RomanNumeralExtensions.md#Humanizer.RomanNumeralExtensions.FromRoman(thisstring).input 'Humanizer\.RomanNumeralExtensions\.FromRoman\(this string\)\.input') is null\.

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [input](Humanizer.RomanNumeralExtensions.md#Humanizer.RomanNumeralExtensions.FromRoman(thisstring).input 'Humanizer\.RomanNumeralExtensions\.FromRoman\(this string\)\.input') is empty or contains an invalid Roman numeral format\.

### Example

```csharp
"XIV".FromRoman() => 14
"MCMXC".FromRoman() => 1990
"IV".FromRoman() => 4
"MMXXIII".FromRoman() => 2023
```

### Remarks
Valid Roman numerals use the characters M, D, C, L, X, V, and I \(case\-insensitive\)\.
Supports subtractive notation \(e\.g\., IV = 4, IX = 9, XL = 40, XC = 90, CD = 400, CM = 900\)\.
Valid range is 1 to 3999\.

<a name='Humanizer.RomanNumeralExtensions.ToRoman(thisint)'></a>

## RomanNumeralExtensions\.ToRoman\(this int\) Method

Converts an integer to its Roman numeral representation\.

```csharp
public static string ToRoman(this int input);
```
#### Parameters

<a name='Humanizer.RomanNumeralExtensions.ToRoman(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

The integer value to convert\. Must be between 1 and 3999 inclusive\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string containing the Roman numeral representation of the input value\.

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when [input](Humanizer.RomanNumeralExtensions.md#Humanizer.RomanNumeralExtensions.ToRoman(thisint).input 'Humanizer\.RomanNumeralExtensions\.ToRoman\(this int\)\.input') is less than 1 or greater than 3999\.
Roman numerals are traditionally limited to this range\.

### Example

```csharp
14.ToRoman() => "XIV"
1990.ToRoman() => "MCMXC"
4.ToRoman() => "IV"
2023.ToRoman() => "MMXXIII"
3999.ToRoman() => "MMMCMXCIX"
```

### Remarks
Uses standard Roman numeral notation including subtractive notation for 4, 9, 40, 90, 400, and 900\.
The implementation is optimized for performance and avoids string allocations where possible\.