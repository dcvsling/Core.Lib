using Core.Lib.Configuration;
using Core.Lib.Reflections.Comments;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Xunit;

namespace Core.Lib.Tests
{
    /// <summary>
    /// type ok
    /// </summary>
    public class CommentTests
    {
        private static ICommentProvider CommentProvider
             => new ServiceCollection()
                .AddCommentProvider()
                .AddSingleton<IConfiguration>(
                    new ConfigurationBuilder().AddSettings("").Build())
                .BuildServiceProvider()
                .GetRequiredService<ICommentProvider>();

        /// <summary>
        /// method ok
        /// </summary>
        [Fact]
        public void get_this_method_comment_will_get_ok()
        {
            var comment = CommentProvider.Get(typeof(CommentTests).GetMethod(nameof(get_this_method_comment_will_get_ok)));
            Assert.Equal(
                "method ok", 
                comment.Summary.Split('\n',StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Aggregate(string.Concat));
        }

        [Fact]
        public void get_this_type_comment_will_get_ok()
        {
            Assert.Equal("type ok",
                CommentProvider.Get(typeof(CommentTests)).Summary
                .Split('\n',StringSplitOptions.RemoveEmptyEntries)
                    .Where(x => !string.IsNullOrWhiteSpace(x))
                    .Select(x => x.Trim())
                    .Aggregate(string.Concat));
        }
    }

    // /// <summary>
    // /// type summary
    // /// </summary>
    // /// <exception cref="FormatException">format</exception>
    // /// <exception cref="TypeInitializationException">init</exception>
    // /// <see cref="MarshalByRefObject"/>
    // /// <seealso cref="object"/>
    // /// <remarks>remarks</remarks>
    // /// <example>new CommentExample()</example>
    // /// <typeparam name="T">tt</typeparam>
    // public class CommentExample<T> : MarshalByRefObject where T : class, new()
    // {
    //     /// <summary>
    //     /// property summary
    //     /// </summary>
    //     /// <exception cref="FormatException">format</exception>
    //     /// <exception cref="TypeInitializationException">init</exception>
    //     /// <see cref="MarshalByRefObject"/>
    //     /// <seealso cref="object"/>
    //     /// <remarks>remarks</remarks>
    //     /// <example>new CommentExample()</example>
    //     /// <value>value</value>
    //     public CommentExample<T> Self { get; set; }

    //     /// <summary>
    //     /// property summary
    //     /// </summary>
    //     /// <exception cref="FormatException">format</exception>
    //     /// <exception cref="TypeInitializationException">init</exception>
    //     /// <see cref="MarshalByRefObject"/>
    //     /// <seealso cref="object"/>
    //     /// <remarks>remarks</remarks>
    //     /// <example>new CommentExample()</example>
    //     /// <param name="left">left</param>
    //     /// <param name="right">right</param>
    //     /// <returns>return</returns>
    //     public bool Condition(object left, object right)
    //         => true;
    // }
}
