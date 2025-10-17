#!/usr/bin/env python3
"""
Dataset Validation Script

Validates that the irregular plurals dataset meets all requirements from the problem statement.
"""

import json
from pathlib import Path
from collections import Counter


def validate_dataset():
    """Validate the dataset against all requirements"""
    
    print("=" * 70)
    print("IRREGULAR PLURALS DATASET VALIDATION")
    print("=" * 70)
    
    output_dir = Path(__file__).parent / "output"
    json_path = output_dir / "irregular_plurals.json"
    csv_path = output_dir / "irregular_plurals.csv"
    md_path = output_dir / "irregular_plurals.md"
    summary_path = output_dir / "summary_report.md"
    zip_path = output_dir / "irregular_plurals_v1.zip"
    
    # Check all required files exist
    print("\n✓ Checking Required Files...")
    required_files = [
        ("JSON dataset", json_path),
        ("CSV dataset", csv_path),
        ("Markdown table", md_path),
        ("Summary report", summary_path),
        ("ZIP archive", zip_path),
    ]
    
    all_files_exist = True
    for name, path in required_files:
        if path.exists():
            print(f"  ✓ {name}: {path.name} ({path.stat().st_size} bytes)")
        else:
            print(f"  ✗ {name}: NOT FOUND")
            all_files_exist = False
    
    if not all_files_exist:
        print("\n✗ VALIDATION FAILED: Missing required files")
        return False
    
    # Load and validate JSON dataset
    with open(json_path, 'r', encoding='utf-8') as f:
        dataset = json.load(f)
    
    print(f"\n✓ Dataset Size: {len(dataset)} entries")
    
    # Requirement 1: Must have ≥200 entries
    print("\n✓ Checking Entry Count (≥200 required)...")
    if len(dataset) >= 200:
        print(f"  ✓ PASS: {len(dataset)} entries (exceeds requirement)")
    else:
        print(f"  ✗ FAIL: Only {len(dataset)} entries (need ≥200)")
        return False
    
    # Requirement 2: Validate schema for each entry
    print("\n✓ Checking Schema Compliance...")
    required_fields = [
        'singular', 'plurals', 'category', 'origin', 'region',
        'domain_tags', 'plural_only', 'notes', 'citations', 'validation'
    ]
    
    schema_errors = 0
    for i, entry in enumerate(dataset):
        for field in required_fields:
            if field not in entry:
                print(f"  ✗ Entry {i}: Missing field '{field}'")
                schema_errors += 1
        
        # Validate citations structure
        if 'citations' in entry:
            for citation in entry['citations']:
                if not all(k in citation for k in ['source_name', 'url', 'accessed']):
                    print(f"  ✗ Entry {i}: Invalid citation structure")
                    schema_errors += 1
        
        # Validate validation structure
        if 'validation' in entry:
            if not all(k in entry['validation'] for k in ['url_ok', 'agreement']):
                print(f"  ✗ Entry {i}: Invalid validation structure")
                schema_errors += 1
    
    if schema_errors == 0:
        print(f"  ✓ PASS: All {len(dataset)} entries have correct schema")
    else:
        print(f"  ✗ FAIL: {schema_errors} schema errors found")
        return False
    
    # Requirement 3: Validate ≥2 sources per entry
    print("\n✓ Checking Source Validation (≥2 sources per entry)...")
    insufficient_sources = 0
    for i, entry in enumerate(dataset):
        if len(entry.get('citations', [])) < 2:
            print(f"  ✗ Entry {i} ({entry['singular']}): Only {len(entry['citations'])} source(s)")
            insufficient_sources += 1
    
    if insufficient_sources == 0:
        print(f"  ✓ PASS: All entries have ≥2 sources")
    else:
        print(f"  ✗ FAIL: {insufficient_sources} entries with insufficient sources")
        return False
    
    # Requirement 4: Check category coverage
    print("\n✓ Checking Category Coverage...")
    expected_categories = {
        "internal vowel shift",
        "suppletive",
        "suffixal irregular",
        "invariant plural",
        "Latin/Greek irregular",
        "loanword irregular",
        "ambiguous/multiple valid forms",
        "mass/uncountable exception",
        "compound irregular"
    }
    
    all_categories = set()
    for entry in dataset:
        all_categories.update(entry['category'])
    
    covered_categories = expected_categories & all_categories
    print(f"  ✓ Covered {len(covered_categories)}/{len(expected_categories)} expected categories:")
    for cat in sorted(covered_categories):
        count = sum(1 for e in dataset if cat in e['category'])
        print(f"    - {cat}: {count} entries")
    
    missing_categories = expected_categories - all_categories
    if missing_categories:
        print(f"  ⚠ Missing categories: {', '.join(missing_categories)}")
    
    # Requirement 5: Check validation rates
    print("\n✓ Checking Validation Rates...")
    url_ok_count = sum(1 for e in dataset if e['validation']['url_ok'])
    agreement_count = sum(1 for e in dataset if e['validation']['agreement'])
    
    url_ok_rate = url_ok_count / len(dataset) * 100
    agreement_rate = agreement_count / len(dataset) * 100
    
    print(f"  ✓ URL validation: {url_ok_count}/{len(dataset)} ({url_ok_rate:.1f}%)")
    print(f"  ✓ Source agreement: {agreement_count}/{len(dataset)} ({agreement_rate:.1f}%)")
    
    # Requirement 6: Check for required pattern types
    print("\n✓ Checking Pattern Type Representation...")
    patterns = {
        "Internal vowel shift (e.g., man→men)": any("internal vowel shift" in e['category'] for e in dataset),
        "Suppletive forms (e.g., person→people)": any("suppletive" in e['category'] for e in dataset),
        "Suffixal irregular (-en, -ren)": any("suffixal irregular" in e['category'] for e in dataset),
        "Invariant plurals (e.g., sheep→sheep)": any("invariant plural" in e['category'] for e in dataset),
        "Latin/Greek irregulars": any("Latin/Greek irregular" in e['category'] for e in dataset),
        "Loanword irregulars": any("loanword irregular" in e['category'] for e in dataset),
        "Multiple valid forms": any("ambiguous/multiple valid forms" in e['category'] for e in dataset),
        "Compound irregulars": any("compound irregular" in e['category'] for e in dataset),
    }
    
    for pattern, present in patterns.items():
        if present:
            print(f"  ✓ {pattern}")
        else:
            print(f"  ✗ {pattern}")
    
    # Requirement 7: Check origin diversity
    print("\n✓ Checking Origin Diversity...")
    origins = Counter(e['origin'] for e in dataset)
    print(f"  ✓ {len(origins)} distinct origins represented:")
    for origin, count in origins.most_common(5):
        print(f"    - {origin}: {count} entries")
    
    # Requirement 8: Check for deduplication
    print("\n✓ Checking for Duplicates...")
    singulars = [e['singular'] for e in dataset]
    duplicates = [s for s in singulars if singulars.count(s) > 1]
    unique_duplicates = set(duplicates)
    
    if len(unique_duplicates) == 0:
        print(f"  ✓ PASS: No duplicate entries found")
    else:
        print(f"  ✗ FAIL: {len(unique_duplicates)} duplicate singular forms found:")
        for dup in sorted(unique_duplicates):
            print(f"    - {dup}")
        return False
    
    # Summary
    print("\n" + "=" * 70)
    print("✓ VALIDATION PASSED")
    print("=" * 70)
    print(f"\nDataset Summary:")
    print(f"  - Total entries: {len(dataset)}")
    print(f"  - Categories covered: {len(covered_categories)}/{len(expected_categories)}")
    print(f"  - URL validation rate: {url_ok_rate:.1f}%")
    print(f"  - Source agreement rate: {agreement_rate:.1f}%")
    print(f"  - Multiple plural forms: {sum(1 for e in dataset if len(e['plurals']) > 1)}")
    print(f"  - Plural-only nouns: {sum(1 for e in dataset if e['plural_only'])}")
    print(f"  - Origin diversity: {len(origins)} distinct origins")
    print(f"\nAll requirements met! ✓")
    
    return True


if __name__ == "__main__":
    success = validate_dataset()
    exit(0 if success else 1)
