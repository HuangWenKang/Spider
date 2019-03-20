using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using HtmlSpider.Models;

namespace HtmlSpider.Services
{
    public interface ICatalogService
    {
        Task<IList<ApiCatalog>> SearchAsync(string homePage);
    }
}
