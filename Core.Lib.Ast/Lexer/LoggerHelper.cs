using Microsoft.Extensions.Logging;
using System;

namespace Core.Lib.Ast.Lexer
{

    internal static class LoggerHelper
    {
        private static Action<ILogger, string, string, Exception> LogOperationMatching
            = LoggerMessage.Define<string, string>(LogLevel.Debug, 1, "{char} is match {typeName}");
        private static Action<ILogger, string, string, Exception> LogTokenCreatedOperation
            = LoggerMessage.Define<string, string>(LogLevel.Debug, 2, "{token} is Created by {typeName}");
        private static Action<ILogger, Exception> LogLexerStart
            = LoggerMessage.Define(LogLevel.Debug, 3, "Start Lexer");
        private static Action<ILogger, Exception> LogLexerEnd
            = LoggerMessage.Define(LogLevel.Debug, 4, "End Lexer");
        private static Action<ILogger, string, Exception> LogLexerError
            = LoggerMessage.Define<string>(LogLevel.Error, 5, "Lexer Error : {message}");
        private static Action<ILogger, string, string, Exception> LogLexerOperationError
            = LoggerMessage.Define<string, string>(LogLevel.Error, 6, "Operation {typename} Error : {message}");
        private static Action<ILogger, string, Exception> LogOperatorsCreating
            = LoggerMessage.Define<string>(LogLevel.Debug, 7, "Operator {operator} Creating");

        public static void OnOperationMatching(this ILogger logger, LexerOperationContext context, Type type)
            => LogOperationMatching(logger, context.Content[0].ToString(), type.Name, null);
        public static void OnTokenCreatedOperation(this ILogger logger, LexerOperationContext context, Type type)
            => LogTokenCreatedOperation(logger, context.Token, type.Name, null);
        public static void OnLexerStart(this ILogger logger)
            => LogLexerStart(logger, null);
        public static void OnLexerEnd(this ILogger logger)
            => LogLexerEnd(logger, null);
        public static void OnLexerError(this ILogger logger, Exception exception, string message = default)
            => LogLexerError(logger, message ?? exception.Message, exception);
        public static void OnLexerOperationError(this ILogger logger, Exception exception, Type type, string message)
            => LogLexerOperationError(logger, type.Name, message, exception);
        public static void OnOperatorsCreating(this ILogger logger, string name)
            => LogOperatorsCreating(logger, name, null);

    }
}