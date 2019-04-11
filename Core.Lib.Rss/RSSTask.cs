using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;

namespace Core.Lib.RSS.Models
{
    public static class RSSTask
    {
        public static async Task<List<RSSItem>> ReadFeed(string filepath)
        {
            //
            // Create an XmlReader from file
            // Example: ..\tests\TestFeeds\rss20-2items.xml
            using (var xmlReader = XmlReader.Create(filepath, new XmlReaderSettings() { Async = true }))
            {
                var parser = new RssParser();
                var feedReader = new RssFeedReader(xmlReader, parser);
                var list = new List<RSSItem>();
                //
                // Read the feed
                while (await feedReader.Read())
                {
                    if (feedReader.ElementType == SyndicationElementType.Item)
                    {
                        //
                        // Read the item as generic content
                        ISyndicationContent content = await feedReader.ReadContent();

                        //
                        // Parse the item if needed (unrecognized tags aren't available)
                        // Utilize the existing parser
                        list.Add(parser.CreateItem(content).MapTo());
                    }
                }
                return list;
            }
        }
    }
}

