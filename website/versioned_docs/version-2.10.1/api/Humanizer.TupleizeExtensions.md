## TupleizeExtensions Class

Convert int to named tuple strings \(1 \-\> 'single', 2\-\> 'double' etc\.\)\.
Only values 1\-10, 100, and 1000 have specific names\. All others will return 'n\-tuple'\.

```csharp
public static class TupleizeExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → TupleizeExtensions
### Methods

<a name='Humanizer.TupleizeExtensions.Tupleize(thisint)'></a>

## TupleizeExtensions\.Tupleize\(this int\) Method

Converts integer to named tuple \(e\.g\. 'single', 'double' etc\.\)\.

```csharp
public static string Tupleize(this int input);
```
#### Parameters

<a name='Humanizer.TupleizeExtensions.Tupleize(thisint).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Integer

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
Named tuple