namespace Core.Lib.Ast.Internal
{
    using Microsoft.Extensions.Primitives;
    using Models;
    using Reflections;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    internal static class OperatorsHelper
    {
        public static Operators LoadSign(this Operators operators)
            => operators.Add(nameof(LoadSign), () => new Dictionary<string, string> {
                ["+"] = nameof(ExpressionType.Add),
                ["-"] = nameof(ExpressionType.Subtract),
                ["*"] = nameof(ExpressionType.Multiply),
                ["/"] = nameof(ExpressionType.Divide),
                ["%"] = nameof(ExpressionType.Modulo),
                ["+="] = nameof(ExpressionType.AddAssign),
                ["-="] = nameof(ExpressionType.SubtractAssign),
                ["*="] = nameof(ExpressionType.MultiplyAssign),
                ["/="] = nameof(ExpressionType.DivideAssign),
                ["%="] = nameof(ExpressionType.ModuloAssign),
                [">"] = nameof(ExpressionType.GreaterThan),
                ["<"] = nameof(ExpressionType.LessThan),
                [">="] = nameof(ExpressionType.GreaterThanOrEqual),
                ["<="] = nameof(ExpressionType.LessThanOrEqual),
                ["=="] = nameof(ExpressionType.Equal),
                ["<<"] = nameof(ExpressionType.LeftShift),
                [">>"] = nameof(ExpressionType.RightShift),
                ["<<="] = nameof(ExpressionType.LeftShiftAssign),
                [">>="] = nameof(ExpressionType.RightShiftAssign)
            }.Select(x => new Operator {
                Name = x.Value,
                Value = x.Key,
                Middleware = operators[x.Value].Middleware
            }).AggregateSeed(operators.New(), (op, x) => op.Add(x)));

        public static Operators LoadCommonSign(this Operators operators)
            => operators.Add(
                nameof(LoadCommonSign),
                () => new Dictionary<string, StringValues> {
                    ["["] = "LeftSquareBracket",
                    ["]"] = "RightSquareBracket",
                    ["("] = "LeftCircleBracket",
                    [")"] = "RightCircleBracket",
                    ["{"] = "LeftCurlyBracket",
                    ["}"] = "RightCurlyBracket",
                    [":"] = "Colon",
                    [","] = "Comma",
                    ["،"] = "Period",
                    ["-"] = new[] { "Dash", "Minus" },
                    ["..."] = "Ellipsis",
                    ["!"] = "Exclamation",
                    ["=>"] = "DoubleRightArrow",
                    ["<="] = "DOubleLeftArrow",
                    ["->"] = "RightArrow",
                    ["<-"] = "LeftArrow",
                    ["?"] = "Question",
                    [";"] = "Semicolon",
                    ["/"] = "Slash",
                    ["//"] = "DoubleSlash",
                    ["///"] = "TripleSlash",
                    [" "] = "Space",
                    ["&"] = "Ampersand",
                    ["@"] = "At",
                    ["\\"] = "Backslash",
                    ["\\\\"] = "DoubleBackslash",
                    ["^"] = "Caret",
                    ["="] = "Assign",
                    ["=="] = "Equals",
                    ["==="] = "TripleEquals",
                    ["*"] = "Multiply",
                    ["#"] = "Hash",
                    ["÷"] = "Obelus",
                    ["%"] = "Percent",
                    ["+"] = "Plus",
                    ["'"] = "Prime",
                    ["\""] = "DoublePrime",
                    ["~"] = "Tilde",
                    ["_"] = "Underscore",
                    ["|"] = "VerticalBar",
                    ["||"] = "Pipe",
                    ["¦"] = "BrokenBar",
                    ["$"] = "Currency",
                    ["\r\n"] = nameof(Environment.NewLine),
                    ["\n"] = nameof(Environment.NewLine),
                    ["\t"] = "Tab"
                }
            .Select(x => new Operator {
                Name = x.Value,
                Value = x.Key
            })
            .AggregateSeed(operators.New(),
                (op, x) => op.Add(x)));
    }
}