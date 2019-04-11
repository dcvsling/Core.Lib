using System.IO;
using System.Text;

namespace Core.Lib.RSS.Serializations
{

    public class StringWriterWithEncoding : StringWriter
    {
        private readonly Encoding _encoding;

        public StringWriterWithEncoding(Encoding encoding)
        {
            this._encoding = encoding;
        }

        public override Encoding Encoding {
            get { return _encoding; }
        }
    }
}

