using HtmlSpider.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Spider.API.Contexts;
using Spider.API.Extensions;
using Spider.API.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using VideoSpider.Services;

namespace Spider.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddDbContext<MSDNContext>(o => o.UseInMemoryDatabase("MSDNDatabase"));

            services.AddScoped<IVideoSpiderService, Channel9VideoSpiderService>();
            services.AddScoped<ICatalogService, MsdnCatalogService>();

            services.AddScoped<IVideosRepository, VideosRepository>();
            services.AddScoped<ICatalogsRepository, CatalogsRepository>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "MSDN API", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
            });
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors();
            app.ConfigureCustomExceptionMiddleware();
            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Radar API V1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
