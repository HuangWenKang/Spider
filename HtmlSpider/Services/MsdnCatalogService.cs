using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using CsQuery;
using HtmlSpider.Models;
using System.Linq;

namespace HtmlSpider.Services
{
    public class MsdnCatalogService : ICatalogService
    {
        public async Task<IList<ApiCatalog>> SearchAsync(string homePage)
        {
            IList<ApiCatalog> apiCatalogs = new List<ApiCatalog>();
            HttpClient _httpClient = new HttpClient();
            var request = new HttpRequestMessage(
               HttpMethod.Get,homePage);

            using (var response = await _httpClient.SendAsync(request,
                HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();
                var stream = await response.Content.ReadAsStreamAsync();
                var dom = CQ.Create(stream);

                var headers = dom.Select("div.catalog > h2");
                apiCatalogs = headers.Select( h=> new ApiCatalog
                {
                    Name = h.TextContent,
                    Items = ExtractClinks(h.NextElementSibling)
                }).ToList();                
            }                        
            return apiCatalogs;            
        }

        private IList<ApiItem> ExtractClinks(IDomObject element)
        {
            return element.ChildElements.Select(e => new ApiItem
            {
                Id = e.FirstElementChild.Attributes["id"].ToString(),
                Name = e.FirstElementChild.TextContent,
                Link = e.FirstElementChild.Attributes["href"].ToString()
            }).ToList();                        
        }
    }
}
