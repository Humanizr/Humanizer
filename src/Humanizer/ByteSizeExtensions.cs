using Humanizer.Bytes;

namespace Humanizer
{
    public static class ByteSizeExtensions
    {
        /// <summary>
        /// Use value as a quantity of bits
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ByteSize Bits(this long val)
        {
            return ByteSize.FromBits(val);
        }

        /// <summary>
        /// Use value as a quantity of bits
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ByteSize Bits(this int val)
        {
            return ByteSize.FromBits(val);
        }

        /// <summary>
        /// Use value as a quantity of bytes
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ByteSize Bytes(this double val)
        {
            return ByteSize.FromBytes(val);
        }

        /// <summary>
        /// Use value as a quantity of kilobytes
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ByteSize Kilobytes(this double val)
        {
            return ByteSize.FromKiloBytes(val);
        }

        /// <summary>
        /// Use value as a quantity of megabytes
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ByteSize Megabytes(this double val)
        {
            return ByteSize.FromMegaBytes(val);
        }

        /// <summary>
        /// Use value as a quantity of gigabytes
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ByteSize Gigabytes(this double val)
        {
            return ByteSize.FromGigaBytes(val);
        }

        /// <summary>
        /// Use value as a quantity of terabytes
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static ByteSize Terabytes(this double val)
        {
            return ByteSize.FromTeraBytes(val);
        }
    }

    public static class HumanizeExtension
    {
        /// <summary>
        /// Turns a byte quantity into human readable form, eg 2 GB
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string Humanize(this ByteSize val)
        {
            return val.ToString();
        }
    }
}
