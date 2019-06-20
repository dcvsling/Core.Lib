using System.Xml.Serialization;

namespace Core.Lib.Reflections.Comments
{

    [XmlRoot(ElementName = "see")]
    public class See
    {
        [XmlAttribute(AttributeName = "cref")]
        public string Cref { get; set; }
    }
}
