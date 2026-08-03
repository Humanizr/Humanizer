# Inflection Unicode data generator

This maintainer tool deterministically regenerates and verifies the Unicode 16
tables used by localized inflection. It requires the unmodified Unicode 16.0.0
`UnicodeData.txt`, `CaseFolding.txt`, `Scripts.txt`, `ScriptExtensions.txt`, and
`DerivedNormalizationProps.txt` files in one directory. The tool rejects any
input whose SHA-256 does not match the pinned Unicode 16 release bytes.

From the repository root, regenerate the checked-in tables with:

```shell
dotnet run --project tools/Humanizer.UnicodeDataGenerator -- <ucd-directory>
```

Verify that the checked-in tables are current without changing files with:

```shell
dotnet run --project tools/Humanizer.UnicodeDataGenerator -- <ucd-directory> --check
```

Verify deterministic output under both English and German process cultures with:

```powershell
./tools/Humanizer.UnicodeDataGenerator/verify-cultures.ps1 <ucd-directory>
```

The normal build and runtime never download Unicode data.
