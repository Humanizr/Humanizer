## Configurator Class

Provides a configuration point for Humanizer

```csharp
public static class Configurator
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Configurator
### Properties

<a name='Humanizer.Configurator.CollectionFormatters'></a>

## Configurator\.CollectionFormatters Property

A registry of formatters used to format collections based on the current locale

```csharp
public static Humanizer.LocaliserRegistry<Humanizer.ICollectionFormatter> CollectionFormatters { get; }
```

#### Property Value
[Humanizer\.LocaliserRegistry&lt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')[ICollectionFormatter](Humanizer.ICollectionFormatter.md 'Humanizer\.ICollectionFormatter')[&gt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configurator.DateOnlyHumanizeStrategy'></a>

## Configurator\.DateOnlyHumanizeStrategy Property

The strategy to be used for DateOnly\.Humanize

```csharp
public static Humanizer.IDateOnlyHumanizeStrategy DateOnlyHumanizeStrategy { get; set; }
```

#### Property Value
[IDateOnlyHumanizeStrategy](Humanizer.IDateOnlyHumanizeStrategy.md 'Humanizer\.IDateOnlyHumanizeStrategy')

### Remarks
This property should be set only once during application startup before any humanization operations occur\.
For thread\-safety, use volatile reads or appropriate synchronization when accessing this property in multi\-threaded scenarios\.
In production applications, avoid changing this value after the application has started serving requests\.

<a name='Humanizer.Configurator.DateOnlyToOrdinalWordsConverters'></a>

## Configurator\.DateOnlyToOrdinalWordsConverters Property

A registry of ordinalizers used to localise Ordinalize method

```csharp
public static Humanizer.LocaliserRegistry<Humanizer.IDateOnlyToOrdinalWordConverter> DateOnlyToOrdinalWordsConverters { get; }
```

#### Property Value
[Humanizer\.LocaliserRegistry&lt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')[IDateOnlyToOrdinalWordConverter](Humanizer.IDateOnlyToOrdinalWordConverter.md 'Humanizer\.IDateOnlyToOrdinalWordConverter')[&gt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configurator.DateTimeHumanizeStrategy'></a>

## Configurator\.DateTimeHumanizeStrategy Property

The strategy to be used for DateTime\.Humanize

```csharp
public static Humanizer.IDateTimeHumanizeStrategy DateTimeHumanizeStrategy { get; set; }
```

#### Property Value
[IDateTimeHumanizeStrategy](Humanizer.IDateTimeHumanizeStrategy.md 'Humanizer\.IDateTimeHumanizeStrategy')

### Remarks
This property should be set only once during application startup before any humanization operations occur\.
For thread\-safety, use volatile reads or appropriate synchronization when accessing this property in multi\-threaded scenarios\.
In production applications, avoid changing this value after the application has started serving requests\.

<a name='Humanizer.Configurator.DateTimeOffsetHumanizeStrategy'></a>

## Configurator\.DateTimeOffsetHumanizeStrategy Property

The strategy to be used for DateTimeOffset\.Humanize

```csharp
public static Humanizer.IDateTimeOffsetHumanizeStrategy DateTimeOffsetHumanizeStrategy { get; set; }
```

#### Property Value
[IDateTimeOffsetHumanizeStrategy](Humanizer.IDateTimeOffsetHumanizeStrategy.md 'Humanizer\.IDateTimeOffsetHumanizeStrategy')

### Remarks
This property should be set only once during application startup before any humanization operations occur\.
For thread\-safety, use volatile reads or appropriate synchronization when accessing this property in multi\-threaded scenarios\.
In production applications, avoid changing this value after the application has started serving requests\.

<a name='Humanizer.Configurator.DateToOrdinalWordsConverters'></a>

## Configurator\.DateToOrdinalWordsConverters Property

A registry of ordinalizers used to localise Ordinalize method

```csharp
public static Humanizer.LocaliserRegistry<Humanizer.IDateToOrdinalWordConverter> DateToOrdinalWordsConverters { get; }
```

#### Property Value
[Humanizer\.LocaliserRegistry&lt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')[IDateToOrdinalWordConverter](Humanizer.IDateToOrdinalWordConverter.md 'Humanizer\.IDateToOrdinalWordConverter')[&gt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configurator.Formatters'></a>

## Configurator\.Formatters Property

A registry of formatters used to format strings based on the current locale

```csharp
public static Humanizer.LocaliserRegistry<Humanizer.IFormatter> Formatters { get; }
```

#### Property Value
[Humanizer\.LocaliserRegistry&lt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')[IFormatter](Humanizer.IFormatter.md 'Humanizer\.IFormatter')[&gt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configurator.NumberToWordsConverters'></a>

## Configurator\.NumberToWordsConverters Property

A registry of number to words converters used to localise ToWords and ToOrdinalWords methods

```csharp
public static Humanizer.LocaliserRegistry<Humanizer.INumberToWordsConverter> NumberToWordsConverters { get; }
```

#### Property Value
[Humanizer\.LocaliserRegistry&lt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')[INumberToWordsConverter](Humanizer.INumberToWordsConverter.md 'Humanizer\.INumberToWordsConverter')[&gt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configurator.Ordinalizers'></a>

## Configurator\.Ordinalizers Property

A registry of ordinalizers used to localise Ordinalize method

```csharp
public static Humanizer.LocaliserRegistry<Humanizer.IOrdinalizer> Ordinalizers { get; }
```

#### Property Value
[Humanizer\.LocaliserRegistry&lt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')[IOrdinalizer](Humanizer.IOrdinalizer.md 'Humanizer\.IOrdinalizer')[&gt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configurator.TimeOnlyHumanizeStrategy'></a>

## Configurator\.TimeOnlyHumanizeStrategy Property

The strategy to be used for TimeOnly\.Humanize

```csharp
public static Humanizer.ITimeOnlyHumanizeStrategy TimeOnlyHumanizeStrategy { get; set; }
```

#### Property Value
[ITimeOnlyHumanizeStrategy](Humanizer.ITimeOnlyHumanizeStrategy.md 'Humanizer\.ITimeOnlyHumanizeStrategy')

### Remarks
This property should be set only once during application startup before any humanization operations occur\.
For thread\-safety, use volatile reads or appropriate synchronization when accessing this property in multi\-threaded scenarios\.
In production applications, avoid changing this value after the application has started serving requests\.

<a name='Humanizer.Configurator.TimeOnlyToClockNotationConverters'></a>

## Configurator\.TimeOnlyToClockNotationConverters Property

A registry of time to clock notation converters used to localise ToClockNotation methods

```csharp
public static Humanizer.LocaliserRegistry<Humanizer.ITimeOnlyToClockNotationConverter> TimeOnlyToClockNotationConverters { get; }
```

#### Property Value
[Humanizer\.LocaliserRegistry&lt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')[ITimeOnlyToClockNotationConverter](Humanizer.ITimeOnlyToClockNotationConverter.md 'Humanizer\.ITimeOnlyToClockNotationConverter')[&gt;](Humanizer.LocaliserRegistry_TLocaliser_.md 'Humanizer\.LocaliserRegistry\<TLocaliser\>')

<a name='Humanizer.Configurator.TimeSpanHumanizeStrategy'></a>

## Configurator\.TimeSpanHumanizeStrategy Property

The strategy used by the [System\.TimeSpan](https://learn.microsoft.com/en-us/dotnet/api/system.timespan 'System\.TimeSpan') humanization extension methods\.

```csharp
public static Humanizer.ITimeSpanHumanizeStrategy TimeSpanHumanizeStrategy { get; set; }
```

#### Property Value
[ITimeSpanHumanizeStrategy](Humanizer.ITimeSpanHumanizeStrategy.md 'Humanizer\.ITimeSpanHumanizeStrategy')

### Remarks
This property should be set only once during application startup before any humanization operations occur\.
For thread\-safety, use volatile reads or appropriate synchronization when accessing this property in multi\-threaded scenarios\.
In production applications, avoid changing this value after the application has started serving requests\.
### Methods

<a name='Humanizer.Configurator.IsCultureSupported(System.Globalization.CultureInfo)'></a>

## Configurator\.IsCultureSupported\(CultureInfo\) Method

Determines whether Humanizer includes complete generated locale support for the specified culture\.

```csharp
public static bool IsCultureSupported(System.Globalization.CultureInfo culture);
```
#### Parameters

<a name='Humanizer.Configurator.IsCultureSupported(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to check\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
[true](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool') when the culture or one of its named parents has generated locale support; otherwise, [false](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/bool 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/builtin\-types/bool')\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [culture](Humanizer.Configurator.md#Humanizer.Configurator.IsCultureSupported(System.Globalization.CultureInfo).culture 'Humanizer\.Configurator\.IsCultureSupported\(System\.Globalization\.CultureInfo\)\.culture') is `null`\.

### Remarks
This considers parent\-culture fallback, but does not report support merely because Humanizer can fall back to its default English localizers\.

<a name='Humanizer.Configurator.UseEnumDescriptionPropertyLocator(System.Func_System.Reflection.PropertyInfo,bool_)'></a>

## Configurator\.UseEnumDescriptionPropertyLocator\(Func\<PropertyInfo,bool\>\) Method

Use a predicate function for description property of attribute to use for Enum\.Humanize

```csharp
public static void UseEnumDescriptionPropertyLocator(System.Func<System.Reflection.PropertyInfo,bool> func);
```
#### Parameters

<a name='Humanizer.Configurator.UseEnumDescriptionPropertyLocator(System.Func_System.Reflection.PropertyInfo,bool_).func'></a>

`func` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Reflection\.PropertyInfo](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo 'System\.Reflection\.PropertyInfo')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')