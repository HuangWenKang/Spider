using System.Collections.Generic;

namespace VideoSpider.Models
{
    public class VideoSpiderResult
    {
        public string id { get; set; }
        public int total { get; set; }
        public List<Datum> Data { get; set; }
        public Paging paging { get; set; }
    }
}