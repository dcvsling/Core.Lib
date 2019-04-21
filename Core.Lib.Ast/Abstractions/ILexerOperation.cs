using System;
using System.Threading.Tasks;
using Core.Lib.Ast.Models;

namespace Core.Lib.Ast.Abstractions
{
    public interface ILexerOperation
    {
        Task<Token> GetTokenAsync(ReadOnlyMemory<char> memory);
    }

    internal interface IChainableOperation : ILexerOperation
    {
        IChainableOperation Append(ILexerOperation operation);
    }
}