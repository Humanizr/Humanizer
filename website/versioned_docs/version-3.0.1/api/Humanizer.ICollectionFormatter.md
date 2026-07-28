## ICollectionFormatter Interface

An interface you should implement to localize Humanize for collections

```csharp
public interface ICollectionFormatter
```
### Methods

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>\) Method

Formats the collection for display, calling ToString\(\) on each object\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_).T'></a>

`T`
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, string\) Method

Formats the collection for display, calling ToString\(\) on each object
and using [separator](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).separator 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, string\)\.separator') before the final item\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, string separator);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).T'></a>

`T`
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, Func\<T,object\>\) Method

Formats the collection for display, calling [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.objectFormatter') on each element\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T,object?> objectFormatter);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T'></a>

`T`
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).objectFormatter'></a>

`objectFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, Func\<T,object\>, string\) Method

Formats the collection for display, calling [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.objectFormatter') on each element\.
and using [separator](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).separator 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.separator') before the final item\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T,object?> objectFormatter, string separator);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T'></a>

`T`
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).objectFormatter'></a>

`objectFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, Func\<T,string\>\) Method

Formats the collection for display, calling [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.objectFormatter') on each element\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T,string?> objectFormatter);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T'></a>

`T`
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).objectFormatter'></a>

`objectFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, Func\<T,string\>, string\) Method

Formats the collection for display, calling [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.objectFormatter') on each element\.
and using [separator](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).separator 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.separator') before the final item\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T,string?> objectFormatter, string separator);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T'></a>

`T`
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).objectFormatter'></a>

`objectFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')