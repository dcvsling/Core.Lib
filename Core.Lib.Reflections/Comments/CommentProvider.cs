using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace Core.Lib.Reflections.Comments
{
    internal class CommentProvider : ICommentProvider
    {
        private readonly IConfiguration _config;
        private readonly ConcurrentDictionary<string, StringValues> _cache;
        private const string MEMBERSPATH = "members:member:";
        private const string SUMMARYPATH = ":summary";
        private const string SEPERATOR = "\n";
        public CommentProvider(IConfiguration config, ConcurrentDictionary<string, StringValues> cache)
        {
            _config = config;
            _cache = cache;
        }

        public string Get(Type type)
            => _cache.GetOrAdd(
                type.FullName,
                GetSectionFactory($"{GetMemberTypeAtom(type.GetTypeInfo())}:{type.FullName}"));

        public string Get(MemberInfo member)
            => _cache.GetOrAdd(
                $"{member.DeclaringType.FullName}.{member.Name}",
                GetSectionFactory($"{GetMemberTypeAtom(member)}:{member.DeclaringType.FullName}.{member.Name}"));

        private StringValues GetSectionFactory(string path)
           => _config.GetSection(MEMBERSPATH + path + SUMMARYPATH).Value
                .Split(new[] { SEPERATOR }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(WithoutNullOrWhiteSpace)
                .ToArray();

        private char GetMemberTypeAtom(MemberInfo member)
            => Enum.GetName(typeof(MemberTypes), member.MemberType).First();

        private static bool WithoutNullOrWhiteSpace(string str)
            => !string.IsNullOrWhiteSpace(str);
    }
}
