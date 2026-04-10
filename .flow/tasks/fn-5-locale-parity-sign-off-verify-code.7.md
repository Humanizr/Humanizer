# fn-5-locale-parity-sign-off-verify-code.7 Fix net48 test build break (#if guard) and remove stale fn-4 framing

## Description
Fix the net48 build break in `tests/Humanizer.Tests/Humanizer.Tests.csproj` so the test project compiles for all three target frameworks (`net10.0`, `net8.0`, `net48`). The library project (`src/Humanizer/Humanizer.csproj`) already references `Polyfill` (9.18.0) with `PrivateAssets="all"`, which provides `Enum.GetValues<TEnum>()` as a true `extension(Enum)` static method on .NET Framework / netstandard2.0. The test project does not currently reference Polyfill, so the one usage at `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs:379` (`Enum.GetValues<GrammaticalGender>()`) fails to compile under `TargetFramework=net48` with `error CS0308: The non-generic method 'Enum.GetValues(Type)' cannot be used with type arguments`.

This task subsumes the work of `fn-4-fix-net48-test-suite-blocker`. fn-4 will be closed as superseded by `fn-5.9` after this task lands.

**Size:** S→M (verification spans 3 TFMs).

**Files:**
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs:379` (add `#if NET5_0_OR_GREATER` guard around `Enum.GetValues<GrammaticalGender>()`)
- `CLAUDE.md` line 17 (drop "blocked on all platforms by Enum.GetValues<T>() — see fn-4" framing)
- `AGENTS.md` line 29 (same)
- ~~`tests/Humanizer.Tests/Humanizer.Tests.csproj`~~ (Polyfill PackageReference was attempted but caused type conflicts; no csproj change needed)

## Approach

**Preferred path — add Polyfill reference to the test project:**

`src/Humanizer/Humanizer.csproj:11` already declares `<PackageReference Include="Polyfill" PrivateAssets="all" />`. Add the same line to `tests/Humanizer.Tests/Humanizer.Tests.csproj` (no Condition — Polyfill is no-op on net8.0/net10.0). The package is already pinned in `Directory.Packages.props:42` (`<PackageVersion Include="Polyfill" Version="9.18.0" />`), so no version specification is needed.

Polyfill 9.18.0 implements `Enum.GetValues<TEnum>()` via the C# 14 `extension(Enum)` syntax (verified at `~/.nuget/packages/polyfill/9.18.0/contentFiles/cs/net471/EnumPolyfill.cs:8-20`), so the existing call site at `LocaleTheoryMatrixCompletenessTests.cs:379` continues to compile unchanged.

**Fallback path — `#if` guard at the call site (only if Polyfill ref turns out to break something):**

If for any reason adding the Polyfill PackageReference cannot be made to work for the test project (e.g., conflict with `Verify.XunitV3` or `xunit.v3.mtp-v2` typegen, surprising trim warning under `TreatWarningsAsErrors=true`), fall back to ifdef'ing the single call site:

```csharp
#if NET5_0_OR_GREATER
            foreach (var gender in Enum.GetValues<GrammaticalGender>())
#else
            foreach (GrammaticalGender gender in (GrammaticalGender[])Enum.GetValues(typeof(GrammaticalGender)))
#endif
```

The fallback is the same shape as the existing `#if NET5_0_OR_GREATER` guard already used in `src/Humanizer/EnumCache.cs:53-57` for `Enum.GetName`. Do NOT introduce a new helper or wrapper — one ifdef on one call site is the minimum surgery.

**Documentation update (after the build is green for all 3 TFMs):**

- `CLAUDE.md:17` — change the inline comment from `# Run tests (net10.0 or net8.0; net48 is blocked on all platforms by Enum.GetValues<T>() — see fn-4)` to a single concise comment that says net10.0 / net8.0 / net48 all build successfully on every platform; net48 *test execution* requires a Windows host (the .NET Framework 4.8 runtime is Windows-only) but the project compiles cleanly elsewhere. No mention of fn-4 (it is closed superseded).
- `AGENTS.md:29` — the same reframing in agent-targeted prose. Drop the "do not invoke it" instruction; net48 compiles on macOS/Linux now.

## Investigation targets

**Required** (read before coding):
- `src/Humanizer/Humanizer.csproj:11` — existing Polyfill PackageReference in the library project (template for the test project edit)
- `Directory.Packages.props:42` — central Polyfill version pin (do not duplicate the version)
- `tests/Humanizer.Tests/Humanizer.Tests.csproj` — full file (verify no existing Polyfill reference, confirm `<PackageReference>` block style)
- `tests/Humanizer.Tests/Localisation/LocaleTheoryMatrixCompletenessTests.cs:374-386` — the failing call site context
- `~/.nuget/packages/polyfill/9.18.0/contentFiles/cs/net471/EnumPolyfill.cs:8-20` — confirms the `extension(Enum) GetValues<TEnum>()` API surface
- `src/Humanizer/EnumCache.cs:51-57` — existing `#if NET5_0_OR_GREATER` ifdef pattern (template for the fallback path only)

