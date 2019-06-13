using Spider.Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.Scheduler.Infrastructure.Repositories
{
    public interface IUnitOfWork
    {
        IRepository<ScheduleJob> ScheduleJobs { get; }
        
        void SaveChanges();
    }
}
