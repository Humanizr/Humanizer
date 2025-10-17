# Humanizer v3 Namespace Migration

This document provides guidance on migrating code from Humanizer v2 to v3, focusing on the namespace consolidation changes.

## What Changed in v3

Humanizer v3 consolidates all sub-namespaces into the root `Humanizer` namespace. This is a **source-breaking change** that requires code updates.

### Consolidated Namespaces

All of the following namespaces have been moved into `Humanizer`:

- `Humanizer.Bytes`
- `Humanizer.Localisation`
- `Humanizer.Localisation.Formatters`
- `Humanizer.Localisation.NumberToWords`
- `Humanizer.DateTimeHumanizeStrategy`
- `Humanizer.Configuration`
- `Humanizer.Localisation.DateToOrdinalWords`
- `Humanizer.Localisation.Ordinalizers`
- `Humanizer.Inflections`
- `Humanizer.Localisation.CollectionFormatters`
- `Humanizer.Localisation.TimeToClockNotation`

### Migration Path

**Before (v2):**
```csharp
using Humanizer.Bytes;
using Humanizer.Localisation;
using Humanizer.Configuration;

public class Example
{
    public void Method()
    {
        var size = ByteSize.FromKilobytes(10);
        var formatter = new DefaultFormatter("en-US");
        Configurator.Localisation = new CultureInfo("en-US");
    }
}
```

**After (v3):**
```csharp
using Humanizer;

public class Example
{
    public void Method()
    {
        var size = ByteSize.FromKilobytes(10);
        var formatter = new DefaultFormatter("en-US");
        Configurator.Localisation = new CultureInfo("en-US");
    }
}
```

## Automated Migration with Roslyn Analyzer

Humanizer v3 includes a Roslyn analyzer that automatically detects and fixes namespace usage. No separate installation is required—the analyzer is bundled with the `Humanizer.Core` package starting from v3.0.0.

### Usage

Once you have installed or updated to `Humanizer.Core` v3.0.0 or later, the analyzer will be available automatically in your project.
### Usage in Visual Studio / Rider

1. The analyzer will highlight old namespace usages with warnings (HUMANIZER001)
2. Click the lightbulb 💡 icon or press `Ctrl+.` (Windows/Linux) / `Cmd+.` (Mac)
3. Select "Update to Humanizer namespace"
4. To fix all occurrences:
   - Right-click → Quick Actions and Refactorings
   - Choose "Fix all occurrences in Document/Project/Solution"

### Usage with dotnet CLI

The analyzer runs automatically during build and reports warnings:

```bash
# Build and see warnings
dotnet build

# Apply automatic fixes using dotnet format
dotnet format analyzers --diagnostics HUMANIZER001
```

## Manual Migration

If you prefer to migrate manually or can't use the analyzer:

### Step 1: Find Old Namespaces

Search your codebase for using directives:

```bash
# Using grep
grep -r "using Humanizer\." . --include="*.cs" | grep -v "using Humanizer;"

# Using PowerShell
Get-ChildItem -Recurse -Filter *.cs | Select-String "using Humanizer\." | Where-Object { $_ -notmatch "using Humanizer;" }
```

### Step 2: Replace with Single Using

Replace all old namespace usings with:
```csharp
using Humanizer;
```

### Step 3: Remove Qualified Names

If you use fully qualified names in your code, update them:

**Before:**
```csharp
var size = Humanizer.Bytes.ByteSize.FromKilobytes(10);
```

**After:**
```csharp
var size = Humanizer.ByteSize.FromKilobytes(10);
// Or with using directive:
using Humanizer;
// ...
var size = ByteSize.FromKilobytes(10);
```

### Step 4: Test Your Changes

After migration, rebuild and test your application:

```bash
dotnet build
dotnet test
```

## Common Migration Scenarios

### Scenario 1: Multiple Old Namespaces

**Before:**
```csharp
using Humanizer.Bytes;
using Humanizer.Localisation;
using Humanizer.Configuration;
using Humanizer.Inflections;
```

**After:**
```csharp
using Humanizer;
```

### Scenario 2: Mix of Old and New

**Before:**
```csharp
using Humanizer;
using Humanizer.Bytes;
using Humanizer.Localisation;
```

**After:**
```csharp
using Humanizer;
```

### Scenario 3: Qualified Names in Code

**Before:**
```csharp
public Humanizer.Bytes.ByteSize GetFileSize(string path)
{
    var info = new FileInfo(path);
    return Humanizer.Bytes.ByteSize.FromBytes(info.Length);
}
```

**After:**
```csharp
public Humanizer.ByteSize GetFileSize(string path)
{
    var info = new FileInfo(path);
    return Humanizer.ByteSize.FromBytes(info.Length);
}
// Or better with using:
using Humanizer;

public ByteSize GetFileSize(string path)
{
    var info = new FileInfo(path);
    return ByteSize.FromBytes(info.Length);
}
```

## Troubleshooting

### "The type or namespace name could not be found"

If you get compiler errors after migration, ensure:

1. You've updated to Humanizer v3
2. You have `using Humanizer;` at the top of your file
3. You're targeting a supported framework (net8.0, net10.0, or net48)

### Analyzer Not Showing Warnings

If the analyzer doesn't show warnings:

1. Rebuild your solution (`dotnet clean && dotnet build`)
2. Restart your IDE
3. Check that the analyzer package is correctly referenced
4. Ensure you're using a compatible IDE/editor (VS 2022, Rider, VS Code with C# extension)

### Can't Use the Analyzer

If you can't use the Roslyn analyzer (e.g., older IDE, build system limitations):

1. Use the manual migration steps above
2. Use find/replace in your editor
3. Use command-line tools (grep, sed, PowerShell) to find and replace

## Support

- **Documentation:** See [Humanizer README](../readme.md)
- **Issues:** [GitHub Issues](https://github.com/Humanizr/Humanizer/issues)
- **Discussions:** [GitHub Discussions](https://github.com/Humanizr/Humanizer/discussions)

## Summary

- ✅ **Recommended:** Use `Humanizer.Analyzers` for automatic detection and fixing
- ✅ **Alternative:** Manual migration using find/replace
- ⚠️ **Breaking Change:** Old namespaces will not work in v3
- 📦 **Minimal Change:** Usually just need to update using directives to `using Humanizer;`
- 🎯 **Goal:** All Humanizer types are now in the root `Humanizer` namespace
