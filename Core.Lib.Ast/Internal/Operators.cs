namespace Core.Lib.Ast.Internal
{
    using Lexer;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Logging.Abstractions;
    using Models;
    using Reflections;
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    public delegate Func<Expression, Expression> VisitMiddleware(params Expression[] inputs);
    internal class Operators : ConcurrentDictionary<string, Operator>
    {
        private static ConcurrentDictionary<string, Operators> _globalCache = new ConcurrentDictionary<string, Operators>();
        private readonly ILogger _logger;

        public Operators(ILogger logger)
        {
            _logger = logger;
        }

        private Operators(Operators oldOp, Operators newOp)
        {
            _logger = oldOp._logger;
            oldOp.Union(newOp).Each(Add);
        }

        public Operators New()
            => new Operators(_logger);
        public Operators Add(string name, Func<Operators> factory)
            => new Operators(this, _globalCache.GetOrAdd(name, _ => DecorateLogger(name, factory).Invoke()));

        private Func<Operators> DecorateLogger(string name, Func<Operators> factory)
            => () =>
            {
                _logger.OnOperatorsCreating(name);
                return factory();
            };

        private void Add(KeyValuePair<string, Operator> kvp)
            => TryAdd(kvp.Key, kvp.Value);

        public static Operators LoadDefualt(ILogger logger = default)
            => new Operators(logger ?? NullLogger.Instance)
                .LoadOperator(typeof(Enumerable))
                .LoadOperator(typeof(Expression))
                .LoadSign()
                .LoadCommonSign();

        public Operators LoadOperator(Type type)
            => Add(type.Name, () => type.GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Select(x => new Operator {
                    Name = x.Name,
                    Middleware = new Lazy<VisitMiddleware>(() => inputs => last => Expression.Call(null, x, LinqHelper.Prepend(inputs, last)))
                })
                .AggregateSeed(new Operators(_logger),
                   (op, x) => op.AddOrUpdate(
                   x.Name,
                   x,
                   (_, old) => Merge(old, x)
                   )));

        private static Operator Merge(Operator old, Operator @new)
        {
            if (string.IsNullOrWhiteSpace(old.Value))
                old.Value = @new.Value;
            if (old.Middleware == default)
                old.Middleware = @new.Middleware;
            return old;
        }

        public void Add(Operator @operator)
            => AddOrUpdate(
                @operator.Value,
                @operator,
                (_, old) => Merge(old, @operator));

        public static Operators operator +(Operators left, Operators right)
            => new Operators(left, right);

        public static bool operator ==(Operators left, Operators right)
            => left.GetHashCode() == right.GetHashCode();
        public static bool operator !=(Operators left, Operators right)
            => !(left == right);

        public override int GetHashCode()
            => Values.Distinct().Sum(x => x.GetHashCode());

        public override bool Equals(object obj)
            => GetHashCode().Equals(obj.GetHashCode());
    }
}