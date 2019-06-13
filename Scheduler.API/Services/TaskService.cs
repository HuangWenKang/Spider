using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hangfire;
using Scheduler.API.Infrastructure.Clients;
using Scheduler.API.Infrastructure.Converters;
using Spider.Scheduler.Infrastructure.Repositories;
using Spider.Scheduler.Models;
using static Spider.Scheduler.Models.ScheduleJob;

namespace Scheduler.API.Services
{
    public class TaskService : ITaskService
    {
        private IUnitOfWork _uof;
        private IRepository<ScheduleJob> _repository;
        private WebApiClients _webApiClients;
        private BasicDataClients _basicDataClients;
        private IPayloadEnricher _payloadEnricher;

        public TaskService(IUnitOfWork uof, WebApiClients webApiClients, BasicDataClients basicDataClients, IPayloadEnricher payloadEnricher)
        {
            _uof = uof;            
            _webApiClients = webApiClients;
            _basicDataClients = basicDataClients;
            _payloadEnricher = payloadEnricher;
        }

        public List<ScheduleJob> FindAll()
        {
            return _uof.ScheduleJobs.FindAll();
        }

        public ScheduleJob FindById(int id)
        {
            var job = _uof.ScheduleJobs.FindByID(id);
            return job;
        }

        public List<ScheduleJob> FindByJobType(JobType jobType)
        {
            var jobs = _uof.ScheduleJobs.Find(j => j.JobCategory == jobType);
            return jobs;
        }

        public void Remove(ScheduleJob job)
        {
            _uof.ScheduleJobs.Remove(job);
            _uof.SaveChanges();
            RecurringJob.RemoveIfExists(job.ID.ToString());
        }

        public void SaveThenRun(ScheduleJob job)
        {
            if (job.ID > 0)
            {
                _uof.ScheduleJobs.Update(job);
            }
            else
            {
                _uof.ScheduleJobs.Add(job);
            }            
            _uof.SaveChanges();
            
            switch (job.JobCategory)
            {
                case JobType.GithubCommit:                    
                    RecurringJob.AddOrUpdate(job.ID.ToString(), () => SyncGithubCommits(job), job.CronExpression);
                    break;
                case JobType.GithubRepository:
                    RecurringJob.AddOrUpdate(job.ID.ToString(), () => SyncGithubRepositories(job), job.CronExpression);
                    break;
                case JobType.GithubTopic:
                    RecurringJob.AddOrUpdate(job.ID.ToString(), () => SyncGithubTopics(job), job.CronExpression);
                    break;
                case JobType.MSDNBlog:
                    RecurringJob.AddOrUpdate(job.ID.ToString(), () => SyncMSDNBlog(job), job.CronExpression);
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }
        }

        public async Task SyncGithubCommits(ScheduleJob job)
        {
            string jobPayload = job.JsonPayload;
            if (!string.IsNullOrWhiteSpace(job.LanguageListUrl))
            {
                var languages = await _basicDataClients.FetchLanguageList(job.LanguageListUrl);
                jobPayload = _payloadEnricher.AppendLanguages(job.JsonPayload, languages);
            }
            await _webApiClients.CallWebApiAsync(job.WebApiUrl, jobPayload);
        }

        public async Task SyncGithubRepositories(ScheduleJob job)
        {
            await SyncGithubCommits(job);
        }

        public async Task SyncGithubTopics(ScheduleJob job)
        {
            await SyncGithubCommits(job);
        }

        public async Task SyncMSDNBlog(ScheduleJob job)
        {
            string jobPayload = job.JsonPayload;
            if (!string.IsNullOrWhiteSpace(job.LanguageListUrl))
            {
                var languages = await _basicDataClients.FetchLanguageList(job.LanguageListUrl);
                jobPayload = _payloadEnricher.AppendTags(job.JsonPayload, languages);
            }
            await _webApiClients.CallWebApiAsync(job.WebApiUrl, jobPayload);
        }        
    }
}
