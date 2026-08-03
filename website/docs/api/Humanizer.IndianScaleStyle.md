---
title: 'Humanizer.IndianScaleStyle'
sidebar_label: 'Humanizer.IndianScaleStyle'
description: 'API reference for Humanizer.IndianScaleStyle.'
---
## IndianScaleStyle Enum

Selects the large\-number vocabulary used by [ToIndianWords\(this long, IndianScaleStyle\)](Humanizer.NumberToWordsExtension.md#Humanizer.NumberToWordsExtension.ToIndianWords(thislong,Humanizer.IndianScaleStyle) 'Humanizer\.NumberToWordsExtension\.ToIndianWords\(this long, Humanizer\.IndianScaleStyle\)')\.

```csharp
public enum IndianScaleStyle
```
- *Fields*
  - **[CroreBased](Humanizer.IndianScaleStyle.md#Humanizer.IndianScaleStyle.CroreBased 'Humanizer\.IndianScaleStyle\.CroreBased')**
  - **[NamedScales](Humanizer.IndianScaleStyle.md#Humanizer.IndianScaleStyle.NamedScales 'Humanizer\.IndianScaleStyle\.NamedScales')**
### Fields

<a name='Humanizer.IndianScaleStyle.NamedScales'></a>

`NamedScales` 0

Uses the named Indian scales, including arab, kharab, neel, padma, and shankh\.

<a name='Humanizer.IndianScaleStyle.CroreBased'></a>

`CroreBased` 1

Uses common crore\-based expressions through lakh crore, while retaining named scales above that range\.
