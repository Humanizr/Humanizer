## `DataUnit`

Units of data
```csharp
public enum Humanizer.Localisation.DataUnit
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Bit | Bit | 
| `1` | Byte | Byte | 
| `2` | Kilobyte | Kilobyte | 
| `3` | Megabyte | Megabyte | 
| `4` | Gigabyte | Gigabyte | 
| `5` | Terabyte | Terrabyte | 


## `ResourceKeys`

```csharp
public class Humanizer.Localisation.ResourceKeys

```

## `Resources`

Provides access to the resources of Humanizer
```csharp
public static class Humanizer.Localisation.Resources

```

Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | GetResource(`String` resourceKey, `CultureInfo` culture = null) | Returns the value of the specified string resource | 


## `Tense`

Enumerates the possible time references; past or future.
```csharp
public enum Humanizer.Localisation.Tense
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Future | Indicates the future. | 
| `1` | Past | Indicates the past. | 


## `TimeUnit`

Units of time.
```csharp
public enum Humanizer.Localisation.TimeUnit
    : Enum, IComparable, IFormattable, IConvertible

```

Enum

| Value | Name | Summary | 
| --- | --- | --- | 
| `0` | Millisecond |  | 
| `1` | Second |  | 
| `2` | Minute |  | 
| `3` | Hour |  | 
| `4` | Day |  | 
| `5` | Week |  | 
| `6` | Month |  | 
| `7` | Year |  | 


