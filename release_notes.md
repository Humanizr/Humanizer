###In Development
  - [#277](https://github.com/MehdiK/Humanizer/pull/277): Added support for custom enum description attribute property names
  - [#276](https://github.com/Mehdik/Humanizer/pull/276): Added Farsi ToOrdinalWords
  - [#281](https://github.com/Mehdik/Humanizer/pull/281): Changed the logic for handling hyphenation and large numbers ending in twelve for English ordinal words; e.g. before "twenty first" now "twenty-first"
  - [#278](https://github.com/MehdiK/Humanizer/pull/278): Changed DefaultDateTimeHumanizeStrategy to turn 60 min to one hour not 45
  - [#283](https://github.com/MehdiK/Humanizer/pull/283): Added Neutral nb support for DateTime and TimeSpan Humanize
  
[Commits](https://github.com/MehdiK/Humanizer/compare/v1.26.1...master)

###v1.26.1 - 2014-05-20
  - [#257](https://github.com/Mehdik/Humanizer/pull/257): Added German localisation for ToOrdinalWords and Ordinalize
  - [#261](https://github.com/Mehdik/Humanizer/pull/261): Added future dates to Portuguese - Brazil
  - [#269](https://github.com/MehdiK/Humanizer/pull/269): Added Vietnamese localisation
  - [#268](https://github.com/Mehdik/Humanizer/pull/268): Added humanization of collections

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.25.4...v1.26.1)

###v1.25.4 - 2014-04-27
  - [#236](https://github.com/Mehdik/Humanizer/pull/236): Added Turkish localisation
  - [#239](https://github.com/Mehdik/Humanizer/pull/239): Added Serbian localisation
  - [#241](https://github.com/Mehdik/Humanizer/pull/241): Added German ToWords localisation
  - [#244](https://github.com/MehdiK/Humanizer/pull/244): Added Slovenian localisation
  - [#247](https://github.com/MehdiK/Humanizer/pull/247): Added Slovenian number to words localisation
  - [#227](https://github.com/MehdiK/Humanizer/pull/227) & [#243](https://github.com/MehdiK/Humanizer/pull/243): Moved localiser registry to their own classes, allowed public access via Configurator, and made the default registrations lazy loaded

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.24.1...v1.25.4)

###v1.24.1 - 2014-04-21
  - [#232](https://github.com/Mehdik/Humanizer/pull/232): Adding code & tests to handle Arabic numbers to ordinal 
  - [#235](https://github.com/Mehdik/Humanizer/pull/235): Fixed the conversion for "1 millon" in SpanishNumberToWordsConverter
  - [#233](https://github.com/Mehdik/Humanizer/pull/233): Added build.cmd and Verify build configuration for strict project build and analysis

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.23.1...v1.24.1)

###v1.23.1 - 2014-04-19
  - [#217](https://github.com/Mehdik/Humanizer/pull/217): Added pt-BR and Spanish Ordinalize localisation.
  - [#220](https://github.com/Mehdik/Humanizer/pull/220): Added string formatting options to ToQuantity
  - [#219](https://github.com/Mehdik/Humanizer/pull/219): Added Japanese translation for date and timespan
  - [#221](https://github.com/Mehdik/Humanizer/pull/221): Added Russian ordinalizer
  - [#228](https://github.com/Mehdik/Humanizer/pull/228): Fixed the "twenties" in SpanishNumberToWordsConverter
  - [#231](https://github.com/Mehdik/Humanizer/pull/231): Added more settings for FromNow, Dual and Plural (Arabic)
  - [#222](https://github.com/Mehdik/Humanizer/pull/222): Updated Ordinalize and ToOrdinalWords to account for special exceptions with 1 and 3.

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.22.1...v1.23.1)

###v1.22.1 - 2014-04-14
  - [#188](https://github.com/Mehdik/Humanizer/pull/188): Added Spanish ToOrdinalWords translations
  - [#166](https://github.com/MehdiK/Humanizer/pull/166): Added Dutch (NL) Number to words and ordinals
  - [#199](https://github.com/MehdiK/Humanizer/pull/199): Added Hebrew Number to words (both genders)
  - [#202](https://github.com/MehdiK/Humanizer/pull/202): Fixed typo sekunttia -> sekuntia (Finnish translation)
  - [#203](https://github.com/MehdiK/Humanizer/pull/203): Added feminine gender for french ordinal words
  - [#208](https://github.com/MehdiK/Humanizer/pull/208): Added Hebrew implementation of future DateTime 
  
[Commits](https://github.com/MehdiK/Humanizer/compare/v1.21.1...v1.22.1)

###v1.21.1 - 2014-04-12
  - [#196](https://github.com/MehdiK/Humanizer/pull/196): Added Gender for ToOrdinalWords (needed for Brazilian Portuguese). Added pt-br OrdinalToWords localisation
  - [#194](https://github.com/MehdiK/Humanizer/pull/194): Added pt-BR NumberToWords localisation
  - [#147](https://github.com/MehdiK/Humanizer/pull/147): Added Russian translation for ToWords
  - [#190](https://github.com/MehdiK/Humanizer/pull/190): Added French translation for ToWords and ToOrdinalWords
  - [#179](https://github.com/MehdiK/Humanizer/pull/179): Added Hungarian localisation
  - [#181](https://github.com/Mehdik/Humanizer/pull/181): Added Bulgarian localization, date and timespan tests
  - [#141](https://github.com/MehdiK/Humanizer/pull/141): Added Indonesian localization 
  - [#148](https://github.com/Mehdik/Humanizer/pull/148): Added Hebrew localization for date and timespan

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.20.15...v1.21.1)

###v1.20.15 - 2014-04-12
  - [#186](https://github.com/Mehdik/Humanizer/pull/186): Refactored `ToOrdinalWords` to use existing `NumberToWordsExtension` to prepare for Ordinal localization
  - [#193](https://github.com/Mehdik/Humanizer/pull/193): Fixed the NullException error on DateTime.Humanize

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.20.2...v1.20.15)

###v1.20.2 - 2014-04-11
  - [#171](https://github.com/MehdiK/Humanizer/pull/171): T4-Template fix: Using EnglishNumberToWordsConverter instead of 'ToWords()' while dogfooding the template with the library.
  - [#165](https://github.com/MehdiK/Humanizer/pull/165): Added precision based `DateTime.Humanize` strategy
  - [#155](https://github.com/MehdiK/Humanizer/pull/155): French and Belgian French localisation
  - [#151](https://github.com/MehdiK/Humanizer/pull/151): Added Spanish ToWords Translations
  - [#172](https://github.com/MehdiK/Humanizer/pull/172): Added Polish translation for ToWords
  - [#184](https://github.com/Mehdik/Humanizer/pull/184): Fixed spelling error with forth/fourth in EnglishNumberToWordsConverter

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.19.1...v1.20.2)

###v1.19.1 - 2014-04-10
  - [#149](https://github.com/MehdiK/Humanizer/pull/149): Improved & refactored number to words localisation 
  - [#143](https://github.com/MehdiK/Humanizer/pull/143): Added Russian translation for future DateTime, TimeSpan and Now
  - [#144](https://github.com/MehdiK/Humanizer/pull/144): Added Danish localization (strings, tests)
  - [#146](https://github.com/MehdiK/Humanizer/pull/146): Added Spanish translation for future DateTime, TimeSpan and Now
  
           
[Commits](https://github.com/MehdiK/Humanizer/compare/v1.18.1...v1.19.1)

###v1.18.1 - 2014-04-09
  - [#137](https://github.com/MehdiK/Humanizer/pull/137): Fixed grammar error in `nb-NO` resource file & added missing Norwegian resource strings (mainly `DateHumanize_*FromNow`)
  - [#135](https://github.com/MehdiK/Humanizer/pull/135): Added Swedish localization (strings, tests)
  - [#140](https://github.com/MehdiK/Humanizer/pull/140): Added Polish localization (strings, formatter, tests)

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.17.1...v1.18.1)

###v1.17.1 - 2014-04-06
  - [#124](https://github.com/MehdiK/Humanizer/pull/124): Added Slovak localization (strings, formatter, tests)
  - [#130](https://github.com/MehdiK/Humanizer/pull/130): Added Czech localization (strings, formatter, tests)
  - [#131](https://github.com/MehdiK/Humanizer/pull/131): Clean date humanize tests and renamed `TimeUnitTense` to `Tense`

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.16.4...v1.17.1)

###v1.16.4 - 2014-04-04
  - [#129](https://github.com/MehdiK/Humanizer/pull/129): Removed all but PCL references
  - [#121](https://github.com/MehdiK/Humanizer/pull/121): Added Farsi translation for DateTime, TimeSpan and NumberToWords
  - [#120](https://github.com/MehdiK/Humanizer/pull/120): Added German translation for DateTime and TimeSpan
  - [#117](https://github.com/MehdiK/Humanizer/pull/117): Added `FormatWith` string extension

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.15.1...v1.16.4)

###v1.15.1 - 2014-03-28
  - [#113](https://github.com/MehdiK/Humanizer/pull/113): Added `Truncate` feature
  - [#109](https://github.com/MehdiK/Humanizer/pull/109): Made Dutch (NL) localization a neutral culture, not just for Belgium

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.14.1...v1.15.1)

###v1.14.1 - 2014-03-26
  - [#108](https://github.com/MehdiK/Humanizer/pull/108): Added support for custom description attributes
  - [#106](https://github.com/MehdiK/Humanizer/pull/106): 
    - Refactored IFormatter and DefaultFormatter
	- Refactored `DateTime.Humanize` and `TimeSpan.Humanize`
	- Changed `ResourceKeys` to use a dynamic key generation
	- Fixed the intermittent failing tests on `DateTime.Humanize`

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.13.2...v1.14.1)

###v1.13.2 - 2014-03-17
  - [#99](https://github.com/MehdiK/Humanizer/pull/99): Added `ByteSize` feature

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.12.4...v1.13.2)

###v1.12.4 - 2014-02-25
  - [#95](https://github.com/MehdiK/Humanizer/pull/95): Added NoMatch optional parameter to DehumanizeTo & renamed `CannotMapToTargetException` to `NoMatchFoundException`

####Breaking Changes
If you were catching `CannotMapToTargetException` on a `DehumanizeTo` call, that's been renamed to `NoMatchFoundException` to make it more generic.

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.11.3...v1.12.4)

###v1.11.3 - 2014-02-18
  - [#93](https://github.com/MehdiK/Humanizer/pull/93): added non-generic DehumanizeTo for Enums unknown at compile time

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.10.1...v1.11.3)

###v1.10.1 - 2014-02-15
  - [#89](https://github.com/MehdiK/Humanizer/pull/89): added `ToRoman` and `FromRoman` extensions
  - [#82](https://github.com/MehdiK/Humanizer/pull/82): fixed Greek locale code

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.9.1...v1.10.1)

###v1.9.1 - 2014-02-12
  - [#78](https://github.com/MehdiK/Humanizer/pull/78): added support for billions to `ToWords`

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.8.1...v1.9.1)

###v1.8.16 - 2014-02-12
  - [#81](https://github.com/MehdiK/Humanizer/pull/81): fixed issue with localised methods returning null in Windows Store apps
  - Created [Humanizr.net](http://humanizr.net) website as GitHub pages

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.8.1...v1.8.16)

###v1.8.1 - 2014-02-04
  - [#73](https://github.com/MehdiK/Humanizer/pull/73): added `ToWords` implementation for Arabic

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.7.1...v1.8.1)

###v1.7.1 - 2014-02-31
  - [#68](https://github.com/MehdiK/Humanizer/pull/68): `DateTime.Humanize()` now supports future dates

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.6.1...v1.7.1)

###v1.6.1 - 2014-01-27
  - [#69](https://github.com/MehdiK/Humanizer/pull/69): changed the return type of `DehumanizeTo<TTargetEnum>` to `TTargetEnum`

####Potential breaking change
The return type of `DehumanizeTo<TTargetEnum>` was changed from `Enum` to `TTargetEnum` to make the API a lot easier to work with.
That also potentially means that your calls to the old method may be broken.
Depending on how you were using the method you might have to either drop the now redundant cast to `TTargetEnum` in your code, or 
fix it based on your requirements.

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.5.1...v1.6.1)

###v1.5.1 - 2014-01-23
  - [#65](https://github.com/MehdiK/Humanizer/pull/65): added precision to `TimeSpan.Humanize`

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.4.1...v1.5.1)

###v1.4.1 - 2014-01-20
  - [#62](https://github.com/MehdiK/Humanizer/pull/62): added `ShowQuantityAs` parameter to `ToQuantity`

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.3.1...v1.4.1)

###v1.3.1 - 2014-14-01
  - [#51](https://github.com/MehdiK/Humanizer/pull/51): added Spanish translation for `DateTime.Humanize`
  - [#52](https://github.com/MehdiK/Humanizer/pull/52): added Arabic translation for `DateTime.Humanize`
  - [#53](https://github.com/MehdiK/Humanizer/pull/53): added `Hyphenate` as an overload for `Dasherize`
  - [#54](https://github.com/MehdiK/Humanizer/pull/54): added Portuguese translation for `DateTime.Humanize`
  - [#55](https://github.com/MehdiK/Humanizer/pull/55): added Arabic translation for `TimeSpan.Humanize`

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.1.0...v1.3.1)

###v1.1.0 - 2014-01-01
  - [#37](https://github.com/MehdiK/Humanizer/pull/37): added `ToQuantity` method
  - [#43](https://github.com/MehdiK/Humanizer/pull/43): 
    - added `Plurality` enum
    - can call `Singularize` on singular and `Pluralize` on plural words
    - `ToQuantity` can be called on words with unknown plurality

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.0.29...v1.1.0)

###v1.0.29 - 2013-12-25
  - [#26](https://github.com/MehdiK/Humanizer/pull/26): added Norwegian (nb-NO) localization for `DateTime.Humanize()`
  - [#33](https://github.com/MehdiK/Humanizer/pull/33): 
    - changed to Portable Class Library with support for .Net 4+, SilverLight 5, Windows Phone 8 and Win Store applications
    - symbols nuget package is published so you can step through Humanizer code while debugging your code

[Commits](https://github.com/MehdiK/Humanizer/compare/v1.0.0...v1.0.29)

###v1.0.0 - 2013-11-10
No release history before this point: check out http://www.mehdi-khalili.com/humanizer-v1 for the feature-set at V1

