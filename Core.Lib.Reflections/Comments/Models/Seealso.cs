using System.Xml.Serialization;

namespace Core.Lib.Reflections.Comments
{
    [XmlRoot(ElementName = "seealso")]
    public class Seealso
    {
        [XmlAttribute(AttributeName = "cref")]
        public string Cref { get; set; }
    }
}
