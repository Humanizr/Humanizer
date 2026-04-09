# Heading Humanization

Humanizer can convert numeric compass headings (in degrees) into human-readable text, Unicode arrow characters, and back again.

## Basic Usage

```csharp
using Humanizer;

360.0.ToHeading();  // => "N"
90.0.ToHeading();   // => "E"
225.0.ToHeading();  // => "SW"
720.0.ToHeading();  // => "N"
```

## Abbreviated Headings

The default `ToHeading()` call returns an abbreviated compass direction. There are 16 possible values, each covering a 22.5-degree segment:

```csharp
0.0.ToHeading();     // => "N"
22.5.ToHeading();    // => "NNE"
45.0.ToHeading();    // => "NE"
67.5.ToHeading();    // => "ENE"
90.0.ToHeading();    // => "E"
112.5.ToHeading();   // => "ESE"
135.0.ToHeading();   // => "SE"
157.5.ToHeading();   // => "SSE"
180.0.ToHeading();   // => "S"
202.5.ToHeading();   // => "SSW"
225.0.ToHeading();   // => "SW"
247.5.ToHeading();   // => "WSW"
270.0.ToHeading();   // => "W"
292.5.ToHeading();   // => "WNW"
315.0.ToHeading();   // => "NW"
337.5.ToHeading();   // => "NNW"
```

The textual representation has a maximum deviation of 11.25 degrees. Values greater than 360 wrap around automatically.

## Full Headings

Pass `HeadingStyle.Full` to get the full compass direction name:

```csharp
0.0.ToHeading(HeadingStyle.Full);     // => "north"
22.5.ToHeading(HeadingStyle.Full);    // => "north-northeast"
45.0.ToHeading(HeadingStyle.Full);    // => "northeast"
67.5.ToHeading(HeadingStyle.Full);    // => "east-northeast"
90.0.ToHeading(HeadingStyle.Full);    // => "east"
180.0.ToHeading(HeadingStyle.Full);   // => "south"
270.0.ToHeading(HeadingStyle.Full);   // => "west"
360.0.ToHeading(HeadingStyle.Full);   // => "north"
```

## Heading Arrows

`ToHeadingArrow()` returns a Unicode arrow character representing the heading. There are 8 possible arrows, each covering a 45-degree segment (maximum deviation of 22.5 degrees):

```csharp
0.0.ToHeadingArrow();     // => '↑'
45.0.ToHeadingArrow();    // => '↗'
90.0.ToHeadingArrow();    // => '→'
135.0.ToHeadingArrow();   // => '↘'
180.0.ToHeadingArrow();   // => '↓'
225.0.ToHeadingArrow();   // => '↙'
270.0.ToHeadingArrow();   // => '←'
315.0.ToHeadingArrow();   // => '↖'
```

## Converting Back to Degrees

### From Abbreviated Headings

Use `FromAbbreviatedHeading()` to convert a short compass string back to degrees:

```csharp
"N".FromAbbreviatedHeading();   // => 0
"NNE".FromAbbreviatedHeading(); // => 22.5
"E".FromAbbreviatedHeading();   // => 90
"S".FromAbbreviatedHeading();   // => 180
"SW".FromAbbreviatedHeading();  // => 225
"NNW".FromAbbreviatedHeading(); // => 337.5
```

Returns `-1` if the input cannot be parsed.

### From Heading Arrows

Use `FromHeadingArrow()` to convert an arrow character back to degrees:

```csharp
'↑'.FromHeadingArrow(); // => 0
'→'.FromHeadingArrow(); // => 90
'↓'.FromHeadingArrow(); // => 180
'←'.FromHeadingArrow(); // => 270
```

This method also accepts a string and returns `-1` for unrecognized input.

## Culture Support

Both `ToHeading()` and `FromAbbreviatedHeading()` accept an optional `CultureInfo` parameter for localized results:

```csharp
// German uses "O" (Ost) instead of "E" (East)
"O".FromAbbreviatedHeading(new CultureInfo("de-DE"));  // => 90

// Danish uses "ØNØ" instead of "ENE"
"ØNØ".FromAbbreviatedHeading(new CultureInfo("da"));   // => 67.5
```

## Related Topics

- [Localization](localization.md) - Culture-specific formatting
