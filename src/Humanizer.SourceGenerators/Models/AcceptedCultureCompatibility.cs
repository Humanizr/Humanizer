using System.Collections.Immutable;

namespace Humanizer.SourceGenerators;

public sealed partial class HumanizerSourceGenerator
{
    internal static class AcceptedCultureCompatibility
    {
        // Frozen input: union of the 554-name macOS/ICU ledger
        // (SHA-256 6c9d99872c0dcc3e4ad519231168c8493bab2eae380e548f5d471651d5ec9d33),
        // the 504-name Iris/Linux ICU ledger
        // (SHA-256 f7afdeb2418a62c95d1ffb71d33fbee28c10a0c059f4a1422617c3d8a7d2a686),
        // and the 480-name Windows NLS/net48 ledger
        // (SHA-256 f9f8ae4548a61feebc1d38b1f89ae454d4f8bf3aff2affb393406d9826ec9620),
        // plus every canonical YAML root.
        static readonly (string Owner, string Names)[] Groups =
        [
            ("af", "af,af-NA,af-ZA"),
            ("am", "am,am-ET"),
            ("ar", "ar,ar-001,ar-AE,ar-BH,ar-DJ,ar-DZ,ar-EG,ar-EH,ar-ER,ar-IL,ar-IQ,ar-JO,ar-KM,ar-KW,ar-LB,ar-LY,ar-MA,ar-MR,ar-OM,ar-PS,ar-QA,ar-SA,ar-SD,ar-SO,ar-SS,ar-SY,ar-TD,ar-TN,ar-YE"),
            ("as", "as,as-IN"),
            ("az", "az,az-Cyrl,az-Cyrl-AZ,az-Latn,az-Latn-AZ"),
            ("be", "be,be-BY"),
            ("bg", "bg,bg-BG"),
            ("bn", "bn,bn-BD,bn-IN"),
            ("bs", "bs,bs-Cyrl,bs-Cyrl-BA,bs-Latn,bs-Latn-BA"),
            ("ca", "ca,ca-AD,ca-ES,ca-ES-valencia,ca-FR,ca-IT"),
            ("cs", "cs,cs-CZ"),
            ("cy", "cy,cy-GB"),
            ("da", "da,da-DK,da-GL"),
            ("de", "de,de-AT,de-BE,de-DE,de-IT,de-LU"),
            ("de-CH", "de-CH"),
            ("de-LI", "de-LI"),
            ("el", "el,el-CY,el-GR"),
            ("en", "en,en-001,en-029,en-150,en-AE,en-AG,en-AI,en-AL,en-AR,en-AS,en-AT,en-AU,en-BB,en-BD,en-BE,en-BG,en-BI,en-BM,en-BN,en-BR,en-BS,en-BW,en-BZ,en-CA,en-CC,en-CH,en-CL,en-CM,en-CN,en-CO,en-CK,en-CV,en-CX,en-CY,en-CZ,en-DE,en-DG,en-DK,en-DM,en-EE,en-ER,en-ES,en-FI,en-FJ,en-FK,en-FM,en-FR,en-GD,en-GE,en-GG,en-GH,en-GI,en-GM,en-GR,en-GS,en-GU,en-GY,en-HK,en-HU,en-ID,en-IE,en-IL,en-IM,en-IO,en-IT,en-JE,en-JM,en-JP,en-KE,en-KI,en-KN,en-KR,en-KY,en-LC,en-LR,en-LS,en-LT,en-LV,en-MG,en-MH,en-MM,en-MO,en-MP,en-MS,en-MT,en-MU,en-MV,en-MW,en-MX,en-MY,en-NA,en-NF,en-NG,en-NL,en-NO,en-NR,en-NU,en-NZ,en-PG,en-PH,en-PK,en-PL,en-PN,en-PR,en-PT,en-PW,en-RO,en-RU,en-RW,en-SA,en-SB,en-SC,en-SD,en-SE,en-SG,en-SH,en-SI,en-SK,en-SL,en-SS,en-SX,en-SZ,en-TC,en-TH,en-TK,en-TO,en-TR,en-TT,en-TV,en-TW,en-TZ,en-UA,en-UG,en-UM,en-VC,en-VG,en-VI,en-VU,en-WS,en-ZA,en-ZM,en-ZW"),
            ("en-GB", "en-GB"),
            ("en-IN", "en-IN"),
            ("en-US", "en-US,en-US-POSIX"),
            ("es", "es,es-003,es-419,es-AG,es-AR,es-BB,es-BM,es-BO,es-BQ,es-BR,es-BS,es-BZ,es-CA,es-CL,es-CO,es-CR,es-CU,es-CW,es-DM,es-DO,es-EA,es-EC,es-ES,es-GD,es-GQ,es-GT,es-GY,es-HN,es-HT,es-IC,es-KN,es-KY,es-LC,es-MX,es-NI,es-PA,es-PE,es-PH,es-PR,es-PY,es-SV,es-TC,es-TT,es-US,es-UY,es-VC,es-VE,es-VG,es-VI"),
            ("et", "et,et-EE"),
            ("eu", "eu,eu-ES"),
            ("fa", "fa,fa-AF,fa-IR"),
            ("fi", "fi,fi-FI"),
            ("fil", "fil,fil-PH"),
            ("fr", "fr,fr-029,fr-BF,fr-BI,fr-BJ,fr-BL,fr-CA,fr-CD,fr-CF,fr-CG,fr-CI,fr-CM,fr-DJ,fr-DZ,fr-FR,fr-GA,fr-GF,fr-GN,fr-GP,fr-GQ,fr-HT,fr-KM,fr-LU,fr-MA,fr-MC,fr-MF,fr-MG,fr-ML,fr-MQ,fr-MR,fr-MU,fr-NC,fr-NE,fr-PF,fr-PM,fr-RE,fr-RW,fr-SC,fr-SN,fr-SY,fr-TD,fr-TG,fr-TN,fr-VU,fr-WF,fr-YT"),
            ("fr-BE", "fr-BE"),
            ("fr-CH", "fr-CH"),
            ("ga", "ga,ga-GB,ga-IE"),
            ("gl", "gl,gl-ES"),
            ("gu", "gu,gu-IN"),
            ("ha", "ha,ha-GH,ha-Latn,ha-Latn-GH,ha-Latn-NE,ha-Latn-NG,ha-NE,ha-NG"),
            ("he", "he,he-IL"),
            ("hi", "hi,hi-IN,hi-Latn,hi-Latn-IN"),
            ("hr", "hr,hr-BA,hr-HR"),
            ("hu", "hu,hu-HU"),
            ("hy", "hy,hy-AM"),
            ("id", "id,id-ID"),
            ("ig", "ig,ig-NG"),
            ("is", "is,is-IS"),
            ("it", "it,it-CH,it-IT,it-SM,it-VA"),
            ("ja", "ja,ja-JP"),
            ("ka", "ka,ka-GE"),
            ("kk", "kk,kk-Arab,kk-Arab-CN,kk-Cyrl,kk-Cyrl-KZ,kk-KZ"),
            ("km", "km,km-KH"),
            ("kn", "kn,kn-IN"),
            ("ko", "ko,ko-CN,ko-KP,ko-KR"),
            ("kok", "kok,kok-Deva,kok-Deva-IN,kok-IN,kok-Latn,kok-Latn-IN"),
            ("ku", "ku,ku-Arab,ku-Arab-IQ,ku-Arab-IR,ku-Latn,ku-Latn-IQ,ku-Latn-SY,ku-Latn-TR,ku-TR"),
            ("ky", "ky,ky-KG"),
            ("lb", "lb,lb-LU"),
            ("lo", "lo,lo-LA"),
            ("lt", "lt,lt-LT"),
            ("lv", "lv,lv-LV"),
            ("mk", "mk,mk-MK"),
            ("ml", "ml,ml-IN"),
            ("mn", "mn,mn-Cyrl,mn-MN,mn-Mong,mn-Mong-CN,mn-Mong-MN"),
            ("mr", "mr,mr-IN"),
            ("ms", "ms,ms-Arab,ms-Arab-BN,ms-Arab-MY,ms-BN,ms-ID,ms-MY,ms-SG"),
            ("mt", "mt,mt-MT"),
            ("my", "my,my-MM"),
            ("nb", "nb,nb-NO,nb-SJ"),
            ("ne", "ne,ne-IN,ne-NP"),
            ("nl", "nl,nl-AW,nl-BE,nl-BQ,nl-CW,nl-NL,nl-SR,nl-SX"),
            ("nn", "nn,nn-NO"),
            ("or", "or,or-IN"),
            ("pa", "pa,pa-Guru,pa-Guru-IN,pa-IN"),
            ("pa-Arab", "pa-Arab,pa-Arab-PK,pa-Aran-PK"),
            ("pl", "pl,pl-PL"),
            ("ps", "ps,ps-AF,ps-PK"),
            ("pt", "pt,pt-AO,pt-CH,pt-CV,pt-FR,pt-GQ,pt-GW,pt-LU,pt-MO,pt-MZ,pt-PT,pt-ST,pt-TL"),
            ("pt-BR", "pt-BR"),
            ("ro", "ro,ro-MD,ro-RO"),
            ("ru", "ru,ru-BY,ru-KG,ru-KZ,ru-MD,ru-RU,ru-UA"),
            ("si", "si,si-LK"),
            ("sk", "sk,sk-SK"),
            ("sl", "sl,sl-SI"),
            ("so", "so,so-DJ,so-ET,so-KE,so-SO"),
            ("sq", "sq,sq-AL,sq-MK,sq-XK"),
            ("sr", "sr,sr-Cyrl,sr-Cyrl-BA,sr-Cyrl-ME,sr-Cyrl-RS,sr-Cyrl-XK"),
            ("sr-Latn", "sr-Latn,sr-Latn-BA,sr-Latn-ME,sr-Latn-RS,sr-Latn-XK"),
            ("sv", "sv,sv-AX,sv-FI,sv-SE"),
            ("sw", "sw,sw-CD,sw-KE,sw-TZ,sw-UG"),
            ("ta", "ta,ta-IN,ta-LK,ta-MY,ta-SG"),
            ("te", "te,te-IN"),
            ("th", "th,th-TH"),
            ("tk", "tk,tk-TM"),
            ("tr", "tr,tr-CY,tr-TR"),
            ("uk", "uk,uk-UA"),
            ("ur", "ur,ur-Arab,ur-Arab-IN,ur-Arab-PK,ur-Aran-IN,ur-Aran-PK"),
            ("ur-IN", "ur-IN"),
            ("ur-PK", "ur-PK"),
            ("uz-Cyrl-UZ", "uz-Cyrl-UZ"),
            ("uz-Latn-UZ", "uz-Latn-UZ"),
            ("vi", "vi,vi-VN"),
            ("yo", "yo,yo-BJ,yo-NG"),
            ("zh-Hans", "zh-CHS,zh-Hans,zh-Hans-CN,zh-Hans-HK,zh-Hans-JP,zh-Hans-MO,zh-Hans-MY,zh-Hans-SG,zh-SG"),
            ("zh-Hant", "zh-CHT,zh-Hant,zh-Hant-CN,zh-Hant-HK,zh-Hant-JP,zh-Hant-MO,zh-Hant-MY,zh-Hant-TW,zh-HK,zh-MO,zh-TW"),
            ("zu-ZA", "zu-ZA")
        ];

