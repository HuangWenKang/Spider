using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VideoSpider;
using VideoSpider.Services;
using Xunit;

namespace HtmlSpider.Test
{
    public class Channel9VideoSpiderTest
    {
        private readonly IVideoSpiderService videoSpiderService = new Channel9VideoSpiderService();

        [Fact]
        public async Task Test_Search_Video_By_Keyword()
        {
            IList<string> keywords = new List<string>() { "api", "core", "wpf", "wcf"};
            string keyword = "webapi";
            int pageNumber = 1;
            int pageSize = 10;
            var spiderResult = await videoSpiderService.SearchAsync(keyword, pageNumber, pageSize);
            Assert.NotNull(spiderResult);
            Assert.Equal(10 ,spiderResult.Data.Count);
        }
    }
}
