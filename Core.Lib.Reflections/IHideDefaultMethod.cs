using System.ComponentModel;

namespace Core.Lib.Abstractions
{
    public interface IHideDefaultMethod
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        int GetHashCode();
        [EditorBrowsable(EditorBrowsableState.Never)]
        string ToString();
        [EditorBrowsable(EditorBrowsableState.Never)]
        bool Equals(object obj);
    }
}
