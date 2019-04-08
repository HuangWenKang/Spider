using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spider.Scheduler.Infrastructure.Repositories;
using Spider.Scheduler.Models;
using Spider.Scheduler.Services;
using Swashbuckle.AspNetCore.Swagger;
using Hangfire.MemoryStorage;
using Hangfire;
using Spider.Scheduler.Infrastructure.Middlewares;

namespace Spider.Scheduler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<JobContext>(o => o.UseInMemoryDatabase("JobDatabase"));

            services.AddScoped<IUnitOfWork, UnitofWork>();
            services.AddScoped<IRepository<ScheduleJob>, ScheduleJobRepository>();            
            services.AddScoped<IBasicDataService, FakeBasicDataService>();

            services.AddHttpClient<WebApiClients>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ScheduleJob API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
            });

            services.AddHangfire(config =>
            {
                config.UseMemoryStorage();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            app.ConfigureCustomExceptionMiddleware();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScheduleJob API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseHangfireServer();
            app.UseHangfireDashboard();
        }
    }
}
