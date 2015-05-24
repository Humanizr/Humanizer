namespace Humanizer.Inflections
{
    /// <summary>
    /// Container for registered Vocabularies.  At present, only a single vocabulary is supported: Default.
    /// </summary>
    public static class Vocabularies
    {
        private static Vocabulary _default;

        /// <summary>
        /// The default vocabulary used for singular/plural irregularities.
        /// Rules can be added to this vocabulary and will be picked up by called to Singularize() and Pluralize().
        /// At this time, multiple vocabularies and removing existing rules are not supported.
        /// </summary>
        public static Vocabulary Default
        {
            get
            {
                if (_default == null)
                    BuildDefault();

                return _default;
            }
        }

        private static void BuildDefault()
        {
            _default = new Vocabulary();

            _default.AddPlural("$", "s");
            _default.AddPlural("s$", "s");
            _default.AddPlural("(ax|test)is$", "$1es");
            _default.AddPlural("(octop|vir|alumn|fung)us$", "$1i");
            _default.AddPlural("(alias|status)$", "$1es");
            _default.AddPlural("(bu)s$", "$1ses");
            _default.AddPlural("(buffal|tomat|volcan)o$", "$1oes");
            _default.AddPlural("([ti])um$", "$1a");
            _default.AddPlural("sis$", "ses");
            _default.AddPlural("(?:([^f])fe|([lr])f)$", "$1$2ves");
            _default.AddPlural("(hive)$", "$1s");
            _default.AddPlural("([^aeiouy]|qu)y$", "$1ies");
            _default.AddPlural("(x|ch|ss|sh)$", "$1es");
            _default.AddPlural("(matr|vert|ind)ix|ex$", "$1ices");
            _default.AddPlural("([m|l])ouse$", "$1ice");
            _default.AddPlural("^(ox)$", "$1en");
            _default.AddPlural("(quiz)$", "$1zes");
            _default.AddPlural("(campus)$", "$1es");
            _default.AddPlural("^is$", "are");

            _default.AddSingular("s$", "");
            _default.AddSingular("(n)ews$", "$1ews");
            _default.AddSingular("([ti])a$", "$1um");
            _default.AddSingular("((a)naly|(b)a|(d)iagno|(p)arenthe|(p)rogno|(s)ynop|(t)he)ses$", "$1$2sis");
            _default.AddSingular("(^analy)ses$", "$1sis");
            _default.AddSingular("([^f])ves$", "$1fe");
            _default.AddSingular("(hive)s$", "$1");
            _default.AddSingular("(tive)s$", "$1");
            _default.AddSingular("([lr])ves$", "$1f");
            _default.AddSingular("([^aeiouy]|qu)ies$", "$1y");
            _default.AddSingular("(s)eries$", "$1eries");
            _default.AddSingular("(m)ovies$", "$1ovie");
            _default.AddSingular("(x|ch|ss|sh)es$", "$1");
            _default.AddSingular("([m|l])ice$", "$1ouse");
            _default.AddSingular("(bus)es$", "$1");
            _default.AddSingular("(o)es$", "$1");
            _default.AddSingular("(shoe)s$", "$1");
            _default.AddSingular("(cris|ax|test)es$", "$1is");
            _default.AddSingular("(octop|vir|alumn|fung)i$", "$1us");
            _default.AddSingular("(alias|status)es$", "$1");
            _default.AddSingular("^(ox)en", "$1");
            _default.AddSingular("(vert|ind)ices$", "$1ex");
            _default.AddSingular("(matr)ices$", "$1ix");
            _default.AddSingular("(quiz)zes$", "$1");
            _default.AddSingular("(campus)es$", "$1");
            _default.AddSingular("^are$", "is");

            _default.AddIrregular("person", "people");
            _default.AddIrregular("man", "men");
            _default.AddIrregular("child", "children");
            _default.AddIrregular("sex", "sexes");
            _default.AddIrregular("move", "moves");
            _default.AddIrregular("goose", "geese");
            _default.AddIrregular("alumna", "alumnae");
            _default.AddIrregular("criterion", "criteria");
            _default.AddIrregular("wave", "waves");

            _default.AddUncountable("equipment");
            _default.AddUncountable("information");
            _default.AddUncountable("rice");
            _default.AddUncountable("money");
            _default.AddUncountable("species");
            _default.AddUncountable("series");
            _default.AddUncountable("fish");
            _default.AddUncountable("sheep");
            _default.AddUncountable("deer");
            _default.AddUncountable("aircraft");
            _default.AddUncountable("oz");
            _default.AddUncountable("tsp");
            _default.AddUncountable("tbsp");
            _default.AddUncountable("ml");
            _default.AddUncountable("l");
        }
    }
}