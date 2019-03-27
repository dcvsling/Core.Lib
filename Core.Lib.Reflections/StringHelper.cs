using Newtonsoft.Json;
using System.Collections.Generic;
using Humanizer;

namespace Core.Lib.Reflections
{

    /// <summary>
    /// The string helper class.
    /// </summary>
    public static class stringHelper
    {
        /// <summary>
        /// The to json.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>The <see cref="string"/>.</returns>
        /// <typeparam name="T"></typeparam>
        public static string ToJson<T>(this T instance)
            => JsonConvert.SerializeObject(instance, Formatting.Indented);

        /// <summary>
        /// The format.
        /// </summary>
        /// <param name="format">The format.</param>
        /// <param name="args">The args.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string Format(this string format, params string[] args)
            => string.Format(format, args);

        /// <summary>
        /// The to join.
        /// </summary>
        /// <param name="texts">The texts.</param>
        /// <param name="separator">The separator.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToJoin(this IEnumerable<string> texts, string separator)
            => string.Join(separator, texts);

        /// <summary>
        /// The to readable key word.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToReadableKeyWord(this string text)
            => text.Dehumanize().Pascalize();

        /// <summary>
        /// The to print word.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToPrintWord(this string text)
            => text.Humanize(LetterCasing.Sentence).Pluralize();

        /// <summary>
        /// The to config word.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToConfigWord(this string text)
            => text.Underscore().ToLowerInvariant();

        /// <summary>
        /// The to inner key word.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public static string ToInnerKeyWord(this string text)
            => text.Underscore().ToUpperInvariant();

    }


}
