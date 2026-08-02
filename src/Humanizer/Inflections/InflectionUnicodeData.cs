namespace Humanizer;

/// <summary>
/// Provides the pinned Unicode data used by localized inflection validation.
/// </summary>
internal static class InflectionUnicodeData
{
    // Unicode 16.0.0 letter and mark categories, compressed into 998 ranges.
    // UnicodeData.txt SHA-256:
    // ff58e5823bd095166564a006e47d111130813dcf8bf234ef79fa51a870edb48f.
    const int UnicodeLetterMarkRangeCount = 998;
    const string UnicodeLetterMarkData = """
QTIgMkkACwAFAAYsGDwgkgfOAxYaCAwAAgAS3wFwCAYCBAYFAAcAAgQEAAImFaQBVJQCjAENB8oCpwFKKAAHUDFZLgECAwMDAwEJ
NB8GIRUQVCspIwICAQHEAWQAAQ0JCwYCAgMDBwQCDAQFABEAAQEBOh41HbABWRULABlAIREJAgYAAwEDKhYHBAABEQkAAQUDAAEJ
FzAZBQcUEC4ZCg4RCVIqLxlBIWo2BQMAASMSAAENBxIKAw8eEAUEDgoCBCoXDAgABAYGAQEAAQ0JAwQFAwAJAQUCAwQDAw4CDAAC
AQMFBAoKAgQqFwwIAgMCAwIEAQIJCQMEBQYBCAYFABIDAgQDAQwFBBAKBAQqFwwIAgMIBwEBAAEPCQUEBQUAEAICAxcAAQsHBQQO
CgIEKhcMCAIDCAcBAQABDQkDBAUKBQcCAwQDAw8AEQEBAAIKCQQEBgcCAwACAgUCBQQGFhAJCAUEBwYABwEpCQUOCQQELBgeEgEB
AAENCAUEBwsDAwQFAAMCAgMeAAEFBA4JBAQsGBILCAcBAQABDQgFBAcLAwgCAwICAw8CAgENBwQQCgQEUCkDAgABDQgFBAcEAAYE
AwEIBAMDGAoHBQQiFS4ZEAoAAwwKAQULBwECDxoDD14wAQECAg0MDAcPOgIDAAIIBi4ZAAISCgEBAgIRCQADCAYAAg0UBiQAGAMd
AQIBAgEFAwIOCUYoJxUDAggFFQxHLQE6VCsnFAARCgYHBAYEBQMAAQUDAgINBwQDBwQYDRcMAAEBCwcGSicABgADVCyYBc4CBgYM
CAACBgZQKgYGQCIGBgwIAAIGBhwQcDoGBoQBRQUjHiCqAVgKCdYJ7gQgEjIflAFRDg8iEgcNJBMFDiISAw4YDgQEAw5mND8jAAUA
AQEuBQQBEbABYAgFAwJCIgEBAAaKAVA8IBcQFyA6IAgQVjAyUCwXCQloNRMLOR8BKAAJPVAJBVwvIREOJhEVBQM6HhkNAgxWLBsa
RiQnKQQNRiYUEFQtBBMFBCkVBgQBAQoGAQECAgUDAAb+AsABf0CqBJgCCghKKAoIDgkAAgACAAI8IWg2DAgABAQEDAoGBgoKGBIE
BAx7AA4AERhAQTIABQADEgsABAgLAAIAAgACBgUUDQYJCAkANQL9FMgD6wEGBAUDAg5KJwAGAANuPwAQAQEsIAwIDAgMCAwIDAgM
CAwIDAg/TwDWAwIlCwcICgIGqgFYAwQEBLIBWwYJVCy6AW8+UB6QBP5mgDSY2gLQrQFaMJgEkAIeGgIWXC8HBRMLPB8DAooBUAMn
EAvMAWmEAUUCAwACDh0eEAEBBAMBAQYEAQEsFwkJARRmQAMCYjIjLCMSCgkAAgICAQs2HA8KLBcZGTggBwRcLxscABEIBQEBEhQI
BlApGxcEAwEBDggDFCwaAAEFA2IyAQEAAQUDAgIDAggFAwIAAQEBABkEBRQLCQcEAwMMCggKCAoPDAgMCFQsGhTkAXMPCQMUxq4B
sFcsG2C1QtoF8ALSAZABDBMICgABAQESCxgOCAYAAgIDAgPWAY0B1AX9An5Cal4WEB8gH1AIBowCqwEyIDIlsAFcCggKCAoIBCYW
DTIbJBQCAxwRGjD0Af0CAYMBOCBgQAEgPi0mFQ4OSiYJCjogRigOOLoCsAFGKEYoTjBmQBQMHBAMCAIDFAwcEAwIAgVmQOwEwAIq
IA4gCgdSKxBOCggAAlYtAgUAAywhLCA8YCQUAgwqIDJgbj4CQgABBQQDBwcEBgUEBDgfBQcBITggOEAOCTYcAxtqQCogJCAigAGQ
AYABZEBkQEYkByY2HwkGLJECUisDBQISBDoHBDgnAAkqFhUqIhIHLigwLCAFA2g1HTgBAQICAwIACgcEWC0VEgEOMDAFA0YkGx0A
AQMCAAlEIwEDAAoFA14wGw4GCAcFAwwAAgAkIhMwGRcSAQECAgE/DAgAAgYFHBASEVwvFyEHBQ4KAgQqFwwIAgMIBgMCAAENCQME
BQUABwEGCAUDBA0KCRASCwADAAJKJwABEQoBAwECBwUJBQABAQEADgMfaDUjEgYXAQEEIV4wJxQCAwC5AVwvDQkRIAYEAyReMCEU
ADxUKxkNAEg0HR0jDMABViwddH5fDgoAAw4JAgMuGAsHAwQHBAABAQEAAQNeDgpMJw0JDQcAAgABARwAARMKTigNBwABBwwBCQAB
FQtaLh8TABOQAZACQEAQCkglDwkPCAAyOiArFxtXDAgCA0omCwkBAgMDDQcAAQEZCgcCAz4gCQYDAwkFAMgCJBMHDQMCAAEBARgO
QiINCgkcAVYAULIOgAmGA5AWwAFw3hDACAEBCgYdGbQ+oB+MCYA6Oh4j4g3wCMAEPDCcAWA6IAkQXjANEAYjKBokwwNYgAJ+wAGU
AU8BAQABbT4HBBhNAgMAAQEMAxDuX4AwqhP/CRLxRQYFDAgCA8QEsgIAHgQFAA8GDJYGkBXUAXAYEBAQEg0D4yRbMC21BAkICw4P
Cg0lB5gBBb4DqAFWjAFIAgQAAwIEBgUWDQACDAiAAUIGBg4JDAg2HQYFCAYABAwIpgXWAjAaMBo8IDAaPCAwGjwgMBo8IDAaDrwE
bTtjOgEPARcJBh3fCDwlCtsBDQghEw0IAwMJCnpfAXFYMA0HDBcAwgI6HgESViwH5AM2HAfkAToeAwIA8AMMCAYFAgMcEIgD0AEN
MIYBRA0HALUJBgU0HAIDAAMAAhILBgUAAgAHAAUAAgACAAIEBAIDAAMAAgACAAIAAgACAgMAAwYFDAgGBQYFAAISCyAWBAQIBiDV
Ir6bBYDOAvJAwCC6A+ABglqQLeB0wDraCZAYugiAEJRN0Ca+QbDbK98D
""";

