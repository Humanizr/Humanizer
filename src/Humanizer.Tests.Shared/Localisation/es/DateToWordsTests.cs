using System;
using Xunit;

namespace Humanizer.Tests.Localisation.es
{
    [UseCulture("es-419")]
    public class DateToWordsTests
    {
        [Fact]
        public void ConvertDateToWordsString()
        {
            Assert.Equal("dos de enero de dos mil veintidós", new DateTime(2022, 1, 2).ToWords());
            Assert.Equal("diez de junio de dos mil veinte", new DateTime(2020, 6, 10).ToWords());
            Assert.Equal("veinticinco de septiembre de dos mil diecisiete", new DateTime(2017, 9, 25).ToWords());
            Assert.Equal("treinta y uno de diciembre de mil novecientos noventa y nueve", new DateTime(1999, 12, 31).ToWords());
        }

#if NET6_0_OR_GREATER
        [Fact]
        public void ConvertDateOnlyToWordsString()
        {
            Assert.Equal("dos de enero de dos mil veintidós", new DateOnly(2022, 1, 2).ToWords());
            Assert.Equal("diez de junio de dos mil veinte", new DateOnly(2020, 6, 10).ToWords());
            Assert.Equal("veinticinco de septiembre de dos mil diecisiete", new DateOnly(2017, 9, 25).ToWords());
            Assert.Equal("treinta y uno de diciembre de mil novecientos noventa y nueve", new DateOnly(1999, 12, 31).ToWords());
        }
#endif
    }
}
