using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Reflection;

namespace Core.Lib.Reflections.Comments
{
    internal class CommentProvider : ICommentProvider
    {
        private readonly IConfiguration _config;
        private readonly IOptionsMonitorCache<CommentDetail> _cache;
        private const string MEMBERSPATH = "members:member:";
        private const string MEMBER_SEPERATOR = ".";
        public CommentProvider(IConfiguration config, IOptionsMonitorCache<CommentDetail> cache)
        {
            _config = config;
            _cache = cache;
        }

        public CommentDetail Get(Type type)
            => _cache.GetOrAdd(
                type.FullName,
                () => GetMember(
                    GetMemberTypeAtom(type.GetTypeInfo())
                    + ConfigurationPath.KeyDelimiter
                    + type.FullName));

        public CommentDetail Get(MemberInfo member)
            => _cache.GetOrAdd(
                member.DeclaringType.FullName
                + MEMBER_SEPERATOR
                + member.Name,
                () => GetMember(
                    GetMemberTypeAtom(member)
                    + ConfigurationPath.KeyDelimiter
                    + member.DeclaringType.FullName
                    + MEMBER_SEPERATOR
                    + member.Name));

        private char GetMemberTypeAtom(MemberInfo member)
           => Enum.GetName(typeof(MemberTypes), member.MemberType).First();

        private CommentDetail GetMember(string path)
           => _config.GetSection(MEMBERSPATH + path)
                .Get<CommentDetail>();

    }
}
