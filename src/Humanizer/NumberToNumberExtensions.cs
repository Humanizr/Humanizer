using System.Runtime.CompilerServices;

namespace Humanizer;

/// <summary>
/// Number to Number extensions
/// </summary>
public static class NumberToNumberExtensions
{
    /// <summary>
    /// Multiplies an integer by 10, providing a more readable way to express multiples of ten in code.
    /// </summary>
    /// <param name="input">The integer value to multiply by 10.</param>
    /// <returns>The input value multiplied by 10.</returns>
    /// <example>
    /// <code>
    /// 5.Tens() => 50
    /// 3.Tens() => 30
    /// 10.Tens() => 100
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Tens(this int input) =>
        input * 10;

    /// <summary>
    /// Multiplies an unsigned integer by 10, providing a more readable way to express multiples of ten in code.
    /// </summary>
    /// <param name="input">The unsigned integer value to multiply by 10.</param>
    /// <returns>The input value multiplied by 10.</returns>
    /// <example>
    /// <code>
    /// 5U.Tens() => 50U
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Tens(this uint input) =>
        input * 10;

    /// <summary>
    /// Multiplies a long integer by 10, providing a more readable way to express multiples of ten in code.
    /// </summary>
    /// <param name="input">The long integer value to multiply by 10.</param>
    /// <returns>The input value multiplied by 10.</returns>
    /// <example>
    /// <code>
    /// 5L.Tens() => 50L
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Tens(this long input) =>
        input * 10;

    /// <summary>
    /// Multiplies an unsigned long integer by 10, providing a more readable way to express multiples of ten in code.
    /// </summary>
    /// <param name="input">The unsigned long integer value to multiply by 10.</param>
    /// <returns>The input value multiplied by 10.</returns>
    /// <example>
    /// <code>
    /// 5UL.Tens() => 50UL
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Tens(this ulong input) =>
        input * 10;

    /// <summary>
    /// Multiplies a double by 10, providing a more readable way to express multiples of ten in code.
    /// </summary>
    /// <param name="input">The double value to multiply by 10.</param>
    /// <returns>The input value multiplied by 10.</returns>
    /// <example>
    /// <code>
    /// 5.5.Tens() => 55.0
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Tens(this double input) =>
        input * 10;

    /// <summary>
    /// Multiplies an integer by 100, providing a more readable way to express hundreds in code.
    /// </summary>
    /// <param name="input">The integer value to multiply by 100.</param>
    /// <returns>The input value multiplied by 100.</returns>
    /// <example>
    /// <code>
    /// 4.Hundreds() => 400
    /// 2.Hundreds() => 200
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Hundreds(this int input) =>
        input * 100;

    /// <summary>
    /// Multiplies an unsigned integer by 100, providing a more readable way to express hundreds in code.
    /// </summary>
    /// <param name="input">The unsigned integer value to multiply by 100.</param>
    /// <returns>The input value multiplied by 100.</returns>
    /// <example>
    /// <code>
    /// 4U.Hundreds() => 400U
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Hundreds(this uint input) =>
        input * 100;

    /// <summary>
    /// Multiplies a long integer by 100, providing a more readable way to express hundreds in code.
    /// </summary>
    /// <param name="input">The long integer value to multiply by 100.</param>
    /// <returns>The input value multiplied by 100.</returns>
    /// <example>
    /// <code>
    /// 4L.Hundreds() => 400L
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Hundreds(this long input) =>
        input * 100;

    /// <summary>
    /// Multiplies an unsigned long integer by 100, providing a more readable way to express hundreds in code.
    /// </summary>
    /// <param name="input">The unsigned long integer value to multiply by 100.</param>
    /// <returns>The input value multiplied by 100.</returns>
    /// <example>
    /// <code>
    /// 4UL.Hundreds() => 400UL
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Hundreds(this ulong input) =>
        input * 100;

    /// <summary>
    /// Multiplies a double by 100, providing a more readable way to express hundreds in code.
    /// </summary>
    /// <param name="input">The double value to multiply by 100.</param>
    /// <returns>The input value multiplied by 100.</returns>
    /// <example>
    /// <code>
    /// 4.0.Hundreds() => 400.0
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Hundreds(this double input) =>
        input * 100;

    /// <summary>
    /// Multiplies an integer by 1000, providing a more readable way to express thousands in code.
    /// </summary>
    /// <param name="input">The integer value to multiply by 1000.</param>
    /// <returns>The input value multiplied by 1000.</returns>
    /// <example>
    /// <code>
    /// 3.Thousands() => 3000
    /// 10.Thousands() => 10000
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Thousands(this int input) =>
        input * 1000;

    /// <summary>
    /// Multiplies an unsigned integer by 1000, providing a more readable way to express thousands in code.
    /// </summary>
    /// <param name="input">The unsigned integer value to multiply by 1000.</param>
    /// <returns>The input value multiplied by 1000.</returns>
    /// <example>
    /// <code>
    /// 3U.Thousands() => 3000U
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Thousands(this uint input) =>
        input * 1000;

    /// <summary>
    /// Multiplies a long integer by 1000, providing a more readable way to express thousands in code.
    /// </summary>
    /// <param name="input">The long integer value to multiply by 1000.</param>
    /// <returns>The input value multiplied by 1000.</returns>
    /// <example>
    /// <code>
    /// 3L.Thousands() => 3000L
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Thousands(this long input) =>
        input * 1000;

