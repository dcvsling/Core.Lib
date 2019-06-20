using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.Lib.Reflections.Comments
{
    [XmlRoot(ElementName = "member")]
    public class CommentDetail
    {
        [XmlElement(ElementName = "summary")]
        public string Summary { get; set; }
        [XmlElement(ElementName = "exception")]
        public List<Exception> Exception { get; set; }
        [XmlElement(ElementName = "see")]
        public See See { get; set; }
        [XmlElement(ElementName = "seealso")]
        public Seealso Seealso { get; set; }
        [XmlElement(ElementName = "remarks")]
        public string Remarks { get; set; }
        [XmlElement(ElementName = "example")]
        public string Example { get; set; }
        [XmlElement(ElementName = "typeparam")]
        public Typeparam Typeparam { get; set; }
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlElement(ElementName = "value")]
        public string Value { get; set; }
        [XmlElement(ElementName = "param")]
        public List<Param> Param { get; set; }
        [XmlElement(ElementName = "returns")]
        public string Returns { get; set; }
    }
}
