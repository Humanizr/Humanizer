## ShowQuantityAs Enum

Enumerates the ways of displaying a quantity value when converting
a word to a quantity string\.

```csharp
public enum ShowQuantityAs
```
### Fields

<a name='Humanizer.ShowQuantityAs.None'></a>

`None` 0

Indicates that no quantity will be included in the formatted string\.

<a name='Humanizer.ShowQuantityAs.Numeric'></a>

`Numeric` 1

Indicates that the quantity will be included in the output, formatted
as its numeric value \(e\.g\. "1"\)\.

<a name='Humanizer.ShowQuantityAs.Words'></a>

`Words` 2

Incidates that the quantity will be included in the output, formatted as
words \(e\.g\. 123 =\> "one hundred and twenty three"\)\.