    /// <summary>
    /// Multiplies an unsigned long integer by 1000, providing a more readable way to express thousands in code.
    /// </summary>
    /// <param name="input">The unsigned long integer value to multiply by 1000.</param>
    /// <returns>The input value multiplied by 1000.</returns>
    /// <example>
    /// <code>
    /// 3UL.Thousands() => 3000UL
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Thousands(this ulong input) =>
        input * 1000;

    /// <summary>
    /// Multiplies a double by 1000, providing a more readable way to express thousands in code.
    /// </summary>
    /// <param name="input">The double value to multiply by 1000.</param>
    /// <returns>The input value multiplied by 1000.</returns>
    /// <example>
    /// <code>
    /// 3.0.Thousands() => 3000.0
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Thousands(this double input) =>
        input * 1000;

    /// <summary>
    /// Multiplies an integer by 1,000,000, providing a more readable way to express millions in code.
    /// </summary>
    /// <param name="input">The integer value to multiply by 1,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000.</returns>
    /// <example>
    /// <code>
    /// 2.Millions() => 2000000
    /// 5.Millions() => 5000000
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Millions(this int input) =>
        input * 1000000;

    /// <summary>
    /// Multiplies an unsigned integer by 1,000,000, providing a more readable way to express millions in code.
    /// </summary>
    /// <param name="input">The unsigned integer value to multiply by 1,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000.</returns>
    /// <example>
    /// <code>
    /// 2U.Millions() => 2000000U
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Millions(this uint input) =>
        input * 1000000;

    /// <summary>
    /// Multiplies a long integer by 1,000,000, providing a more readable way to express millions in code.
    /// </summary>
    /// <param name="input">The long integer value to multiply by 1,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000.</returns>
    /// <example>
    /// <code>
    /// 2L.Millions() => 2000000L
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Millions(this long input) =>
        input * 1000000;

    /// <summary>
    /// Multiplies an unsigned long integer by 1,000,000, providing a more readable way to express millions in code.
    /// </summary>
    /// <param name="input">The unsigned long integer value to multiply by 1,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000.</returns>
    /// <example>
    /// <code>
    /// 2UL.Millions() => 2000000UL
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Millions(this ulong input) =>
        input * 1000000;

    /// <summary>
    /// Multiplies a double by 1,000,000, providing a more readable way to express millions in code.
    /// </summary>
    /// <param name="input">The double value to multiply by 1,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000.</returns>
    /// <example>
    /// <code>
    /// 2.0.Millions() => 2000000.0
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Millions(this double input) =>
        input * 1000000;

    /// <summary>
    /// Multiplies an integer by 1,000,000,000 (one billion in short scale), providing a more readable way to express billions in code.
    /// </summary>
    /// <param name="input">The integer value to multiply by 1,000,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000,000.</returns>
    /// <example>
    /// <code>
    /// 1.Billions() => 1000000000
    /// 2.Billions() => 2000000000
    /// </code>
    /// </example>
    /// <remarks>
    /// Uses the short scale definition where 1 billion = 1,000,000,000 (10^9).
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int Billions(this int input) =>
        input * 1000000000;

    /// <summary>
    /// Multiplies an unsigned integer by 1,000,000,000 (one billion in short scale), providing a more readable way to express billions in code.
    /// </summary>
    /// <param name="input">The unsigned integer value to multiply by 1,000,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000,000.</returns>
    /// <remarks>
    /// Uses the short scale definition where 1 billion = 1,000,000,000 (10^9).
    /// </remarks>
    /// <example>
    /// <code>
    /// 1U.Billions() => 1000000000U
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static uint Billions(this uint input) =>
        input * 1000000000;

    /// <summary>
    /// Multiplies a long integer by 1,000,000,000 (one billion in short scale), providing a more readable way to express billions in code.
    /// </summary>
    /// <param name="input">The long integer value to multiply by 1,000,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000,000.</returns>
    /// <remarks>
    /// Uses the short scale definition where 1 billion = 1,000,000,000 (10^9).
    /// </remarks>
    /// <example>
    /// <code>
    /// 1L.Billions() => 1000000000L
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static long Billions(this long input) =>
        input * 1000000000;

    /// <summary>
    /// Multiplies an unsigned long integer by 1,000,000,000 (one billion in short scale), providing a more readable way to express billions in code.
    /// </summary>
    /// <param name="input">The unsigned long integer value to multiply by 1,000,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000,000.</returns>
    /// <remarks>
    /// Uses the short scale definition where 1 billion = 1,000,000,000 (10^9).
    /// </remarks>
    /// <example>
    /// <code>
    /// 1UL.Billions() => 1000000000UL
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ulong Billions(this ulong input) =>
        input * 1000000000;

    /// <summary>
    /// Multiplies a double by 1,000,000,000 (one billion in short scale), providing a more readable way to express billions in code.
    /// </summary>
    /// <param name="input">The double value to multiply by 1,000,000,000.</param>
    /// <returns>The input value multiplied by 1,000,000,000.</returns>
    /// <remarks>
    /// Uses the short scale definition where 1 billion = 1,000,000,000 (10^9).
    /// </remarks>
    /// <example>
    /// <code>
    /// 1.0.Billions() => 1000000000.0
    /// </code>
    /// </example>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Billions(this double input) =>
        input * 1000000000;
}