using Microsoft.CodeAnalysis;

namespace Humanizer.SourceGenerators;

[Generator]
public sealed partial class HumanizerSourceGenerator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var additionalFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => GeneratorInput.Create(additionalText, cancellationToken))
            .Where(static input => input is not null);

        context.RegisterSourceOutput(additionalFiles, static (productionContext, input) =>
        {
            if (input is null)
            {
                return;
            }

            input.Emit(productionContext);
        });

        var tokenMapFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => TokenMapLocaleFile.Create(additionalText, cancellationToken))
            .Where(static file => file is not null)
            .Collect()
            .Select(static (files, _) => TokenMapWordsToNumberInput.Create(files));

        context.RegisterSourceOutput(tokenMapFiles, static (productionContext, input) => input.Emit(productionContext));

        var localeFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => LocaleDefinitionFile.Create(additionalText, cancellationToken))
            .Where(static file => file is not null)
            .Collect();

        var formatterProfileFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => JsonProfileFile.Create(additionalText, cancellationToken, "\\CodeGen\\Profiles\\Formatters\\"))
            .Where(static file => file is not null)
            .Collect()
            .Select(static (files, _) => FormatterProfileCatalogInput.Create(files));

        context.RegisterSourceOutput(formatterProfileFiles, static (productionContext, input) => input.Emit(productionContext));

        var numberToWordsProfileFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => JsonProfileFile.Create(additionalText, cancellationToken, "\\CodeGen\\Profiles\\NumberToWords\\"))
            .Where(static file => file is not null)
            .Collect();

        var numberToWordsSchemaFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => JsonSchemaFile.Create(additionalText, cancellationToken, "\\CodeGen\\Schemas\\NumberToWords\\"))
            .Where(static file => file is not null)
            .Collect();

        var numberToWordsProfiles = numberToWordsProfileFiles
            .Combine(numberToWordsSchemaFiles)
            .Select(static (input, _) => NumberToWordsProfileCatalogInput.Create(input.Left, input.Right));

        context.RegisterSourceOutput(numberToWordsProfiles, static (productionContext, input) => input.Emit(productionContext));

        var ordinalizerProfileFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => JsonProfileFile.Create(additionalText, cancellationToken, "\\CodeGen\\Profiles\\Ordinalizers\\"))
            .Where(static file => file is not null)
            .Collect()
            .Select(static (files, _) => OrdinalizerProfileCatalogInput.Create(files));

        context.RegisterSourceOutput(ordinalizerProfileFiles, static (productionContext, input) => input.Emit(productionContext));

        var localeRegistryInput = localeFiles
            .Combine(formatterProfileFiles)
            .Combine(ordinalizerProfileFiles)
            .Select(static (input, _) => LocaleRegistryInput.Create(
                input.Left.Left,
                input.Left.Right.DataBackedProfileNames,
                input.Right.DataBackedProfileNames));

        context.RegisterSourceOutput(localeRegistryInput, static (productionContext, input) => input.Emit(productionContext));

        var ordinalDateProfileFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => JsonProfileFile.Create(additionalText, cancellationToken, "\\CodeGen\\Profiles\\OrdinalDates\\"))
            .Where(static file => file is not null)
            .Collect()
            .Select(static (files, _) => OrdinalDateProfileCatalogInput.Create(files));

        context.RegisterSourceOutput(ordinalDateProfileFiles, static (productionContext, input) => input.Emit(productionContext));

        var timeOnlyProfileFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => JsonProfileFile.Create(additionalText, cancellationToken, "\\CodeGen\\Profiles\\TimeOnlyToClockNotation\\"))
            .Where(static file => file is not null)
            .Collect();

        var timeOnlySchemaFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => JsonSchemaFile.Create(additionalText, cancellationToken, "\\CodeGen\\Schemas\\TimeOnlyToClockNotation\\"))
            .Where(static file => file is not null)
            .Collect();

        var timeOnlyProfiles = timeOnlyProfileFiles
            .Combine(timeOnlySchemaFiles)
            .Select(static (input, _) => TimeOnlyToClockNotationProfileCatalogInput.Create(input.Left, input.Right));

        context.RegisterSourceOutput(timeOnlyProfiles, static (productionContext, input) => input.Emit(productionContext));

        var wordsToNumberProfileFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => JsonProfileFile.Create(additionalText, cancellationToken, "\\CodeGen\\Profiles\\WordsToNumber\\"))
            .Where(static file => file is not null)
            .Collect();

        var wordsToNumberSchemaFiles = context.AdditionalTextsProvider
            .Select(static (additionalText, cancellationToken) => JsonSchemaFile.Create(additionalText, cancellationToken, "\\CodeGen\\Schemas\\WordsToNumber\\"))
            .Where(static file => file is not null)
            .Collect();

        var wordsToNumberProfiles = wordsToNumberProfileFiles
            .Combine(wordsToNumberSchemaFiles)
            .Select(static (input, _) => WordsToNumberProfileCatalogInput.Create(input.Left, input.Right));

        context.RegisterSourceOutput(wordsToNumberProfiles, static (productionContext, input) => input.Emit(productionContext));
    }
}
