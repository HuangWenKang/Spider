using CsQuery;
using HtmlSpider.Models;
using HtmlSpider.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HtmlSpider.Test
{
    public class MsdnSpiderServiceTest
    {
        private ISpiderService spiderService;

        public MsdnSpiderServiceTest()
        {
            spiderService = new MsdnSpiderService();
        }

        [Fact]
        public async Task Test_Msdn_HomePage()
        {
            string homePage = "https://msdn.microsoft.com/en-us/library/ms123401.aspx";
            IList<ApiCatalog> apiCatalogs = await spiderService.fetch(homePage);
            Assert.True(apiCatalogs.Count > 0);
        }
    }
}
