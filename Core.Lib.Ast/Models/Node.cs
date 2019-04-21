using System;
using System.Linq.Expressions;
using Core.Lib.Reflections;

namespace Core.Lib.Ast.Models
{
    public delegate Func<Expression, Expression> MiddlewareFactory(params Expression[] exps);

    public class Node
    {
        public virtual string Name { get; set; } = Empty.String;

        public Func<MiddlewareFactory, MiddlewareFactory> Middleware { get; set; }
            = factory => inputs => exp => factory(inputs).Invoke(exp);
    }
}