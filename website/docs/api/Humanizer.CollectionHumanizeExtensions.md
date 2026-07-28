## CollectionHumanizeExtensions Class

Humanizes an IEnumerable into a human readable list

```csharp
public static class CollectionHumanizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → CollectionHumanizeExtensions
### Methods

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>\) Method

Transforms a collection into a human\-readable string representation by calling [System\.Object\.ToString](https://learn.microsoft.com/en-us/dotnet/api/system.object.tostring 'System\.Object\.ToString') 
on each element and combining them with the default separator for the current culture \(typically ", " with 
"and" before the last item\)\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_).T'></a>

`T`

The type of elements in the collection\.
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to be humanized\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A formatted string representation of the collection elements separated by culture\-specific separators\.
For English, this typically produces: "item1, item2 and item3"\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [collection](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_).collection 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>\)\.collection') is null\.

### Example

```csharp
new[] { 1, 2, 3 }.Humanize() => "1, 2 and 3"
new[] { "Alice", "Bob", "Charlie" }.Humanize() => "Alice, Bob and Charlie"
new[] { "single" }.Humanize() => "single"
new string[] { }.Humanize() => ""
```

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, string\) Method

Transforms a collection into a string representation by calling [System\.Object\.ToString](https://learn.microsoft.com/en-us/dotnet/api/system.object.tostring 'System\.Object\.ToString')
on each element and combining them with the specified separator\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, string separator);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string).T'></a>

`T`

The type of elements in the collection\.
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to be humanized\. Must not be null\.

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to use as a separator between elements\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string representation of the collection elements separated by the specified separator\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [collection](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,string).collection 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, string\)\.collection') is null\.

### Example

```csharp
new[] { 1, 2, 3 }.Humanize(" | ") => "1 | 2 | 3"
new[] { "Alice", "Bob" }.Humanize("; ") => "Alice; Bob"
```

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, Func\<T,object\>\) Method

Transforms a collection into a human\-readable string representation using a custom formatter function
that returns an object for each element \(which will be converted to string\), combined with the default 
separator for the current culture\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T,object> displayFormatter);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T'></a>

`T`

The type of elements in the collection\.
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to be humanized\. Must not be null\.

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).displayFormatter'></a>

`displayFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that converts each element of type [T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.T') to an object that will be 
converted to its string representation\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A formatted string representation of the collection elements, where each element is formatted
using [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.displayFormatter') and separated by culture\-specific separators\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [collection](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).collection 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.collection') or [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>\)\.displayFormatter') is null\.

### Example

```csharp
var numbers = new[] { 1, 2, 3 };
numbers.Humanize(n => n * 2) => "2, 4 and 6"
```

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, Func\<T,object\>, string\) Method

Transforms a collection into a string representation using a custom formatter function
that returns an object for each element \(which will be converted to string\), combined with the specified separator\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T,object> displayFormatter, string separator);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T'></a>

`T`

The type of elements in the collection\.
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to be humanized\. Must not be null\.

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).displayFormatter'></a>

`displayFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that converts each element of type [T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.T') to an object that will be
converted to its string representation\. Must not be null\.

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to use as a separator between formatted elements\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string representation of the collection elements, where each element is formatted
using [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.displayFormatter') and separated by the specified separator\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [collection](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).collection 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.collection') or [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,object_,string).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,object\>, string\)\.displayFormatter') is null\.

### Example

```csharp
var numbers = new[] { 1, 2, 3 };
numbers.Humanize(n => n * 2, " - ") => "2 - 4 - 6"
```

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, Func\<T,string\>\) Method

Transforms a collection into a human\-readable string representation using a custom formatter function
for each element, combined with the default separator for the current culture\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T,string> displayFormatter);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T'></a>

`T`

The type of elements in the collection\.
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to be humanized\. Must not be null\.

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).displayFormatter'></a>

`displayFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that converts each element of type [T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.T') to a string representation\.
Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A formatted string representation of the collection elements, where each element is formatted
using [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.displayFormatter') and separated by culture\-specific separators\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [collection](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).collection 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.collection') or [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>\)\.displayFormatter') is null\.

### Example

```csharp
var people = new[] { new Person { Name = "Alice", Age = 30 }, new Person { Name = "Bob", Age = 25 } };
people.Humanize(p => p.Name) => "Alice and Bob"
people.Humanize(p => $"{p.Name} ({p.Age})") => "Alice (30) and Bob (25)"
```

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, Func\<T,string\>, string\) Method

Transforms a collection into a string representation using a custom formatter function
for each element, combined with the specified separator\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Func<T,string> displayFormatter, string separator);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T'></a>

`T`

The type of elements in the collection\.
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to be humanized\. Must not be null\.

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).displayFormatter'></a>

`displayFormatter` [System\.Func&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.T')[,](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.func-2 'System\.Func\`2')

A function that converts each element of type [T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.T') to a string representation\.
Must not be null\.

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).separator'></a>

`separator` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

The string to use as a separator between formatted elements\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A string representation of the collection elements, where each element is formatted
using [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.displayFormatter') and separated by the specified separator\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [collection](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).collection 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.collection') or [displayFormatter](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Func_T,string_,string).displayFormatter 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Func\<T,string\>, string\)\.displayFormatter') is null\.

### Example

```csharp
var people = new[] { new Person { Name = "Alice" }, new Person { Name = "Bob" } };
people.Humanize(p => p.Name, " | ") => "Alice | Bob"
```

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Globalization.CultureInfo)'></a>

## CollectionHumanizeExtensions\.Humanize\<T\>\(this IEnumerable\<T\>, CultureInfo\) Method

Transforms a collection into a human\-readable string representation using the default separator
for the specified culture\.

```csharp
public static string Humanize<T>(this System.Collections.Generic.IEnumerable<T> collection, System.Globalization.CultureInfo culture);
```
#### Type parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Globalization.CultureInfo).T'></a>

`T`

The type of elements in the collection\.
#### Parameters

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Globalization.CultureInfo).collection'></a>

`collection` [System\.Collections\.Generic\.IEnumerable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')[T](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Globalization.CultureInfo).T 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Globalization\.CultureInfo\)\.T')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1 'System\.Collections\.Generic\.IEnumerable\`1')

The collection to be humanized\. Must not be null\.

<a name='Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Globalization.CultureInfo).culture'></a>

`culture` [System\.Globalization\.CultureInfo](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo 'System\.Globalization\.CultureInfo')

The culture whose collection formatter should be used\. Must not be null\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A formatted string representation of the collection elements separated by culture\-specific separators\.

#### Exceptions

[System\.ArgumentNullException](https://learn.microsoft.com/en-us/dotnet/api/system.argumentnullexception 'System\.ArgumentNullException')  
Thrown when [collection](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Globalization.CultureInfo).collection 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Globalization\.CultureInfo\)\.collection') or [culture](Humanizer.CollectionHumanizeExtensions.md#Humanizer.CollectionHumanizeExtensions.Humanize_T_(thisSystem.Collections.Generic.IEnumerable_T_,System.Globalization.CultureInfo).culture 'Humanizer\.CollectionHumanizeExtensions\.Humanize\<T\>\(this System\.Collections\.Generic\.IEnumerable\<T\>, System\.Globalization\.CultureInfo\)\.culture') is null\.

### Example

```csharp
new[] { 1, 2, 3 }.Humanize(new CultureInfo("en-GB")) => "1, 2 and 3"
```