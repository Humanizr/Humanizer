---
id: package-selection
title: Choose the right package
sidebar_position: 3
diataxis: explanation
persona: developer choosing dependencies
---

import CodeBlock from '@theme/CodeBlock';
import quickStart from '!!raw-loader!../_examples/quick-start/Program.cs';

# Choose the right package

## Orientation

For ordinary applications using Humanizer `2.14.1`, install `Humanizer`.

| Documentation version | Normal install | Package shape |
| --- | --- | --- |
| `2.14.1` | `Humanizer` | Metapackage that brings in `Humanizer.Core` and locale packages |

The selected API reference is generated from `Humanizer.Core`, because that is where the public implementation assembly lives. That does not change the normal install command.

## Example

Install the selected stable version:

```console
dotnet add package Humanizer --version 2.14.1
```

Then use the `Humanizer` namespace. This verified program is compiled and run against the selected `2.14.1` package:

<CodeBlock language="csharp" title="Program.cs">{quickStart}</CodeBlock>

Choose `Humanizer.Core` directly only when you deliberately want the English-only core or must work around tooling that cannot restore the locale metapackage. Add only locale packages that exist for `2.14.1`.

## Pitfall

Do not copy `Humanizer.Core.<locale>` references from another release. Locale packaging is version-specific, and a package name is not proof that an API or language surface exists in `2.14.1`.

## Version notes

Keep the documentation selector aligned with `project.assets.json` or your lock file when diagnosing restore or runtime behavior.

## Related guides and API

- [Installation](./installation.mdx)
- [Localization and extensibility](../scenarios/localization-and-extensibility.mdx)
- [2.14.1 API reference](../api/index.md)
