## EnumDehumanizeExtensions Class

Contains extension methods for dehumanizing Enum string values\.

```csharp
public static class EnumDehumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → EnumDehumanizeExtensions
### Methods

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch)'></a>

## EnumDehumanizeExtensions\.DehumanizeTo\(this string, Type, OnNoMatch\) Method

Converts a humanized string back to its original enum value using runtime type information\.
This is a non\-generic overload that accepts the target enum type as a [System\.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type 'System\.Type') parameter\.

```csharp
public static System.Enum DehumanizeTo(this string input, System.Type targetEnum, Humanizer.OnNoMatch onNoMatch=Humanizer.OnNoMatch.ThrowsException);
```
#### Parameters

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The humanized string to be converted back to an enum value\. Must not be null\.

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch).targetEnum'></a>

`targetEnum` [System\.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type 'System\.Type')

The [System\.Type](https://learn.microsoft.com/en-us/dotnet/api/system.type 'System\.Type') of the target enum\. Must be an enum type\.

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch).onNoMatch'></a>

`onNoMatch` [OnNoMatch](Humanizer.OnNoMatch.md 'Humanizer\.OnNoMatch')

Specifies what to do when no matching enum member is found\.
Default is [ThrowsException](Humanizer.OnNoMatch.md#Humanizer.OnNoMatch.ThrowsException 'Humanizer\.OnNoMatch\.ThrowsException')\.

#### Returns
[System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')  
The enum value \(as [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')\) that matches the input string\.

#### Exceptions

[NoMatchFoundException](Humanizer.NoMatchFoundException.md 'Humanizer\.NoMatchFoundException')  
Thrown when no enum member matches the input string and [onNoMatch](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch).onNoMatch 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\(this string, System\.Type, Humanizer\.OnNoMatch\)\.onNoMatch') is set to 
[ThrowsException](Humanizer.OnNoMatch.md#Humanizer.OnNoMatch.ThrowsException 'Humanizer\.OnNoMatch\.ThrowsException')\.

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [targetEnum](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo(thisstring,System.Type,Humanizer.OnNoMatch).targetEnum 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\(this string, System\.Type, Humanizer\.OnNoMatch\)\.targetEnum') is not an enum type\.

### Example

```csharp
enum UserType { AnonymousUser, RegisteredUser }
"Anonymous user".DehumanizeTo(typeof(UserType)) => UserType.AnonymousUser (as Enum)
```

### Remarks
This method uses reflection and is less type\-safe than the generic overload\. Use the generic 
[DehumanizeTo&lt;TTargetEnum&gt;\(this string, OnNoMatch\)](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring,Humanizer.OnNoMatch) 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string, Humanizer\.OnNoMatch\)') method when the target enum type is known at compile time\.

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring)'></a>

## EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string\) Method

Converts a humanized string back to its original enum value by matching it against enum member names,
their humanized representations, and configured metadata aliases\.

```csharp
public static TTargetEnum DehumanizeTo<TTargetEnum>(this string input)
    where TTargetEnum : struct, System.Enum;
