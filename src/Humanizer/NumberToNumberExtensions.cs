using System.Runtime.CompilerServices;

namespace Humanizer
{
    /// <summary>
    /// Number to Number extensions
    /// </summary>
    public static class NumberToNumberExtensions
    {
        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Tens(this int input)
        {
            return input * 10;
        }

        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Tens(this uint input)
        {
            return input * 10;
        }

        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Tens(this long input)
        {
            return input * 10;
        }

        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Tens(this ulong input)
        {
            return input * 10;
        }

        /// <summary>
        /// 5.Tens == 50
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Tens(this double input)
        {
            return input * 10;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Hundreds(this int input)
        {
            return input * 100;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Hundreds(this uint input)
        {
            return input * 100;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Hundreds(this long input)
        {
            return input * 100;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Hundreds(this ulong input)
        {
            return input * 100;
        }

        /// <summary>
        /// 4.Hundreds() == 400
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Hundreds(this double input)
        {
            return input * 100;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Thousands(this int input)
        {
            return input * 1000;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Thousands(this uint input)
        {
            return input * 1000;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Thousands(this long input)
        {
            return input * 1000;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Thousands(this ulong input)
        {
            return input * 1000;
        }

        /// <summary>
        /// 3.Thousands() == 3000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Thousands(this double input)
        {
            return input * 1000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Millions(this int input)
        {
            return input * 1000000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Millions(this uint input)
        {
            return input * 1000000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Millions(this long input)
        {
            return input * 1000000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Millions(this ulong input)
        {
            return input * 1000000;
        }

        /// <summary>
        /// 2.Millions() == 2000000
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Millions(this double input)
        {
            return input * 1000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000 (short scale)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Billions(this int input)
        {
            return input * 1000000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000 (short scale)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Billions(this uint input)
        {
            return input * 1000000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000 (short scale)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Billions(this long input)
        {
            return input * 1000000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000 (short scale)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Billions(this ulong input)
        {
            return input * 1000000000;
        }

        /// <summary>
        /// 1.Billions() == 1000000000 (short scale)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Billions(this double input)
        {
            return input * 1000000000;
        }
    }
}
