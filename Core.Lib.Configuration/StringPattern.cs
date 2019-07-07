using Core.Lib.Helper;

namespace Core.Lib.Configuration
{
    public class StringPattern
    {
        public static StringPattern NoPrefix => Empty.String;
        public static StringPattern HasName => "{filename}";
        public static StringPattern HasExtension => "{extension}";
        public static StringPattern ExtensionName => $"{HasExtension}:{HasName}";
        public static StringPattern NameExtension => $"{HasName}:{HasExtension}";
        public static StringPattern OriginFileName => $"{HasName}.{HasExtension}";


        public string Value;

        public override int GetHashCode() => Value.GetHashCode();
        public override bool Equals(object obj) => obj?.GetHashCode() == GetHashCode();

        public static implicit operator string(StringPattern pattern)
            => pattern.Value;

        public static implicit operator StringPattern(string pattern)
            => new StringPattern { Value = pattern };
    }
}
