# Conventions

Project patterns discovered during work. Not in CLAUDE.md but important.

<!-- Entries added manually via `flowctl memory add` -->

## 2026-04-09 manual [convention]
When adding a new YAML data surface to Humanizer's source generator, explicitly plan the plumbing: (1) ResolveX method in LocaleYamlCatalog, (2) property on ResolvedLocaleDefinition, (3) downstream generator reads the property. Data not carried on ResolvedLocaleDefinition is invisible to generators.

## 2026-04-09 manual [convention]
Humanizer runtime locale resolution uses CultureInfo.Parent walking in LocaliserRegistry.FindLocaliser. Any new per-locale override registry must use the same fallback semantics, otherwise a user with culture ar-EG would get formatter resolution via ar but miss decimal separator override.

## 2026-04-09 manual [convention]
When multiple locale overrides use different selection criteria (e.g., technical-formatting vs locale-identity), state the governing principle explicitly before the individual decisions so the reasoning framework is clear

## 2026-04-13 manual [convention]
For long.MinValue/int.MinValue overflow in number-to-words converters, use throw NotImplementedException() matching existing codebase convention (WestSlavic, ScaleStrategy)

## 2026-04-13 manual [convention]
When adding a new optional calendar surface (like hijriMonths), the emitter in OrdinalDateProfileCatalogInput must handle it independently of existing fields — the new field's presence guard must not be nested under an existing field's guard. The OrdinalDatePattern constructor accepts the new array as an optional parameter.

## 2026-04-13 manual [convention]
For ordinalizer engines that call other resolvers internally (e.g. number-word-suffix calls NumberToWords), RequiresCulture must be intrinsic to the engine name — culture binding is a structural requirement of the engine, not just an author-configured flag
