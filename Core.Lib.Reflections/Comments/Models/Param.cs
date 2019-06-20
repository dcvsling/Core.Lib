using System.Xml.Serialization;

namespace Core.Lib.Reflections.Comments
{
    [XmlRoot(ElementName = "param")]
    public class Param
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlText]
        public string Text { get; set; }
    }
}
