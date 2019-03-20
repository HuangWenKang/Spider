using System;
using System.Collections.Generic;
using System.Text;

namespace VideoSpider.Models
{
    public class Datum
    {
        public double searchScore { get; set; }
        public string id { get; set; }
        public string url { get; set; }
        public DateTime published { get; set; }
        public DateTime modified { get; set; }
        public string previewImage { get; set; }
        public string mediumPreviewImage { get; set; }
        public string largePreviewImage { get; set; }
        public string title { get; set; }
        public string groupName { get; set; }
        public string groupType { get; set; }
        public string type { get; set; }
        public string groupUrl { get; set; }
        public string language { get; set; }
        public string summary { get; set; }
        public int mediaDuration { get; set; }
        public List<string> tags { get; set; }
        public List<string> authors { get; set; }
        public List<string> captionLanguages { get; set; }
        public string primaryAudience { get; set; }
        public Rating rating { get; set; }
        public Views views { get; set; }
        public int commentCount { get; set; }
        public bool hasCaptions { get; set; }
    }
}
