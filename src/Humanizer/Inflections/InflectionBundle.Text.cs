namespace Humanizer;

sealed partial class InflectionBundle
{
    InflectionResult Project(
        string input,
        string output,
        CaseProjection projection,
        InflectionStatus status)
    {
        if (output.Length == 0 || !IsWellFormedUtf16(output))
        {
            return new(InflectionStatus.Unsupported, input);
        }

        string projected;
        if (casing == InflectionCasing.LowerTitleUpper &&
            projection == CaseProjection.Title)
        {
            if (!TryProjectUpper(output, firstOnly: true, out projected))
            {
                return new(InflectionStatus.Unsupported, input);
            }
        }
        else if (casing == InflectionCasing.LowerTitleUpper &&
                 projection == CaseProjection.Upper)
        {
            if (!TryProjectUpper(output, firstOnly: false, out projected))
            {
                return new(InflectionStatus.Unsupported, input);
            }
        }
        else
        {
            projected = output;
        }

        if (!IsScriptEligible(projected, allowNonLetters: true))
        {
            return new(InflectionStatus.Unsupported, input);
        }

        return string.Equals(projected, input, StringComparison.Ordinal)
            ? new(InflectionStatus.Invariant, input)
            : new(status, projected);
    }

    static bool TryProjectUpper(
        string value,
        bool firstOnly,
        out string projected)
    {
        char[]? characters = null;
        for (var index = 0; index < value.Length;)
        {
            var scalarOffset = index;
            var scalar = ReadScalar(value, ref index, value.Length);
            var upper = InflectionUnicodeData.ToUpperSimple(scalar);
            if (upper != scalar)
            {
                var scalarLength = index - scalarOffset;
                var upperLength = upper <= char.MaxValue ? 1 : 2;
                if (upperLength != scalarLength)
                {
                    projected = value;
                    return false;
                }

                characters ??= value.ToCharArray();
                if (upperLength == 1)
                {
                    characters[scalarOffset] = (char)upper;
                }
                else
                {
                    var supplementary = upper - 0x10000;
                    characters[scalarOffset] =
                        (char)((supplementary / 0x400) + 0xD800);
                    characters[scalarOffset + 1] =
                        (char)((supplementary % 0x400) + 0xDC00);
                }
            }

            if (firstOnly)
            {
                break;
            }
        }

        projected = characters is null ? value : new(characters);
        return true;
    }

    static CaseProjection GetProjection(string value)
    {
        if (value.Length == 0)
        {
            return CaseProjection.Exact;
        }

        if (IsAscii(value))
        {
            var hasLower = false;
            var hasUpper = false;
            var hasUpperAfterFirst = false;
            for (var index = 0; index < value.Length; index++)
            {
                var character = value[index];
                hasLower |= character is >= 'a' and <= 'z';
                if (character is >= 'A' and <= 'Z')
                {
                    hasUpper = true;
                    hasUpperAfterFirst |= index > 0;
                }
            }

            if (!hasUpper)
            {
                return CaseProjection.Lower;
            }

            if (!hasLower)
            {
                return CaseProjection.Upper;
            }

            return value[0] is >= 'A' and <= 'Z' && !hasUpperAfterFirst
                ? CaseProjection.Title
                : CaseProjection.Mixed;
        }

        var hasUnicodeLower = false;
        var hasUnicodeUpper = false;
        var hasUnicodeUpperAfterFirst = false;
        var firstScalarIsUpper = false;
        for (var index = 0; index < value.Length;)
        {
            var isFirstScalar = index == 0;
            var scalar = ReadScalar(value, ref index, value.Length);
            var unicodeCase = InflectionUnicodeData.GetCase(scalar);
            if (unicodeCase == InflectionUnicodeCase.Title)
            {
                return CaseProjection.Mixed;
            }

            if (unicodeCase == InflectionUnicodeCase.Lower)
            {
                hasUnicodeLower = true;
            }
            else if (unicodeCase == InflectionUnicodeCase.Upper)
            {
                hasUnicodeUpper = true;
                firstScalarIsUpper |= isFirstScalar;
                hasUnicodeUpperAfterFirst |= !isFirstScalar;
            }
        }

        if (!hasUnicodeUpper)
        {
            return CaseProjection.Lower;
        }

        if (!hasUnicodeLower)
        {
            return CaseProjection.Upper;
        }

        return firstScalarIsUpper && !hasUnicodeUpperAfterFirst
            ? CaseProjection.Title
            : CaseProjection.Mixed;
    }

