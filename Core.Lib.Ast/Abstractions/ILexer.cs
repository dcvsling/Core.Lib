using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Lib.Ast.Models;

namespace Core.Lib.Ast.Abstractions
{
    public interface ILexer
    {
        IEnumerable<Token> Lex(string source);
    }
}