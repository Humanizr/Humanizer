# Copilot Instructions for Humanizer

## Project Overview

Humanizer is a .NET library that meets all your needs for manipulating and displaying strings, enums, dates, times, timespans, numbers and quantities. The project provides human-friendly text transformations and localizations for multiple languages.

## Technology Stack

- **Language**: C# (C# 8+)
- **Frameworks**: .NET 8.0, .NET 10.0, .NET 4.8
- **Test Framework**: xUnit
- **Build System**: .NET CLI (dotnet)
- **CI/CD**: Azure Pipelines

## Code Style and Conventions

### General Principles

1. **Follow .editorconfig**: The project has a comprehensive `.editorconfig` file that must be respected
2. **No ReSharper warnings**: Code should be clean of ReSharper warnings
3. **Minimal comments**: Write clean, self-documenting code. Comments should only be used when necessary to explain complex logic
4. **No curly braces for one-line blocks**: Use single-line syntax when appropriate
5. **Spaces over tabs**: Always use spaces for indentation (4 spaces for C#)
6. **File-scoped namespaces**: Use file-scoped namespace declarations

### Naming Conventions

- **Private fields**: camelCase (e.g., `fieldName`)
- **Private constants**: PascalCase (e.g., `ConstantName`)
- **Private static readonly**: PascalCase
- **Public members**: PascalCase
- **Avoid `this.`**: Don't use `this.` qualifier unless necessary

### Code Organization

- Remove redundant empty lines between methods or code blocks
- Use `var` for built-in types and simple type declarations
- Sort using directives with System.* appearing first
- Use language keywords instead of framework type names (e.g., `string` not `String`)
- Prefer modern C# language features (object initializers, collection initializers, pattern matching)

## Testing Requirements

### Test Framework

- Use **xUnit** for all tests
- Test files should be in the `src/Humanizer.Tests` directory
- Follow existing test naming conventions (e.g., `FeatureNameTests.cs`)

### Test Guidelines

1. **Write comprehensive tests**: Every new feature or bug fix must include tests
2. **Follow existing patterns**: Look at existing test files for structure and style
3. **Test localization**: For localization features, create tests in the appropriate `Localisation/{culture}` folder
4. **Use `UseCultureAttribute`**: For culture-specific tests, use the `UseCulture` attribute
5. **API approval tests**: Changes to public APIs will need approval test updates

### Running Tests

```bash
# Run all tests
dotnet test src/Humanizer.Tests/Humanizer.Tests.csproj -c Release

# Run tests with code coverage
dotnet test src/Humanizer.Tests/Humanizer.Tests.csproj -c Release --collect:"XPlat code coverage" -s src/CodeCoverage.runsettings
```

## Build and Validation

### Building the Project

```bash
# Build from the src directory
cd src
dotnet build Humanizer/Humanizer.csproj -c Release

# Or use the build script from the root
./src/build.cmd  # Windows
./src/build.ps1  # PowerShell
```

### Pre-commit Checklist

Before submitting changes:

1. **Build succeeds**: Run the build command and ensure no errors
2. **All tests pass**: Run the test suite and verify all tests pass
3. **No new warnings**: Check that no new compiler or ReSharper warnings were introduced
4. **EditorConfig compliance**: Verify code follows .editorconfig rules
5. **XML documentation**: Add or update XML documentation for public APIs
6. **Update README**: If adding/changing features, update `readme.md`

## Localization Guidelines

Humanizer supports extensive localization. When working with localization:

### Adding a New Localization

1. **Resource files**: Duplicate `src/Humanizer/Properties/Resources.resx` and add the locale code (e.g., `Resources.ru.resx`)
2. **Register formatter**: Add your formatter in `src/Humanizer/Configuration/FormatterRegistry.cs`
3. **Complex rules**: For languages with complex number rules, subclass `DefaultFormatter` (see `RomanianFormatter` or `RussianFormatter`)
4. **Number converters**: For `ToWords` and `ToOrdinalWords`, create a converter (see `DutchNumberToWordsConverter` or `RussianNumberToWordsConverter`)
5. **Register converter**: Add your converter to `ConverterFactory` in `NumberToWordsExtension.cs`
6. **Write tests**: Create tests in `src/Humanizer.Tests/Localisation/{culture}/` directory

### Localization Best Practices

- Test all plural forms for the language
- Handle special cases (e.g., "24 de zile" in Romanian, "يومان" in Arabic)
- Follow existing localization patterns in the codebase
- Include test cases for edge cases specific to the language

## File Structure

```
Humanizer/
├── .github/
│   ├── CONTRIBUTING.md
│   ├── CODE_OF_CONDUCT.md
│   └── workflows/
├── src/
│   ├── Humanizer/              # Main library
│   │   ├── Configuration/      # Configuration and registries
│   │   ├── Inflections/        # Pluralization and singularization
│   │   ├── Localisation/       # Localization implementations
│   │   └── Properties/         # Resource files
│   ├── Humanizer.Tests/        # Test suite
│   │   └── Localisation/       # Localization tests by culture
│   └── Benchmarks/             # Performance benchmarks
├── readme.md                   # Main documentation
└── release_notes.md           # Release history
```

## Common Patterns

### Extension Methods

Humanizer provides many extension methods. When adding new ones:

- Place in appropriate file (e.g., string extensions in `StringExtensions.cs`)
- Use descriptive names that indicate the transformation
- Include XML documentation with examples
- Make methods fluent where appropriate

### Fluent API Design

Follow existing fluent API patterns:

```csharp
// Example: Fluent date/time
In.Three.Days
On.January.The4th

// Example: Fluent numbers
1.25.Billions()
3.Hundreds().Thousands()
```

### Resource Keys

When adding new resource strings:

- Use descriptive keys that indicate usage
- Follow existing naming patterns
- Add to all relevant localization files
- Test that resource lookup works correctly

## Documentation

### XML Documentation

- All public APIs must have XML documentation
- Include `<summary>`, `<param>`, `<returns>`, and `<example>` tags where appropriate
- Examples should show actual usage patterns

### README Updates

When adding features:

- Add examples to `readme.md` in the appropriate section
- Use C# code blocks with proper syntax
- Show both input and expected output
- Keep examples concise but meaningful

## Pull Request Guidelines

1. **Branch naming**: Use descriptive branch names (e.g., `feature/add-spanish-localization`)
2. **Link issues**: Reference related issues in PR description using `fixes #123`
3. **Small, focused changes**: Keep PRs focused on a single feature or fix
4. **Rebase on main**: Ensure your branch is rebased on the latest main before submitting
5. **Clean history**: Squash commits if necessary to maintain a clean history
6. **Complete checklist**: Ensure all items in CONTRIBUTING.md checklist are addressed

## Common Pitfalls to Avoid

1. **Don't break existing behavior**: Humanizer is widely used; breaking changes need careful consideration
2. **Don't remove tests**: Existing tests ensure functionality; only modify tests if the behavior intentionally changes
3. **Don't add unnecessary dependencies**: Keep the library lightweight
4. **Don't ignore localization**: Features should work across all supported cultures
5. **Don't skip XML docs**: Public APIs without documentation will be rejected
6. **Don't violate EditorConfig**: Consistent style is important for maintainability

## Getting Help

- Check existing issues and PRs for similar problems
- Look at existing code for patterns and examples
- Review `CONTRIBUTING.md` for detailed guidelines
- For localization help, review existing localization implementations

## Additional Notes

- The project uses semantic versioning
- Breaking changes are tracked and need special consideration
- Code signing is handled automatically in CI/CD
- Test coverage is measured and reported in Azure Pipelines
- The project follows the .NET Foundation Code of Conduct
