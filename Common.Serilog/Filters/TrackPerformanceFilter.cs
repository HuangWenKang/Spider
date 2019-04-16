using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Serilog.Filters
{
    public class TrackPerformanceFilter : IActionFilter
    {
        private PerfTracker _tracker;
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(context.ActionDescriptor is ControllerActionDescriptor cad)) return;

            _tracker = new PerfTracker($"{cad.ControllerName}-{cad.ActionName}");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            _tracker?.Stop();
        }
    }
}
