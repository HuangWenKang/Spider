using System.Collections.Generic;
using System.Net;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Spider.Scheduler.Models;
using static Spider.Scheduler.Models.ScheduleJob;
using System.Linq;
using Scheduler.API.Services;

namespace Spider.Scheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private ITaskService _taskService;        

        public SchedulerController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public ActionResult<List<dynamic>> ListJobs()
        {
            var jobs = _taskService.FindAll().Select(j => new
            {
                j.ID,
                j.Name,
                j.CronExpression,
                j.WebApiUrl,
                j.JsonPayload,                
                j.LanguageListUrl,                
            });
            return Ok(jobs);
        }

        [Route("{jobType}")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult Create([FromRoute]JobType jobType, [FromBody] JobViewModel jobForCreation)
        {
            var job = new ScheduleJob()
            {
                WebApiUrl = jobForCreation.WebApiUrl,
                JsonPayload = jobForCreation.JsonPayload,                
                Name = jobForCreation.Name,
                JobCategory = jobType,
                LanguageListUrl = jobForCreation.LanguageListUrl,
                CronExpression = jobForCreation.CronExpression
            };

            _taskService.SaveThenRun(job);
            
            return Ok();
        }

        [Route("{jobType}")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult Edit([FromRoute]JobType jobType, [FromBody] JobViewModel jobForUpdate)
        {
            var job = new ScheduleJob()
            {
                ID = jobForUpdate.Id,
                WebApiUrl = jobForUpdate.WebApiUrl,
                JsonPayload = jobForUpdate.JsonPayload,                
                Name = jobForUpdate.Name,                                
                JobCategory = jobType,
                LanguageListUrl = jobForUpdate.LanguageListUrl,
                CronExpression = jobForUpdate.CronExpression

            };

            _taskService.SaveThenRun(job);
            return Ok();
        }

        [Route("{jobId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult Delete([FromRoute] int jobId)
        {
            var job = _taskService.FindById(jobId);
            if (job == null)
            {
                return BadRequest();
            }
            _taskService.Remove(job);

            RecurringJob.RemoveIfExists(jobId.ToString());
            return Ok();
        }
       

    }
}