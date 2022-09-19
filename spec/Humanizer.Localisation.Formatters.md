## `DefaultFormatter`

Default implementation of IFormatter interface.
```csharp
public class Humanizer.Localisation.Formatters.DefaultFormatter
    : IFormatter

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | DataUnitHumanize(`DataUnit` dataUnit, `Double` count, `Boolean` toSymbol = True) |  | 
| `String` | DateHumanize(`TimeUnit` timeUnit, `Tense` timeUnitTense, `Int32` unit) | Returns the string representation of the provided DateTime | 
| `String` | DateHumanize_Never() | Never | 
| `String` | DateHumanize_Now() | Now | 
| `String` | Format(`String` resourceKey) | Formats the specified resource key. | 
| `String` | Format(`String` resourceKey, `Int32` number, `Boolean` toWords = False) | Formats the specified resource key. | 
| `String` | GetResourceKey(`String` resourceKey, `Int32` number) | Override this method if your locale has complex rules around multiple units; e.g. Arabic, Russian | 
| `String` | GetResourceKey(`String` resourceKey) | Override this method if your locale has complex rules around multiple units; e.g. Arabic, Russian | 
| `String` | TimeSpanHumanize(`TimeUnit` timeUnit, `Int32` unit, `Boolean` toWords = False) | Returns the string representation of the provided TimeSpan | 
| `String` | TimeSpanHumanize_Zero() | 0 seconds | 
| `String` | TimeUnitHumanize(`TimeUnit` timeUnit) |  | 


## `IFormatter`

Implement this interface if your language has complex rules around dealing with numbers.  For example in Romanian "5 days" is "5 zile", while "24 days" is "24 de zile" and  in Arabic 2 days is يومين not 2 يوم
```csharp
public interface Humanizer.Localisation.Formatters.IFormatter

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | DataUnitHumanize(`DataUnit` dataUnit, `Double` count, `Boolean` toSymbol = True) | Returns the string representation of the provided DataUnit, either as a symbol or full word | 
| `String` | DateHumanize(`TimeUnit` timeUnit, `Tense` timeUnitTense, `Int32` unit) | Returns the string representation of the provided DateTime | 
| `String` | DateHumanize_Never() | Never | 
| `String` | DateHumanize_Now() | Now | 
| `String` | TimeSpanHumanize(`TimeUnit` timeUnit, `Int32` unit, `Boolean` toWords = False) | Returns the string representation of the provided TimeSpan | 
| `String` | TimeSpanHumanize_Zero() | 0 seconds | 
| `String` | TimeUnitHumanize(`TimeUnit` timeUnit) | Returns the symbol for the given TimeUnit | 


