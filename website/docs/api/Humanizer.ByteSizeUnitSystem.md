## ByteSizeUnitSystem Enum

Selects the unit system used by explicit [ByteSize](Humanizer.ByteSize.md 'Humanizer\.ByteSize') parsing and formatting APIs\.

```csharp
public enum ByteSizeUnitSystem
```
### Fields

<a name='Humanizer.ByteSizeUnitSystem.Legacy'></a>

`Legacy` 0

Uses Humanizer's established mixed units: binary KB through TB and decimal PB and EB\.

<a name='Humanizer.ByteSizeUnitSystem.DecimalSi'></a>

`DecimalSi` 1

Uses decimal SI units where each successive unit is 1000 times the previous unit\.

<a name='Humanizer.ByteSizeUnitSystem.BinaryIec'></a>

`BinaryIec` 2

Uses binary IEC units where each successive unit is 1024 times the previous unit\.

### Remarks
For [DecimalSi](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.DecimalSi 'Humanizer\.ByteSizeUnitSystem\.DecimalSi') and [BinaryIec](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.BinaryIec 'Humanizer\.ByteSizeUnitSystem\.BinaryIec'), explicit parsers and format\-token selectors
match SI/IEC\-prefixed unit symbols without regard to letter casing, while formatting emits canonical symbol casing\.
The `b` and `B` symbols remain case\-sensitive\.
[Legacy](Humanizer.ByteSizeUnitSystem.md#Humanizer.ByteSizeUnitSystem.Legacy 'Humanizer\.ByteSizeUnitSystem\.Legacy') preserves the established parsing and formatting behavior\.