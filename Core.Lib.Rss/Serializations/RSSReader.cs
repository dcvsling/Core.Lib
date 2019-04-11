using Core.Lib.RSS.Models;
using Microsoft.SyndicationFeed;
using Microsoft.SyndicationFeed.Rss;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace Core.Lib.RSS.Serializations
{
    public class RSSReader
    {
        async public Task<List<RSSItem>> Read(Stream content)
        {
            var list = new List<RSSItem>();

            using (var xmlReader = XmlReader.Create(content))
            {
                var feedReader = new RssFeedReader(xmlReader);
                RSSItem rss = null;
                while (await feedReader.Read())
                {
                    switch (feedReader.ElementType)
                    {
                        // Read category
                        case SyndicationElementType.Category:
                            rss.Categories.Add((await feedReader.ReadCategory()).MapTo());
                            break;

                        // Read Image
                        case SyndicationElementType.Image:
                            rss.Images.Add(await feedReader.ReadImage());
                            break;

                        // Read Item
                        case SyndicationElementType.Item:
                            rss = (await feedReader.ReadItem()).MapTo();
                            break;

                        // Read link
                        case SyndicationElementType.Link:
                            rss.Links.Add((await feedReader.ReadLink()).MapTo());
                            break;

                        // Read Person
                        case SyndicationElementType.Person:
                            rss.Contributors.Add((await feedReader.ReadPerson()).MapTo());
                            break;

                        // Read content
                        default:
                            rss.Content = await feedReader.ReadContent();
                            break;
                    }
                }
                list.Add(rss);
            }
            return list;
        }
    }
}

