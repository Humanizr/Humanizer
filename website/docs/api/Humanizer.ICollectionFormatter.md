## ICollectionFormatter Interface

Formats collections into localized, human\-readable lists\.

```csharp
public interface ICollectionFormatter
```

### Remarks
Built\-in implementations ignore rendered items that are `null`, empty, or whitespace after
formatting, and they may trim the surviving values before joining them\. They also expect the
input collection and any formatter delegate to be non\-`null`\.
### Methods

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>\) Method

Formats the collection for display by calling `ToString()` on each item\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_).T'></a>

`T`

The item type in the collection\.
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to format\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The formatted collection\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
If [collection](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_).collection 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>\)\.collection') is `null`\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, string\) Method

Formats the collection for display by calling `ToString()` on each item and using
[separator](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).separator 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, string\)\.separator') to join the rendered items\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, string separator);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).T'></a>

`T`

The item type in the collection\.
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to format\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The localized separator used to join the rendered items\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The formatted collection\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
If [collection](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,string).collection 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, string\)\.collection') is `null`\.

### Remarks
Implementations may place the separator before the final item or between every item depending on the list style\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, Func\<T,object\>\) Method

Formats the collection for display by calling [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.objectFormatter') on each item\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T,object?> objectFormatter);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T'></a>

`T`

The item type in the collection\.
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to format\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).objectFormatter'></a>

`objectFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

The formatter used to convert each item into text\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The formatted collection\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
If [collection](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).collection 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.collection') or [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.objectFormatter') is `null`\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, Func\<T,object\>, string\) Method

Formats the collection for display by calling [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.objectFormatter') on each item and
using [separator](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).separator 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.separator') to join the rendered items\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T,object?> objectFormatter, string separator);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T'></a>

`T`

The item type in the collection\.
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to format\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).objectFormatter'></a>

`objectFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

The formatter used to convert each item into text\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The localized separator used to join the rendered items\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The formatted collection\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
If [collection](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).collection 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.collection') or [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.objectFormatter') is `null`\.

### Remarks
Implementations may place the separator before the final item or between every item depending on the list style\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, Func\<T,string\>\) Method

Formats the collection for display by calling [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.objectFormatter') on each item\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T,string?> objectFormatter);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T'></a>

`T`

The item type in the collection\.
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to format\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).objectFormatter'></a>

`objectFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

The formatter used to convert each item into text\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The formatted collection\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
If [collection](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).collection 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.collection') or [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.objectFormatter') is `null`\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string)'></a>

## ICollectionFormatter\.Humanize\<T\>\(IEnumerable\<T\>, Func\<T,string\>, string\) Method

Formats the collection for display by calling [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.objectFormatter') on each item and
using [separator](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).separator 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.separator') to join the rendered items\.

```csharp
string Humanize<T>(System.Collections.Generic.IEnumerable<T> collection, System.Func<T,string?> objectFormatter, string separator);
```
#### Type parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T'></a>

`T`

The item type in the collection\.
#### Parameters

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to format\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).objectFormatter'></a>

`objectFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

The formatter used to convert each item into text\.

<a name='Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The localized separator used to join the rendered items\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
The formatted collection\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
If [collection](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).collection 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.collection') or [objectFormatter](Humanizer.ICollectionFormatter.md#Humanizer.ICollectionFormatter.Humanize_T_(System.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).objectFormatter 'Humanizer\.ICollectionFormatter\.Humanize\<T\>\(System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.objectFormatter') is `null`\.

### Remarks
Implementations may place the separator before the final item or between every item depending on the list style\.