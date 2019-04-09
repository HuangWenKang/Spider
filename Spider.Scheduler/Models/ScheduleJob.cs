using System;

namespace Spider.Scheduler.Models
{
    public class ScheduleJob
    {
        public enum RequestStatus
        {
            Scheduled,
            Processing,
            Complete,
            Failed,
            Removed,
            Stopped
        }

        public enum RecurringScheduleType
        {
            Daily,
            Hourly,
            Minutely,
            Monthly,
            Weekly,
            Yearly
        }

        public enum JobType
        {
            MSDNBlog,
            GithubRepository,
            GithubCommit,
            GithubTopic            
        }

        public ScheduleJob()
        {
            RequestDate = DateTime.Now;
            Status = RequestStatus.Scheduled;
        }

        
        public DateTime RequestDate { get; set; }
        
        public string StatusText { get { return Status.ToString(); } }
        
        public RecurringScheduleType RecurringSchedule { get; set; }

        public RequestStatus Status { get; set; }

        public JobType JobCategory { get; set; }

        public string JobCategoryText => JobCategory.ToString();

        public int ID { get; set; }

        public string WebApiUrl { get; set; }
        public string JsonPayload { get; set; }

        public string Name { get; set; }

        public string LanguageListUrl { get; set; }
    }
}
