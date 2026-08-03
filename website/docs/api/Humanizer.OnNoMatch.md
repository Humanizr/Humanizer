---
title: 'Humanizer.OnNoMatch'
sidebar_label: 'Humanizer.OnNoMatch'
description: 'API reference for Humanizer.OnNoMatch.'
---
## OnNoMatch Enum

Dictating what should be done when a match is not found \- currently used only for DehumanizeTo

```csharp
public enum OnNoMatch
```
- *Fields*
  - **[ReturnsNull](Humanizer.OnNoMatch.md#Humanizer.OnNoMatch.ReturnsNull 'Humanizer\.OnNoMatch\.ReturnsNull')**
  - **[ThrowsException](Humanizer.OnNoMatch.md#Humanizer.OnNoMatch.ThrowsException 'Humanizer\.OnNoMatch\.ThrowsException')**
### Fields

<a name='Humanizer.OnNoMatch.ThrowsException'></a>

`ThrowsException` 0

This is the default behavior which throws a NoMatchFoundException

<a name='Humanizer.OnNoMatch.ReturnsNull'></a>

`ReturnsNull` 1

If set to ReturnsNull the method returns null instead of throwing an exception
