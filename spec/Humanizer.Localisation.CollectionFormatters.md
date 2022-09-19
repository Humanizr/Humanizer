## `ICollectionFormatter`

An interface you should implement to localize Humanize for collections
```csharp
public interface Humanizer.Localisation.CollectionFormatters.ICollectionFormatter

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Humanize(`IEnumerable<T>` collection) | Formats the collection for display, calling ToString() on each object. | 
| `String` | Humanize(`IEnumerable<T>` collection, `Func<T, String>` objectFormatter) | Formats the collection for display, calling ToString() on each object. | 
| `String` | Humanize(`IEnumerable<T>` collection, `Func<T, Object>` objectFormatter) | Formats the collection for display, calling ToString() on each object. | 
| `String` | Humanize(`IEnumerable<T>` collection, `String` separator) | Formats the collection for display, calling ToString() on each object. | 
| `String` | Humanize(`IEnumerable<T>` collection, `Func<T, String>` objectFormatter, `String` separator) | Formats the collection for display, calling ToString() on each object. | 
| `String` | Humanize(`IEnumerable<T>` collection, `Func<T, Object>` objectFormatter, `String` separator) | Formats the collection for display, calling ToString() on each object. | 


