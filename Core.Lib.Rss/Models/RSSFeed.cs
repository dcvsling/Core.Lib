using System;

namespace Core.Lib.RSS.Models
{
    public class RSSFeed
    {
        public int Id { get; set; }
        public string FeedUrl { get; set; }
        public string Name { get; set; }
        public TimeSpan Interval { get; set; }
    }
}

