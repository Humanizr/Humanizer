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
- Run the test suite for the supported .NET targets: `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net8.0`, `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0`, and `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net11.0` on all platforms. The `net48` TFM is only included in the test project on Windows; run `dotnet test tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net48` tests only on Windows hosts. Allow a few minutes for each run to complete.
- The repository uses Microsoft Testing Platform. Filter targeted runs with `--filter-class <fully-qualified-class-name>`, `--filter-method <fully-qualified-method-name>`, or `--filter-namespace <namespace>`; do not use the legacy VSTest `--filter FullyQualifiedName~...` syntax.
- Tests that require a specific culture must declare it with `UseCulture`; do not rely on the process's ambient culture, which is invariant-like when WSL starts with `C.UTF-8`.

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
- When adding a locale, duplicate and translate the relevant YAML locale file under `src/Humanizer/Locales`; the source generator wires all registries automatically (see `website/docs/contributing/adding-or-updating-a-locale.mdx`).
- When ICU-supplied data (month names, decimal separators) differs across platforms, author explicit overrides in `calendar:` and/or `number.formatting:` YAML surfaces rather than changing `CultureInfo` directly.
- Cover new localization behavior with targeted tests under `tests/Humanizer.Tests/Localisation/{culture}`.

## Documentation Updates
- Update `readme.md`, resource comments, or XML docs when introducing new features or behavior changes.
- Provide meaningful examples in documentation and XML summaries where appropriate.

## Pull Request Guidelines
- Keep changes focused with clear commit messages.
- Follow repository PR template expectations: summarize changes, list tests run, and reference related issues (e.g., `fixes #123`) when applicable.
- Ensure the codebase remains backward-compatible unless intentionally introducing a documented breaking change.

## Terminal Pull Request Gate
- Every agent-managed pull request must be ready, never draft. Before merging a new or existing pull request, reconcile its body with the current template: remove stale draft instructions and add the Terminal evidence section.
- After the final push, authenticate and record the exact `{baseSha, headSha}` pair in the pull request body. Run the actual `agent-utilities:thermo-nuclear-review` and `agent-utilities:thermo-nuclear-code-quality-review` skills against that pair; both must finish with final clean/APPROVE, merge-eligible outcomes. Record an explicit disposition for every finding: fix and push valid findings; for invalid or non-actionable findings, record the evidence and reason and obtain final reviewer acceptance. If there are no valid findings, no push is required.
- Confirm the head is current and mergeable. Run every applicable test, format, build, package, documentation, browser, security, and platform gate. Resolve actionable review threads; mark any remaining item `needs-human` and pause the merge. Require terminal hosted CI and ruleset checks to be green.
- Run the actual `compound-engineering:ce-babysit-pr` skill against the exact pair through the current-head reviewer lifecycle, CI, base movement, and a quiet settle, and record its terminal clean evidence. If either SHA changes, first update the pull request body with the replacement `{baseSha, headSha}` pair, then invalidate prior evidence and rerun both Thermos reviews, every applicable check, and babysitting against that recorded pair. Immediately before merging, reauthenticate the live base and head, verify that they exactly match the recorded pair, and refuse the merge on any mismatch.
- Security changes also require Codex Security proof-of-concept or attack-path closure. Changes affecting rendered documentation, the site, or UI require desktop and mobile checks in light and dark modes, accessibility and link checks, and version-snapshot validation. Localization or source-generator changes require every applicable locale, schema, generator, and runtime matrix with no partial or English fallback.
- Merge only the exact approved head. Then verify the merge on the default branch, the changed behavior, and any applicable security dashboard; safely remove worktrees and branches and archive completed tasks.
- Thermos is a merge-director evidence gate recorded in each pull request body using the template, not a GitHub required status. Subject to its configured bypass, the active repository ruleset separately enforces hosted CI, CodeQL, DevSkim, and code-quality checks; organization-admin bypass will not be used for agent-managed merges.
