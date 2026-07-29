---
name: Humanizer Documentation
description: An editorial working proof sheet for human-friendly .NET APIs.
colors:
  paper: "#f7f3e9"
  paper-raised: "#fffdf7"
  ink: "#172026"
  ink-muted: "#566269"
  rule: "#c9c4b8"
  accent-teal: "#056b84"
  accent-rust: "#ad3e2a"
  proof-text: "#edf3f2"
  proof-output: "#91d9e8"
  promise-teal: "#045b6e"
  white: "#fff"
  promise-text-muted: "#d7f3f8"
  promise-text-soft: "#e5f6f9"
  dark-paper: "#11191c"
  dark-paper-raised: "#192327"
  dark-ink: "#edf3f2"
  dark-ink-muted: "#a9b9b9"
  dark-rule: "#3a494d"
  dark-accent-teal: "#63c4d8"
  dark-accent-rust: "#f28b73"
typography:
  docs-display:
    fontFamily: "Iowan Old Style, Baskerville, Times New Roman, serif"
    fontSize: "clamp(2.25rem, 7vw, 4.5rem)"
    fontWeight: 600
  home-display:
    fontFamily: "Avenir Next, Segoe UI, Helvetica Neue, sans-serif"
    fontSize: "clamp(3rem, 8vw, 6rem)"
    fontWeight: 760
    lineHeight: 0.98
    letterSpacing: "-0.04em"
  section-heading:
    fontFamily: "Avenir Next, Segoe UI, Helvetica Neue, sans-serif"
    fontSize: "clamp(2.3rem, 6vw, 4.75rem)"
    fontWeight: 720
    lineHeight: 1
    letterSpacing: "-0.04em"
  body:
    fontFamily: "Avenir Next, Segoe UI, Helvetica Neue, sans-serif"
    fontSize: "1rem"
    lineHeight: 1.65
  label:
    fontFamily: "Avenir Next, Segoe UI, Helvetica Neue, sans-serif"
    fontSize: "0.78rem"
    fontWeight: 800
    letterSpacing: "0.16em"
  identity:
    fontSize: "0.9rem"
  lede:
    fontSize: "clamp(1.1rem, 2vw, 1.35rem)"
  proof-label:
    fontSize: "0.72rem"
  proof-code:
    fontSize: "clamp(0.76rem, 2.4vw, 0.95rem)"
  route-heading:
    fontSize: "clamp(1.4rem, 3vw, 2rem)"
  reference-heading:
    fontSize: "1.25rem"
  promise-copy:
    fontSize: "1.1rem"
rounded:
  square: "0"
  focus: "0.15rem"
spacing:
  xs: "0.4rem"
  sm: "0.75rem"
  md: "1rem"
  lg: "1.5rem"
  touch-target: "2.75rem"
  home-gutter: "clamp(1rem, 4vw, 4rem)"
  section-block: "clamp(5rem, 10vw, 9rem)"
components:
  button-primary:
    backgroundColor: "{colors.ink}"
    textColor: "{colors.paper}"
    typography: "{typography.body}"
    rounded: "{rounded.square}"
    padding: "0.65rem 1.1rem"
    height: "{spacing.touch-target}"
  button-primary-hover:
    backgroundColor: "{colors.accent-teal}"
    textColor: "#fff"
    rounded: "{rounded.square}"
  proof-sheet:
    backgroundColor: "{colors.ink}"
    textColor: "{colors.proof-text}"
    rounded: "{rounded.square}"
    padding: "clamp(1rem, 3vw, 2rem)"
  language-promise:
    backgroundColor: "{colors.promise-teal}"
    textColor: "#fff"
    rounded: "{rounded.square}"
    padding: "clamp(2rem, 5vw, 4rem)"
---

# Design System: Humanizer Documentation

## Overview

The Working Proof Sheet is a cream-paper and ink editorial world where real C#
input and output prove the product. The homepage uses forceful sans-serif type;
documentation uses an old-style serif for long-form headings. The canonical
logo, ruled composition, restrained teal and rust accents, and code-forward
examples replace decorative imagery, invented statistics, and card grids.

The system is task-first, editorial, flat, version-visible, and accessible.

## Colors

Paper and raised paper form the reading surface. Ink and muted ink carry the
content hierarchy. Teal signals navigation or capability; rust labels structure
or focus. Neither becomes ambient decoration. Dark mode uses the paired dark
tokens rather than inverting arbitrary values.

## Typography

Documentation headings use the serif display stack. Body copy, navigation,
labels, and the homepage use the sans-serif stack. Homepage display text is
tight and heavy; labels are compact and letter-spaced. Reading copy is limited
to 72 characters per line.

The Proof-Sheet Split: editorial serif establishes the reference manual while
precise sans-serif type identifies controls, tasks, and executable proof.

## Layout

The homepage is capped at 90rem with a fluid gutter and generous section
spacing. Task routes become a grid at 48rem. Documentation text remains within
72ch. The shell accounts for 360px and 480px phones and the 996px/997px
navigation transition. Tables stay contained; historical language coverage
keeps its bounded sticky table. Interactive targets are at least 2.75rem high.

## Elevation and Depth

The interface is flat: no cards, gradients, or shadows. Paper tones,
one-pixel rules, and whitespace establish hierarchy. The navigation boundary is
the sole persistent elevation cue.

Ruled-Not-Raised: use a rule or a tone change before introducing visual depth.

## Shapes

Buttons, proof sheets, task routes, and promises are square. A 0.15rem radius is
reserved for focus treatments and existing shell controls.

## Components

- Primary button: ink on paper, changing to teal on hover.
- Text link: underlined or rule-led; never disguised as a pill.
- Proof sheet: dark code surface with distinct output color.
- Task route: ruled text destination with a clear action.
- Language promise: teal full-width statement of the localization contract.
- Navigation: compact, version-visible, and separated by one rule.
- Supported culture list: readable code list, not a partial-capability matrix.

## Do

- Use deterministic, runnable C# examples.
- Reuse the canonical Humanizer logo and existing documentation assets.
- Put the product promise, NuGet install command, and proof in the first viewport.
- Use one-pixel rules, visible focus, and reduced-motion support.
- Keep tables contained and use familiar public API names.

## Do Not

- Add card grids, pills, gradients, decorative shadows, or invented imagery.
- Invent metrics, customers, testimonials, or benchmarks.
- Use nondeterministic examples or hide keyboard focus.
- Permit horizontal page scrolling.
- Present cultures as tiers of partial feature support.
