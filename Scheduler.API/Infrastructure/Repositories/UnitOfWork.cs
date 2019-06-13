using Microsoft.EntityFrameworkCore;
using Spider.Scheduler.Models;

namespace Spider.Scheduler.Infrastructure.Repositories
{
    public class UnitofWork : IUnitOfWork
    {
        private readonly IRepository<ScheduleJob> _scheduleJobs;
        private JobContext _dbContext;

        public UnitofWork(DbContextOptions<JobContext> options)
        {
            _dbContext = new JobContext(options);

            _scheduleJobs = new ScheduleJobRepository(_dbContext);            
        }

        public IRepository<ScheduleJob> ScheduleJobs => _scheduleJobs;

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
