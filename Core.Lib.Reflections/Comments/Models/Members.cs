using System.Collections.Generic;
using System.Xml.Serialization;

namespace Core.Lib.Reflections.Comments
{
    [XmlRoot(ElementName = "members")]
    public class Members
    {
        [XmlElement(ElementName = "member")]
        public List<CommentDetail> Member { get; set; }
    }


}
