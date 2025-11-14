# Heading Helpers

Humanizer can translate numeric headings into readable text, compass arrows, or abbreviated tokens. These helpers are useful for mapping orientation data in geospatial or navigation scenarios.

## Converting to text

`ToHeading()` maps degrees to standard compass directions. The output respects the current UI culture.

```csharp
using Humanizer;

0.ToHeading();          // "north"
45.ToHeading();         // "north-east"
225.ToHeading();        // "south-west"

// Culture-specific wording
var previousCulture = CultureInfo.CurrentCulture;
var previousUICulture = CultureInfo.CurrentUICulture;
try
{
    var culture = CultureInfo.GetCultureInfo("de-DE");
    CultureInfo.CurrentCulture = culture;
    CultureInfo.CurrentUICulture = culture;

    180.ToHeading();    // "süd"
}
finally
{
    CultureInfo.CurrentCulture = previousCulture;
    CultureInfo.CurrentUICulture = previousUICulture;
}
```

If you only need short tokens, call `ToHeading(HeadingStyle.Abbreviated)` to receive values such as `N`, `SW`, or `ENE`.

## Arrow representations

`ToHeadingArrow()` emits Unicode arrow characters corresponding to the heading. This is handy for dashboards or console output.

```csharp
90.ToHeadingArrow();    // "→"
225.ToHeadingArrow();   // "↙"
```

## Parsing short headings

`FromAbbreviatedHeading()` resolves abbreviations back into degrees, enabling round-tripping with external systems.

```csharp
"NW".FromAbbreviatedHeading();     // 315
"S".FromAbbreviatedHeading();      // 180
```

## Precision and tolerance

Heading conversions operate with fixed tolerances:

- Textual descriptions have a maximum deviation of **11.25°**.
- Unicode arrows round to the nearest **22.5°**.

When higher precision is required, display the numeric value alongside the textual representation.
