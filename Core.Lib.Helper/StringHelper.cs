using Humanizer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Core.Lib.Helper
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
        public static string ToJson<T>(this T instance, Newtonsoft.Json.Formatting formatting = Newtonsoft.Json.Formatting.Indented)
            => JsonConvert.SerializeObject(instance, formatting);

        public static string ToYaml<T>(this T instance)
            => _yamlSerializer
                .Serialize(instance);

        private static ISerializer _yamlSerializer
            = new SerializerBuilder()
                .DisableAliases()
                .EmitDefaults()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .WithNamingConvention(new PascalCaseNamingConvention())
                .Build();

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

        public static string WithAssemblyDir(this string path)
            => Path.Combine(AppContext.BaseDirectory, path);

        public static string WithCurrentDir(this string path)
            => Path.Combine(Directory.GetCurrentDirectory(), path);

        public static string Format<T>(this T context, string format)
            => new ContextValuesFormatter(format).Format(context);
    }

}
