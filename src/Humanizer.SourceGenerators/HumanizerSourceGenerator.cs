using Microsoft.CodeAnalysis;

namespace Humanizer.SourceGenerators;

[Generator]
public sealed partial class HumanizerSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var localeFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => LocaleDefinitionFile.Create(additionalText, cancellationToken))
            .Where(static file => file is not null)
            .Collect();

        var localeCatalog = localeFiles
            .Select(static (files, _) => LocaleCatalogInput.Create(files));

        var localePhraseTables = localeCatalog
            .Select(static (catalog, _) => LocalePhraseTableCatalogInput.Create(catalog));

        context.RegisterSourceOutput(localePhraseTables, static (productionContext, input) => input.Emit(productionContext));

        var headingTables = localeCatalog
            .Select(static (catalog, _) => HeadingTableCatalogInput.Create(catalog));

        context.RegisterSourceOutput(headingTables, static (productionContext, input) => input.Emit(productionContext));

        var tokenMapLocales = localeCatalog
            .Select(static (catalog, _) => TokenMapWordsToNumberInput.Create(catalog));

        context.RegisterSourceOutput(tokenMapLocales, static (productionContext, input) => input.Emit(productionContext));

        var formatterProfiles = localeCatalog
            .Select(static (catalog, _) => FormatterProfileCatalogInput.Create(catalog));

        context.RegisterSourceOutput(formatterProfiles, static (productionContext, input) => input.Emit(productionContext));

        var numberToWordsProfiles = localeCatalog
            .Select(static (catalog, _) => NumberToWordsProfileCatalogInput.Create(catalog));

        context.RegisterSourceOutput(numberToWordsProfiles, static (productionContext, input) => input.Emit(productionContext));

        var ordinalizerProfiles = localeCatalog
            .Select(static (catalog, _) => OrdinalizerProfileCatalogInput.Create(catalog));

        context.RegisterSourceOutput(ordinalizerProfiles, static (productionContext, input) => input.Emit(productionContext));

        var localeRegistryInput = localeCatalog
            .Select(static (catalog, _) => LocaleRegistryInput.Create(catalog));

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
