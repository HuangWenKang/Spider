using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spider.Scheduler.Infrastructure.Repositories;
using Spider.Scheduler.Models;
using Swashbuckle.AspNetCore.Swagger;
using Hangfire.MemoryStorage;
using Hangfire;
using Spider.Scheduler.Infrastructure.Middlewares;
using Scheduler.API;
using Hangfire.Mongo;
using Scheduler.API.Infrastructure.Clients;
using Scheduler.API.Infrastructure.Converters;
using Scheduler.API.Services;

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

            services.Configure<ScheduleSettings>(options => Configuration.GetSection("ScheduleSettings").Bind(options));
            services.AddDbContext<JobContext>(o => o.UseInMemoryDatabase("JobDatabase"));

            services.AddScoped<IUnitOfWork, UnitofWork>();
            services.AddScoped<IRepository<ScheduleJob>, ScheduleJobRepository>();
            services.AddScoped<IPayloadEnricher, PayloadEnricher>();

            services.AddScoped<ITaskService, TaskService>();


            services.AddHttpClient<WebApiClients>();
            services.AddHttpClient<BasicDataClients>();


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "ScheduleJob API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
            });

            var settings = Configuration.GetSection("ScheduleSettings").Get<ScheduleSettings>();
            services.AddHangfire(config =>
            {
                if (string.IsNullOrWhiteSpace(settings.ConnectionString))
                {
                    config.UseMemoryStorage();
                }
                else
                {
                    config.UseMongoStorage(settings.ConnectionString, settings.Database);
                }
                
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
