using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Core.Lib.Helper
{
    /// <summary>
    /// The trace helper class.
    /// </summary>
    [DebuggerStepThrough]
    public static class TraceHelper
    {
        /// <summary>
        /// The ignore stack.
        /// </summary>
        private static readonly string[] _ignoreStack = new[] { nameof(GetCallerMethodBase), nameof(GetCallerMethodInfo), "op_", "__" };

        /// <summary>
        /// Get the caller method base.
        /// </summary>
        /// <returns>The <see cref="MethodBase"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerHidden]
        public static MethodBase GetCallerMethodBase()
            => new StackTrace()
                .GetFrames()
                .Select(x => x.GetMethod())
                .First(x => !_ignoreStack.Any(x.Name.Contains));

        /// <summary>
        /// Get the caller method info.
        /// </summary>
        /// <returns>The <see cref="MethodInfo"/>.</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerHidden]
        public static MethodInfo GetCallerMethodInfo()
            => (MethodInfo)GetCallerMethodBase();
    }
}
