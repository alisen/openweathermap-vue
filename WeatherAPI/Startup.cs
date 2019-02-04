using System;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WeatherAPI.Filters;
using WeatherAPI.Models;
using WeatherAPI.OpenWeatherMap;

namespace WeatherAPI {
    public class Startup {
        private IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }
        private ILoggerFactory LoggerFactory { get; }

        public Startup (IConfiguration configuration, IHostingEnvironment environment, ILoggerFactory loggerFactory) {
            Configuration = configuration;
            Environment = environment;
            LoggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {
            services.AddMvc (options => {
                options.Filters.Add<JsonExceptionFilter> ();
            }).SetCompatibilityVersion (CompatibilityVersion.Version_2_2);

            services.AddRouting (options => options.LowercaseUrls = true);

            services.AddApiVersioning (options => {
                options.DefaultApiVersion = new ApiVersion (1, 0);
                options.ApiVersionReader = new MediaTypeApiVersionReader ();
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
                options.ApiVersionSelector = new CurrentImplementationApiVersionSelector (options);
            });

            services.Configure<ApiBehaviorOptions> (options => {
                options.InvalidModelStateResponseFactory = context => {
                    var errorResponse = new ApiError (context.ModelState);
                    return new BadRequestObjectResult (errorResponse);
                };
            });

            services.AddCors (options => {
                options.AddPolicy ("WeatherApp",
                    policy => policy
                    .AllowAnyOrigin ());
            });

            services.AddSwaggerDocument (config => {
                config.PostProcess = document => {
                    document.Info.Version = "v1";
                    document.Info.Title = "Weather API";
                    document.Info.Description = "A simple ASP.NET Core Web API for OpenWeatherMap Services.";
                    document.Info.Contact = new NSwag.SwaggerContact {
                        Name = "Alisen Erdogan",
                        Email = "alisen@alisen.net",
                        Url = "https://alisen.net"
                    };
                };
            });

            // Register IConfiguration with DI system to support IConfiguration.GetValue approach
            services.AddSingleton (Configuration);

            services.AddHttpClient<OpenWeatherMapApiHandler> ();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment ()) {
                app.UseDeveloperExceptionPage ();
            } else {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts ();
            }

            app.UseCors ("WeatherApp");

            // Add OpenAPI/Swagger middleware
            app.UseSwagger (); // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
            app.UseSwaggerUi3 (); // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`

            app.UseStatusCodePages (async context => {
                context.HttpContext.Response.ContentType = "application/json";
                await context.HttpContext.Response.WriteAsync (
                    JsonConvert.SerializeObject (new {
                        context.HttpContext.Response.StatusCode
                    }));
            });

            app.UseMvc ();
            var option = new RewriteOptions ();
            option.AddRedirect ("^$", "swagger");
            app.UseRewriter (option);
        }
    }
}