**Optional** (reference as needed):
- `tools/locale-probe-net48/locale-probe-net48.csproj` — only project that targets net48 directly today; confirms net48 reachability
- `src/Humanizer/TimeSpanHumanizeExtensions.cs:12` — second `Enum.GetValues<T>()` call site in the library (no action needed; included only as evidence Polyfill is already wiring this for net48 in the library project)

## Key context

- **Reproducer** (current state, before fix): `dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net48` → `error CS0308: The non-generic method 'Enum.GetValues(Type)' cannot be used with type arguments` at `LocaleTheoryMatrixCompletenessTests.cs(379,41)`. Library builds fine for net48 because Polyfill is already referenced there.
- **Polyfill 9.18.0 uses C# 14 `extension(Enum)` syntax** — this is unconditional surface, not opt-in. As long as the project's compiler can parse `extension(Type)`, the `Enum.GetValues<T>()` symbol resolves on every TFM that doesn't already have the BCL method. The repo is already on .NET 10 SDK (`global.json: 10.0.100`), which ships C# 14, so this works without any LangVersion changes.
- **`PrivateAssets="all"` is correct on the test project too.** The test assembly is not redistributed, so flowing Polyfill helpers to consumers is moot. Match the library project's setting for consistency.
- **net48 test EXECUTION on macOS/Linux is still impossible** — the .NET Framework 4.8 runtime is Windows-only. After this fix, `dotnet build -f net48` succeeds on every platform, but `dotnet test -f net48` only runs on Windows. That is a host-OS limitation, not a code defect, and must be honestly stated in CLAUDE.md / AGENTS.md / the sign-off doc — NOT framed as "deferred" or "blocked".

## Acceptance

- [ ] `LocaleTheoryMatrixCompletenessTests.cs:379` uses `#if NET5_0_OR_GREATER` guard around `Enum.GetValues<GrammaticalGender>()` with non-generic fallback for net48 (Polyfill PackageReference was attempted first but caused CS0436/CS0121 type conflicts)
- [ ] `dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net48` exits 0 with 0 errors and 0 warnings
- [ ] `dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net8.0` exits 0 with 0 errors and 0 warnings (net8.0 regression check)
- [ ] `dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net10.0` exits 0 with 0 errors and 0 warnings (net10.0 regression check)
- [ ] `dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0` passes (full suite, no regressions)
- [ ] `dotnet build Humanizer.slnx -c Release` exits 0 with 0 errors and 0 warnings (whole-solution sanity)
- [ ] `dotnet format Humanizer.slnx --verify-no-changes --verbosity diagnostic` passes
- [ ] `LocaleTheoryMatrixCompletenessTests.cs:379` uses `#if NET5_0_OR_GREATER` guard (fallback path taken — Polyfill PackageReference caused type conflicts documented in task evidence).
- [ ] `CLAUDE.md` has no remaining occurrences of `blocked on all platforms by Enum.GetValues<T>()` or `see fn-4`. The replacement line accurately states all 3 TFMs build everywhere; net48 test execution requires a Windows host.
- [ ] `AGENTS.md` has no remaining occurrences of `currently blocked on all platforms by` or `tracked as fn-4` or `do not invoke it`. The replacement prose accurately states the same.
- [ ] `grep -rn "see fn-4\|tracked as fn-4\|blocked.*Enum\.GetValues" CLAUDE.md AGENTS.md` returns zero matches
- [ ] Task evidence records: chosen path (preferred / fallback), exact build commands run, exit codes, and a one-line note that fn-4 is superseded (final close happens in fn-5.9)

## Done summary
Fixed net48 test build break by adding #if NET5_0_OR_GREATER guard around Enum.GetValues<GrammaticalGender>() in LocaleTheoryMatrixCompletenessTests.cs (Polyfill PackageReference approach caused type conflicts), and updated CLAUDE.md/AGENTS.md to remove stale fn-4 blocked framing, accurately documenting that all 3 TFMs build everywhere and net48 test execution requires a Windows host.
## Evidence
- Commits: 424ed0d2, 3f17c906
- Tests: dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net48 (0 errors, 0 warnings), dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net8.0 (0 errors, 0 warnings), dotnet build tests/Humanizer.Tests/Humanizer.Tests.csproj -c Release -f net10.0 (0 errors, 0 warnings), dotnet test --project tests/Humanizer.Tests/Humanizer.Tests.csproj --framework net10.0 (38908 passed, 0 failed), dotnet build Humanizer.slnx -c Release (0 errors, 0 warnings), dotnet format Humanizer.slnx --verify-no-changes (0 of 1596 formatted), grep -rn fn-4/blocked CLAUDE.md AGENTS.md (zero matches)
- PRs: