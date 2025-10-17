// This assembly provides binary compatibility for applications compiled against Humanizer v2.x
// that reference types in the old sub-namespaces (Humanizer.Bytes, Humanizer.Localisation, etc.).
//
// TypeForwardedTo attributes redirect type references from this assembly (Humanizer.Core.dll)
// to the main implementation assembly (Humanizer.dll) where all types now reside in the root
// Humanizer namespace.
//
// This allows v2.x compiled applications to run against v3.0 without recompilation.

using System.Runtime.CompilerServices;

// Humanizer.Bytes → Humanizer
[assembly: TypeForwardedTo(typeof(Humanizer.ByteRate))]
[assembly: TypeForwardedTo(typeof(Humanizer.ByteSize))]

// Humanizer.Configuration → Humanizer
[assembly: TypeForwardedTo(typeof(Humanizer.LocaliserRegistry<>))]

// Humanizer.DateTimeHumanizeStrategy → Humanizer
#if NET6_0_OR_GREATER
[assembly: TypeForwardedTo(typeof(Humanizer.DefaultDateOnlyHumanizeStrategy))]
#endif
[assembly: TypeForwardedTo(typeof(Humanizer.DefaultDateTimeHumanizeStrategy))]
[assembly: TypeForwardedTo(typeof(Humanizer.DefaultDateTimeOffsetHumanizeStrategy))]
#if NET6_0_OR_GREATER
[assembly: TypeForwardedTo(typeof(Humanizer.DefaultTimeOnlyHumanizeStrategy))]
[assembly: TypeForwardedTo(typeof(Humanizer.IDateOnlyHumanizeStrategy))]
#endif
[assembly: TypeForwardedTo(typeof(Humanizer.IDateTimeHumanizeStrategy))]
[assembly: TypeForwardedTo(typeof(Humanizer.IDateTimeOffsetHumanizeStrategy))]
#if NET6_0_OR_GREATER
[assembly: TypeForwardedTo(typeof(Humanizer.ITimeOnlyHumanizeStrategy))]
[assembly: TypeForwardedTo(typeof(Humanizer.PrecisionDateOnlyHumanizeStrategy))]
#endif
[assembly: TypeForwardedTo(typeof(Humanizer.PrecisionDateTimeHumanizeStrategy))]
[assembly: TypeForwardedTo(typeof(Humanizer.PrecisionDateTimeOffsetHumanizeStrategy))]
#if NET6_0_OR_GREATER
[assembly: TypeForwardedTo(typeof(Humanizer.PrecisionTimeOnlyHumanizeStrategy))]
#endif

// Humanizer.Inflections → Humanizer
[assembly: TypeForwardedTo(typeof(Humanizer.Vocabulary))]

// Humanizer.Localisation → Humanizer
[assembly: TypeForwardedTo(typeof(Humanizer.DataUnit))]
[assembly: TypeForwardedTo(typeof(Humanizer.Tense))]
[assembly: TypeForwardedTo(typeof(Humanizer.TimeUnit))]

// Humanizer.Localisation.CollectionFormatters → Humanizer
[assembly: TypeForwardedTo(typeof(Humanizer.ICollectionFormatter))]

// Humanizer.Localisation.DateToOrdinalWords → Humanizer
#if NET6_0_OR_GREATER
[assembly: TypeForwardedTo(typeof(Humanizer.IDateOnlyToOrdinalWordConverter))]
#endif
[assembly: TypeForwardedTo(typeof(Humanizer.IDateToOrdinalWordConverter))]

// Humanizer.Localisation.Formatters → Humanizer
[assembly: TypeForwardedTo(typeof(Humanizer.DefaultFormatter))]
[assembly: TypeForwardedTo(typeof(Humanizer.IFormatter))]

// Humanizer.Localisation.NumberToWords → Humanizer
[assembly: TypeForwardedTo(typeof(Humanizer.INumberToWordsConverter))]

// Humanizer.Localisation.Ordinalizers → Humanizer
[assembly: TypeForwardedTo(typeof(Humanizer.IOrdinalizer))]

// Humanizer.Localisation.TimeToClockNotation → Humanizer
#if NET6_0_OR_GREATER
[assembly: TypeForwardedTo(typeof(Humanizer.ITimeOnlyToClockNotationConverter))]
#endif

// Humanizer.Localisation.WordsToNumber → Humanizer
[assembly: TypeForwardedTo(typeof(Humanizer.IWordsToNumberConverter))]
