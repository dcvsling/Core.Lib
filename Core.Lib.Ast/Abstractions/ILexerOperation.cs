using Core.Lib.Ast.Lexer;
using Core.Lib.Ast.Models;

namespace Core.Lib.Ast.Abstractions
{
    public interface ILexerOperation
    {
        void GetToken(LexerOperationContext context);
    }

    internal interface IChainableOperation : ILexerOperation
    {
        IChainableOperation Append(ILexerOperation operation);
    }
}