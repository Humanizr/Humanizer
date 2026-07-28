## DynamicNumberOfCharactersAndPreserveWordsTruncator Class

Truncate a string to a fixed number of letters or digits,
preserving whole words by never cutting a word in half\.
If a complete word \(plus the delimiter, if any\) cannot fit, then only the delimiter is returned\.
When truncating from the left, the delimiter is prepended if a complete word can be preserved;
otherwise, only the delimiter is returned\.
The allowed count is computed by counting only letters/digits\.

```csharp
public class DynamicNumberOfCharactersAndPreserveWordsTruncator : Humanizer.ITruncator
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → DynamicNumberOfCharactersAndPreserveWordsTruncator

Implements [ITruncator](Humanizer.ITruncator.md 'Humanizer\.ITruncator')