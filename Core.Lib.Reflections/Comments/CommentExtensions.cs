using Core.Lib.Reflections.Comments;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class CommentExtensions
    {
        public static IServiceCollection AddCommentProvider(this IServiceCollection services)
            => services
                .AddOptions()
                .AddSingleton<ICommentProvider, CommentProvider>();
    }
}