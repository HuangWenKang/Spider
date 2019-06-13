using Microsoft.EntityFrameworkCore;
using Spider.Scheduler.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Spider.Scheduler.Infrastructure.Repositories
{
    public class ScheduleJobRepository : IRepository<ScheduleJob>
    {
        private JobContext _dbContext;

        public ScheduleJobRepository(JobContext dbContext)
        {
            _dbContext = dbContext;            
        }

        public void Add(ScheduleJob newEntity)
        {
            _dbContext.ScheduleJobs.Add(newEntity);
        }

        public void AddRange(List<ScheduleJob> range)
        {
            _dbContext.ScheduleJobs.AddRange(range);
        }

        public List<ScheduleJob> Find(Func<ScheduleJob, bool> match)
        {
            return _dbContext.ScheduleJobs.Where(match).ToList();
        }

        public List<ScheduleJob> FindAll()
        {
            return _dbContext.ScheduleJobs.ToList();
        }

        public ScheduleJob FindByID(int id)
        {
            var results = _dbContext.ScheduleJobs.Where(d => d.ID == id);

            return results.FirstOrDefault();
        }

        public void Remove(ScheduleJob entity)
        {
            _dbContext.ScheduleJobs.Remove(entity);
        }

        public void Update(ScheduleJob entity)
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
    }
}
