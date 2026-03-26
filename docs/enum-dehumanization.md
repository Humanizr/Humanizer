# Enum Dehumanization

Enum dehumanization converts humanized strings back to their original enum values. It matches against enum member names, humanized names, and `DescriptionAttribute` values. Matching is case-insensitive.

## Basic Usage

```csharp
using Humanizer;

public enum UserType
{
    [Description("Custom description")]
    MemberWithDescriptionAttribute,
    MemberWithoutDescriptionAttribute,
    ALLCAPITALS
}

"Member without description attribute".DehumanizeTo<UserType>()
    // => UserType.MemberWithoutDescriptionAttribute

"Custom description".DehumanizeTo<UserType>()
    // => UserType.MemberWithDescriptionAttribute

"ALLCAPITALS".DehumanizeTo<UserType>()
    // => UserType.ALLCAPITALS
```

## Case-Insensitive Matching

The `DehumanizeTo` method performs case-insensitive matching, so all of the following return the same result:

```csharp
"Member without description attribute".DehumanizeTo<UserType>()
    // => UserType.MemberWithoutDescriptionAttribute

"Member Without Description Attribute".DehumanizeTo<UserType>()
    // => UserType.MemberWithoutDescriptionAttribute

"member without description attribute".DehumanizeTo<UserType>()
    // => UserType.MemberWithoutDescriptionAttribute
```

## OnNoMatch Behavior

By default, `DehumanizeTo` throws a `NoMatchFoundException` when no matching enum member is found. You can change this behavior with the `OnNoMatch` parameter:

```csharp
// Default: throws NoMatchFoundException
"Invalid".DehumanizeTo<UserType>()
    // => throws NoMatchFoundException

// Returns null instead of throwing
"Invalid".DehumanizeTo<UserType>(OnNoMatch.ReturnsNull)
    // => null

// Explicitly throw (same as default)
"Invalid".DehumanizeTo<UserType>(OnNoMatch.ThrowsException)
    // => throws NoMatchFoundException
```

## Non-Generic Version

When the target enum type is only known at runtime, use the non-generic overload that accepts a `Type` parameter:

```csharp
"Member without description attribute".DehumanizeTo(typeof(UserType))
    // => UserType.MemberWithoutDescriptionAttribute

"Custom description".DehumanizeTo(typeof(UserType))
    // => UserType.MemberWithDescriptionAttribute
```

The non-generic version also supports the `OnNoMatch` parameter:

```csharp
"Invalid".DehumanizeTo(typeof(UserType), OnNoMatch.ReturnsNull)
    // => null
```

Note that the non-generic version uses reflection internally and returns `Enum` rather than the specific enum type. Prefer the generic `DehumanizeTo<T>` when the type is known at compile time.

## Related Topics

- [Enum Humanization](enum-humanization.md) - Convert enum values to human-readable strings
- [String Dehumanization](string-dehumanization.md) - Convert humanized strings back to PascalCase
- [String Humanization](string-humanization.md) - Humanize programming identifiers
