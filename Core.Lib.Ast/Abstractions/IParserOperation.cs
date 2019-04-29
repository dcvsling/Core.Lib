using System;
using System.Collections.Generic;
using Core.Lib.Ast.Models;

namespace Core.Lib.Ast.Abstractions
{
    public interface IParserOperation
    {
        Node Parse(IEnumerable<Token> span);
    }
}