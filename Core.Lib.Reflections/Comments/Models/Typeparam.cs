using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.Lib.Reflections.Comments
{

    [XmlRoot(ElementName = "typeparam")]
    public class Typeparam
    {
        [XmlAttribute(AttributeName = "name")]
        public string Name { get; set; }
        [XmlText]
        public string Text { get; set; }
    }

    
}
