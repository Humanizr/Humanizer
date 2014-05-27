using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Localisation.Quantifiers
{
    internal class FarsiQuantifier : DefaultQuantifier
    {
        protected override string TransformInput(string input, int quantity, ShowQuantityAs showQuantityAs)
        {
            //TODO: Use singularize and pluralize for Farsi
            string postFix = string.Empty;

            if (showQuantityAs == ShowQuantityAs.None && quantity > 1)
            {
                postFix = " ها";
            }

            return string.Format("{0}{1}", input, postFix);
        }
    }
}
