using System;
using System.Collections.Generic;
using System.Text;

namespace HtmlSpider.Models
{
    public class ApiCatalog
    {        
        public string Name { get; set; }
        public IList<ApiItem> Items { get; set; }
    }
}
