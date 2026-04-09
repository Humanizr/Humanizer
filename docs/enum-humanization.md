# Enum Humanization

Enum humanization converts enum member names into human-readable strings. It automatically adds spaces between words in PascalCase member names and respects `DescriptionAttribute`, `DisplayAttribute`, and any custom attribute with a `Description` property.

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

UserType.MemberWithDescriptionAttribute.Humanize()
    // => "Custom description"

UserType.MemberWithoutDescriptionAttribute.Humanize()
    // => "Member without description attribute"

UserType.ALLCAPITALS.Humanize()
    // => "ALLCAPITALS"
```

## Description Attribute

When a `DescriptionAttribute` (or any attribute with a string `Description` property) is present on an enum member, its value is returned instead of the humanized member name.

```csharp
public enum Status
{
    [Description("Currently active")]
    Active,
    [Description("No longer in use")]
    Inactive
}

Status.Active.Humanize()   // => "Currently active"
Status.Inactive.Humanize() // => "No longer in use"
```

Humanizer also supports `DisplayAttribute` from `System.ComponentModel.DataAnnotations`, including localized descriptions via `ResourceType`:

```csharp
public enum Priority
{
    [Display(Description = "Highest priority")]
    Critical,
    [Display(Description = "LocalizedKey", ResourceType = typeof(MyResources))]
    Normal
}

Priority.Critical.Humanize() // => "Highest priority"
Priority.Normal.Humanize()   // => value from MyResources.LocalizedKey
```

## Letter Casing

Pass a `LetterCasing` value to control the output casing:

```csharp
UserType.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.Title)
    // => "Member Without Description Attribute"

UserType.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.LowerCase)
    // => "member without description attribute"

UserType.MemberWithoutDescriptionAttribute.Humanize(LetterCasing.AllCaps)
    // => "MEMBER WITHOUT DESCRIPTION ATTRIBUTE"
```

Available casing options:

- `LetterCasing.Title` - Capitalizes the first letter of each word
- `LetterCasing.Sentence` - Capitalizes only the first letter
- `LetterCasing.AllCaps` - All uppercase
- `LetterCasing.LowerCase` - All lowercase

## Flags Enums

Enums decorated with `[Flags]` are handled automatically. When multiple flags are set, each flag is humanized individually and the results are joined with "and".

```csharp
[Flags]
public enum BitFieldEnum
{
    [Display(Description = "None")]
    NONE = 0,
    [Display(Description = "Red")]
    RED = 1,
    [Display(Description = "Dark Gray")]
    DARK_GRAY = 2
}

BitFieldEnum.RED.Humanize()                          // => "Red"
BitFieldEnum.DARK_GRAY.Humanize()                    // => "Dark Gray"
(BitFieldEnum.RED | BitFieldEnum.DARK_GRAY).Humanize() // => "Red and Dark Gray"
BitFieldEnum.NONE.Humanize()                         // => "None"
```

The zero-valued member is only included when it is the sole value. Non-zero flags are humanized and combined when multiple flags are set.

## Related Topics

- [Enum Dehumanization](enum-dehumanization.md) - Convert humanized strings back to enum values
- [String Humanization](string-humanization.md) - Humanize programming identifiers
- [String Transformations](string-transformations.md) - Custom casing and transformations
