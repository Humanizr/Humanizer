# Quick Start Guide - Irregular Plurals Dataset

This guide will help you get started with the irregular plurals dataset builder in under 5 minutes.

## Step 1: Build the Dataset

```bash
cd scripts/irregular-plurals-dataset
python3 build_dataset.py
```

**Expected output:**
- The script will process 216+ irregular plural entries
- It validates each entry against Wiktionary and Merriam-Webster
- Progress is shown for each entry
- Takes approximately 2-3 minutes to complete

**Output files created in `output/` directory:**
```
output/
├── irregular_plurals.json      # Structured JSON data
├── irregular_plurals.csv       # Tabular CSV format
├── irregular_plurals.md        # Markdown tables
├── summary_report.md           # Statistics report
└── irregular_plurals_v1.zip    # Complete archive
```

## Step 2: View Example Usage

```bash
python3 example_usage.py
```

This demonstrates:
- Loading and parsing the JSON dataset
- Filtering by categories (internal vowel shift, Latin/Greek, etc.)
- Finding words with multiple plural forms
- Searching by domain tags (mathematics, biology)
- Creating lookup functions for programmatic access

## Step 3: Validate the Dataset

```bash
python3 validate_dataset.py
```

This verifies:
- ✓ All 216 entries meet schema requirements
- ✓ Each entry has ≥2 authoritative sources
- ✓ All 9 expected categories are covered
- ✓ URL validation rate (99.5%)
- ✓ No duplicate entries
- ✓ Complete pattern type representation

## Quick Examples

### Load and Query the Dataset

```python
import json
from pathlib import Path

# Load dataset
with open('output/irregular_plurals.json') as f:
    dataset = json.load(f)

# Find all vowel shift plurals
vowel_shifts = [
    entry for entry in dataset 
    if "internal vowel shift" in entry['category']
]

# Look up a specific word
lookup = {entry['singular']: entry for entry in dataset}
person_entry = lookup['person']
print(person_entry['plurals'])  # ['people', 'persons']
```

### Access CSV Data

```python
import csv

with open('output/irregular_plurals.csv', 'r', encoding='utf-8') as f:
    reader = csv.DictReader(f)
    for row in reader:
        print(f"{row['singular']} → {row['plurals']}")
```

## Dataset Statistics

- **Total entries:** 216
- **Categories:** 9 distinct pattern types
- **Origins:** 27 different etymological sources
- **Validation:** 99.5% URL accessibility, 100% source agreement
- **Multiple forms:** 79 entries with ambiguous/multiple accepted plurals

## Key Features

1. **Comprehensive Coverage**
   - Internal vowel shifts (man→men)
   - Latin/Greek irregulars (phenomenon→phenomena)
   - Invariant plurals (sheep→sheep)
   - Compound irregulars (mother-in-law→mothers-in-law)
   - And 5 more pattern types

2. **High-Quality Metadata**
   - Etymology/origin information
   - Regional variants (US/UK)
   - Domain tags (biology, mathematics, etc.)
   - Multiple accepted forms
   - Authoritative citations with URLs

3. **Multiple Output Formats**
   - JSON for programmatic access
   - CSV for spreadsheet tools
   - Markdown for documentation
   - ZIP for easy distribution

## Troubleshooting

**Issue:** Script fails with network error  
**Solution:** The script fetches data from Wiktionary and Merriam-Webster. Ensure you have internet connectivity. Rate limiting is already implemented.

**Issue:** Output directory not found  
**Solution:** The script creates the `output/` directory automatically. Ensure you have write permissions.

**Issue:** Example script can't find dataset  
**Solution:** Run `build_dataset.py` first to generate the dataset files.

## Next Steps

- Read the full [README.md](README.md) for detailed documentation
- Explore the generated `summary_report.md` for statistics
- Browse `irregular_plurals.md` for a human-readable table
- Use the JSON/CSV files in your own applications

## Support

For issues or questions, refer to:
- Main documentation: [README.md](README.md)
- Dataset validation: Run `validate_dataset.py`
- Usage examples: Run `example_usage.py`
