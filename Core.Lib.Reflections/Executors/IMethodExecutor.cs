using System;
using System.Collections.Generic;
using System.Reflection;

namespace Core.Lib.Reflections.Executors
{

    /// <summary>
    /// The IMethodExecutor interface.
    /// </summary>
    public interface IMethodExecutor
    {
        /// <summary>
        /// Gets the method info.
        /// </summary>
        /// <value>The <see cref="MethodInfo"/>.</value>
        MethodInfo MethodInfo { get; }

        /// <summary>
        /// Gets the method parameters.
        /// </summary>
        /// <value>The <see cref="T:ParameterInfo[]"/>.</value>
        IEnumerable<ParameterInfo> MethodParameters { get; }

        /// <summary>
        /// Gets the target type info.
        /// </summary>
        /// <value>The <see cref="TypeInfo"/>.</value>
        TypeInfo TargetTypeInfo { get; }

        /// <summary>
        /// Gets the method return type.
        /// </summary>
        /// <value>The <see cref="Type"/>.</value>
        Type MethodReturnType { get; }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The <see cref="object"/>.</returns>
        object Execute(object target, params object[] parameters);
    }
}
