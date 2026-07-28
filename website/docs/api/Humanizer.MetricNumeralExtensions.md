## MetricNumeralExtensions Class

Contains extension methods for changing a number to Metric representation \(ToMetric\)
and from Metric representation back to the number \(FromMetric\)

```csharp
public static class MetricNumeralExtensions
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → MetricNumeralExtensions
### Methods

<a name='Humanizer.MetricNumeralExtensions.FromMetric(thisstring)'></a>

## MetricNumeralExtensions\.FromMetric\(this string\) Method

Converts a Metric representation into a number\.

```csharp
public static double FromMetric(this string input);
```
#### Parameters

<a name='Humanizer.MetricNumeralExtensions.FromMetric(thisstring).input'></a>

`input` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')

Metric representation to convert to a number

#### Returns
[System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')  
A number after a conversion from a Metric representation\.

### Example

```csharp
"1k".FromMetric() => 1000d
"123".FromMetric() => 123d
"100m".FromMetric() => 1E-1
```

### Remarks
We don't support input in the format \{number\}\{name\} nor \{number\} \{name\}\.
We only provide a solution for \{number\}\{symbol\} and \{number\} \{symbol\}\.

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thisdouble,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_)'></a>

## MetricNumeralExtensions\.ToMetric\(this double, Nullable\<MetricNumeralFormats\>, Nullable\<int\>\) Method

Converts a number into a valid and Human\-readable Metric representation\.

```csharp
public static string ToMetric(this double input, System.Nullable<Humanizer.MetricNumeralFormats> formats=null, System.Nullable<int> decimals=null);
```
#### Parameters

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thisdouble,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_).input'></a>

`input` [System\.Double](https://learn.microsoft.com/en-us/dotnet/api/system.double 'System\.Double')

Number to convert to a Metric representation\.

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thisdouble,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_).formats'></a>

`formats` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[MetricNumeralFormats](Humanizer.MetricNumeralFormats.md 'Humanizer\.MetricNumeralFormats')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

A bitwise combination of [MetricNumeralFormats](Humanizer.MetricNumeralFormats.md 'Humanizer\.MetricNumeralFormats') enumeration values that format the metric representation\.

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thisdouble,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_).decimals'></a>

`decimals` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The maximum number of fractional digits to include\. When [KeepTrailingZeros](Humanizer.MetricNumeralFormats.md#Humanizer.MetricNumeralFormats.KeepTrailingZeros 'Humanizer\.MetricNumeralFormats\.KeepTrailingZeros') is used, exactly this many fractional digits are included\. If null, all available precision is preserved and the flag has no effect\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A valid Metric representation

### Example

```csharp
1000d.ToMetric() => "1k"
123d.ToMetric() => "123"
1E-1.ToMetric() => "100m"
```

### Remarks
Inspired by a snippet from Thom Smith\.
See \<a href="http://stackoverflow\.com/questions/12181024/formatting\-a\-number\-with\-a\-metric\-prefix"\>this link\</a\> for more\.

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thisint,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_)'></a>

## MetricNumeralExtensions\.ToMetric\(this int, Nullable\<MetricNumeralFormats\>, Nullable\<int\>\) Method

Converts a number into a valid and Human\-readable Metric representation\.

```csharp
public static string ToMetric(this int input, System.Nullable<Humanizer.MetricNumeralFormats> formats=null, System.Nullable<int> decimals=null);
```
#### Parameters

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thisint,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_).input'></a>

`input` [System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')

Number to convert to a Metric representation\.

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thisint,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_).formats'></a>

`formats` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[MetricNumeralFormats](Humanizer.MetricNumeralFormats.md 'Humanizer\.MetricNumeralFormats')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

A bitwise combination of [MetricNumeralFormats](Humanizer.MetricNumeralFormats.md 'Humanizer\.MetricNumeralFormats') enumeration values that format the metric representation\.

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thisint,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_).decimals'></a>

`decimals` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The maximum number of fractional digits to include\. When [KeepTrailingZeros](Humanizer.MetricNumeralFormats.md#Humanizer.MetricNumeralFormats.KeepTrailingZeros 'Humanizer\.MetricNumeralFormats\.KeepTrailingZeros') is used, exactly this many fractional digits are included\. If null, all available precision is preserved and the flag has no effect\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A valid Metric representation

### Example

```csharp
1000.ToMetric() => "1k"
123.ToMetric() => "123"
1E-1.ToMetric() => "100m"
```

### Remarks
Inspired by a snippet from Thom Smith\.
See \<a href="http://stackoverflow\.com/questions/12181024/formatting\-a\-number\-with\-a\-metric\-prefix"\>this link\</a\> for more\.

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thislong,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_)'></a>

## MetricNumeralExtensions\.ToMetric\(this long, Nullable\<MetricNumeralFormats\>, Nullable\<int\>\) Method

Converts a number into a valid and Human\-readable Metric representation\.

```csharp
public static string ToMetric(this long input, System.Nullable<Humanizer.MetricNumeralFormats> formats=null, System.Nullable<int> decimals=null);
```
#### Parameters

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thislong,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_).input'></a>

`input` [System\.Int64](https://learn.microsoft.com/en-us/dotnet/api/system.int64 'System\.Int64')

Number to convert to a Metric representation\.

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thislong,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_).formats'></a>

`formats` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[MetricNumeralFormats](Humanizer.MetricNumeralFormats.md 'Humanizer\.MetricNumeralFormats')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

A bitwise combination of [MetricNumeralFormats](Humanizer.MetricNumeralFormats.md 'Humanizer\.MetricNumeralFormats') enumeration values that format the metric representation\.

<a name='Humanizer.MetricNumeralExtensions.ToMetric(thislong,System.Nullable_Humanizer.MetricNumeralFormats_,System.Nullable_int_).decimals'></a>

`decimals` [System\.Nullable&lt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')[System\.Int32](https://learn.microsoft.com/en-us/dotnet/api/system.int32 'System\.Int32')[&gt;](https://learn.microsoft.com/en-us/dotnet/api/system.nullable-1 'System\.Nullable\`1')

The maximum number of fractional digits to include\. When [KeepTrailingZeros](Humanizer.MetricNumeralFormats.md#Humanizer.MetricNumeralFormats.KeepTrailingZeros 'Humanizer\.MetricNumeralFormats\.KeepTrailingZeros') is used, exactly this many fractional digits are included\. If null, all available precision is preserved and the flag has no effect\.

#### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')  
A valid Metric representation

### Example

```csharp
1000.ToMetric() => "1k"
123.ToMetric() => "123"
1E-1.ToMetric() => "100m"
```

### Remarks
Inspired by a snippet from Thom Smith\.
See \<a href="http://stackoverflow\.com/questions/12181024/formatting\-a\-number\-with\-a\-metric\-prefix"\>this link\</a\> for more\.