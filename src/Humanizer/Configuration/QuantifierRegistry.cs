using Humanizer.Localisation.Quantifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Configuration
{
    internal class QuantifierRegistry : LocaliserRegistry<IQuantifier>
    {
        public QuantifierRegistry()
            :base(new DefaultQuantifier())
        {
            Register<FarsiQuantifier>("fa");
        }
    }
}