    bool IsEligibleToken(
        string value,
        out InflectionUnicodeScripts detectedScripts)
    {
        var comparison = casing == InflectionCasing.LowerTitleUpper
            ? StringComparison.OrdinalIgnoreCase
            : StringComparison.Ordinal;
        if (Contains(skipSimpleWords, value, comparison))
        {
            detectedScripts = InflectionUnicodeScripts.None;
            return false;
        }

        return TryGetDetectedScripts(
            value,
            allowNonLetters: false,
            out detectedScripts);
    }

    bool IsScriptEligible(string value, bool allowNonLetters)
        => TryGetDetectedScripts(value, allowNonLetters, out _);

    bool TryGetDetectedScripts(
        string value,
        bool allowNonLetters,
        out InflectionUnicodeScripts detectedScripts)
    {
        var allowedScripts = InflectionUnicodeScripts.None;
        foreach (var script in scripts)
        {
            if (!InflectionUnicodeData.TryGetScript(script, out var scriptValue))
            {
                detectedScripts = InflectionUnicodeScripts.None;
                return false;
            }

            allowedScripts |= scriptValue;
        }

        detectedScripts = allowedScripts;
        var hasLetter = false;
        for (var index = 0; index < value.Length; index++)
        {
            var scalar = char.ConvertToUtf32(value, index);
            var category = CharUnicodeInfo.GetUnicodeCategory(value, index);
            if (char.IsHighSurrogate(value[index]))
            {
                index++;
            }

            var pinnedLetter = false;
            var pinnedMark = false;
            if (category == UnicodeCategory.OtherNotAssigned)
            {
                _ = InflectionUnicodeData.TryGetPinnedLetterOrMark(
                    scalar,
                    out pinnedLetter,
                    out pinnedMark);
            }

            var isLetter = pinnedLetter ||
                category is UnicodeCategory.UppercaseLetter or
                UnicodeCategory.LowercaseLetter or
                UnicodeCategory.TitlecaseLetter or
                UnicodeCategory.ModifierLetter or
                UnicodeCategory.OtherLetter;
            var isMark = pinnedMark ||
                category is UnicodeCategory.NonSpacingMark or
                UnicodeCategory.SpacingCombiningMark or
                UnicodeCategory.EnclosingMark;
            if (!isLetter && !isMark)
            {
                if (allowNonLetters)
                {
                    continue;
                }

                detectedScripts = InflectionUnicodeScripts.None;
                return false;
            }

            var scalarScripts = InflectionUnicodeData.GetScripts(scalar);
            if ((scalarScripts & allowedScripts) == InflectionUnicodeScripts.None)
            {
                detectedScripts = InflectionUnicodeScripts.None;
                return false;
            }

            detectedScripts &= scalarScripts;
            if (detectedScripts == InflectionUnicodeScripts.None)
            {
                return false;
            }

            hasLetter |= isLetter;
        }

        return hasLetter;
    }

    static bool IsWellFormedUtf16(string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            if (char.IsHighSurrogate(value[index]))
            {
                if (++index >= value.Length || !char.IsLowSurrogate(value[index]))
                {
                    return false;
                }
            }
            else if (char.IsLowSurrogate(value[index]))
            {
                return false;
            }
        }

        return true;
    }

    static bool IsDefinitelyNormalized(string value)
    {
        for (var index = 0; index < value.Length; index++)
        {
            var scalar = char.ConvertToUtf32(value, index);
            var category = CharUnicodeInfo.GetUnicodeCategory(value, index);
            if (category is UnicodeCategory.NonSpacingMark or
                UnicodeCategory.SpacingCombiningMark or
                UnicodeCategory.EnclosingMark ||
                InflectionUnicodeData.HasUncertainNfcQuickCheck(scalar))
            {
                return false;
            }

            if (char.IsHighSurrogate(value[index]))
            {
                index++;
            }
        }

        return true;
    }

    enum CaseProjection
    {
        Exact,
        Lower,
        Title,
        Upper,
        Mixed
    }
}