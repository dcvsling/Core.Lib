using Core.Lib.Reflections.Comments;
using System;
using Xunit;

namespace Core.Lib.Tests
{
    /// <summary>
    /// type ok
    /// </summary>
    public class CommentTests
    {
        /// <summary>
        /// method ok
        /// </summary>
        [Fact]
        public void get_this_method_comment_will_get_ok()
        {
            Comment.SetRoot(AppContext.BaseDirectory);
            var comment = Comment.Get(typeof(CommentTests).GetMethod(nameof(get_this_method_comment_will_get_ok)));
            Assert.Equal("method ok", comment);
        }

        [Fact]
        public void get_this_type_comment_will_get_ok()
        {
            Comment.SetRoot(AppContext.BaseDirectory);
            var comment = Comment.Get(typeof(CommentTests));
            Assert.Equal("type ok", comment);
        }
    }
}
