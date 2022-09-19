## `INumberToWordsConverter`

An interface you should implement to localise ToWords and ToOrdinalWords methods
```csharp
public interface Humanizer.Localisation.NumberToWords.INumberToWordsConverter

```

Methods

| Type | Name | Summary | 
| --- | --- | --- | 
| `String` | Convert(`Int64` number) | Converts the number to string using the locale's default grammatical gender | 
| `String` | Convert(`Int64` number, `WordForm` wordForm) | Converts the number to string using the locale's default grammatical gender | 
| `String` | Convert(`Int64` number, `Boolean` addAnd) | Converts the number to string using the locale's default grammatical gender | 
| `String` | Convert(`Int64` number, `Boolean` addAnd, `WordForm` wordForm) | Converts the number to string using the locale's default grammatical gender | 
| `String` | Convert(`Int64` number, `GrammaticalGender` gender, `Boolean` addAnd = True) | Converts the number to string using the locale's default grammatical gender | 
| `String` | Convert(`Int64` number, `WordForm` wordForm, `GrammaticalGender` gender, `Boolean` addAnd = True) | Converts the number to string using the locale's default grammatical gender | 
| `String` | ConvertToOrdinal(`Int32` number) | Converts the number to ordinal string using the locale's default grammatical gender | 
| `String` | ConvertToOrdinal(`Int32` number, `WordForm` wordForm) | Converts the number to ordinal string using the locale's default grammatical gender | 
| `String` | ConvertToOrdinal(`Int32` number, `GrammaticalGender` gender) | Converts the number to ordinal string using the locale's default grammatical gender | 
| `String` | ConvertToOrdinal(`Int32` number, `GrammaticalGender` gender, `WordForm` wordForm) | Converts the number to ordinal string using the locale's default grammatical gender | 
| `String` | ConvertToTuple(`Int32` number) | Converts integer to named tuple (e.g. 'single', 'double' etc.). | 


