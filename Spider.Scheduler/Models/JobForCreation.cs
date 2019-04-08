using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Spider.Scheduler.Models.ScheduleJob;

namespace Spider.Scheduler.Models
{
    public class JobForCreation
    {        
        public RecurringScheduleType RecurringScheduleType { get; set; }

        public string WebApiUrl { get; set; }
        public string JsonPayload { get; set; }
    }
}
