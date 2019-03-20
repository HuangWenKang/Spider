using Microsoft.AspNetCore.Builder;
using Spider.API.Middlewares;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spider.API.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {       
        public static void ConfigureCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
