using System;

namespace Core.Lib.RSS.Models
{
    public class RSSLink
    {
        public Uri Uri { get; set; }
        public String Title { get; set; }
        public String MediaType { get; set; }
        public String RelationshipType { get; set; }
        public Int64 Length { get; set; }
        public DateTimeOffset LastUpdated { get; set; }



    }
}

