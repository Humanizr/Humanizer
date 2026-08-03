---
title: 'Humanizer.EnumHumanizeSource'
sidebar_label: 'Humanizer.EnumHumanizeSource'
description: 'API reference for Humanizer.EnumHumanizeSource.'
---
## EnumHumanizeSource Enum

Specifies the source used to humanize an enum value\.

```csharp
public enum EnumHumanizeSource
```
- *Fields*
  - **[Default](Humanizer.EnumHumanizeSource.md#Humanizer.EnumHumanizeSource.Default 'Humanizer\.EnumHumanizeSource\.Default')**
  - **[DisplayDescription](Humanizer.EnumHumanizeSource.md#Humanizer.EnumHumanizeSource.DisplayDescription 'Humanizer\.EnumHumanizeSource\.DisplayDescription')**
  - **[DisplayName](Humanizer.EnumHumanizeSource.md#Humanizer.EnumHumanizeSource.DisplayName 'Humanizer\.EnumHumanizeSource\.DisplayName')**
  - **[DisplayShortName](Humanizer.EnumHumanizeSource.md#Humanizer.EnumHumanizeSource.DisplayShortName 'Humanizer\.EnumHumanizeSource\.DisplayShortName')**
  - **[EnumName](Humanizer.EnumHumanizeSource.md#Humanizer.EnumHumanizeSource.EnumName 'Humanizer\.EnumHumanizeSource\.EnumName')**
### Fields

<a name='Humanizer.EnumHumanizeSource.Default'></a>

`Default` 0

Uses the default metadata precedence, falling back to the enum member name\.

<a name='Humanizer.EnumHumanizeSource.EnumName'></a>

`EnumName` 1

Uses the enum member name\.

<a name='Humanizer.EnumHumanizeSource.DisplayName'></a>

`DisplayName` 2

Uses [System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Name](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.name 'System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Name'),
falling back to the enum member name\.

<a name='Humanizer.EnumHumanizeSource.DisplayDescription'></a>

`DisplayDescription` 3

Uses [System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Description](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.description 'System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Description'),
falling back to the enum member name\.

<a name='Humanizer.EnumHumanizeSource.DisplayShortName'></a>

`DisplayShortName` 4

Uses [System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.ShortName](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.shortname 'System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.ShortName'),
then [System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Name](https://learn.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations.displayattribute.name 'System\.ComponentModel\.DataAnnotations\.DisplayAttribute\.Name'),
falling back to the enum member name when neither is available\.