        static readonly ImmutableDictionary<string, string> DefaultScripts =
            new Dictionary<string, string>(StringComparer.Ordinal)
            {
                ["af"] = "Latn",
                ["am"] = "Ethi",
                ["ar"] = "Arab",
                ["as"] = "Beng",
                ["az"] = "Latn",
                ["be"] = "Cyrl",
                ["bg"] = "Cyrl",
                ["bn"] = "Beng",
                ["bs"] = "Latn",
                ["ca"] = "Latn",
                ["cs"] = "Latn",
                ["cy"] = "Latn",
                ["da"] = "Latn",
                ["de"] = "Latn",
                ["de-CH"] = "Latn",
                ["de-LI"] = "Latn",
                ["el"] = "Grek",
                ["en"] = "Latn",
                ["en-GB"] = "Latn",
                ["en-IN"] = "Latn",
                ["en-US"] = "Latn",
                ["es"] = "Latn",
                ["et"] = "Latn",
                ["eu"] = "Latn",
                ["fa"] = "Arab",
                ["fi"] = "Latn",
                ["fil"] = "Latn",
                ["fr"] = "Latn",
                ["fr-BE"] = "Latn",
                ["fr-CH"] = "Latn",
                ["ga"] = "Latn",
                ["gl"] = "Latn",
                ["gu"] = "Gujr",
                ["ha"] = "Latn",
                ["he"] = "Hebr",
                ["hi"] = "Deva",
                ["hr"] = "Latn",
                ["hu"] = "Latn",
                ["hy"] = "Armn",
                ["id"] = "Latn",
                ["ig"] = "Latn",
                ["is"] = "Latn",
                ["it"] = "Latn",
                ["ja"] = "Jpan",
                ["ka"] = "Geor",
                ["kk"] = "Cyrl",
                ["km"] = "Khmr",
                ["kn"] = "Knda",
                ["ko"] = "Kore",
                ["kok"] = "Deva",
                ["ku"] = "Latn",
                ["ky"] = "Cyrl",
                ["lb"] = "Latn",
                ["lo"] = "Laoo",
                ["lt"] = "Latn",
                ["lv"] = "Latn",
                ["mk"] = "Cyrl",
                ["ml"] = "Mlym",
                ["mn"] = "Cyrl",
                ["mr"] = "Deva",
                ["ms"] = "Latn",
                ["mt"] = "Latn",
                ["my"] = "Mymr",
                ["nb"] = "Latn",
                ["ne"] = "Deva",
                ["nl"] = "Latn",
                ["nn"] = "Latn",
                ["or"] = "Orya",
                ["pa"] = "Guru",
                ["pa-Arab"] = "Arab",
                ["pl"] = "Latn",
                ["ps"] = "Arab",
                ["pt"] = "Latn",
                ["pt-BR"] = "Latn",
                ["ro"] = "Latn",
                ["ru"] = "Cyrl",
                ["si"] = "Sinh",
                ["sk"] = "Latn",
                ["sl"] = "Latn",
                ["so"] = "Latn",
                ["sq"] = "Latn",
                ["sr"] = "Cyrl",
                ["sr-Latn"] = "Latn",
                ["sv"] = "Latn",
                ["sw"] = "Latn",
                ["ta"] = "Taml",
                ["te"] = "Telu",
                ["th"] = "Thai",
                ["tk"] = "Latn",
                ["tr"] = "Latn",
                ["uk"] = "Cyrl",
                ["ur"] = "Arab",
                ["ur-IN"] = "Arab",
                ["ur-PK"] = "Arab",
                ["uz-Cyrl-UZ"] = "Cyrl",
                ["uz-Latn-UZ"] = "Latn",
                ["vi"] = "Latn",
                ["yo"] = "Latn",
                ["zh-Hans"] = "Hans",
                ["zh-Hant"] = "Hant",
                ["zu-ZA"] = "Latn"
            }.ToImmutableDictionary(StringComparer.Ordinal);

