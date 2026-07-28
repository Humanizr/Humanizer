## Configurator Class

Provides a configuration point for Humanizer

```csharp
public static class Configurator
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Configurator
### Properties

<a name='Humanizer.Configuration.Configurator.CollectionFormatters'></a>

## Configurator\.CollectionFormatters Property

A registry of formatters used to format collections based on the current locale

```csharp
public static Humanizer.Configuration.LocaliserRegistry<Humanizer.Localisation.CollectionFormatters.ICollectionFormatter> CollectionFormatters { get; }
```

#### Property Value
[Humanizer\.Configuration\.LocaliserRegistry&lt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')[ICollectionFormatter](Humanizer.Localisation.CollectionFormatters.ICollectionFormatter.md 'Humanizer\.Localisation\.CollectionFormatters\.ICollectionFormatter')[&gt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configuration.Configurator.DateOnlyHumanizeStrategy'></a>

## Configurator\.DateOnlyHumanizeStrategy Property

The strategy to be used for DateOnly\.Humanize

```csharp
public static Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy DateOnlyHumanizeStrategy { get; set; }
```

#### Property Value
[IDateOnlyHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.IDateOnlyHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.IDateOnlyHumanizeStrategy')

<a name='Humanizer.Configuration.Configurator.DateOnlyToOrdinalWordsConverters'></a>

## Configurator\.DateOnlyToOrdinalWordsConverters Property

A registry of ordinalizers used to localise Ordinalize method

```csharp
public static Humanizer.Configuration.LocaliserRegistry<Humanizer.Localisation.DateToOrdinalWords.IDateOnlyToOrdinalWordConverter> DateOnlyToOrdinalWordsConverters { get; }
```

#### Property Value
[Humanizer\.Configuration\.LocaliserRegistry&lt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')[IDateOnlyToOrdinalWordConverter](Humanizer.Localisation.DateToOrdinalWords.IDateOnlyToOrdinalWordConverter.md 'Humanizer\.Localisation\.DateToOrdinalWords\.IDateOnlyToOrdinalWordConverter')[&gt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configuration.Configurator.DateTimeHumanizeStrategy'></a>

## Configurator\.DateTimeHumanizeStrategy Property

The strategy to be used for DateTime\.Humanize

```csharp
public static Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy DateTimeHumanizeStrategy { get; set; }
```

#### Property Value
[IDateTimeHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.IDateTimeHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.IDateTimeHumanizeStrategy')

<a name='Humanizer.Configuration.Configurator.DateTimeOffsetHumanizeStrategy'></a>

## Configurator\.DateTimeOffsetHumanizeStrategy Property

The strategy to be used for DateTimeOffset\.Humanize

```csharp
public static Humanizer.DateTimeHumanizeStrategy.IDateTimeOffsetHumanizeStrategy DateTimeOffsetHumanizeStrategy { get; set; }
```

#### Property Value
[IDateTimeOffsetHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.IDateTimeOffsetHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.IDateTimeOffsetHumanizeStrategy')

<a name='Humanizer.Configuration.Configurator.DateToOrdinalWordsConverters'></a>

## Configurator\.DateToOrdinalWordsConverters Property

A registry of ordinalizers used to localise Ordinalize method

```csharp
public static Humanizer.Configuration.LocaliserRegistry<Humanizer.Localisation.DateToOrdinalWords.IDateToOrdinalWordConverter> DateToOrdinalWordsConverters { get; }
```

#### Property Value
[Humanizer\.Configuration\.LocaliserRegistry&lt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')[IDateToOrdinalWordConverter](Humanizer.Localisation.DateToOrdinalWords.IDateToOrdinalWordConverter.md 'Humanizer\.Localisation\.DateToOrdinalWords\.IDateToOrdinalWordConverter')[&gt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configuration.Configurator.EnumDescriptionPropertyLocator'></a>

## Configurator\.EnumDescriptionPropertyLocator Property

A predicate function for description property of attribute to use for Enum\.Humanize

```csharp
public static System.Func<System.Reflection.PropertyInfo,bool> EnumDescriptionPropertyLocator { get; set; }
```

#### Property Value
[System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Reflection\.PropertyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo 'System\.Reflection\.PropertyInfo')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

<a name='Humanizer.Configuration.Configurator.Formatters'></a>

## Configurator\.Formatters Property

A registry of formatters used to format strings based on the current locale

```csharp
public static Humanizer.Configuration.LocaliserRegistry<Humanizer.Localisation.Formatters.IFormatter> Formatters { get; }
```

#### Property Value
[Humanizer\.Configuration\.LocaliserRegistry&lt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')[IFormatter](Humanizer.Localisation.Formatters.IFormatter.md 'Humanizer\.Localisation\.Formatters\.IFormatter')[&gt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configuration.Configurator.NumberToWordsConverters'></a>

## Configurator\.NumberToWordsConverters Property

A registry of number to words converters used to localise ToWords and ToOrdinalWords methods

```csharp
public static Humanizer.Configuration.LocaliserRegistry<Humanizer.Localisation.NumberToWords.INumberToWordsConverter> NumberToWordsConverters { get; }
```

#### Property Value
[Humanizer\.Configuration\.LocaliserRegistry&lt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')[INumberToWordsConverter](Humanizer.Localisation.NumberToWords.INumberToWordsConverter.md 'Humanizer\.Localisation\.NumberToWords\.INumberToWordsConverter')[&gt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configuration.Configurator.Ordinalizers'></a>

## Configurator\.Ordinalizers Property

A registry of ordinalizers used to localise Ordinalize method

```csharp
public static Humanizer.Configuration.LocaliserRegistry<Humanizer.Localisation.Ordinalizers.IOrdinalizer> Ordinalizers { get; }
```

#### Property Value
[Humanizer\.Configuration\.LocaliserRegistry&lt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')[IOrdinalizer](Humanizer.Localisation.Ordinalizers.IOrdinalizer.md 'Humanizer\.Localisation\.Ordinalizers\.IOrdinalizer')[&gt;](Humanizer.Configuration.LocaliserRegistry_TLocaliser_.md 'Humanizer\.Configuration\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configuration.Configurator.TimeOnlyHumanizeStrategy'></a>

## Configurator\.TimeOnlyHumanizeStrategy Property

The strategy to be used for TimeOnly\.Humanize

```csharp
public static Humanizer.DateTimeHumanizeStrategy.ITimeOnlyHumanizeStrategy TimeOnlyHumanizeStrategy { get; set; }
```

#### Property Value
[ITimeOnlyHumanizeStrategy](Humanizer.DateTimeHumanizeStrategy.ITimeOnlyHumanizeStrategy.md 'Humanizer\.DateTimeHumanizeStrategy\.ITimeOnlyHumanizeStrategy')