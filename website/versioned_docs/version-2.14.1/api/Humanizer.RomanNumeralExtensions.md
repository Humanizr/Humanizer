## RomanNumeralExtensions Class

Contains extension methods for changing a number to Roman representation \(ToRoman\) and from Roman representation back to the number \(FromRoman\)

```csharp
public static class RomanNumeralExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → RomanNumeralExtensions
### Methods

<a name='Humanizer.RomanNumeralExtensions.FromRoman(thisstring)'></a>

## RomanNumeralExtensions\.FromRoman\(this string\) Method

Converts Roman numbers into integer

```csharp
public static int FromRoman(this string input);
```
#### Parameters

<a name='Humanizer.RomanNumeralExtensions.FromRoman(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Roman number

#### Returns
[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')  
Human\-readable number

<a name='Humanizer.RomanNumeralExtensions.ToRoman(thisint)'></a>

## RomanNumeralExtensions\.ToRoman\(this int\) Method

Converts the input to Roman number

```csharp
public static string ToRoman(this int input);
```
#### Parameters

<a name='Humanizer.RomanNumeralExtensions.ToRoman(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Integer input

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Roman number

#### Exceptions

[System\.ArgumentOutOfRangeException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentoutofrangeexception 'System\.ArgumentOutOfRangeException')  
Thrown when the input is smaller than 1 or larger than 3999