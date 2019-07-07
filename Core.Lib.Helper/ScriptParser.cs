using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Core.Lib.Helper
{
    public static class ScriptParser
    {

        private static ConcurrentDictionary<(string script, Type input, Type output), object> _caches
            = new ConcurrentDictionary<(string script, Type input, Type output), object>();
        private const string PATTERN_FORMAT = @"((?!\.)(^|\W|\n)({0})(\W|$))";
        private const string NAME_SEPERATOR = "|";
        private const string FACTORY_SCRIPT = "() => t => ({1})(Convert.ChangeType({0},typeof({1})))";
        private const string PARAMETER_ACCESS = "t.";
        private const BindingFlags LegalMember = BindingFlags.Public
            | BindingFlags.Instance
            | BindingFlags.GetProperty
            | BindingFlags.DeclaredOnly
            | BindingFlags.InvokeMethod;

        public static Func<TInput, TOutput> Parse<TInput, TOutput>(this string script)
        {
            if (_caches.TryGetValue((script, typeof(TInput), typeof(TOutput)), out var value))
            {
                return (Func<TInput, TOutput>)value;
            }
            return CreateAndAdd<TInput, TOutput>(script).GetAwaiter().GetResult();
        }


        async private static Task<Func<TInput, TOutput>> CreateAndAdd<TInput, TOutput>(string script)
        {

            var method = (await CSharpScript.EvaluateAsync<Func<Expression<Func<TInput, TOutput>>>>(
                    string.Format(FACTORY_SCRIPT, script.GetMemberRelationScript<TInput>(PARAMETER_ACCESS), typeof(TOutput).Name),
                    CreateOptions(typeof(TInput), typeof(TOutput), typeof(Func<>))))
                .Invoke()
                .Compile();
            _caches.TryAdd(
                (script, typeof(TInput), typeof(TOutput)),
                method);
            return method;
        }

        private static string CreatePattern(this Type type) => string.Format(PATTERN_FORMAT, type.GetNames());
        private static string GetNames(this Type type) => string.Join(NAME_SEPERATOR, type.GetMembers(LegalMember).Select(x => x.Name));

        private static string GetMemberRelationScript<TInput>(this string script, string accessScript)
            => Regex.Replace(
                script,
                typeof(TInput).CreatePattern(),
                match => match.Index == 0
                    ? accessScript + match.Value
                    : match.Value.Substring(0, 1) + accessScript + match.Value.Substring(1));

        private static ScriptOptions CreateOptions(params Type[] types)
            => types.Aggregate(
                ScriptOptions.Default,
                (options, type) => options.AddReferences(type.Assembly).AddImports(type.Namespace ?? nameof(System)));
    }
}
