using Microsoft.EntityFrameworkCore;
using Spider.Scheduler.Models;

namespace Spider.Scheduler.Infrastructure.Repositories
{
    public class JobContext : DbContext
    {
        public DbSet<ScheduleJob> ScheduleJobs { get; set; }
        
        public JobContext(DbContextOptions<JobContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
