## EnumHumanizeExtensions Class

Contains extension methods for humanizing Enums

```csharp
public static class EnumHumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → EnumHumanizeExtensions
### Methods

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum)'></a>

## EnumHumanizeExtensions\.Humanize\(this Enum\) Method

Converts an enum value to a human\-readable string when the concrete enum type is only known at runtime\.

```csharp
public static string Humanize(this System.Enum input);
```
#### Parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum).input'></a>

`input` [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')

The enum value to be humanized\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A human\-readable string representation of the enum value\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing)'></a>

## EnumHumanizeExtensions\.Humanize\(this Enum, LetterCasing\) Method

Converts an enum value to a human\-readable string with the specified letter casing applied to the enum member name
when the concrete enum type is only known at runtime\. Authored metadata on a defined enum value is returned unchanged\.

```csharp
public static string Humanize(this System.Enum input, Humanizer.LetterCasing casing);
```
#### Parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing).input'></a>

`input` [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')

The enum value to be humanized\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing).casing'></a>

`casing` [LetterCasing](Humanizer.LetterCasing.md 'Humanizer\.LetterCasing')

The desired letter casing to apply when humanizing the enum member name\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A human\-readable string representation of the enum value\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource)'></a>

## EnumHumanizeExtensions\.Humanize\(this Enum, LetterCasing, EnumHumanizeSource\) Method

Converts an enum value to a human\-readable string using the specified casing and source when the concrete enum type is only known at runtime\.

```csharp
public static string Humanize(this System.Enum input, Humanizer.LetterCasing casing, Humanizer.EnumHumanizeSource source);
```
#### Parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource).input'></a>

`input` [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')

The enum value to be humanized\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource).casing'></a>

`casing` [LetterCasing](Humanizer.LetterCasing.md 'Humanizer\.LetterCasing')

The desired letter casing to apply when humanizing the enum member name\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize(thisSystem.Enum,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource).source'></a>

`source` [EnumHumanizeSource](Humanizer.EnumHumanizeSource.md 'Humanizer\.EnumHumanizeSource')

The source used to humanize the enum value\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A human\-readable string representation of the enum value\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT)'></a>

## EnumHumanizeExtensions\.Humanize\<T\>\(this T\) Method

Converts an enum value to a human\-readable string by intelligently formatting the enum member name
and respecting any [System\.ComponentModel\.DescriptionAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.descriptionattribute 'System\.ComponentModel\.DescriptionAttribute') applied to the member\.

```csharp
public static string Humanize<T>(this T input)
    where T : struct, System.Enum;
```
#### Type parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT).T'></a>

`T`

The enum type\. Must be a struct and implement [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')\.
#### Parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT).input'></a>

`input` [T](Humanizer.EnumHumanizeExtensions.md#Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT).T 'Humanizer\.EnumHumanizeExtensions\.Humanize\<T\>\(this T\)\.T')

The enum value to be humanized\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A human\-readable string representation of the enum value\.
If the enum has the [System\.FlagsAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.flagsattribute 'System\.FlagsAttribute') and multiple flags are set, returns a humanized,
comma\-separated list of the flag values\.
If a [System\.ComponentModel\.DescriptionAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.descriptionattribute 'System\.ComponentModel\.DescriptionAttribute') is present on the enum member, its value is returned\.
Otherwise, the enum member name is humanized \(e\.g\., "AnonymousUser" becomes "Anonymous user"\)\.

### Example

```csharp
enum UserType { AnonymousUser, RegisteredUser }
UserType.AnonymousUser.Humanize() => "Anonymous user"

[Flags]
enum Permission { None = 0, Read = 1, Write = 2, Delete = 4 }
(Permission.Read | Permission.Write).Humanize() => "Read, Write"

enum Status 
{ 
    [Description("Currently active")]
    Active 
}
Status.Active.Humanize() => "Currently active"
```

### Remarks
For flags enums, only non\-zero flags are included in the output, and each flag is humanized individually\.
The humanization process converts PascalCase to space\-separated text with appropriate capitalization\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing)'></a>

## EnumHumanizeExtensions\.Humanize\<T\>\(this T, LetterCasing\) Method

Converts an enum value to a human\-readable string with the specified letter casing applied to the enum member name\.
Authored metadata on a defined enum value is returned unchanged\.

```csharp
public static string Humanize<T>(this T input, Humanizer.LetterCasing casing)
    where T : struct, System.Enum;
```
#### Type parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing).T'></a>

`T`

The enum type\. Must be a struct and implement [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')\.
#### Parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing).input'></a>

`input` [T](Humanizer.EnumHumanizeExtensions.md#Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing).T 'Humanizer\.EnumHumanizeExtensions\.Humanize\<T\>\(this T, Humanizer\.LetterCasing\)\.T')

The enum value to be humanized\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing).casing'></a>

`casing` [LetterCasing](Humanizer.LetterCasing.md 'Humanizer\.LetterCasing')

The desired letter casing to apply when humanizing the enum member name\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A human\-readable string representation of the enum value\.
If a defined enum value has authored metadata such as [System\.ComponentModel\.DescriptionAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.descriptionattribute 'System\.ComponentModel\.DescriptionAttribute'), its value is returned unchanged\.

### Example

```csharp
enum UserType { AnonymousUser, RegisteredUser }
UserType.AnonymousUser.Humanize(LetterCasing.AllCaps) => "ANONYMOUS USER"
UserType.AnonymousUser.Humanize(LetterCasing.Title) => "Anonymous User"
UserType.AnonymousUser.Humanize(LetterCasing.LowerCase) => "anonymous user"
```

### Remarks
For a defined enum value, the specified casing is applied only when the output is derived from the enum member name\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource)'></a>

## EnumHumanizeExtensions\.Humanize\<T\>\(this T, LetterCasing, EnumHumanizeSource\) Method

Converts an enum value to a human\-readable string using the specified casing and source\.

```csharp
public static string Humanize<T>(this T input, Humanizer.LetterCasing casing, Humanizer.EnumHumanizeSource source)
    where T : struct, System.Enum;
```
#### Type parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource).T'></a>

`T`

The enum type\. Must be a struct and implement [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')\.
#### Parameters

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource).input'></a>

`input` [T](Humanizer.EnumHumanizeExtensions.md#Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource).T 'Humanizer\.EnumHumanizeExtensions\.Humanize\<T\>\(this T, Humanizer\.LetterCasing, Humanizer\.EnumHumanizeSource\)\.T')

The enum value to be humanized\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource).casing'></a>

`casing` [LetterCasing](Humanizer.LetterCasing.md 'Humanizer\.LetterCasing')

The desired letter casing to apply when humanizing the enum member name\.

<a name='Humanizer.EnumHumanizeExtensions.Humanize_T_(thisT,Humanizer.LetterCasing,Humanizer.EnumHumanizeSource).source'></a>

`source` [EnumHumanizeSource](Humanizer.EnumHumanizeSource.md 'Humanizer\.EnumHumanizeSource')

The source used to humanize the enum value\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A human\-readable string representation of the enum value\.