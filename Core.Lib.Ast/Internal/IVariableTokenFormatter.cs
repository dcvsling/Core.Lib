using System.Collections.Generic;

namespace Core.Lib.Ast.Internal
{
    public interface IVariableTokenFormatter
    {
        string OriginalFormat { get; }

        string Format(object[] values);
        KeyValuePair<string, object> GetValue(object[] values, int index);
        IEnumerable<KeyValuePair<string, object>> GetValues(object[] values);
    }
}