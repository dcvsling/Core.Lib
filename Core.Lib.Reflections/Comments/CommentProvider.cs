using System.Linq;
using System.Reflection;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;

namespace Core.Net.GraphQLConventions
{
    internal class CommentProvider : ICommentProvider
    {
        private readonly IConfiguration _config;
        private readonly ConcurrentDictionary<string,IConfigurationSection> _cache;
        private const string MembersPath = "doc:Members";

        public CommentProvider(IConfiguration config, ConcurrentDictionary<string,IConfigurationSection> cache)
        {
            _config = config;
            _cache = cache;
        }

        public string Get(Type type)
            => _cache.GetOrAdd(
                type.FullName,
                GetSectionFactory($"T:{type.FullName}"))
                .GetValue<string>("Summary");

        public string Get(MemberInfo member)
            => _cache.GetOrAdd(
                $"{member.DeclaringType.FullName}.{member.Name}",
                GetSectionFactory($"{Enum.GetName(typeof(MemberTypes), member.MemberType).First()}:{member.DeclaringType.FullName}.{member.Name}"))
                .GetValue<string>("Summary");

        private IConfigurationSection GetSectionFactory(string fullName)
           => _config.GetSection(MembersPath)
                .GetChildren()
                .FirstOrDefault(x => x.GetValue<string>("Name") == $"{fullName}");

        
    }
}
