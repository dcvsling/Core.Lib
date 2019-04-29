namespace Core.Lib.Ast.Lexer
{
    using Models;
    using System;

    public class LexerOperationException : Exception
    {
        public LexerOperationException(
            LexerOperationContext context,
            Location loca,
            Type lexerType,
            Exception ex) : base($"throw at {loca}", ex)
        {
            Loca = loca;
            Context = context;
            LexerType = lexerType;
        }

        public Location Loca { get; }
        public LexerOperationContext Context { get; }
        public Type LexerType { get; }
    }
}