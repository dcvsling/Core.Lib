using System;
using System.Reflection;

namespace Core.Lib.Reflections.Comments
{
    public interface ICommentProvider
    {
        string Get(MemberInfo member);
        string Get(Type type);
    }
}