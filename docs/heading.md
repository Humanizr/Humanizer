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
var currentCulture = new CultureInfo("de-DE");
using (new CultureContext(currentCulture))
{
    180.ToHeading();    // "süd"
}
```

If you only need short tokens, call `ToHeading(HeadingStyle.Short)` to receive values such as `N`, `SW`, or `ENE`.

## Arrow representations

`ToHeadingArrow()` emits Unicode arrow characters corresponding to the heading. This is handy for dashboards or console output.

```csharp
90.ToHeadingArrow();    // "→"
225.ToHeadingArrow();   // "↙"
```

## Parsing short headings

`FromShortHeading()` resolves abbreviations back into degrees, enabling round-tripping with external systems.

```csharp
"NW".FromShortHeading();     // 315
"S".FromShortHeading();      // 180
```

## Precision and tolerance

Heading conversions operate with fixed tolerances:

- Textual descriptions have a maximum deviation of **11.25°**.
- Unicode arrows round to the nearest **22.5°**.

When higher precision is required, display the numeric value alongside the textual representation.
