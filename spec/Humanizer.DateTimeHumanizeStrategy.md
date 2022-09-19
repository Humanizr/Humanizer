## `DefaultDateTimeHumanizeStrategy`

The default 'distance of time' -&gt; words calculator.
```csharp
public class Humanizer.DateTimeHumanizeStrategy.DefaultDateTimeHumanizeStrategy
    : IDateTimeHumanizeStrategy

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(`DateTime` input, `DateTime` comparisonBase, `CultureInfo` culture) | Calculates the distance of time in words between two provided dates | 


## `DefaultDateTimeOffsetHumanizeStrategy`

The default 'distance of time' -&gt; words calculator.
```csharp
public class Humanizer.DateTimeHumanizeStrategy.DefaultDateTimeOffsetHumanizeStrategy
    : IDateTimeOffsetHumanizeStrategy

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(`DateTimeOffset` input, `DateTimeOffset` comparisonBase, `CultureInfo` culture) | Calculates the distance of time in words between two provided dates | 


## `IDateTimeHumanizeStrategy`

Implement this interface to create a new strategy for DateTime.Humanize and hook it in the Configurator.DateTimeHumanizeStrategy
```csharp
public interface Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(`DateTime` input, `DateTime` comparisonBase, `CultureInfo` culture) | Calculates the distance of time in words between two provided dates used for DateTime.Humanize | 


## `IDateTimeOffsetHumanizeStrategy`

Implement this interface to create a new strategy for DateTime.Humanize and hook it in the Configurator.DateTimeOffsetHumanizeStrategy
```csharp
public interface Humanizer.DateTimeHumanizeStrategy.IDateTimeOffsetHumanizeStrategy

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(`DateTimeOffset` input, `DateTimeOffset` comparisonBase, `CultureInfo` culture) | Calculates the distance of time in words between two provided dates used for DateTimeOffset.Humanize | 


## `PrecisionDateTimeHumanizeStrategy`

Precision-based calculator for distance between two times
```csharp
public class Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeHumanizeStrategy
    : IDateTimeHumanizeStrategy

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(`DateTime` input, `DateTime` comparisonBase, `CultureInfo` culture) | Returns localized &amp; humanized distance of time between two dates; given a specific precision. | 


## `PrecisionDateTimeOffsetHumanizeStrategy`

Precision-based calculator for distance between two times
```csharp
public class Humanizer.DateTimeHumanizeStrategy.PrecisionDateTimeOffsetHumanizeStrategy
    : IDateTimeOffsetHumanizeStrategy

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(`DateTimeOffset` input, `DateTimeOffset` comparisonBase, `CultureInfo` culture) | Returns localized &amp; humanized distance of time between two dates; given a specific precision. | 


