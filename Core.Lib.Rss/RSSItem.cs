using Microsoft.SyndicationFeed;
using System;
using System.Collections.Generic;

namespace Core.Lib.RSS
{
    public class RSSItem
    {
        public String Id { get; set; }
        public String Title { get; set; }
        public String Description { get; set; }
        public List<RSSCategory> Categories { get; set; } = new List<RSSCategory>();
        public List<RSSPerson> Contributors { get; set; } = new List<RSSPerson>();
        public List<RSSLink> Links { get; set; } = new List<RSSLink>();
        public DateTimeOffset LastUpdated { get; set; }
        public DateTimeOffset Published { get; set; }
        public ISyndicationContent Content { get; set; }
        public List<ISyndicationImage> Images { get; set; } = new List<ISyndicationImage>();
    }

    public class RSSChannel : RSSItem
    {
        public List<RSSItem> Items { get; set; } = new List<RSSItem>();
    }
}

