using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSwag.AspNetCore;
using WeatherAPI.Filters;
using WeatherAPI.Models;
using WeatherAPI.OpenWeatherMap;

namespace WeatherAPI {
    public class Startup {
        public Startup (IConfiguration configuration, IHostingEnvironment environment, ILoggerFactory loggerFactory) {
            this.Configuration = configuration;
            this.Environment = environment;
            this.LoggerFactory = loggerFactory;

        }
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }
        public ILoggerFactory LoggerFactory { get; }

        public Startup (IHostingEnvironment env, IConfiguration configuration, ILoggerFactory loggerFactory) {
            Configuration = configuration;
            Environment = env;
            LoggerFactory = loggerFactory;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices (IServiceCollection services) {

            var logger = LoggerFactory.CreateLogger<Startup> ();

            services.AddMvc (options => {
                options.Filters.Add<JsonExceptionFilter> ();
                options.Filters.Add<RequireHttpsOrCloseAttribute> ();
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

            string OpenWeatherMapApiKey = "";
            if (Environment.IsDevelopment ()) {
                logger.LogDebug ("OpenWeatherMapApiKey=" + Configuration["OpenWeatherMapApiKey"]);
                OpenWeatherMapApiKey = Configuration["OpenWeatherMapApiKey"];
            } else if (Environment.IsProduction ()) {
                OpenWeatherMapApiKey = System.Environment.GetEnvironmentVariable ("OpenWeatherMapApiKey");
            } else {
                logger.LogError ("fail to get credentials!");
            }

            services.AddSingleton<OpenWeatherMapApiHandler> (new OpenWeatherMapApiHandler (OpenWeatherMapApiKey));
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

            // Add OpenAPI/Swagger middlewares
            app.UseSwagger (); // Serves the registered OpenAPI/Swagger documents by default on `/swagger/{documentName}/swagger.json`
            app.UseSwaggerUi3 (); // Serves the Swagger UI 3 web ui to view the OpenAPI/Swagger documents by default on `/swagger`

            app.UseMvc ();
        }
    }
}