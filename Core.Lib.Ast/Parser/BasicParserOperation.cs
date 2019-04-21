namespace Core.Lib.Ast.Parser
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using Core.Lib.Ast.Abstractions;
    using Humanizer;
    using Microsoft.Extensions.Primitives;
    using Models;
    using Reflections;

    internal class BasicParserOperation : IParserOperation
    {
        delegate Func<Expression, Expression> VisitMiddleware(params Expression[] inputs);
        public Node Parse(ReadOnlySpan<Token> span)
        {
            var i = 0;
            while (span.Length >= i)
            {


                i++;
            }

            return default;
        }


        static BasicParserOperation()
        {
            _parsers = new Dictionary<StringValues, VisitMiddleware>();
            typeof(Enumerable).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Each(x => _parsers.Add(
                    x.Name,
                    inputs => last => Expression.Call(null, x, LinqHelper.Prepend(inputs, last))
                ));

            typeof(Expression).GetMethods(BindingFlags.Public | BindingFlags.Static)
                .Each(x => _parsers.Add(
                    x.Name,
                    inputs => last => Expression.Call(null, x, LinqHelper.Prepend(inputs, last))
                ));

            new Dictionary<StringValues, VisitMiddleware>
            {
                ["+"] = _parsers[nameof(ExpressionType.Add)],
                ["-"] = _parsers[nameof(ExpressionType.Subtract)],
                ["*"] = _parsers[nameof(ExpressionType.Multiply)],
                ["/"] = _parsers[nameof(ExpressionType.Divide)],
                ["%"] = _parsers[nameof(ExpressionType.Modulo)],
                [SplitEach("+=")] = _parsers[Humanize(nameof(ExpressionType.AddAssign))],
                [SplitEach("-=")] = _parsers[Humanize(nameof(ExpressionType.SubtractAssign))],
                [SplitEach("*=")] = _parsers[Humanize(nameof(ExpressionType.MultiplyAssign))],
                [SplitEach("/=")] = _parsers[Humanize(nameof(ExpressionType.DivideAssign))],
                [SplitEach("%=")] = _parsers[Humanize(nameof(ExpressionType.ModuloAssign))],
                [">"] = _parsers[Humanize(nameof(ExpressionType.GreaterThan))],
                ["<"] = _parsers[Humanize(nameof(ExpressionType.LessThan))],
                [SplitEach(">=")] = _parsers[Humanize(nameof(ExpressionType.GreaterThanOrEqual))],
                [SplitEach("<=")] = _parsers[Humanize(nameof(ExpressionType.LessThanOrEqual))],
                [SplitEach("==")] = _parsers[nameof(ExpressionType.Equal)],
                [SplitEach("<<")] = _parsers[Humanize(nameof(ExpressionType.LeftShift))],
                [SplitEach(">>")] = _parsers[Humanize(nameof(ExpressionType.RightShift))],
                [SplitEach("<<=")] = _parsers[Humanize(nameof(ExpressionType.LeftShiftAssign))],
                [SplitEach(">>=")] = _parsers[Humanize(nameof(ExpressionType.RightShiftAssign))]
            }.Each(x => _parsers.Add(x.Key, x.Value));

            _orderedParsers = _parsers.GroupBy(x => x.Key.Count)
                .ToDictionary(x => x.Key, x => x.ToDictionary(y => y.Key, y => y.Value));
        }
        private static Dictionary<int, Dictionary<StringValues, VisitMiddleware>> _orderedParsers { get; }

        private static Dictionary<StringValues, VisitMiddleware> _parsers { get; }

        private static StringValues SplitEach(string str)
            => str.Select(x => x.ToString()).ToArray();

        private static StringValues Humanize(string str)
            => str.Humanize(str).Split(' ');
    }
}