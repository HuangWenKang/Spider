using Spider.Scheduler.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Spider.Scheduler.Models.ScheduleJob;

namespace Scheduler.API.Services
{
    public interface ITaskService
    {
        void SaveThenRun(ScheduleJob job);

        Task SyncGithubRepositories(ScheduleJob job);

        Task SyncGithubTopics(ScheduleJob job);

        Task SyncGithubCommits(ScheduleJob job);

        Task SyncMSDNBlog(ScheduleJob job);

        List<ScheduleJob> FindAll();

        ScheduleJob FindById(int id);

        List<ScheduleJob> FindByJobType(JobType jobType);

        void Remove(ScheduleJob job);
    }
}
