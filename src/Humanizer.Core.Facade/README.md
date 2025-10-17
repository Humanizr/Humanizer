# Humanizer.Core.Facade

This project provides binary compatibility for applications compiled against Humanizer v2.x.

## Purpose

In Humanizer v3.0, all types were moved from various sub-namespaces (like `Humanizer.Bytes`, `Humanizer.Localisation`, etc.) to the root `Humanizer` namespace. This facade assembly maintains binary compatibility by using `[TypeForwardedTo]` attributes to redirect type references.

## How It Works

1. This project builds an assembly named `Humanizer.Core.dll` (the old assembly name from v2.x)
2. The main Humanizer project builds `Humanizer.dll` (contains all actual implementations)
3. Both assemblies are included in the Humanizer.Core NuGet package
4. When v2.x compiled code references types like `[Humanizer.Core]Humanizer.Bytes.ByteSize`, the runtime:
   - Loads `Humanizer.Core.dll` (this facade)
   - Finds `[assembly: TypeForwardedTo(typeof(Humanizer.ByteSize))]`
   - Redirects to `[Humanizer]Humanizer.ByteSize` in the main assembly

## Result

Applications compiled against Humanizer v2.x can upgrade to v3.0 without recompilation, maintaining binary compatibility while benefiting from the simplified namespace structure.

## Contents

- `TypeForwards.cs` - Contains all TypeForwardedTo attributes mapping old namespaces to new locations
- Conditional compilation directives for types that only exist in certain target frameworks (e.g., DateOnly/TimeOnly types in .NET 6+)

## Build Output

This project is not packaged separately. Its output (`Humanizer.Core.dll`) is included in the main Humanizer.Core NuGet package alongside `Humanizer.dll`.
