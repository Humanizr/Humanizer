---
title: 'Humanizer.EnglishArticle'
sidebar_label: 'Humanizer.EnglishArticle'
description: 'API reference for Humanizer.EnglishArticle.'
---
## EnglishArticle Class

Contains methods for removing, appending and prepending article prefixes for sorting strings ignoring the article\.

```csharp
public static class EnglishArticle
```

Inheritance [System\.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object 'System\.Object') → EnglishArticle
- *Methods*
  - **[AppendArticlePrefix\(string\[\]\)](Humanizer.EnglishArticle.md#Humanizer.EnglishArticle.AppendArticlePrefix(string[]) 'Humanizer\.EnglishArticle\.AppendArticlePrefix\(string\[\]\)')**
  - **[PrependArticleSuffix\(string\[\]\)](Humanizer.EnglishArticle.md#Humanizer.EnglishArticle.PrependArticleSuffix(string[]) 'Humanizer\.EnglishArticle\.PrependArticleSuffix\(string\[\]\)')**
### Methods

<a name='Humanizer.EnglishArticle.AppendArticlePrefix(string[])'></a>

#### EnglishArticle\.AppendArticlePrefix\(string\[\]\) Method

Removes the prefixed article and appends it to the same string\.

```csharp
public static string[] AppendArticlePrefix(string[] items);
```
##### Parameters

<a name='Humanizer.EnglishArticle.AppendArticlePrefix(string[]).items'></a>

`items` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

The input array of strings

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')  
Sorted string array

<a name='Humanizer.EnglishArticle.PrependArticleSuffix(string[])'></a>

#### EnglishArticle\.PrependArticleSuffix\(string\[\]\) Method

Removes the previously appended article and prepends it to the same string\.

```csharp
public static string[] PrependArticleSuffix(string[] appended);
```
##### Parameters

<a name='Humanizer.EnglishArticle.PrependArticleSuffix(string[]).appended'></a>

`appended` [System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')

Sorted string array

##### Returns
[System\.String](https://learn.microsoft.com/en-us/dotnet/api/system.string 'System\.String')[\[\]](https://learn.microsoft.com/en-us/dotnet/api/system.array 'System\.Array')  
String array
