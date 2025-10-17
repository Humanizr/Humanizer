# Irregular Plurals Dataset Builder

This tool builds a comprehensive, validated dataset of English irregular plural nouns from authoritative online sources.

## Overview

The dataset builder collects and validates irregular plural nouns across multiple categories:

- **Internal vowel shift** (e.g., man → men, foot → feet)
- **Suppletive forms** (e.g., person → people)
- **Suffixal irregular** (-en, -ren patterns)
- **Invariant plurals** (e.g., sheep → sheep)
- **Latin/Greek irregular** (e.g., phenomenon → phenomena, index → indices)
- **Loanword irregular** (French, Hebrew, etc.)
- **Ambiguous/multiple valid forms** (e.g., cactus → cacti/cactuses)
- **Mass/uncountable exceptions**
- **Compound irregular** (e.g., mothers-in-law)
- **Plural-only nouns** (e.g., scissors, pants)

## Requirements

- Python 3.8 or higher
- No external dependencies (uses only Python standard library)

## Usage

### Running the Script

```bash
cd scripts/irregular-plurals-dataset
python3 build_dataset.py
```

### Output

The script generates the following files in the `output/` directory:

1. **irregular_plurals.json** - Structured JSON with full metadata
2. **irregular_plurals.csv** - Flat CSV table (arrays pipe-delimited)
3. **irregular_plurals.md** - Markdown table grouped by category
4. **summary_report.md** - Statistical analysis and validation report
5. **irregular_plurals_v1.zip** - Archive containing all above files

### Output Directory Structure

```
scripts/irregular-plurals-dataset/
├── build_dataset.py
├── README.md
└── output/
    ├── irregular_plurals.json
    ├── irregular_plurals.csv
    ├── irregular_plurals.md
    ├── summary_report.md
    └── irregular_plurals_v1.zip
```

## Data Schema

Each entry in the dataset includes:

```json
{
  "singular": "phenomenon",
  "plurals": ["phenomena"],
  "category": ["Latin/Greek irregular"],
  "origin": "Greek",
  "region": "US/UK",
  "domain_tags": ["science"],
  "plural_only": false,
  "notes": "...",
  "citations": [
    {
      "source_name": "Wiktionary",
      "url": "https://en.wiktionary.org/wiki/phenomenon",
      "accessed": "2025-10-17"
    }
  ],
  "validation": {
    "url_ok": true,
    "agreement": true
  }
}
```

## Data Sources

The script validates entries against multiple authoritative sources:

- **Wiktionary** (en.wiktionary.org) - Community-maintained multilingual dictionary
- **Merriam-Webster** (merriam-webster.com) - Authoritative American English dictionary
- Linguistic references and established corpus data

Each entry is validated against ≥2 sources to ensure accuracy.

## Features

- **200+ entries** covering all major irregular plural patterns
- **Multi-source validation** with URL checking
- **Comprehensive metadata** including etymology, regional variants, and domain tags
- **Pattern categorization** for linguistic analysis
- **Multiple output formats** for different use cases
- **Statistical summary** with distribution analysis

## Dataset Statistics

The generated dataset includes:

- Total entries: 200+
- Categories: 9 distinct pattern types
- Origins tracked: Latin, Greek, Old English, French, Hebrew, etc.
- Regional variants: US, UK, or US/UK
- Domain tags: biology, mathematics, science, etc.

## Notes

- The script uses rate limiting to be respectful to external servers
- URL validation checks accessibility of cited sources
- Duplicate entries are automatically deduplicated
- Both commonly accepted plural forms are included for ambiguous cases
- The seed list is based on established linguistic references

## Contributing

To expand the dataset:

1. Add entries to `SEED_IRREGULARS` or `ADDITIONAL_IRREGULARS` lists in the script
2. Follow the format: `(singular, [plurals], [categories], origin, region)`
3. Ensure each entry has proper categorization and etymological information
4. Run the script to regenerate the dataset

## License

This dataset is intended for educational and research purposes. The script itself follows the Humanizer project's MIT license.
