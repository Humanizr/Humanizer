# Installation

## NuGet Packages

Humanizer is available as a single NuGet package with all supported locale data included:

### All Languages (Recommended)

```bash
dotnet add package Humanizer
```

This package includes the full Humanizer runtime, all supported locale data, and the shipped analyzers. Locale data is generated from checked-in YAML and packaged with the main library, so you do not install separate locale packages.

## Supported Frameworks

Humanizer supports the following .NET target frameworks:

- **.NET 10.0** (net10.0)
- **.NET 8.0** (net8.0)
- **.NET Framework 4.8** (net48)
- **.NET Standard 2.0** (netstandard2.0) - Special case for Roslyn Analyzers and MSBuild tasks

> **Note:** While .NET Framework versions 4.6.1 through 4.7.2 can technically consume netstandard2.0 libraries, they are **not officially supported** by Humanizer and may not work correctly. Use one of the explicitly supported frameworks above.

## Requirements for Humanizer 3.0

Humanizer now ships as a single package, so there is no metapackage or locale satellite restore path to account for during installation. The locale inheritance chain is resolved from the generated data at runtime, so supported locale variants travel with the main package.

## Source Link Support

Humanizer symbols are source-indexed with [SourceLink](https://github.com/dotnet/sourcelink) and included in the package. This means you can step through Humanizer's source code while debugging your own application.

## Verification

After installation, verify Humanizer is working:

```csharp
using Humanizer;

"PascalCaseString".Humanize(); // Returns "Pascal case string"
DateTime.UtcNow.AddHours(-2).Humanize(); // Returns "2 hours ago"
```

## Next Steps

- [Quick Start Guide](quick-start.md)
- [Migration from 2.14.1 to 3.0.8](migration-v3.md)
- [String Humanization](string-humanization.md)
- [DateTime Humanization](datetime-humanization.md)
