using System;
using Microsoft.AspNetCore.Http;

namespace Common.Serilog.Middleware
{
    public class ApiExceptionOptions
    {       
        public Action<HttpContext, Exception, ApiError> AddResponseDetails { get; set; }        
    }
}
