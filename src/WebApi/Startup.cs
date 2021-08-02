using FluentValidation.AspNetCore;
using Infrastructure;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using TaskManager.Api.Application;
using TaskManager.Api.Application.Common.Interfaces;
using WebApi.Filters;
using WebApi.Services;

namespace WebApi
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
            services.AddApplication();
            services.AddIntrastructure();
            services.AddTransient<ApplicationDbContext>(); // To run seed
            services.AddSingleton<IRequestJwtService, HttpHeaderJwtService>();

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    //Enums as strings (readability)
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                }); ;

            services.AddControllersWithViews(options =>
                options.Filters.Add<ApiExceptionFilterAttribute>())
                    .AddFluentValidation(x => x.AutomaticValidationEnabled = false);

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddHealthChecks()
                .AddDbContextCheck<ApplicationDbContext>();

            services.AddOpenApiDocument(config =>
            {
                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "TaskManager Api";
                    document.Info.Description = "A simple task management example implementation";
                    document.Info.TermsOfService = "None";
                    document.Info.Contact = new NSwag.OpenApiContact
                    {
                        Name = "M3ales",
                        Email = string.Empty,
                        Url = "https://github.com/m3ales"
                    };
                    document.Info.License = new NSwag.OpenApiLicense
                    {
                        Name = "MIT",
                        Url = "https://github.com/M3ales/TaskManager.Api/blob/master/LICENSE"
                    };
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseStaticFiles(); 

            app.UseOpenApi();

            app.UseSwaggerUi3(config => {
                config.Path = "/api";
                config.EnableTryItOut = true;
                config.DocumentPath = "api/specification.json";
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHealthChecks("/health");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
