using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Narato.Correlations;
using Narato.ExecutionTimingMiddleware;
using Narato.ResponseMiddleware;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using NLog.Web;
using Narato.Libraries.POC.DataProvider.Mappers;
#if (EnableExample)
using Narato.Libraries.POC.DataProvider.DataProviders;
using Narato.Libraries.POC.Domain.Contracts.DataProviders;
using Narato.Libraries.POC.Domain.Managers;
using Narato.Libraries.POC.Domain.Managers.Interfaces;
#endif
using Swashbuckle.Swagger.Model;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Narato.Libraries.POC.API.Mappers;
#if (EnableEntityFramework)
using Narato.Libraries.POC.DataProvider.Contexts;
using Microsoft.EntityFrameworkCore;
#endif

namespace Narato.Libraries.POC.API
{
    public class Startup
    {
        private MapperConfiguration _mapperConfiguration { get; set; }
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile("config.json")
                .AddJsonFile("config.json.local", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            env.ConfigureNLog("nlog.config");

            _mapperConfiguration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DataProviderAutoMapperProfileConfiguration());
                cfg.AddProfile(new DTOAutoMapperProfileConfiguration());
            });
            _mapperConfiguration.AssertConfigurationIsValid();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(
            //Add this filter globally so every request runs this filter to recored execution time
                config =>
                {
                    config.AddResponseFilters();
                })
                .AddJsonOptions(x =>
                {
                    x.SerializerSettings.ContractResolver =
                     new CamelCasePropertyNamesContractResolver();
                    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                }
            );

#if (EnableExample)
            services.AddTransient<IBookDataProvider, BookDataProvider>();
            services.AddTransient<IBookManager, BookManager>();
#endif

#if (EnableEntityFrameworkPostgres)
            services.AddEntityFrameworkNpgsql();
#endif
#if (EnableEntityFramework)
            services.AddDbContext<DataContext>(options => {
    #if (EnableEntityFrameworkPostgres)
                options.UseNpgsql(Configuration["DatabaseConfiguration:ConnectionString"]);
    #endif
            });
#endif

            services.AddSingleton(sp => _mapperConfiguration.CreateMapper());

            services.AddSwaggerGen();
            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Contact = new Contact { Name = "Narato NV" },
                    Description = "Narato Libraries Narato.Libraries.POC.API API",
                    Version = "v1",
                    Title = "Narato.Libraries.POC.API"
                });
                //options.OperationFilter<ProducesConsumesFilter>();

                var xmlPaths = GetXmlCommentsPaths();
                foreach (var entry in xmlPaths)
                {
                    try
                    {
                        options.IncludeXmlComments(entry);
                    }
                    catch (Exception e)
                    {
                    }
                }
            });

            services.AddCorrelations();
            services.AddResponseMiddleware();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddNLog();
            app.AddNLogWeb();

            app.UseExceptionHandler();
            app.UseCorrelations();
            app.UseExecutionTiming();

            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();

            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();

            app.UseMvc();
        }

        private List<string> GetXmlCommentsPaths()
        {
            var app = PlatformServices.Default.Application;
            var files = new List<string>()
                        {
                            "Narato.Libraries.POC.API.xml"
                        };

            List<string> paths = new List<string>();
            foreach (var file in files)
            {
                paths.Add(Path.Combine(app.ApplicationBasePath, file));
            }

            return paths;
        }
    }
}
