﻿namespace Humanizer;

class FormatterRegistry : LocaliserRegistry<IFormatter>
{
    public FormatterRegistry()
        : base(c => new DefaultFormatter(c))
    {
        Register("ar", c => new ArabicFormatter(c));
        Register("de", c => new GermanFormatter(c));
        Register("he", c => new HebrewFormatter(c));
        Register("ro", c => new RomanianFormatter(c));
        Register("ru", c => new RussianFormatter(c));
        Register("sl", c => new SlovenianFormatter(c));
        Register("hr", c => new CroatianFormatter(c));
        Register("sr", c => new SerbianFormatter(c));
        Register("sr-Latn", c => new SerbianFormatter(c));
        Register("uk", c => new UkrainianFormatter(c));
        Register("fr", c => new FrenchFormatter(c));
        Register("fr-BE", c => new FrenchFormatter(c));
        RegisterCzechSlovakPolishFormatter("cs");
        RegisterCzechSlovakPolishFormatter("pl");
        RegisterCzechSlovakPolishFormatter("sk");
        Register("bg", c => new BulgarianFormatter(c));
        RegisterDefaultFormatter("ku");
        RegisterDefaultFormatter("pt");
        RegisterDefaultFormatter("sv");
        RegisterDefaultFormatter("tr");
        RegisterDefaultFormatter("vi");
        RegisterDefaultFormatter("en-US");
        RegisterDefaultFormatter("af");
        RegisterDefaultFormatter("az");
        RegisterDefaultFormatter("da");
        RegisterDefaultFormatter("el");
        RegisterDefaultFormatter("es");
        RegisterDefaultFormatter("fa");
        RegisterDefaultFormatter("fi-FI");
        RegisterDefaultFormatter("fil-PH");
        RegisterDefaultFormatter("hu");
        RegisterDefaultFormatter("hy");
        RegisterDefaultFormatter("id");
        Register("is", c => new IcelandicFormatter(c));
        RegisterDefaultFormatter("ja");
        RegisterDefaultFormatter("ko-KR");
        RegisterDefaultFormatter("lv");
        Register("mt", c => new MalteseFormatter(c));
        RegisterDefaultFormatter("ms-MY");
        RegisterDefaultFormatter("nb");
        RegisterDefaultFormatter("nb-NO");
        RegisterDefaultFormatter("nl");
        RegisterDefaultFormatter("bn-BD");
        RegisterDefaultFormatter("it");
        RegisterDefaultFormatter("ta");
        RegisterDefaultFormatter("uz-Latn-UZ");
        RegisterDefaultFormatter("uz-Cyrl-UZ");
        RegisterDefaultFormatter("zh-CN");
        RegisterDefaultFormatter("zh-Hans");
        RegisterDefaultFormatter("zh-Hant");
        RegisterDefaultFormatter("th-TH");
        RegisterDefaultFormatter("en-IN");
        Register("lt", c => new LithuanianFormatter(c));
        Register("lb", c => new LuxembourgishFormatter(c));
    }

    void RegisterDefaultFormatter(string localeCode) =>
        Register(localeCode, c => new DefaultFormatter(c));

    void RegisterCzechSlovakPolishFormatter(string localeCode) =>
        Register(localeCode, c => new CzechSlovakPolishFormatter(c));
}