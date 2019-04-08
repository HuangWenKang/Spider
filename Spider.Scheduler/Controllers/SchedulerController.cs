using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Mvc;
using Spider.Scheduler.Infrastructure.Repositories;
using Spider.Scheduler.Models;
using Spider.Scheduler.Services;

namespace Spider.Scheduler.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulerController : ControllerBase
    {
        private IUnitOfWork _uof;
        private IRepository<ScheduleJob> _repository;
        private WebApiClients _clients;

        public SchedulerController(IUnitOfWork uof, IRepository<ScheduleJob> repository, WebApiClients clients)
        {
            _uof = uof;
            _repository = repository;
            _clients = clients;
        }

        [HttpGet]
        public ActionResult<List<ScheduleJob>> ListJobs()
        {
            var jobs = _uof.ScheduleJobs.FindAll();
            return Ok(jobs);
        }

        [Route("")]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult Create([FromBody] JobForCreation jobForCreation)
        {
            if (ModelState.IsValid)
            {
                var job = new ScheduleJob()
                {
                    JsonPayload = jobForCreation.JsonPayload,
                    RecurringSchedule = jobForCreation.RecurringScheduleType,
                    WebApiUrl = jobForCreation.WebApiUrl
                };

                _uof.ScheduleJobs.Add(job);
                _uof.SaveChanges();

                var cronType = GetCronFromRecurringType(job.RecurringSchedule);
                RecurringJob.AddOrUpdate(job.ID.ToString(), () => _clients.CallWebApiAsync(job.WebApiUrl, job.JsonPayload), cronType);

                return Ok();
            }

            return BadRequest();
        }

        [Route("")]
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult Edit([FromBody] ScheduleJob updateScheduleJob)
        {
            if (ModelState.IsValid)
            {
                _uof.ScheduleJobs.Update(updateScheduleJob);
                _uof.SaveChanges();

                var cronType = GetCronFromRecurringType(updateScheduleJob.RecurringSchedule);
                RecurringJob.AddOrUpdate(updateScheduleJob.ID.ToString(), () => _clients.CallWebApiAsync(updateScheduleJob.WebApiUrl, updateScheduleJob.JsonPayload), cronType);
                return Ok();
            }

            return BadRequest();
        }

        [Route("{jobId:int}")]
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public ActionResult Delete([FromRoute] int jobId)
        {
            var job = _uof.ScheduleJobs.FindByID(jobId);
            if (job == null)
            {
                return BadRequest();
            }
            _uof.ScheduleJobs.Remove(job);
            _uof.SaveChanges();


            RecurringJob.RemoveIfExists(jobId.ToString());
            return Ok();            
        }

        private Func<string> GetCronFromRecurringType(ScheduleJob.RecurringScheduleType recurringSchedule)
        {
            switch (recurringSchedule)
            {
                case ScheduleJob.RecurringScheduleType.Daily:
                    return Cron.Daily;
                case ScheduleJob.RecurringScheduleType.Hourly:
                    return Cron.Hourly;
                case ScheduleJob.RecurringScheduleType.Minutely:
                    return Cron.Minutely;
                case ScheduleJob.RecurringScheduleType.Monthly:
                    return Cron.Monthly;
                case ScheduleJob.RecurringScheduleType.Weekly:
                    return Cron.Weekly;
                case ScheduleJob.RecurringScheduleType.Yearly:
                    return Cron.Yearly;
                default:
                    return Cron.Daily;
            }
        }

    }
}