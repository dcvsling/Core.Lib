using System;
using System.Reflection;

namespace Core.Lib.Reflections.Comments
{
    public interface ICommentProvider
    {
        CommentDetail Get(MemberInfo member);
        CommentDetail Get(Type type);
    }
}