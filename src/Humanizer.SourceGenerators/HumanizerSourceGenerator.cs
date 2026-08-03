using Microsoft.CodeAnalysis;

namespace Humanizer.SourceGenerators;

[Generator]
public sealed partial class HumanizerSourceGenerator : IIncrementalGenerator
{
    /// <inheritdoc />
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var localeFile = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => LocaleDefinitionFile.Create(additionalText, cancellationToken))
            .Where(static file => file is not null);

        var localeFiles = localeFile.Collect();

        var localeCatalog = localeFiles
            .Select(static (files, _) => LocaleCatalogInput.Create(files));

        var localePhraseTables = localeCatalog
            .Select(static (catalog, _) => LocalePhraseTableCatalogInput.Create(catalog));

        context.RegisterSourceOutput(localePhraseTables, static (productionContext, input) => input.Emit(productionContext));

        var localeDurationCases = localeCatalog
            .Select(static (catalog, _) => LocaleDurationCaseTableCatalogInput.Create(catalog));

        context.RegisterSourceOutput(localeDurationCases, static (productionContext, input) => input.Emit(productionContext));

        var headingTables = localeCatalog
            .Select(static (catalog, _) => HeadingTableCatalogInput.Create(catalog));

        context.RegisterSourceOutput(headingTables, static (productionContext, input) => input.Emit(productionContext));

        var tokenMapLocales = localeCatalog
            .Select(static (catalog, _) => TokenMapWordsToNumberInput.Create(catalog));

        context.RegisterSourceOutput(tokenMapLocales, static (productionContext, input) => input.Emit(productionContext));

        var formatterProfiles = localeCatalog
            .Select(static (catalog, _) => FormatterProfileCatalogInput.Create(catalog));

        context.RegisterSourceOutput(formatterProfiles, static (productionContext, input) => input.Emit(productionContext));

        var inflectionOwnerSources = localeFile
            .Select(static (file, _) => PerLocaleInflectionSourceInput.Create(file));

        var inflectionOwnerSourceCatalog = inflectionOwnerSources
            .Collect()
            .Select(static (sources, _) => InflectionOwnerSourceCatalog.Create(sources));

        var inflectionProfiles = localeCatalog
            .Combine(inflectionOwnerSourceCatalog)
            .Select(static (input, _) => InflectionCatalogInput.Create(
                input.Left,
                input.Right));

        context.RegisterSourceOutput(
            inflectionProfiles,
            static (productionContext, input) => input.EmitDiagnostics(productionContext));

        var inflectionRegistry = localeCatalog
            .Combine(inflectionProfiles)
            .Select(static (input, _) => input.Right.GetRegistryInput(input.Left))
            .WithTrackingName("InflectionRegistrySource");

        context.RegisterSourceOutput(
            inflectionRegistry,
            static (productionContext, input) => input.Emit(productionContext));

        var inflectionOwnerState = inflectionProfiles
            .Select(static (catalog, _) => catalog.GetOwnerEmissionState());

        var inflectionOwners = inflectionOwnerSources
            .Combine(inflectionOwnerState)
            .Select(static (input, _) => PerLocaleInflectionInput.Create(
                input.Left,
                input.Right))
            .WithTrackingName("InflectionOwnerSource");

        context.RegisterSourceOutput(inflectionOwners, static (productionContext, input) => input.Emit(productionContext));

        var numberToWordsProfiles = localeCatalog
            .Select(static (catalog, _) => NumberToWordsProfileCatalogInput.Create(catalog));

        context.RegisterSourceOutput(numberToWordsProfiles, static (productionContext, input) => input.Emit(productionContext));

        var metricScaleWords = localeCatalog
            .Select(static (catalog, _) => MetricScaleWordCatalogInput.Create(catalog));

        context.RegisterSourceOutput(metricScaleWords, static (productionContext, input) => input.Emit(productionContext));

        var ordinalizerProfiles = localeCatalog
            .Select(static (catalog, _) => OrdinalizerProfileCatalogInput.Create(catalog));

        context.RegisterSourceOutput(ordinalizerProfiles, static (productionContext, input) => input.Emit(productionContext));

        var localeRegistryInput = localeCatalog
            .Combine(inflectionRegistry)
            .Select(static (input, _) => LocaleRegistryInput.Create(
                input.Left,
                input.Right))
            .WithTrackingName("LocaleRegistrySource");

        context.RegisterSourceOutput(localeRegistryInput, static (productionContext, input) => input.Emit(productionContext));

        var ordinalDateProfiles = localeCatalog
            .Select(static (catalog, _) => OrdinalDateProfileCatalogInput.Create(catalog));

        context.RegisterSourceOutput(ordinalDateProfiles, static (productionContext, input) => input.Emit(productionContext));

        var timeOnlyProfiles = localeCatalog
            .Select(static (catalog, _) => TimeOnlyToClockNotationProfileCatalogInput.Create(catalog));

        context.RegisterSourceOutput(timeOnlyProfiles, static (productionContext, input) => input.Emit(productionContext));

        var wordsToNumberProfiles = localeCatalog
            .Select(static (catalog, _) => WordsToNumberProfileCatalogInput.Create(catalog));

        context.RegisterSourceOutput(wordsToNumberProfiles, static (productionContext, input) => input.Emit(productionContext));
    }
}