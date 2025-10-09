#if !(NET5_0_OR_GREATER)

using System.Runtime.CompilerServices;

namespace System;


// C# 14 static extension members on the *type* itself.
// Usage on old TFMs:  Double.IsFinite(x), Double.IsIntegral(x), Single.IsFinite(x), ...
static class FloatCompat
{
    // ---- Double ----
    extension(double)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(double x)
        {
            var bits = (ulong)BitConverter.DoubleToInt64Bits(x);
            return (bits & 0x7FF0_0000_0000_0000UL) != 0x7FF0_0000_0000_0000UL;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsIntegral(double x)
        {
            if (!IsFinite(x)) return false;
            return x == Math.Truncate(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinityFast(double x)
        {
            var bits = (ulong)BitConverter.DoubleToInt64Bits(x);
            return (bits & 0x7FFF_FFFF_FFFF_FFFFUL) == 0x7FF0_0000_0000_0000UL;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaNFast(double x)
        {
            var bits = (ulong)BitConverter.DoubleToInt64Bits(x);
            return (bits & 0x7FF0_0000_0000_0000UL) == 0x7FF0_0000_0000_0000UL
                && (bits & 0x000F_FFFF_FFFF_FFFFUL) != 0;
        }


    }

    // ---- float ----
    extension(float)
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFinite(float x)
        {
            // netstandard2.0 / net48: fastest is unsafe bit-cast
            unsafe { var bits = *(uint*)&x; return (bits & 0x7F80_0000u) != 0x7F80_0000u; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsIntegral(float x)
        {
            if (!IsFinite(x)) return false;
            return x == Math.Truncate(x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsInfinityFast(float x)
        {
            unsafe { var bits = *(uint*)&x; return (bits & 0x7FFF_FFFFu) == 0x7F80_0000u; }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNaNFast(float x)
        {
            unsafe
            {
                var bits = *(uint*)&x;
                var isExpAllOnes = (bits & 0x7F80_0000u) == 0x7F80_0000u;
                var hasMantissa = (bits & 0x007F_FFFFu) != 0;
                return isExpAllOnes && hasMantissa;
            }
        }
    }
}
#endif