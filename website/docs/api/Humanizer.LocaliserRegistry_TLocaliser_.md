## LocaliserRegistry\<TLocaliser\> Class

A registry of localised system components with their associated locales

```csharp
public class LocaliserRegistry<TLocaliser>
    where TLocaliser : class
```
#### Type parameters

<a name='Humanizer.LocaliserRegistry_TLocaliser_.TLocaliser'></a>

`TLocaliser`

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → LocaliserRegistry\<TLocaliser\>
### Constructors

<a name='Humanizer.LocaliserRegistry_TLocaliser_.LocaliserRegistry(System.Func_System.Globalization.CultureInfo,TLocaliser_)'></a>

## LocaliserRegistry\(Func\<CultureInfo,TLocaliser\>\) Constructor

Creates a localiser registry with the default localiser factory set to the provided value

```csharp
public LocaliserRegistry(System.Func<System.Globalization.CultureInfo,TLocaliser> defaultLocaliser);
```
#### Parameters

<a name='Humanizer.LocaliserRegistry_TLocaliser_.LocaliserRegistry(System.Func_System.Globalization.CultureInfo,TLocaliser_).defaultLocaliser'></a>

`defaultLocaliser` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TLocaliser](Humanizer.LocaliserRegistry_TLocaliser_.md#Humanizer.LocaliserRegistry_TLocaliser_.TLocaliser 'Humanizer\.LocaliserRegistry\<TLocaliser\>\.TLocaliser')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

<a name='Humanizer.LocaliserRegistry_TLocaliser_.LocaliserRegistry(TLocaliser)'></a>

## LocaliserRegistry\(TLocaliser\) Constructor

Creates a localiser registry with the default localiser set to the provided value

```csharp
public LocaliserRegistry(TLocaliser defaultLocaliser);
```
#### Parameters

<a name='Humanizer.LocaliserRegistry_TLocaliser_.LocaliserRegistry(TLocaliser).defaultLocaliser'></a>

`defaultLocaliser` [TLocaliser](Humanizer.LocaliserRegistry_TLocaliser_.md#Humanizer.LocaliserRegistry_TLocaliser_.TLocaliser 'Humanizer\.LocaliserRegistry\<TLocaliser\>\.TLocaliser')
### Methods

<a name='Humanizer.LocaliserRegistry_TLocaliser_.Register(string,System.Func_System.Globalization.CultureInfo,TLocaliser_)'></a>

## LocaliserRegistry\<TLocaliser\>\.Register\(string, Func\<CultureInfo,TLocaliser\>\) Method

Registers the localiser factory for the culture provided

```csharp
public void Register(string localeCode, System.Func<System.Globalization.CultureInfo,TLocaliser> localiser);
```
#### Parameters

<a name='Humanizer.LocaliserRegistry_TLocaliser_.Register(string,System.Func_System.Globalization.CultureInfo,TLocaliser_).localeCode'></a>

`localeCode` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.LocaliserRegistry_TLocaliser_.Register(string,System.Func_System.Globalization.CultureInfo,TLocaliser_).localiser'></a>

`localiser` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[TLocaliser](Humanizer.LocaliserRegistry_TLocaliser_.md#Humanizer.LocaliserRegistry_TLocaliser_.TLocaliser 'Humanizer\.LocaliserRegistry\<TLocaliser\>\.TLocaliser')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

<a name='Humanizer.LocaliserRegistry_TLocaliser_.Register(string,TLocaliser)'></a>

## LocaliserRegistry\<TLocaliser\>\.Register\(string, TLocaliser\) Method

Registers the localiser for the culture provided

```csharp
public void Register(string localeCode, TLocaliser localiser);
```
#### Parameters

<a name='Humanizer.LocaliserRegistry_TLocaliser_.Register(string,TLocaliser).localeCode'></a>

`localeCode` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.LocaliserRegistry_TLocaliser_.Register(string,TLocaliser).localiser'></a>

`localiser` [TLocaliser](Humanizer.LocaliserRegistry_TLocaliser_.md#Humanizer.LocaliserRegistry_TLocaliser_.TLocaliser 'Humanizer\.LocaliserRegistry\<TLocaliser\>\.TLocaliser')

<a name='Humanizer.LocaliserRegistry_TLocaliser_.ResolveForCulture(System.Globalization.CultureInfo)'></a>

## LocaliserRegistry\<TLocaliser\>\.ResolveForCulture\(CultureInfo\) Method

Gets the localiser for the specified culture

```csharp
public TLocaliser ResolveForCulture(System.Globalization.CultureInfo? culture);
```
#### Parameters

<a name='Humanizer.LocaliserRegistry_TLocaliser_.ResolveForCulture(System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture to retrieve localiser for\. If not specified, current thread's culture is used\.

#### Returns
[TLocaliser](Humanizer.LocaliserRegistry_TLocaliser_.md#Humanizer.LocaliserRegistry_TLocaliser_.TLocaliser 'Humanizer\.LocaliserRegistry\<TLocaliser\>\.TLocaliser')

<a name='Humanizer.LocaliserRegistry_TLocaliser_.ResolveForUiCulture()'></a>

## LocaliserRegistry\<TLocaliser\>\.ResolveForUiCulture\(\) Method

Gets the localiser for the current thread's UI culture

```csharp
public TLocaliser ResolveForUiCulture();
```

#### Returns
[TLocaliser](Humanizer.LocaliserRegistry_TLocaliser_.md#Humanizer.LocaliserRegistry_TLocaliser_.TLocaliser 'Humanizer\.LocaliserRegistry\<TLocaliser\>\.TLocaliser')