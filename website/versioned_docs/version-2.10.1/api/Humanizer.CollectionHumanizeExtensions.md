## CollectionHumanizeExtensions Class

Humanizes an IEnumerable into a human readable list

```csharp
public static class CollectionHumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → CollectionHumanizeExtensions
### Methods

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>\) Method

Formats the collection for display, calling ToString\(\) on each object and
using the default separator for the current culture\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_).T'></a>

`T`
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, string\) Method

Formats the collection for display, calling ToString\(\) on each object
and using the provided separator\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, string separator);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string).T'></a>

`T`
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, Func\<T,object\>\) Method

Formats the collection for display, calling [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.displayFormatter') on each element
and using the default separator for the current culture\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T,object> displayFormatter);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T'></a>

`T`
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).displayFormatter'></a>

`displayFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, Func\<T,object\>, string\) Method

Formats the collection for display, calling [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.displayFormatter') on each element
and using the provided separator\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T,object> displayFormatter, string separator);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T'></a>

`T`
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).displayFormatter'></a>

`displayFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, Func\<T,string\>\) Method

Formats the collection for display, calling [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.displayFormatter') on each element
and using the default separator for the current culture\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T,string> displayFormatter);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T'></a>

`T`
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).displayFormatter'></a>

`displayFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, Func\<T,string\>, string\) Method

Formats the collection for display, calling [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.displayFormatter') on each element
and using the provided separator\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T,string> displayFormatter, string separator);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T'></a>

`T`
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).displayFormatter'></a>

`displayFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')