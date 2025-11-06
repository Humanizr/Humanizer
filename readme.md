# <img width="30px" src="logo.png" /> Humanizer

[![Build Status](https://dev.azure.com/dotnet/Humanizer/_apis/build/status/Humanizer-CI?branchName=main)](https://dev.azure.com/dotnet/Humanizer/_build?definitionId=14)
[![NuGet version](https://img.shields.io/nuget/v/Humanizer.svg?logo=nuget&cacheSeconds=300)](https://www.nuget.org/packages/Humanizer)
[![NuGet downloads](https://img.shields.io/nuget/dt/Humanizer.Core.svg?logo=nuget&cacheSeconds=300)](https://www.nuget.org/packages/Humanizer.Core)

Humanizer turns numbers, dates, enums, quantities, and strings into natural language across more than 40 locales. Use it to surface human-friendly text in any .NET application with minimal ceremony.

> Docs: see the [documentation folder](docs/index.md) for conceptual guides, API reference, and operational playbooks.

## Install

Install from NuGet with the package that matches your localization needs:

```bash
# All languages
dotnet add package Humanizer

# English only
dotnet add package Humanizer.Core
```

Add satellite packages when you only need specific languages:

| Scenario | Package | Example command |
| --- | --- | --- |
| French | `Humanizer.Core.fr` | `dotnet add package Humanizer.Core.fr` |
| Spanish | `Humanizer.Core.es` | `dotnet add package Humanizer.Core.es` |
| Multiple locales | Install each locale package alongside `Humanizer.Core` | `dotnet add package Humanizer.Core.de` |

### Supported frameworks
- .NET 10.0
- .NET 9.0
- .NET 8.0
- .NET Framework 4.8
- .NET Standard 2.0 (analyzer and MSBuild scenarios)

> Restore requirements: the `Humanizer` metapackage needs the NuGet locale fix included in .NET SDK 9.0.200 and corresponding Visual Studio or MSBuild updates. Older toolsets must reference `Humanizer.Core` plus specific `Humanizer.Core.<culture>` packages.

## Quick start

```csharp
using Humanizer;
using Humanizer.Bytes;

"PascalCaseString".Humanize();           // "Pascal case string"
DateTime.UtcNow.AddHours(-2).Humanize();  // "2 hours ago"
TimeSpan.FromMinutes(90).Humanize();      // "an hour"
1234.ToWords();                           // "one thousand two hundred and thirty-four"
var backlog = new[] { "apple", "banana", "cherry" };
backlog.Humanize();                       // "apple, banana, and cherry"
512.Megabytes().ToString();               // "512 MB"
```

Explore additional walk-throughs in the [Quick Start guide](docs/quick-start.md).

## Highlights

- Strings: [humanize, dehumanize, truncate, and transform identifiers](docs/string-humanization.md).
- Dates and times: [relative time, durations, clock notation, and fluent arithmetic](docs/dates-and-times.md).
- Numbers and quantities: [words, ordinals, metrics, Roman numerals, tupleize, and ToQuantity helpers](docs/numbers.md).
- Collections: [natural-language list formatting](docs/collection-humanization.md).
- Localization: [dozens of cultures, grammatical gender and case, and custom vocabularies](docs/localization.md).
- Extensibility: [swap strategies, register formatters, and extend transformers](docs/extensibility.md).
- Operations: [integration guidance, performance tips, testing patterns, and troubleshooting](docs/application-integration.md).

## Documentation and samples

Generate and preview the DocFX site locally:

```bash
dotnet tool restore
dotnet tool run docfx build docs/docfx.json
# Local preview
dotnet tool run docfx serve docs/_site
```

The `samples/Humanizer.MvcSample` project demonstrates metadata providers for ASP.NET MVC 4.x. Modern ASP.NET Core and Blazor scenarios are covered in [Application Integration](docs/application-integration.md).

## Production readiness checklist

- SourceLink-enabled symbols let you step into Humanizer during debugging.
- Semantic versioning with Azure DevOps CI on every pull request and main branch update.
- Comprehensive xUnit test suites across .NET 10.0 and .NET 8.0 targets.
- DocFX documentation describing features, localization coverage, configuration, and operations.

## Community and support

- Search existing [issues](https://github.com/Humanizr/Humanizer/issues) or [open a new issue](https://github.com/Humanizr/Humanizer/issues/new) for bugs and feature requests.
- Follow NuGet releases for [Humanizer](https://www.nuget.org/packages/Humanizer) and [Humanizer.Core](https://www.nuget.org/packages/Humanizer.Core).
- Monitor build health via the Azure DevOps pipeline badge above.

## Contributing

Contributions are welcome. Review [.github/CONTRIBUTING.md](.github/CONTRIBUTING.md) for workflow details and read the [Code of Conduct](.github/CODE_OF_CONDUCT.md) before participating.

## Related projects

- [Humanizer.Annotations](https://resharper-plugins.jetbrains.com/packages/Humanizer.Annotations/) - JetBrains ReSharper annotations.
- [PowerShell Humanizer](https://github.com/dfinke/PowerShellHumanizer) - PowerShell module built on Humanizer.
- [Humanizer.jvm](https://github.com/MehdiK/Humanizer.jvm) - Kotlin adaptation for the JVM.
- [Humanizer.node](https://github.com/fakoua/humanizer.node) - TypeScript port.

## License

Humanizer is licensed under the [MIT License](license.txt). Copyright (c) .NET Foundation and contributors.
