using System;
using Core.Lib.Ast.Models;

namespace Core.Lib.Ast.Abstractions
{
    public interface IParserOperation
    {
        Node Parse(ReadOnlySpan<Token> span);
    }
}