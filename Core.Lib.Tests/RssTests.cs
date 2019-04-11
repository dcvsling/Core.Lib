using Core.Lib.RSS.Models;
using Core.Lib.RSS.Serializations;
using Microsoft.SyndicationFeed;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Core.Lib.Tests
{
    public class RssTests
    {
        [Fact]
        async public Task Test_RSS()
        {
            var list = await RSSTask.ReadFeed("https://blogs.technet.microsoft.com/cloudplatform/rssfeeds/devblogs");
            Assert.NotEmpty(list);
        }


        [Fact]
        async public Task Test_rss_convert_each_other()
        {
            var expect = new RSSItem {
                Categories = { new RSSCategory { Name = "test" } },
                Contributors = { new RSSPerson { Name = "Kevin", Email = "dcvsling@dcvsling.outlook.com", Uri = "dcvsling.github.io" } },
                LastUpdated = DateTime.Now,
                Links = { new RSSLink { Uri = new Uri("https://localhost:5000/blog/README"), Title = "test" } },
                Title = "test",
                Id = "test",
                Content = new SyndicationContent("test", "test contnet"),
                Description = "for test",
                Published = DateTimeOffset.Now,
                Images = null
            };
            var rss = await new RSSWriter().Write(expect);
            var actual = await new RSSReader().Read(new MemoryStream(Encoding.UTF8.GetBytes(rss)));
            Assert.Equal(
#if NETCOREAPP30
                Microsoft.JSInterop.Json.Serialize(expect), 
                Microsoft.JSInterop.Json.Serialize(actual.First())
#else
                Newtonsoft.Json.JsonConvert.SerializeObject(expect),
                Newtonsoft.Json.JsonConvert.SerializeObject(actual.First())
#endif
            );
        }

    }
}
