# Product

<!-- impeccable:product-schema 1 -->

## Platform

web

## Users

The primary reader is a .NET developer who has a value to present and wants
the shortest correct Humanizer call. Contributors are a secondary audience
served by dedicated contribution references.

## Product Purpose

Humanizer is a mature .NET library that turns identifiers, dates, times,
numbers, quantities, enums, and collections into text people can read.
Humanizr.net is its versioned documentation and API reference. Success means a
developer can identify the applicable API, install the matching NuGet package,
and verify the call without learning Humanizer's internal localization model.

## Positioning

Humanizer exposes focused, familiar .NET extension methods such as
`Humanize`, `Pluralize`, and `Singularize`. Localization is part of those
operations rather than a separate formatting pass.

## Operating Context

Readers arrive with a typed .NET value and use the site to install Humanizer,
find a task guide, copy a runnable example, or confirm an exact signature.
Versioned guides, examples, search results, and API references must stay aligned
with the selected NuGet package.

## Capabilities and Constraints

- Locale support is all-or-nothing: every listed culture supports every
  applicable localized feature. Missing behavior or English fallback is a bug.
- Current application documentation describes Humanizer 4 as a NuGet package.
  Source checkout and build instructions belong only in contributor guides.
- Historical documentation remains version-correct and must not inherit
  unreleased APIs or current locale claims.
- Application documentation uses public API language, not source-generator
  ownership, locale inheritance, or linguistic implementation terminology.

## Brand Commitments

Preserve the Humanizer name, canonical logo, NuGet package identity, and
direct developer-to-developer voice. Do not invent customers, testimonials,
benchmarks, or product imagery.

## Evidence on Hand

- Canonical logo: `website/static/img/logo.png`
- Runnable documentation examples: `website/docs/_examples`
- Generated API reference: `website/docs/api`
- Per-version manifest: `website/humanizer-versions.json`
- Supported-culture inventory: `website/docs/languages/language-coverage.json`

## Product Principles

- Start from the programming task or value the reader already has.
- Show runnable C# before detailed explanation.
- Prefer familiar public API terms over internal taxonomy.
- Keep every claim version-correct and behaviorally verified.
- Treat accessibility as a release requirement.

## Accessibility & Inclusion

Meet WCAG AA contrast, preserve visible keyboard focus, support reduced motion,
avoid horizontal page scrolling, and keep touch targets usable at narrow mobile
widths.
