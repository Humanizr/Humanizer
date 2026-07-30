## MetricNumeralFormats Enum

Flags for formatting the metric representation of numerals\.

```csharp
public enum MetricNumeralFormats
```
### Fields

<a name='Humanizer.MetricNumeralFormats.UseLongScaleWord'></a>

`UseLongScaleWord` 1

Use the metric prefix \<a href="https://en\.wikipedia\.org/wiki/Long\_and\_short\_scales"\>long scale word\</a\>\.

<a name='Humanizer.MetricNumeralFormats.UseName'></a>

`UseName` 2

Use the metric prefix \<a href="https://en\.wikipedia\.org/wiki/Metric\_prefix\#List\_of\_SI\_prefixes"\>name\</a\> instead of the symbol\.

<a name='Humanizer.MetricNumeralFormats.UseShortScaleWord'></a>

`UseShortScaleWord` 4

Use the metric prefix \<a href="https://en\.wikipedia\.org/wiki/Long\_and\_short\_scales"\>short scale word\</a\>\.

<a name='Humanizer.MetricNumeralFormats.WithSpace'></a>

`WithSpace` 8

Include a space after the numeral\.

<a name='Humanizer.MetricNumeralFormats.UseScaleWord'></a>

`UseScaleWord` 16

Use the scale word authored for [System\.Globalization\.CultureInfo\.CurrentUICulture](https://learn.microsoft.com/en-us/dotnet/api/system.globalization.cultureinfo.currentuiculture 'System\.Globalization\.CultureInfo\.CurrentUICulture')\.
When that locale has no standalone word for the selected power of 1000, use the SI symbol\.
The locale\-authored grammatical count form follows the displayed, scaled numeral\.
Inverse scale words are used only when the authored singular form applies; other counts use the SI symbol\.
For example, `1E9` renders as `billion` in `en-US` and `Milliarde` in `de-DE`\.

<a name='Humanizer.MetricNumeralFormats.KeepTrailingZeros'></a>

`KeepTrailingZeros` 32

Include trailing zeros so the number of fractional digits matches the `decimals` argument\.
Has no effect when `decimals` is [null](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/null 'https://docs\.microsoft\.com/en\-us/dotnet/csharp/language\-reference/keywords/null')\.