using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation
{
    internal class PolishFormatter : DefaultFormatter
    {
        private const string PaucalPostfix = "_Paucal";

        protected override string GetResourceKey(string resourceKey, int number)
        {
            if (number > 1 && number < 5)
                return resourceKey + PaucalPostfix;

            return resourceKey;
        }
    }
}
