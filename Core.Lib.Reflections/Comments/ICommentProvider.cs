using System;
using System.Reflection;

namespace Core.Net.GraphQLConventions
{
    public interface ICommentProvider
    {
        string Get(MemberInfo member);
        string Get(Type type);
    }
}