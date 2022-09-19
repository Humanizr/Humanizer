## `Configurator`

Provides a configuration point for Humanizer
```csharp
public static class Humanizer.Configuration.Configurator

```

Static Properties

| Type | Name | Summary | 
| --- | --- | --- | 
| `ICollectionFormatter` | CollectionFormatter |  | 
| `LocaliserRegistry<ICollectionFormatter>` | CollectionFormatters | A registry of formatters used to format collections based on the current locale | 
| `IDateTimeHumanizeStrategy` | DateTimeHumanizeStrategy | The strategy to be used for DateTime.Humanize | 
| `IDateTimeOffsetHumanizeStrategy` | DateTimeOffsetHumanizeStrategy | The strategy to be used for DateTimeOffset.Humanize | 
| `IDateToOrdinalWordConverter` | DateToOrdinalWordsConverter | The ordinalizer to be used | 
| `LocaliserRegistry<IDateToOrdinalWordConverter>` | DateToOrdinalWordsConverters | A registry of ordinalizers used to localise Ordinalize method | 
| `Func<PropertyInfo, Boolean>` | EnumDescriptionPropertyLocator | A predicate function for description property of attribute to use for Enum.Humanize | 
| `LocaliserRegistry<IFormatter>` | Formatters | A registry of formatters used to format strings based on the current locale | 
| `LocaliserRegistry<INumberToWordsConverter>` | NumberToWordsConverters | A registry of number to words converters used to localise ToWords and ToOrdinalWords methods | 
| `IOrdinalizer` | Ordinalizer | The ordinalizer to be used | 
| `LocaliserRegistry<IOrdinalizer>` | Ordinalizers | A registry of ordinalizers used to localise Ordinalize method | 


Static Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `IFormatter` | GetFormatter(`CultureInfo` culture) | The formatter to be used | 
| `INumberToWordsConverter` | GetNumberToWordsConverter(`CultureInfo` culture) | The converter to be used | 


## `LocaliserRegistry<TLocaliser>`

A registry of localised system components with their associated locales
```csharp
public class Humanizer.Configuration.LocaliserRegistry<TLocaliser>

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `void` | Register(`String` localeCode, `TLocaliser` localiser) | Registers the localiser for the culture provided | 
| `void` | Register(`String` localeCode, `Func<CultureInfo, TLocaliser>` localiser) | Registers the localiser for the culture provided | 
| `TLocaliser` | ResolveForCulture(`CultureInfo` culture) | Gets the localiser for the specified culture | 
| `TLocaliser` | ResolveForUiCulture() | Gets the localiser for the current thread's UI culture | 


