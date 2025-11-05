# Enumerations

Humanizer provides helpers for round-tripping between enum values and readable strings. By default it inserts spaces into PascalCase names, honors localization attributes, and supports converting text back into strongly typed enum members.

## Humanizing enum values

Use `Humanize()` to turn an enum member into a readable phrase. If a `DescriptionAttribute`, `DisplayAttribute`, or any attribute exposing a `Description` property is present, that value is preferred.

```csharp
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Humanizer;

public enum OrderStatus
{
    PendingPayment,

    [Description("Awaiting fulfilment")]
    AwaitingFulfillment,

    [Display(Name = "Ready to ship")]
    ReadyToShip,
}

OrderStatus.PendingPayment.Humanize();          // "Pending payment"
OrderStatus.AwaitingFulfillment.Humanize();     // "Awaiting fulfilment"
OrderStatus.ReadyToShip.Humanize();             // "Ready to ship"
OrderStatus.PendingPayment.Humanize(LetterCasing.Title); // "Pending Payment"
```

Key behaviors:

- **PascalCase → words:** Names such as `ReadyToShip` become `Ready to ship` automatically.
- **Attributes override defaults:** `DescriptionAttribute`, `DisplayAttribute(Name = "...")`, or any attribute exposing a `Description` property are prioritized.
- **Culture aware:** Provide a `CultureInfo` to localize attribute lookups and resource-backed `DisplayAttribute`s.
- **Casing control:** Pass `LetterCasing.Title`, `LetterCasing.LowerCase`, etc., to shape the output.

## Dehumanizing strings back to enums

`DehumanizeTo<TEnum>()` parses readable text back into a strongly typed enum. It accepts the friendly text created by `Humanize()`, the original enum name, or attribute values.

```csharp
"Ready to ship".DehumanizeTo<OrderStatus>();         // OrderStatus.ReadyToShip
"ready_to_ship".DehumanizeTo<OrderStatus>();        // OrderStatus.ReadyToShip
"Awaiting fulfilment".DehumanizeTo<OrderStatus>();  // OrderStatus.AwaitingFulfillment
```

Control error handling with the `OnNoMatch` option:

```csharp
var value = "unknown".DehumanizeTo<OrderStatus>(OnNoMatch.ReturnsNull); // null
var fallback = "unknown".DehumanizeTo<OrderStatus>(OnNoMatch.ReturnsDefault); // default(OrderStatus)
```

Additional options include:

- **Case-insensitive matching:** Enabled by default; pass `IgnoreCase: false` to require exact casing.
- **Fallback parsing:** If attribute text fails, Humanizer tries the raw enum name, then removes punctuation and whitespace.

## Localized enum descriptions

When a `DisplayAttribute` references a resource file, `Humanize` and `DehumanizeTo` automatically use the localized strings, provided you set the current UI culture or pass a specific `CultureInfo`.

```csharp
[Display(Name = nameof(Resources.OrderStatus_Ready), ResourceType = typeof(Resources))]
ReadyToShip,

OrderStatus.ReadyToShip.Humanize(culture: new CultureInfo("fr"));
```

> [!TIP]
> For dynamic cultures (e.g., ASP.NET requests), set `Thread.CurrentUICulture` before humanizing enums or pass the culture explicitly. This keeps the text consistent across calls.

## Customizing enum behavior

If you need to influence how enums are dehumanized globally, implement `IStringTransformer` or register custom strategies via `Configurator.EnumHumanize`. Common scenarios include:

- Mapping legacy display text to new enum names during migrations.
- Supporting domain-specific separators such as `/` or `:`.
- Applying custom casing or prefixes before converting to text.

Because these adjustments operate globally, register them once during application startup.
