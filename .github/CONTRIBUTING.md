## <a id="how-to-contribute">How to contribute?</a>
Your contributions to Humanizer are very welcome.
If you find a bug, please raise it as an issue.
Even better fix it and send a pull request.
If you like to help out with existing bugs and feature requests just check out the list of [issues](https://github.com/Humanizr/Humanizer/issues) and grab and fix one.
Some of the issues are labeled `jump in`. These issues are generally low hanging fruit so you can start with easier tasks.

This project has adopted the code of conduct defined by the [Contributor Covenant](http://contributor-covenant.org/)
to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](http://www.dotnetfoundation.org/code-of-conduct).

### <a id="getting-started">Getting started</a>
This project uses modern SDK-style .NET projects, so you'll need a current .NET SDK to open and compile the repository.

### <a id="contribution-guideline">Contribution guideline</a>
This project uses [GitHub flow](http://scottchacon.com/2011/08/31/github-flow.html) for pull requests.
So if you want to contribute, fork the repo, preferably create a local branch, based off of the `main` branch, to avoid conflicts with other activities, fix an issue, run the repository build and test commands, and send a PR if all is green.

Pull requests are code reviewed. Here is a checklist you should tick through before submitting a pull request:

 - Implementation is clean
 - Code adheres to the existing coding standards; e.g. no curlies for one-line blocks, no redundant empty lines between methods or code blocks, spaces rather than tabs, etc. There is an `.editorconfig` file that must be respected.
 - No ReSharper warnings
 - There is proper unit test coverage
 - If the code is copied from StackOverflow (or a blog or OSS) full disclosure is included. That includes required license files and/or file headers explaining where the code came from with proper attribution
 - There are very few or no comments (because comments shouldn't be needed if you write clean code)
 - Xml documentation is added/updated for the addition/change
 - Your PR is (re)based on top of the latest commits from the `main` branch (more info below)
 - Link to the issue(s) you're fixing from your PR description. Use `fixes #<the issue number>`
 - Readme is updated if you change an existing feature or add a new one
 - Run the repository test/build commands and ensure there are no test failures

Please rebase your code on top of the latest `main` branch commits.
Before working on your fork make sure you pull the latest so you work on top of the latest commits to avoid merge conflicts.
Also before sending the pull request please rebase your code as there is a chance there have been new commits pushed after you pulled last.
Please refer to [this guide](https://gist.github.com/jbenet/ee6c9ac48068889b0912#the-workflow) if you're new to git.

### <a id="need-your-help-with-localisation">Need your help with localisation</a>
One area where Humanizer can always use your help is localisation.
Currently Humanizer supports quite a few localisations for `DateTime.Humanize`, `TimeSpan.Humanize`, `ToWords`, `ToOrdinalWords`, and `ToNumber`.

Humanizer could definitely do with more translations.
To add a translation for `DateTime.Humanize` and `TimeSpan.Humanize`,
fork the repository if you haven't done yet, duplicate one of the YAML locale files under [`src/Humanizer/Locales`](https://github.com/Humanizr/Humanizer/tree/main/src/Humanizer/Locales), rename it to your target [locale code](http://msdn.microsoft.com/en-us/library/hh441729.aspx)
(for example `ru.yml` for Russian), translate the locale data, write unit tests for the translation, commit, and send a pull request for it. Thanks.

The YAML schema supports exact phrases, counted forms, aliases, inheritance, and profile data, so most languages can be expressed entirely in locale data without custom runtime code.
If your language still needs formatter-specific behavior beyond the locale schema, subclass [`DefaultFormatter`](https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/Localisation/Formatters/DefaultFormatter.cs) in a class that represents your language;
e.g. [`RomanianFormatter`](https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/Localisation/Formatters/RomanianFormatter.cs) and [`RussianFormatter`](https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/Localisation/Formatters/RussianFormatter.cs) show the older residual-leaf pattern.
Then return an instance of your class in the [`Configurator`](https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/Configuration/Configurator.cs) class in the getter of the [Formatter property](https://github.com/Humanizr/Humanizer/blob/main/src/Humanizer/Configuration/Configurator.cs) based on the current culture.

Number localization is now authored primarily in YAML under `src/Humanizer/Locales`. Supported locales should plan `ToWords` and `ToNumber` together so the same high-range forms round-trip naturally.
Check out the locale YAML files and the shared number kernels under `src/Humanizer/Localisation/NumberToWords` and `src/Humanizer/Localisation/WordsToNumber` for examples of how to express a language in the current pipeline.
If a locale still needs a residual runtime leaf, keep it intentional and document why the shared YAML schema cannot express it yet.

Don't forget to write tests for your localisations. Check out the existing [DateHumanizeTests](https://github.com/Humanizr/Humanizer/blob/main/tests/Humanizer.Tests/Localisation/ru-RU/DateHumanizeTests.cs), [TimeSpanHumanizeTests](https://github.com/Humanizr/Humanizer/blob/main/tests/Humanizer.Tests/Localisation/ru-RU/TimeSpanHumanizeTests.cs), [NumberToWordsTests](https://github.com/Humanizr/Humanizer/blob/main/tests/Humanizer.Tests/Localisation/ru-RU/NumberToWordsTests.cs), and locale-specific `WordsToNumber` tests where the culture supports parsing.
