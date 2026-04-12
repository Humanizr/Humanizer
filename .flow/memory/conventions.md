# Conventions

Project patterns discovered during work. Not in CLAUDE.md but important.

<!-- Entries added manually via `flowctl memory add` -->

## 2026-04-09 manual [convention]
When adding a new YAML data surface to Humanizer's source generator, explicitly plan the plumbing: (1) ResolveX method in LocaleYamlCatalog, (2) property on ResolvedLocaleDefinition, (3) downstream generator reads the property. Data not carried on ResolvedLocaleDefinition is invisible to generators.

## 2026-04-09 manual [convention]
Humanizer runtime locale resolution uses CultureInfo.Parent walking in LocaliserRegistry.FindLocaliser. Any new per-locale override registry must use the same fallback semantics, otherwise a user with culture ar-EG would get formatter resolution via ar but miss decimal separator override.

## 2026-04-09 manual [convention]
When multiple locale overrides use different selection criteria (e.g., technical-formatting vs locale-identity), state the governing principle explicitly before the individual decisions so the reasoning framework is clear
