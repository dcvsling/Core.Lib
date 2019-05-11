using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileSystemGlobbing;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Core.Lib.Reflections.Comments
{
    public static class Comment
    {
        static Comment()
        {
            SetRoot(Directory.GetCurrentDirectory());
        }
        private static ConcurrentDictionary<string, IConfiguration> _configurations
            = new ConcurrentDictionary<string, IConfiguration>();

        private static readonly IConfiguration _empty = new ConfigurationBuilder().Build();

        private static ConcurrentDictionary<string, ConcurrentDictionary<string, StringValues>> _cache
            = new ConcurrentDictionary<string, ConcurrentDictionary<string, StringValues>>();

        public static void SetRoot(string root)
            => _configurations = new ConcurrentDictionary<string, IConfiguration>(new Matcher()
                .AddInclude("**/*.xml")
                .GetResultsInFullPath(root)
                .Select(x => x.Replace(root, Empty.String))
                .ToDictionary(
                    Path.GetFileNameWithoutExtension,
                    x => (IConfiguration)new ConfigurationBuilder()
                        .AddXmlFile(x, true)
                        .Build()));

        public static string Get(MemberInfo member)
            => GetProvider(member.DeclaringType.Assembly.GetName().Name).Get(member);
        public static string Get(Type type)
            => GetProvider(type.Assembly.GetName().Name).Get(type);

        private static ICommentProvider GetProvider(string assemblyName)
            => new CommentProvider(
                _configurations.GetOrAdd(assemblyName, _empty),
                _cache.GetOrAdd(assemblyName, _ => new ConcurrentDictionary<string, StringValues>()));
    }
}