    static readonly int[] UnicodeLetterMarkRanges = DecodeUnicodeLetterMarkRanges();

    internal static bool TryGetPinnedLetterOrMark(
        int scalar,
        out bool isLetter,
        out bool isMark)
    {
        var low = 0;
        var high = UnicodeLetterMarkRangeCount - 1;
        while (low <= high)
        {
            var middle = low + ((high - low) / 2);
            var offset = middle * 3;
            if (scalar < UnicodeLetterMarkRanges[offset])
            {
                high = middle - 1;
            }
            else if (scalar > UnicodeLetterMarkRanges[offset + 1])
            {
                low = middle + 1;
            }
            else
            {
                isMark = UnicodeLetterMarkRanges[offset + 2] != 0;
                isLetter = !isMark;
                return true;
            }
        }

        isLetter = false;
        isMark = false;
        return false;
    }

    static int[] DecodeUnicodeLetterMarkRanges()
    {
        var bytes = Convert.FromBase64String(UnicodeLetterMarkData);
        var ranges = new int[UnicodeLetterMarkRangeCount * 3];
        var byteIndex = 0;
        var previousFirst = 0;
        for (var rangeIndex = 0; rangeIndex < UnicodeLetterMarkRangeCount; rangeIndex++)
        {
            var offset = rangeIndex * 3;
            var first = previousFirst + ReadVarUInt(bytes, ref byteIndex);
            var encodedLengthAndKind = ReadVarUInt(bytes, ref byteIndex);
            ranges[offset] = first;
            ranges[offset + 1] = first + (encodedLengthAndKind / 2);
            ranges[offset + 2] = encodedLengthAndKind & 1;
            previousFirst = first;
        }

        return ranges;
    }

    static int ReadVarUInt(byte[] bytes, ref int index)
    {
        var value = 0;
        var shift = 0;
        byte current;
        do
        {
            current = bytes[index++];
            value |= (current & 0x7F) << shift;
            shift += 7;
        }
        while ((current & 0x80) != 0);

        return value;
    }
}