using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Spider.API.Entities
{
    [Table("Videos")]
    public class Video
    {
        [Key]
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Summary { get; set; }

        public int MediaDuration { get; set; }

        public int TotalViews { get; set; }

        public double SearchScore { get; set; }

        public string Url { get; set; }
    }
}
