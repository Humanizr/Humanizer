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

For ordinary applications, install `Humanizer`. That name is stable even though the package's internal layout changed across release lines.

| Documentation version | Normal install | Package shape |
| --- | --- | --- |
| `2.10.1` through `3.0.10` | `Humanizer` | Metapackage that brings in `Humanizer.Core` and locale packages |
| `4.0` | `Humanizer` | Consolidated library with runtime, generated locale data, and analyzers |

The API reference for releases through `3.0.10` is generated from `Humanizer.Core`, because that is where the public implementation assembly lives. That does not change the normal install command.

## Example

Install Humanizer from NuGet:

```console
dotnet add package Humanizer --version 4.0.0
```

Then use the same `Humanizer` namespace regardless of package layout. This
verified program is compiled against the selected package:

<CodeBlock language="csharp" title="Program.cs">{quickStart}</CodeBlock>

Choose `Humanizer.Core` directly only when you deliberately want the older release line's English-only core or must work around tooling that cannot restore its locale metapackage. Add only locale packages that exist for that exact release.

## Pitfall

Do not copy `Humanizer.Core.<locale>` references from a historical release into
Humanizer 4. Locale packaging is version-specific, and package names are not
proof that an API is present in another release.

## Version notes

Package selection is a compatibility concern, not an API preference. Keep the documentation selector aligned with `project.assets.json` or your lock file when diagnosing restore or runtime behavior.

## Related guides and API

- [Installation](./installation.mdx)
- [Localization and extensibility](../scenarios/localization-and-extensibility.mdx)
- [Selected-version API reference](../api/index.md)
