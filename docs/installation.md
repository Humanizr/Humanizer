# Installation

## NuGet Packages

Humanizer is available as NuGet packages with different language support options:

### All Languages (Recommended)

```bash
dotnet add package Humanizer
```

This package includes all supported languages and pulls in `Humanizer.Core` plus all language satellite packages.

### English Only

```bash
dotnet add package Humanizer.Core
```

This package includes only English language support, resulting in a smaller download size.

### Specific Languages

You can install `Humanizer.Core` along with specific language packages:

```bash
dotnet add package Humanizer.Core
dotnet add package Humanizer.Core.fr  # French
dotnet add package Humanizer.Core.es  # Spanish
dotnet add package Humanizer.Core.de  # German
# Add as many language packages as needed
```

Available language packages include: ar, az, bg, bn-BD, cs, da, de, el, es, fa, fi, fr, he, hr, hu, hy, id, is, it, ja, ko, ku, lv, ms-MY, mt, nb, nb-NO, nl, pl, pt, pt-BR, ro, ru, sk, sl, sr, sr-Latn, sv, th, tr, uk, uz-Cyrl-UZ, uz-Latn-UZ, vi, zh-CN, zh-Hans, zh-Hant.

## Supported Frameworks

Humanizer supports the following .NET target frameworks:

- **.NET 10.0** (net10.0)
- **.NET 9.0** (net9.0)
- **.NET 8.0** (net8.0)
- **.NET Framework 4.8** (net48)
- **.NET Standard 2.0** (netstandard2.0) - Special case for Roslyn Analyzers and MSBuild tasks

> **Note:** While .NET Framework versions 4.6.1 through 4.7.2 can technically consume netstandard2.0 libraries, they are **not officially supported** by Humanizer and may not work correctly. Use one of the explicitly supported frameworks above.

## Requirements for Humanizer 3.0

> [!IMPORTANT]
> The `Humanizer` metapackage requires the NuGet locale parsing fix shipped in **.NET SDK 9.0.200** and corresponding Visual Studio/MSBuild updates.
>
> Restore operations must run on:
> - .NET SDK 9.0.200 or newer, OR
> - Visual Studio 2022/MSBuild versions that include the locale parsing patch
>
> Older SDKs/MSBuild versions will fail to restore the metapackage because they do not recognize three-letter locale identifiers.
>
> **Workaround for older tooling:** Reference `Humanizer.Core` directly and add desired `Humanizer.Core.<locale>` satellite packages individually.

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
- [String Humanization](string-humanization.md)
- [DateTime Humanization](datetime-humanization.md)
