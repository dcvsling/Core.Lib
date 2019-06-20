using System;

namespace Core.Lib.Reflections
{
    /// <summary>
    /// The r.
    /// </summary>
    /// <param name="r">The r.</param>
    /// <returns>The <see cref="T:TDelegate"/>.</returns>
    /// <typeparam name="TDelegate"></typeparam>
    public delegate TDelegate Recursive<TDelegate>(Recursive<TDelegate> r) where TDelegate : Delegate;

    /// <summary>
    /// The recurse factory.
    /// </summary>
    /// <param name="func">The func.</param>
    /// <returns>The <see cref="T:R{TDelegate}"/>.</returns>
    /// <typeparam name="TDelegate"></typeparam>
    public delegate Recursive<TDelegate> RecurseFactory<TDelegate>(Func<TDelegate, TDelegate> func) where TDelegate : Delegate;
}
