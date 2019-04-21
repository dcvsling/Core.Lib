namespace Core.Lib.Ast.Lexer
{
    using Abstractions;

    public interface ILexerOperationFactory
    {
        ILexerOperation GetLexerOperation(string name);
    }
}