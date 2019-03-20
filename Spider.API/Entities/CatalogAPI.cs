using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.API.Entities
{
    [Table("Catalogs")]
    public class Catalog
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        public string Group { get; set; }
    }
}
