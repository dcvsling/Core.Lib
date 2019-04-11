using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using Core.Net.GraphQLConventions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.KeyPerFile;
using Microsoft.Extensions.FileSystemGlobbing;

namespace Core.Lib
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

        private static ConcurrentDictionary<string, ConcurrentDictionary<string, IConfigurationSection>> _cache
            = new ConcurrentDictionary<string, ConcurrentDictionary<string, IConfigurationSection>>();

        public static void SetRoot(string root)
            => new Matcher()
                .AddInclude("**/*.xml")
                .AddInclude("**/*.dll")
                .GetResultsInFullPath(root)
                .GroupBy(Path.GetFileNameWithoutExtension)
                .Where(x => x.Count() != 2)
                .ToDictionary(x => x.Key, x => new ConfigurationBuilder().AddKeyPerFile(".",true));

        public static string Get(MemberInfo member)
            => GetProvider(member.DeclaringType.Assembly.GetName().Name).Get(member);
        public static string Get(Type type)
            => GetProvider(type.Assembly.GetName().Name).Get(type);

        private static ICommentProvider GetProvider(string assemblyName)
            => new CommentProvider(
                _configurations.GetOrAdd(assemblyName, _empty),
                _cache.GetOrAdd(assemblyName, _ => new ConcurrentDictionary<string, IConfigurationSection>()));

    }
}
