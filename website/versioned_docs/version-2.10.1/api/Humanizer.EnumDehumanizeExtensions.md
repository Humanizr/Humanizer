## EnumDehumanizeExtensions Class

Contains extension methods for dehumanizing Enum string values\.

```csharp
public static class EnumDehumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → EnumDehumanizeExtensions
### Methods

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch)'></a>

## EnumDehumanizeExtensions\.DehumanizeTo\(this string, Type, OnNoMatch\) Method

Dehumanizes a string into the Enum it was originally Humanized from\!

```csharp
public static System.Enum DehumanizeTo(this string input, System.Type targetEnum, Humanizer.OnNoMatch onNoMatch=Humanizer.OnNoMatch.ThrowsException);
```
#### Parameters

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be converted

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch).targetEnum'></a>

`targetEnum` [System\.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type 'System\.Type')

The target enum

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch).onNoMatch'></a>

`onNoMatch` [OnNoMatch](Humanizer.OnNoMatch.md 'Humanizer\.OnNoMatch')

What to do when input is not matched to the enum\.

#### Returns
[System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')

#### Exceptions

[NoMatchFoundException](Humanizer.NoMatchFoundException.md 'Humanizer\.NoMatchFoundException')  
Couldn't find any enum member that matches the string

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
If targetEnum is not an enum

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring)'></a>

## EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string\) Method

Dehumanizes a string into the Enum it was originally Humanized from\!

```csharp
public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input)
    where TTargetEnum : struct, System.IComparable, System.IFormattable;
```
#### Type parameters

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring).TTargetEnum'></a>

`TTargetEnum`

The target enum
#### Parameters

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to be converted

#### Returns
[TTargetEnum](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring).TTargetEnum 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string\)\.TTargetEnum')

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
If TTargetEnum is not an enum

[NoMatchFoundException](Humanizer.NoMatchFoundException.md 'Humanizer\.NoMatchFoundException')  
Couldn't find any enum member that matches the string