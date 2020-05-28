using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Hahn.ApplicationProcess.May2020.Domain.Validations;
using Hahn.ApplicationProcess.May2020.Web.Utility;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System;
using System.Collections.Generic;
using SimpleInjector;

namespace Hahn.ApplicationProcess.Application.May2020.Web
{
    /// <summary>
    /// Web Application Initialization Class
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup class entry point
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        
        private static readonly Container container = new Container();


        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();
            var clientSettings = Configuration.GetSection("ClientUriSettings")
               .Get<ApplicatonProcess.May2020.Core.Utility.ClientSettings>();
            services.AddSingleton(clientSettings);
            services.AddSingleton<ApplicationProcess.May2020.Domain.Utility.IGetCountryUtility, 
                ApplicationProcess.May2020.Domain.Utility.GetCountryUtility>();
            services.AddMvc(opt =>
            {
                opt.Filters.Add(typeof(ValidatorActionFilter));
            })
            .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ApplicantValidator>());
            services.AddSimpleInjector(container, options =>
            {
                // AddAspNetCore() wraps web requests in a Simple Injector scope.
                options.AddAspNetCore()
                    // Ensure activation of a specific framework type to be created by
                    // Simple Injector instead of the built-in configuration system.
                    .AddControllerActivation()
                    .AddViewComponentActivation()
                    //.AddPageModelActivation()
                    //.AddTagHelperActivation()
                    ;
            });
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Applicant Project",
                    Description = "Hahn Application Process May 2020",
                    Contact = new OpenApiContact
                    {
                        Name = "Olajide Fagbuji",
                        Email = "jidexl21@gmail.com"
                    }

                });

                // Set the comments path for the Swagger JSON and UI
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
               
                c.IncludeXmlComments(xmlPath);
                //c.DescribeAllEnumsAsStrings();
            });

        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swagger, httpReq) =>
                {
                    swagger.Servers = new List<OpenApiServer>
                    {
                        new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}" },
                    };
                });
            });


            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
               c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hahn Application 2020");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            //container.RegisterPackages(new List<System.Reflection.Assembly> { System.Reflection.Assembly.GetExecutingAssembly() });
            container.RegisterAllDependencies(this.Configuration);

            // Always verify the container
            container.Verify();
        }
        

    }
}