```
#### Type parameters

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring).TTargetEnum'></a>

`TTargetEnum`

The enum type to convert to\. Must be a struct and implement [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')\.
#### Parameters

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The humanized string to be converted back to an enum value\. Must not be null\.

#### Returns
[TTargetEnum](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring).TTargetEnum 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string\)\.TTargetEnum')  
The enum value that matches the input string\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [TTargetEnum](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring).TTargetEnum 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string\)\.TTargetEnum') is not an enum type\.

[NoMatchFoundException](Humanizer.NoMatchFoundException.md 'Humanizer\.NoMatchFoundException')  
Thrown when no enum member matches the input string\.

### Example

```csharp
enum UserType { AnonymousUser, RegisteredUser }
"Anonymous user".DehumanizeTo<UserType>() => UserType.AnonymousUser
"Registered user".DehumanizeTo<UserType>() => UserType.RegisteredUser
"AnonymousUser".DehumanizeTo<UserType>() => UserType.AnonymousUser
```

### Remarks
The method attempts to match the input string against:
1. The exact enum member name.
2. The humanized version of the enum member name.
3. [System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Name](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.name 'System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Name'),
              [System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Description](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.description 'System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Description'), and
              [System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.ShortName](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.shortname 'System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.ShortName') values.
4. The configured description attribute value when selected as the member's authored description.

Matching is case\-insensitive and does not trim whitespace\. If aliases collide, later values in the enum's
unsigned numeric order take precedence, while the current humanized representation takes precedence over
supplemental aliases\.

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring,Humanizer.OnNoMatch)'></a>

## EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string, OnNoMatch\) Method

Converts a humanized string back to its original enum value with configurable behavior when no match is found\.

```csharp
public static System.Nullable<TTargetEnum> DehumanizeTo<TTargetEnum>(this string input, Humanizer.OnNoMatch onNoMatch=Humanizer.OnNoMatch.ThrowsException)
    where TTargetEnum : struct, System.Enum;
```
#### Type parameters

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring,Humanizer.OnNoMatch).TTargetEnum'></a>

`TTargetEnum`

The enum type to convert to\. Must be a struct and implement [System\.Enum](https://learn.microsoft.com/en-us/dotnet/api/system.enum 'System\.Enum')\.
#### Parameters

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring,Humanizer.OnNoMatch).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The humanized string to be converted back to an enum value\. Must not be null\.

<a name='Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring,Humanizer.OnNoMatch).onNoMatch'></a>

`onNoMatch` [OnNoMatch](Humanizer.OnNoMatch.md 'Humanizer\.OnNoMatch')

Specifies what to do when no matching enum member is found\.
Default is [ThrowsException](Humanizer.OnNoMatch.md#Humanizer.OnNoMatch.ThrowsException 'Humanizer\.OnNoMatch\.ThrowsException')\.

#### Returns
[System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[TTargetEnum](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring,Humanizer.OnNoMatch).TTargetEnum 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string, Humanizer\.OnNoMatch\)\.TTargetEnum')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')  
The enum value that matches the input string, or null if no match is found and 
[onNoMatch](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring,Humanizer.OnNoMatch).onNoMatch 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string, Humanizer\.OnNoMatch\)\.onNoMatch') is set to [ReturnsNull](Humanizer.OnNoMatch.md#Humanizer.OnNoMatch.ReturnsNull 'Humanizer\.OnNoMatch\.ReturnsNull')\.

#### Exceptions

[System\.ArgumentException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentexception 'System\.ArgumentException')  
Thrown when [TTargetEnum](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring,Humanizer.OnNoMatch).TTargetEnum 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string, Humanizer\.OnNoMatch\)\.TTargetEnum') is not an enum type\.

[NoMatchFoundException](Humanizer.NoMatchFoundException.md 'Humanizer\.NoMatchFoundException')  
Thrown when no enum member matches the input string and [onNoMatch](Humanizer.EnumDehumanizeExtensions.md#Humanizer.EnumDehumanizeExtensions.DehumanizeTo_TTargetEnum_(thisstring,Humanizer.OnNoMatch).onNoMatch 'Humanizer\.EnumDehumanizeExtensions\.DehumanizeTo\<TTargetEnum\>\(this string, Humanizer\.OnNoMatch\)\.onNoMatch') is set to 
[ThrowsException](Humanizer.OnNoMatch.md#Humanizer.OnNoMatch.ThrowsException 'Humanizer\.OnNoMatch\.ThrowsException')\.

### Example

```csharp
enum UserType { AnonymousUser, RegisteredUser }
"Anonymous user".DehumanizeTo<UserType>() => UserType.AnonymousUser
"Invalid".DehumanizeTo<UserType>(OnNoMatch.ReturnsNull) => null
"Invalid".DehumanizeTo<UserType>(OnNoMatch.ThrowsException) => throws NoMatchFoundException
```

### Remarks
This overload provides more control over error handling compared to the parameterless version\.