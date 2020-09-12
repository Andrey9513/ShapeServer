using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ShapeServer.DataAccess;
using ShapeServer.Interfaces;
using ShapeServer.Mapper;
using Swashbuckle.AspNetCore.SwaggerUI;
using AutoMapper;

namespace ShapeServer
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
            var connectionString = Configuration.GetConnectionString("ShapesDb");
            services
                .AddDbContext<ShapesDbContext>(options => options.UseNpgsql(connectionString))
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo()
                    {
                        Title = "ShapeServer",
                        Version = Assembly.GetExecutingAssembly().GetName().Version?.ToString(),
                        Description = "Server for getting access to ShapeDataBase",
                    });
                    c.EnableAnnotations();

                })
                .AddHealthChecks();
            services.AddControllers();
            services.AddTransient<IMapperConfigurator, ShapesMapperConfigurator>()
                .AddAutoMapper((sp, config) =>
                {
                    var configurations = sp.GetServices<IMapperConfigurator>();

                    foreach (var mapperConf in configurations)
                        mapperConf.Configure(config);
                }, Array.Empty<Assembly>());
        }
       
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseRouting();

            app.UseSwaggerUI(swagOpt =>
            {
                swagOpt.SwaggerEndpoint("/swagger/v1/swagger.json", "Goldway Data Service API");
                swagOpt.DefaultModelsExpandDepth(-1);
                swagOpt.RoutePrefix = string.Empty;
                swagOpt.DocExpansion(DocExpansion.None);
                swagOpt.EnableFilter();
                swagOpt.ShowExtensions();
                swagOpt.DisplayRequestDuration();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks("/health");
            });
        }
    }
}
