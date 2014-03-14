using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Humanizer.Bytes // TODO: Named horribly to force refactoring of namespaces & folder layout
{
    public static class FluentCreation
    {
        public static ByteSize Bits(this long val)
        {
            return ByteSize.FromBits(val);
        }
        public static ByteSize Bytes(this double val)
        {
            return ByteSize.FromBytes(val);
        }
        public static ByteSize Kilobytes(this double val)
        {
            return ByteSize.FromKiloBytes(val);
        }
        public static ByteSize Megabytes(this double val)
        {
            return ByteSize.FromMegaBytes(val);
        }
        public static ByteSize Gigabytes(this double val)
        {
            return ByteSize.FromGigaBytes(val);
        }
        public static ByteSize Terabytes(this double val)
        {
            return ByteSize.FromTeraBytes(val);
        }
    }

    public static class HumanizeExtension
    {
        // TODO: Overload this to give access to formatting options in a Humanizey way
        public static string Humanize(this ByteSize val)
        {
            return val.ToString();
        }
    }
}
