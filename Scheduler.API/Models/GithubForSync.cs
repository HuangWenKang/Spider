using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Scheduler.API.Models
{
    public class GithubForSync
    {
        public GithubForSync()
        {
            Languages = new List<string>();
            StartYear = 2008;
            EndYear = DateTime.Now.Year;
        }

        public List<string> Languages { get; set; }

        public int StartYear { get; set; }

        public int EndYear { get; set; }
    }
}
