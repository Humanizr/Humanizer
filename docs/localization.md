# Localization

Humanizer supports over 40 languages and cultures, with localized implementations for most features.

## Supported Languages

Humanizer includes localization for:

Arabic (ar), Azerbaijani (az), Bulgarian (bg), Bengali (bn-BD), Czech (cs), Danish (da), German (de), Greek (el), Spanish (es), Persian (fa), Finnish (fi), French (fr), Hebrew (he), Croatian (hr), Hungarian (hu), Armenian (hy), Indonesian (id), Icelandic (is), Italian (it), Japanese (ja), Korean (ko), Kurdish (ku), Latvian (lv), Malay (ms-MY), Maltese (mt), Norwegian Bokmål (nb, nb-NO), Dutch (nl), Polish (pl), Portuguese (pt, pt-BR), Romanian (ro), Russian (ru), Slovak (sk), Slovenian (sl), Serbian (sr, sr-Latn), Swedish (sv), Thai (th), Turkish (tr), Ukrainian (uk), Uzbek (uz-Cyrl-UZ, uz-Latn-UZ), Vietnamese (vi), Chinese (zh-CN, zh-Hans, zh-Hant).

## Installing Language Packages

### All Languages

```bash
dotnet add package Humanizer
```

### Specific Languages

```bash
dotnet add package Humanizer.Core
dotnet add package Humanizer.Core.fr  # French
dotnet add package Humanizer.Core.es  # Spanish
dotnet add package Humanizer.Core.de  # German
```

## Using Cultures

Most Humanizer methods respect the current thread's `CurrentCulture` or `CurrentUICulture`. You can also explicitly specify a culture:

### DateTime Humanization

```csharp
var date = DateTime.UtcNow.AddHours(-2);

// Uses current culture
date.Humanize(); 

// Explicit culture
date.Humanize(culture: new CultureInfo("fr-FR")); 
// => "il y a 2 heures"

date.Humanize(culture: new CultureInfo("es")); 
// => "hace 2 horas"
```

### Number to Words

```csharp
1234.ToWords(); // Uses current culture

1234.ToWords(new CultureInfo("es")); 
// => "mil doscientos treinta y cuatro"

1234.ToWords(new CultureInfo("fr")); 
// => "mille deux cent trente-quatre"
```

### TimeSpan Humanization

```csharp
TimeSpan.FromDays(1).Humanize(); // Uses current culture

TimeSpan.FromDays(1).Humanize(culture: new CultureInfo("de")); 
// => "1 Tag"

TimeSpan.FromDays(3).Humanize(culture: new CultureInfo("ru")); 
// => "3 дня"
```

## Grammatical Features

Some languages require additional grammatical information:

### Grammatical Gender

```csharp
// Russian
1.ToWords(GrammaticalGender.Masculine, new CultureInfo("ru")); 
// => "один"

1.ToWords(GrammaticalGender.Feminine, new CultureInfo("ru")); 
// => "одна"

// Portuguese ordinals
1.Ordinalize(GrammaticalGender.Masculine); 
// => "1º"

1.Ordinalize(GrammaticalGender.Feminine); 
// => "1ª"
```

### Grammatical Case

```csharp
// Russian - date to ordinal words
var date = new DateTime(2020, 1, 1);

date.ToOrdinalWords(GrammaticalCase.Nominative); 
// Different form

date.ToOrdinalWords(GrammaticalCase.Genitive); 
// Different form
```

### Word Forms

```csharp
// Spanish - ordinal variations
3.Ordinalize(GrammaticalGender.Masculine, WordForm.Abbreviation); 
// => "3.er"

3.Ordinalize(GrammaticalGender.Masculine, WordForm.Normal); 
// => "3.º"
```

## Feature Support by Language

Not all features are available in all languages:

| Feature | Widely Supported | Limited Support |
|---------|------------------|-----------------|
| String Humanization | All languages | - |
| DateTime Humanization | All languages | - |
| TimeSpan Humanization | All languages | - |
| Number to Words | Most languages | Some Asian languages |
| Ordinalization | Most European languages | Limited in Asian languages |
| Pluralization | English only | - |

Check the specific feature documentation to see which languages are supported.

## Contributing Localizations

To contribute a new language or improve existing localizations:

1. Implement the required interfaces (e.g., `IFormatter`, `IDateToOrdinalWordConverter`)
2. Add resource files with translated strings
3. Register the formatter in `Configurator`
4. Add tests for the new language

See the [Contributing Guide](../CONTRIBUTING.md) for details.

## Related Topics

- [Installation](installation.md) - How to install language packages
- [Number to Words](number-to-words.md) - Language-specific number formatting
- [DateTime Humanization](datetime-humanization.md) - Relative time in different languages
