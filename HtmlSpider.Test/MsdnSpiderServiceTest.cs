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
        private ICatalogService spiderService;

        public MsdnSpiderServiceTest()
        {
            spiderService = new MsdnCatalogService();
        }

        [Fact]
        public async Task Test_Msdn_HomePage()
        {
            string homePage = "https://msdn.microsoft.com/en-us/library/ms123401.aspx";
            IList<ApiCatalog> apiCatalogs = await spiderService.SearchAsync(homePage);
            Assert.True(apiCatalogs.Count > 0);
        }
    }
}
