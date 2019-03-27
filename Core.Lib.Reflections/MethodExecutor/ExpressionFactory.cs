using System.Reflection;
using System;
using System.Linq.Expressions;
using System.Linq;

namespace Core.Lib.MethodExecutor
{
    internal static class ExpressionFactory
    {
        internal static Func<object, object[], object> CreateExecutor(MethodInfo method)
        {
            var declare = Expression.Parameter(typeof(object));
            var instance = declare is null ? null : Expression.Convert(declare, method.DeclaringType);
            var m = method.IsStatic
                ? StaticMethodCallWithArgs(method)
                : InstanceMethodCallWithArgs(method);

            var parameters = Expression.Parameter(typeof(object[]));

            var arguments = method.GetParameters()
                .Select((x,i) => Expression.Convert(Expression.ArrayIndex(parameters,Expression.Constant(i)),x.ParameterType)).ToArray();
            var args = arguments.Any()
                ? WithArguments(arguments)
                : WithOutArguments();

            var r = method.ReturnType == typeof(void)
                ? NoReturn()
                : Return();

            return Expression.Lambda<Func<object,object[],object>>(r(args(m)(instance)), declare, parameters).Compile();
        }

        private static Func<Expression, Expression[], Expression> StaticMethodCallWithArgs(MethodInfo method)
            => (_, arg) => arg == null
                ? Expression.Call(method)
                : Expression.Call(method, arg);

        private static Func<Expression, Expression[], Expression> InstanceMethodCallWithArgs(MethodInfo method)
            => (instance, arg) => arg == null
                ? Expression.Call(instance, method)
                : Expression.Call(instance, method, arg);

        private static Func<Func<Expression, Expression[], Expression>, Func<Expression, Expression>> WithArguments(params Expression[] args)
            => method => exp => method(exp, args);

        private static Func<Func<Expression, Expression[], Expression>, Func<Expression, Expression>> WithOutArguments()
            => method => exp => method(exp, null);

        private static Func<Expression, Expression> Return()
            => result => Expression.Convert(result, typeof(object));

        private static Func<Expression, Expression> NoReturn()
            => result => Expression.Block(
                result,
                Expression.Constant(new object())
            );

    }
}
