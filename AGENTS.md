# Agent Instructions

## Scope
These instructions apply to the entire repository.

## Project Overview
- Humanizer is a .NET library for turning numbers, dates, times, enums, quantities, etc. into human-friendly text across many locales.
- The main library lives in `src/Humanizer`; tests are under `src/Humanizer.Tests`.

## Toolchain
- Primary language: C# (modern features).
- Target frameworks: .NET 8.0, .NET 10.0, and .NET Framework 4.8.
- Tests use xUnit and should live alongside similar tests in `src/Humanizer.Tests`.
- Build with the .NET CLI (`dotnet`). Prefer the latest SDK (see install script in `.github/copilot-instructions.md`).

## Coding Guidelines
- Respect `.editorconfig`; use spaces, 4-space indentation, and file-scoped namespaces.
- Use `var` for obvious types, and language keywords (`string`, `int`, etc.).
- Order `using` directives with `System.*` first; prefer existing global usings.
- Keep code self-documenting; use comments sparingly and never wrap imports in `try/catch`.
- Avoid unnecessary `this.` and braces for one-line blocks; trim redundant blank lines.
- Follow existing naming conventions (camelCase private fields, PascalCase public members/constants/static readonly).
- Add XML documentation for new or modified public APIs.

## Testing Expectations
- Every functional change must include or update xUnit tests in `src/Humanizer.Tests`.
- Use culture-specific folders and `UseCulture` attribute for localization tests when applicable.
- Run relevant `dotnet test` commands (e.g., `dotnet test src/Humanizer.Tests/Humanizer.Tests.csproj`) and ensure they pass before submission.

## Build & Validation
- Ensure `dotnet build Humanizer/Humanizer.csproj /t:PackNuSpecs` (from `src`) succeeds without warnings.
- Do not introduce new compiler warnings or break existing build/test workflows.

## Localization Guidance
- When adding a locale, duplicate and translate the relevant resource files under `src/Humanizer/Properties`.
- Register new formatters/converters in the appropriate registries (see `Configuration/FormatterRegistry.cs` and number converter factories).
- Cover new localization behavior with targeted tests under `src/Humanizer.Tests/Localisation/{culture}`.

## Documentation Updates
- Update `readme.md`, resource comments, or XML docs when introducing new features or behavior changes.
- Provide meaningful examples in documentation and XML summaries where appropriate.

## Pull Request Guidelines
- Keep changes focused with clear commit messages.
- Follow repository PR template expectations: summarize changes, list tests run, and reference related issues (e.g., `fixes #123`) when applicable.
- Ensure the codebase remains backward-compatible unless intentionally introducing a documented breaking change.
