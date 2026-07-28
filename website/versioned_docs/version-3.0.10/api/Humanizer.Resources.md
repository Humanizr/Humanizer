## Resources Class

Provides access to the resources of Humanizer

```csharp
public static class Resources
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → Resources
### Methods

<a name='Humanizer.Resources.GetResource(string,System.Globalization.CultureInfo)'></a>

## Resources\.GetResource\(string, CultureInfo\) Method

Returns the value of the specified string resource

```csharp
public static string GetResource(string resourceKey, System.Globalization.CultureInfo? culture=null);
```
#### Parameters

<a name='Humanizer.Resources.GetResource(string,System.Globalization.CultureInfo).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the resource to retrieve\.

<a name='Humanizer.Resources.GetResource(string,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture of the resource to retrieve\. If not specified, current thread's UI culture is used\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The value of the resource localized for the specified culture\.

<a name='Humanizer.Resources.TryGetResource(string,System.Globalization.CultureInfo,string)'></a>

## Resources\.TryGetResource\(string, CultureInfo, string\) Method

Tries to get the value of the specified string resource, without fallback

```csharp
public static bool TryGetResource(string resourceKey, System.Globalization.CultureInfo? culture, out string? result);
```
#### Parameters

<a name='Humanizer.Resources.TryGetResource(string,System.Globalization.CultureInfo,string).resourceKey'></a>

`resourceKey` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The name of the resource to retrieve\.

<a name='Humanizer.Resources.TryGetResource(string,System.Globalization.CultureInfo,string).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture of the resource to retrieve\. If not specified, current thread's UI culture is used\.

<a name='Humanizer.Resources.TryGetResource(string,System.Globalization.CultureInfo,string).result'></a>

`result` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The value of the resource localized for the specified culture if found; null otherwise\.

#### Returns
[System\.Boolean](https://learn.microsoft.com/en-us/dotnet/api/system.boolean 'System\.Boolean')  
true if the specified string resource was found for the given culture; otherwise, false\.