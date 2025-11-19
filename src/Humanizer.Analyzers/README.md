# Humanizer.Analyzers

A Roslyn analyzer and code fix provider to help migrate code from Humanizer v2 to v3.

## What it does

Humanizer v3 consolidates all sub-namespaces into the root `Humanizer` namespace. This analyzer:

1. **Detects** old namespace usages (e.g., `Humanizer.Bytes`, `Humanizer.Localisation`)
2. **Provides** automatic code fixes to update to `Humanizer`
3. **Supports** batch fixing (Fix All) for entire projects or solutions

## Deprecated Namespaces

The following namespaces have been consolidated into `Humanizer`:

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

## Usage

### Automatic Fixes

The analyzer will highlight old namespace usages with warnings. You can:

1. **Single fix**: Click the lightbulb 💡 icon and select "Update to Humanizer namespace"
2. **Fix All in Document**: Right-click → Quick Actions → Fix All Occurrences in Document
3. **Fix All in Project**: Right-click → Quick Actions → Fix All Occurrences in Project
4. **Fix All in Solution**: Right-click → Quick Actions → Fix All Occurrences in Solution

### Examples

**Before:**
```csharp
using Humanizer.Bytes;
using Humanizer.Localisation;

public class Example
{
    public void Method()
    {
        var size = ByteSize.FromKilobytes(10);
        var formatter = new DefaultFormatter();
    }
}
```

**After (automatic fix):**
```csharp
using Humanizer;

public class Example
{
    public void Method()
    {
        var size = ByteSize.FromKilobytes(10);
        var formatter = new DefaultFormatter();
    }
}
```

## Command Line Usage

You can also use the .NET CLI to apply fixes:

```bash
# Analyze and show diagnostics
dotnet build /p:TreatWarningsAsErrors=false

# Apply code fixes (requires dotnet format or IDE)
dotnet format analyzers --diagnostics HUMANIZER001
```

## Development

### Building

```bash
cd src/Humanizer.Analyzers
dotnet build
```

### Testing

```bash
cd src/Humanizer.Analyzers.Tests
dotnet test
```

## Diagnostic ID

- **HUMANIZER001**: Old Humanizer namespace usage