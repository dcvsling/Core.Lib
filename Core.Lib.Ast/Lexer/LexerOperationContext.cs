namespace Core.Lib.Ast.Lexer
{
    using Models;
    using Reflections;

    public class LexerOperationContext
    {
        public string Content { get; set; } = Empty.String;
        public bool IsSuccess { get; set; }
        public Token Token { get; set; }

    }
}