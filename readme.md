# Humanizer

[![Build Status](https://dev.azure.com/dotnet/Humanizer/_apis/build/status/Humanizer-CI?branchName=main)](https://dev.azure.com/dotnet/Humanizer/_build?definitionId=14)
[![NuGet version](https://img.shields.io/nuget/v/Humanizer.svg?logo=nuget&cacheSeconds=300)](https://www.nuget.org/packages/Humanizer)
[![NuGet downloads](https://img.shields.io/nuget/dt/Humanizer.svg?logo=nuget&cacheSeconds=300)](https://www.nuget.org/packages/Humanizer)

Humanizer is a .NET library for turning strings, enums, dates, times, durations, numbers, quantities, and collections into human-friendly text.

## Install

```console
dotnet add package Humanizer
```

Select your Humanizer version in the [documentation site](https://humanizr.net/docs/) for version-correct package, framework, and API guidance.

## Example

```csharp
using System.Globalization;
using Humanizer;

var culture = CultureInfo.GetCultureInfo("en-US");
var text = TimeSpan.FromMinutes(2).Humanize(culture: culture);
var indianNumber = 1_000_000_000L.ToIndianWords(IndianScaleStyle.CroreBased);
var precise = TimeSpan.FromMilliseconds(1500).HumanizeWithFractionalSeconds(
    precision: 1,
    maxFractionalDigits: 3,
    roundingMode: MidpointRounding.ToEven,
    culture: culture,
    maxUnit: TimeUnit.Second);

Console.WriteLine(text); // 2 minutes
Console.WriteLine(indianNumber); // one hundred crore
Console.WriteLine(precise); // 1.5 seconds

var german = CultureInfo.GetCultureInfo("de-DE");
var duration = TimeSpan.FromDays(7).HumanizeWithCase(
    GrammaticalCase.Dative,
    culture: german);

Console.WriteLine($"in {duration}"); // in einer Woche
```

In v4 previews, applications can provide the exact noun forms for a culture's
CLDR cardinal categories:

```csharp
var files = new PluralizationForms(
    singular: "plik",
    other: "pliku",
    few: "pliki",
    many: "plików");

if (files.TryPluralize(5m, CultureInfo.GetCultureInfo("pl"), out var noun))
    Console.WriteLine(noun); // plików
```

`TryPluralize` uses the required cardinal rule for every supported culture and
returns `false` when the selected form was not supplied. `TrySingularize`
resolves any exact form in the same set. See
[inflection and quantities](https://humanizr.net/docs/scenarios/inflection-and-quantities/)
for examples.

`HumanizeWithCase` returns only the duration phrase; add any required
preposition yourself. Singular forms may include a locale-authored one-word
or article, and a locale may encode a count in the unit form. Locales and custom
components without verified case support throw `NotSupportedException`
instead of falling back to English.

V4 previews also provide opt-in decimal SI and binary IEC byte-size APIs:

```csharp
var size = ByteSize.FromBytes(1_000_000);

Console.WriteLine(size.Format(ByteSizeUnitSystem.DecimalSi)); // 1 MB
Console.WriteLine(size.Format(ByteSizeUnitSystem.BinaryIec)); // 976.56 KiB
```

Legacy byte-size APIs keep their established unit factors. See
[byte sizes and rates](https://humanizr.net/docs/scenarios/byte-sizes-and-rates/)
for explicit formatting, parsing, composites, and rates.

## Documentation

- [Start using Humanizer](https://humanizr.net/docs/start/overview/)
- [Common scenarios](https://humanizr.net/docs/scenarios/strings-and-casing/)
- [Upgrade between versions](https://humanizr.net/docs/upgrading/)
- [Configure the migration analyzer](https://humanizr.net/docs/analyzer/)
- [API reference](https://humanizr.net/docs/api/)

## Repository

The main library is in [`src/Humanizer`](src/Humanizer), and its xUnit tests are in [`tests/Humanizer.Tests`](tests/Humanizer.Tests). Read the [contribution guide](.github/CONTRIBUTING.md) before sending a change.

Humanizer is available under the [MIT license](license.txt).

## Sponsorship

Sponsorship helps fund Humanizer's ongoing maintenance, multilingual development, testing, documentation, release engineering, and the AI and infrastructure costs behind expanding and validating language support.

[Support Claire Novotny's open-source work through GitHub Sponsors](https://github.com/sponsors/clairernovotny).

## Support

- [Report a bug or request a feature](https://github.com/Humanizr/Humanizer/issues)
- [Report a documentation issue](https://github.com/Humanizr/Humanizer/issues/new?labels=documentation)
- [Read the versioned documentation](https://humanizr.net/docs/)