        static readonly ImmutableDictionary<string, string> EffectiveScripts =
            CreateEffectiveScripts();

        public static bool IsFrozenName(string name) =>
            EffectiveScripts.ContainsKey(name);

        public static bool TryGetEffectiveScript(string name, out string script) =>
            EffectiveScripts.TryGetValue(name, out script!);

        public static ImmutableArray<AcceptedCultureInput> Create(
            ImmutableArray<ResolvedLocaleDefinition> locales)
        {
            ValidateUniqueNames(Groups);
            var localeCodes = locales
                .Select(static locale => locale.LocaleCode)
                .ToImmutableHashSet(StringComparer.Ordinal);
            var entries = ImmutableArray.CreateBuilder<AcceptedCultureInput>();
            foreach (var (owner, names) in Groups)
            {
                if (!localeCodes.Contains(owner))
                {
                    continue;
                }

                entries.AddRange(names
                    .Split(',')
                    .Select(name => new AcceptedCultureInput(name, owner, null)));
            }

            var acceptedNames = entries
                .Select(static entry => entry.Name)
                .ToImmutableHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var localeCode in localeCodes.OrderBy(static value => value, StringComparer.Ordinal))
            {
                if (!acceptedNames.Contains(localeCode))
                {
                    entries.Add(new(localeCode, localeCode, null));
                }
            }

