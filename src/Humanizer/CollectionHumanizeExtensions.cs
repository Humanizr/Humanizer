namespace Humanizer;

/// <summary>
/// Humanizes an IEnumerable into a human readable list
/// </summary>
public static class CollectionHumanizeExtensions
{
    /// <summary>
    /// Transforms a collection into a human-readable string representation by calling <see cref="object.ToString"/> 
    /// on each element and combining them with the default separator for the current culture (typically ", " with 
    /// "and" before the last item).
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be humanized. Must not be null.</param>
    /// <returns>
    /// A formatted string representation of the collection elements separated by culture-specific separators.
    /// For English, this typically produces: "item1, item2 and item3".
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> is null.</exception>
    /// <example>
    /// <code>
    /// new[] { 1, 2, 3 }.Humanize() => "1, 2 and 3"
    /// new[] { "Alice", "Bob", "Charlie" }.Humanize() => "Alice, Bob and Charlie"
    /// new[] { "single" }.Humanize() => "single"
    /// new string[] { }.Humanize() => ""
    /// </code>
    /// </example>
    public static string Humanize<T>(this IEnumerable<T> collection) =>
        Configurator.CollectionFormatter.Humanize(collection);

    /// <summary>
    /// Transforms a collection into a human-readable string representation using a custom formatter function
    /// for each element, combined with the default separator for the current culture.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be humanized. Must not be null.</param>
    /// <param name="displayFormatter">
    /// A function that converts each element of type <typeparamref name="T"/> to a string representation.
    /// Must not be null.
    /// </param>
    /// <returns>
    /// A formatted string representation of the collection elements, where each element is formatted
    /// using <paramref name="displayFormatter"/> and separated by culture-specific separators.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="collection"/> or <paramref name="displayFormatter"/> is null.
    /// </exception>
    /// <example>
    /// <code>
    /// var people = new[] { new Person { Name = "Alice", Age = 30 }, new Person { Name = "Bob", Age = 25 } };
    /// people.Humanize(p => p.Name) => "Alice and Bob"
    /// people.Humanize(p => $"{p.Name} ({p.Age})") => "Alice (30) and Bob (25)"
    /// </code>
    /// </example>
    public static string Humanize<T>(this IEnumerable<T> collection, Func<T, string> displayFormatter)
    {
        ArgumentNullException.ThrowIfNull(displayFormatter);

        return Configurator.CollectionFormatter.Humanize(collection, displayFormatter);
    }

    /// <summary>
    /// Transforms a collection into a human-readable string representation using a custom formatter function
    /// that returns an object for each element (which will be converted to string), combined with the default 
    /// separator for the current culture.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be humanized. Must not be null.</param>
    /// <param name="displayFormatter">
    /// A function that converts each element of type <typeparamref name="T"/> to an object that will be 
    /// converted to its string representation. Must not be null.
    /// </param>
    /// <returns>
    /// A formatted string representation of the collection elements, where each element is formatted
    /// using <paramref name="displayFormatter"/> and separated by culture-specific separators.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="collection"/> or <paramref name="displayFormatter"/> is null.
    /// </exception>
    /// <example>
    /// <code>
    /// var numbers = new[] { 1, 2, 3 };
    /// numbers.Humanize(n => n * 2) => "2, 4 and 6"
    /// </code>
    /// </example>
    public static string Humanize<T>(this IEnumerable<T> collection, Func<T, object> displayFormatter)
    {
        ArgumentNullException.ThrowIfNull(displayFormatter);

        return Configurator.CollectionFormatter.Humanize(collection, displayFormatter);
    }

    /// <summary>
    /// Transforms a collection into a string representation by calling <see cref="object.ToString"/>
    /// on each element and combining them with the specified separator.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be humanized. Must not be null.</param>
    /// <param name="separator">The string to use as a separator between elements.</param>
    /// <returns>
    /// A string representation of the collection elements separated by the specified separator.
    /// </returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="collection"/> is null.</exception>
    /// <example>
    /// <code>
    /// new[] { 1, 2, 3 }.Humanize(" | ") => "1 | 2 | 3"
    /// new[] { "Alice", "Bob" }.Humanize("; ") => "Alice; Bob"
    /// </code>
    /// </example>
    public static string Humanize<T>(this IEnumerable<T> collection, string separator) =>
        Configurator.CollectionFormatter.Humanize(collection, separator);

    /// <summary>
    /// Transforms a collection into a string representation using a custom formatter function
    /// for each element, combined with the specified separator.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be humanized. Must not be null.</param>
    /// <param name="displayFormatter">
    /// A function that converts each element of type <typeparamref name="T"/> to a string representation.
    /// Must not be null.
    /// </param>
    /// <param name="separator">The string to use as a separator between formatted elements.</param>
    /// <returns>
    /// A string representation of the collection elements, where each element is formatted
    /// using <paramref name="displayFormatter"/> and separated by the specified separator.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="collection"/> or <paramref name="displayFormatter"/> is null.
    /// </exception>
    /// <example>
    /// <code>
    /// var people = new[] { new Person { Name = "Alice" }, new Person { Name = "Bob" } };
    /// people.Humanize(p => p.Name, " | ") => "Alice | Bob"
    /// </code>
    /// </example>
    public static string Humanize<T>(this IEnumerable<T> collection, Func<T, string> displayFormatter, string separator)
    {
        ArgumentNullException.ThrowIfNull(displayFormatter);

        return Configurator.CollectionFormatter.Humanize(collection, displayFormatter, separator);
    }

    /// <summary>
    /// Transforms a collection into a string representation using a custom formatter function
    /// that returns an object for each element (which will be converted to string), combined with the specified separator.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    /// <param name="collection">The collection to be humanized. Must not be null.</param>
    /// <param name="displayFormatter">
    /// A function that converts each element of type <typeparamref name="T"/> to an object that will be
    /// converted to its string representation. Must not be null.
    /// </param>
    /// <param name="separator">The string to use as a separator between formatted elements.</param>
    /// <returns>
    /// A string representation of the collection elements, where each element is formatted
    /// using <paramref name="displayFormatter"/> and separated by the specified separator.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="collection"/> or <paramref name="displayFormatter"/> is null.
    /// </exception>
    /// <example>
    /// <code>
    /// var numbers = new[] { 1, 2, 3 };
    /// numbers.Humanize(n => n * 2, " - ") => "2 - 4 - 6"
    /// </code>
    /// </example>
    public static string Humanize<T>(this IEnumerable<T> collection, Func<T, object> displayFormatter, string separator)
    {
        ArgumentNullException.ThrowIfNull(displayFormatter);

        return Configurator.CollectionFormatter.Humanize(collection, displayFormatter, separator);
    }
}