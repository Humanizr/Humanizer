#!/usr/bin/env python3
"""
Example Usage - Irregular Plurals Dataset

This script demonstrates how to load and use the generated irregular plurals dataset.
"""

import json
from pathlib import Path


def main():
    """Demonstrate dataset usage"""
    
    # Load the dataset
    dataset_path = Path(__file__).parent / "output" / "irregular_plurals.json"
    
    if not dataset_path.exists():
        print(f"Error: Dataset not found at {dataset_path}")
        print("Please run build_dataset.py first to generate the dataset.")
        return
    
    with open(dataset_path, 'r', encoding='utf-8') as f:
        dataset = json.load(f)
    
    print(f"Loaded {len(dataset)} irregular plural entries\n")
    
    # Example 1: Find all internal vowel shift plurals
    print("=" * 60)
    print("Example 1: Internal Vowel Shift Plurals")
    print("=" * 60)
    vowel_shifts = [
        entry for entry in dataset 
        if "internal vowel shift" in entry['category']
    ]
    for entry in vowel_shifts:
        print(f"  {entry['singular']:15} → {', '.join(entry['plurals'])}")
    
    # Example 2: Find all words with multiple accepted plural forms
    print("\n" + "=" * 60)
    print("Example 2: Words with Multiple Accepted Plurals")
    print("=" * 60)
    multiple_plurals = [
        entry for entry in dataset
        if len(entry['plurals']) > 1
    ]
    print(f"Found {len(multiple_plurals)} words with multiple plural forms\n")
    for entry in multiple_plurals[:10]:
        print(f"  {entry['singular']:15} → {', '.join(entry['plurals'])}")
    print(f"  ... and {len(multiple_plurals) - 10} more")
    
    # Example 3: Find all Latin/Greek irregular plurals in mathematics
    print("\n" + "=" * 60)
    print("Example 3: Mathematical Latin/Greek Irregulars")
    print("=" * 60)
    math_terms = [
        entry for entry in dataset
        if "mathematics" in entry.get('domain_tags', [])
    ]
    for entry in math_terms:
        print(f"  {entry['singular']:15} → {', '.join(entry['plurals'])}")
    
    # Example 4: Find all invariant plurals (same form for singular and plural)
    print("\n" + "=" * 60)
    print("Example 4: Invariant Plurals")
    print("=" * 60)
    invariants = [
        entry for entry in dataset
        if "invariant plural" in entry['category']
        and not entry['plural_only']  # Exclude plural-only nouns
    ]
    print(f"Found {len(invariants)} invariant plurals (singular = plural)\n")
    invariant_words = [entry['singular'] for entry in invariants[:20]]
    print(f"  {', '.join(invariant_words)}")
    
    # Example 5: Find all compound irregulars
    print("\n" + "=" * 60)
    print("Example 5: Compound Irregular Plurals")
    print("=" * 60)
    compounds = [
        entry for entry in dataset
        if "compound irregular" in entry['category']
    ]
    for entry in compounds:
        print(f"  {entry['singular']:25} → {', '.join(entry['plurals'])}")
    
    # Example 6: Statistics by origin
    print("\n" + "=" * 60)
    print("Example 6: Statistics by Origin")
    print("=" * 60)
    from collections import Counter
    origins = Counter(entry['origin'] for entry in dataset)
    print(f"{'Origin':<30} {'Count':>5}")
    print("-" * 37)
    for origin, count in origins.most_common(10):
        print(f"{origin:<30} {count:>5}")
    
    # Example 7: Create a lookup function
    print("\n" + "=" * 60)
    print("Example 7: Lookup Function")
    print("=" * 60)
    
    # Create a lookup dictionary
    lookup = {entry['singular']: entry for entry in dataset}
    
    # Test some lookups
    test_words = ["person", "phenomenon", "sheep", "mother-in-law", "datum"]
    for word in test_words:
        if word in lookup:
            entry = lookup[word]
            print(f"\n  {word}:")
            print(f"    Plural(s): {', '.join(entry['plurals'])}")
            print(f"    Category: {', '.join(entry['category'])}")
            print(f"    Origin: {entry['origin']}")
    
    print("\n" + "=" * 60)
    print("For more examples, see the generated files:")
    print("  - irregular_plurals.json (structured data)")
    print("  - irregular_plurals.csv (tabular data)")
    print("  - irregular_plurals.md (human-readable)")
    print("  - summary_report.md (statistics)")
    print("=" * 60)


if __name__ == "__main__":
    main()