            return entries.ToImmutable();
        }

        static ImmutableDictionary<string, string> CreateEffectiveScripts()
        {
            ValidateUniqueNames(Groups);
            var scripts = ImmutableDictionary.CreateBuilder<string, string>(
                StringComparer.OrdinalIgnoreCase);
            foreach (var (owner, names) in Groups)
            {
                if (!DefaultScripts.TryGetValue(owner, out var defaultScript))
                {
                    throw new InvalidOperationException(
                        $"Frozen accepted-culture owner '{owner}' has no effective script.");
                }

                foreach (var name in names.Split(','))
                {
                    scripts[name] = TryGetExplicitScript(name, out var explicitScript)
                        ? explicitScript
                        : defaultScript;
                }
            }

            return scripts.ToImmutable();
        }

        internal static void ValidateUniqueNames(
            IEnumerable<(string Owner, string Names)> groups)
        {
            var ownersByName = new Dictionary<string, string>(
                StringComparer.OrdinalIgnoreCase);
            foreach (var (owner, names) in groups)
            {
                foreach (var name in names.Split(','))
                {
                    if (ownersByName.TryGetValue(name, out var previousOwner))
                    {
                        throw new InvalidOperationException(
                            $"Frozen accepted-culture name '{name}' is listed more than once by owners '{previousOwner}' and '{owner}'.");
                    }

                    ownersByName.Add(name, owner);
                }
            }
        }

