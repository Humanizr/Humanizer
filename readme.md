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

Console.WriteLine(text); // 2 minutes
```

## Documentation

- [Start using Humanizer](https://humanizr.net/docs/start/overview/)
- [Common scenarios](https://humanizr.net/docs/scenarios/strings-and-casing/)
- [API reference](https://humanizr.net/docs/api/)

## Repository

The main library is in [`src/Humanizer`](src/Humanizer), and its xUnit tests are in [`tests/Humanizer.Tests`](tests/Humanizer.Tests). Read the [contribution guide](.github/CONTRIBUTING.md) before sending a change.

Humanizer is available under the [MIT license](license.txt).

## Support

- [Report a bug or request a feature](https://github.com/Humanizr/Humanizer/issues)
- [Report a documentation issue](https://github.com/Humanizr/Humanizer/issues/new?labels=documentation)
- [Read the versioned documentation](https://humanizr.net/docs/)
