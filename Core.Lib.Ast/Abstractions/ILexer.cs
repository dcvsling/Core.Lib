using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Lib.Ast.Models;

namespace Core.Lib.Ast.Abstractions
{
    public interface ILexer
    {
        Task<IEnumerable<Token>> Lex(ReadOnlyMemory<char> source);
    }
}