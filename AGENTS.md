# Agent Instructions

## Scope
These instructions apply to the entire repository.

## Project Overview
- Humanizer is a .NET library for turning numbers, dates, times, enums, quantities, etc. into human-friendly text across many locales.
- The main library lives in `src/Humanizer`; tests are under `tests/`.

## Toolchain
- Primary language: C# (modern features).
- Target frameworks: .NET 8.0, .NET 10.0, .NET 11.0, and .NET Framework 4.8.
- Tests use xUnit and should live alongside similar tests in `tests/Humanizer.Tests`.
- Build with the .NET CLI (`dotnet`). Prefer the latest SDK.

## Coding Guidelines
- Respect `.editorconfig`; use spaces, 4-space indentation, and file-scoped namespaces.
- Use `var` for obvious types, and language keywords (`string`, `int`, etc.).
- Order `using` directives with `System.*` first; prefer existing global usings.
- Keep code self-documenting; use comments sparingly.
- Never wrap imports in `try/catch`.
- Avoid unnecessary `this.` and braces for one-line blocks; trim redundant blank lines.
- Follow existing naming conventions (camelCase private fields, PascalCase public members/constants/static readonly).
- Add XML documentation for new or modified public APIs.

## Testing Expectations
- Every functional change must include or update xUnit tests in `tests/Humanizer.Tests`.
- Use culture-specific folders and `UseCulture` attribute for localization tests when applicable.
- Run the test suite for the supported .NET targets: `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0`, `--framework net11.0` and `--framework net8.0` on all platforms. The `net48` TFM is only included in the test project on Windows; run `--framework net48` tests only on Windows hosts. Allow a few minutes for each run to complete.

## Build & Validation
- Build command: `dotnet pack src/Humanizer/Humanizer.csproj -c Release -o <path>` (from the repository root). It must succeed without warnings or errors.
- If you need to reference those newly build packages, create or update `NuGet.config` to use that package output path as a package source--but never commit changes to that file.
- When verifying restore, build first, then pass the output path into `tests/verify-packages.ps1`.
- `tests/verify-packages.ps1` validates analyzer packaging and analyzer load behavior for the packed `Humanizer` package.
- Do not introduce new compiler warnings or break existing build/test workflows.

## Lint & Format
- Verify formatting: `dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic`
- Auto-fix formatting: `dotnet format Humanizer.slnx`
- Rules are defined in `.editorconfig` and enforced at build time via `EnforceCodeStyleInBuild=true`.

## Localization Guidance
- When adding a locale, duplicate and translate the relevant YAML locale file under `src/Humanizer/Locales`; the source generator wires all registries automatically (see `docs/adding-a-locale.md`).
- When ICU-supplied data (month names, decimal separators) differs across platforms, author explicit overrides in `calendar:` and/or `number.formatting:` YAML surfaces rather than changing `CultureInfo` directly.
- Cover new localization behavior with targeted tests under `tests/Humanizer.Tests/Localisation/{culture}`.

## Documentation Updates
- Update `readme.md`, resource comments, or XML docs when introducing new features or behavior changes.
- Provide meaningful examples in documentation and XML summaries where appropriate.

## Pull Request Guidelines
- Keep changes focused with clear commit messages.
- Follow repository PR template expectations: summarize changes, list tests run, and reference related issues (e.g., `fixes #123`) when applicable.
- Ensure the codebase remains backward-compatible unless intentionally introducing a documented breaking change.

<!-- BEGIN FLOW-NEXT -->
## Flow-Next

This project uses Flow-Next for task tracking. Use `.flow/bin/flowctl` instead of markdown TODOs or TodoWrite.

**Quick commands:**
```bash
.flow/bin/flowctl list                # List all epics + tasks
.flow/bin/flowctl epics               # List all epics
.flow/bin/flowctl tasks --epic fn-N   # List tasks for epic
.flow/bin/flowctl ready --epic fn-N   # What's ready
.flow/bin/flowctl show fn-N.M         # View task
.flow/bin/flowctl start fn-N.M        # Claim task
.flow/bin/flowctl done fn-N.M --summary-file s.md --evidence-json e.json
```

**Creating a spec** ("create a spec", "spec out X", "write a spec for X"):

A spec = an epic. Create one directly — do NOT use `$flow-next-plan` (that breaks specs into tasks).

```bash
.flow/bin/flowctl epic create --title "Short title" --json
.flow/bin/flowctl epic set-plan <epic-id> --file - --json <<'EOF'
# Title

## Goal & Context
Why this exists, what problem it solves.

## Architecture & Data Models
System design, data flow, key components.

## API Contracts
Endpoints, interfaces, input/output shapes.

## Edge Cases & Constraints
Failure modes, limits, performance requirements.

## Acceptance Criteria
- [ ] Testable criterion 1
- [ ] Testable criterion 2

## Boundaries
What's explicitly out of scope.

## Decision Context
Why this approach over alternatives.
EOF
```

After creating a spec, choose next step:
- `$flow-next-plan <epic-id>` — research + break into tasks
- `$flow-next-interview <epic-id>` — deep Q&A to refine the spec

**Rules:**
- Use `.flow/bin/flowctl` for ALL task tracking
- Do NOT create markdown TODOs or use TodoWrite
- Re-anchor (re-read spec + status) before every task

**More info:** `.flow/bin/flowctl --help` or read `.flow/usage.md`
<!-- END FLOW-NEXT -->
