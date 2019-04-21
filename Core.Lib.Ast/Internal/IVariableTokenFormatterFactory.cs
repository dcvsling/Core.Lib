namespace Core.Lib.Ast.Internal
{
    internal interface IVariableTokenFormatterFactory
    {
        IVariableTokenFormatter Create(string name, string format);
    }
}