        static bool TryGetExplicitScript(string name, out string script)
        {
            foreach (var subtag in name.Split('-').Skip(1))
            {
                if (subtag.Length == 4 && subtag.All(char.IsLetter))
                {
                    script = string.Equals(
                        subtag,
                        "Aran",
                        StringComparison.OrdinalIgnoreCase)
                            ? "Arab"
                            : subtag;
                    return true;
                }
            }

            if (name.Contains("-CHT", StringComparison.OrdinalIgnoreCase))
            {
                script = "Hant";
                return true;
            }

            if (name.Contains("-CHS", StringComparison.OrdinalIgnoreCase))
            {
                script = "Hans";
                return true;
            }

            script = string.Empty;
            return false;
        }
    }

    internal sealed class AcceptedCultureInput(
        string name,
        string localeProfileOwner,
        string? inflectionOwner,
        string? inflectionTerminal = null)
    {
        public string Name { get; } = name;
        public string LocaleProfileOwner { get; } = localeProfileOwner;
        public string? InflectionOwner { get; } = inflectionOwner;
        public string? InflectionTerminal { get; } = inflectionTerminal;

        public AcceptedCultureInput WithInflection(string? owner, string? terminal) =>
            new(Name, LocaleProfileOwner, owner, terminal);
    }
}