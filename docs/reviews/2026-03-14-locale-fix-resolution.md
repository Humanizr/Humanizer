# Locale Fix Resolution

Generated: 2026-03-14

Applied only second-pass `confirmed` and `modified` findings. No `rejected` findings were implemented.

## Commits

- `3579e2bb` `fix: apply adjudicated spanish locale fixes`
  - Locale: `es`

- `3096162b` `fix: apply adjudicated locale fixes for ca th uz-cyrl zh-hant`
  - Locales: `ca`, `th`, `uz-Cyrl-UZ`, `zh-Hant`

- `ed78b2d3` `fix: apply adjudicated zh-hant locale fixes`
  - Locale: `zh-Hant`
  - Superseded by `3096162b` for the final combined branch state.

- `6f1092b1` `fix: apply adjudicated locale fixes for af ar bg el fil he it pt-br`
  - Locales: `af`, `ar`, `bg`, `el`, `fil`, `he`, `it`, `pt-BR`

- `7201c5a1` `fix: apply adjudicated locale fixes for az bn fa hu is mt sv uz-latn`
  - Locales: `az`, `bn`, `fa`, `hu`, `is`, `mt`, `sv`, `uz-Latn-UZ`

- `ce055961` `fix: apply adjudicated locale fixes for cs fr hr hy id ko lv ms nb sr vi`
  - Locales: `cs`, `fr`, `hr`, `hy`, `id`, `ko`, `lv`, `ms`, `nb`, `sr`, `vi`
  - Includes the shared `nb` runtime fix in `FormatterRegistry` to stop English `DataUnit` plural leakage in `ByteSize.ToFullWords()`.

## Notes

- Review artifacts under `docs/reviews` and `docs/reviews/second-pass` were preserved as historical inputs and were not rewritten.
- The implementation followed the second-pass adjudication as the only source of truth.
- No deferred locale fixes remain from the adjudicated actionable set.
