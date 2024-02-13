namespace Humanizer.Localisation.NumberToWords
{
    internal class ThaiNumberToWordsConverter : GenderlessNumberToWordsConverter
    {
        public override string Convert(long numbermoney)
        {
            var Textreturn = "";
            if (numbermoney == 0)
            {
                return "ศูนย์";
            }

            if (numbermoney < 0)
            {
                Textreturn = "ลบ";
                numbermoney = -(numbermoney);
            }

            if ((numbermoney / 1000000) > 0)
            {
                Textreturn += Convert(numbermoney / 1000000) + "ล้าน";
                numbermoney %= 1000000;
            }
            if ((numbermoney / 100000) > 0)
            {
                Textreturn += Convert(numbermoney / 100000) + "แสน";
                numbermoney %= 100000;
            }
            if ((numbermoney / 10000) > 0)
            {
                Textreturn += Convert(numbermoney / 10000) + "หมื่น";
                numbermoney %= 10000;
            }
            if ((numbermoney / 1000) > 0)
            {
                Textreturn += Convert(numbermoney / 1000) + "พัน";
                numbermoney %= 1000;
            }

            if ((numbermoney / 100) > 0)
            {
                Textreturn += Convert(numbermoney / 100) + "ร้อย";
                numbermoney %= 100;
            }

            if (numbermoney > 0)
            {
                if (Textreturn != "")
                {
                    Textreturn += "";
                }

                var unitsMap = new[] { "ศูนย์", "หนึ่ง", "สอง", "สาม", "สี่", "ห้า", "หก", "เจ็ด", "เเปด", "เก้า", "สิบ", "สิบเอ็ด", "สิบสอง", "สิบสาม", "สิบสี่", "สิบห้า", "สิบหก", "สิบเจ็ด", "สิบเเปด", "สิบเก้า" };
                var tensMap = new[] { "ศูนย์", "สิบ", "ยี่สิบ", "สามสิบ", "สี่สิบ", "ห้าสิบ", "หกสิบ", "เจ็ดสิบ", "แปดสิบ", "เก้าสิบ" };

                if (numbermoney < 20)
                {
                    Textreturn += unitsMap[numbermoney];
                }
                else
                {
                    Textreturn += tensMap[numbermoney / 10];
                    if ((numbermoney % 10) > 0)
                    {
                        Textreturn += "" + unitsMap[numbermoney % 10];
                    }
                }
            }

            return Textreturn;
        }

        public override string ConvertToOrdinal(int number)
        {
            return Convert(number);
        }
    }
}
