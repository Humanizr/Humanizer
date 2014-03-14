using Humanizer.Bytes;
using Xunit;

namespace Humanizer.Tests.Bytes
{
    public class FluentMethods
    {
        [Fact]
        public void Terabytes()
        {
            var size = (2.0).Terabytes();
            Assert.Equal(ByteSize.FromTeraBytes(2), size);
        }

        [Fact]
        public void HumanizesTB()
        {
            var humanized = (2.0).Terabytes().Humanize();
            Assert.Equal("2 TB", humanized);
        }

        [Fact]
        public void HumanizesWithFormat()
        {
            // TODO
        }

        [Fact]
        public void Dehumanizes()
        {
            // TODO
        }
    }
}
