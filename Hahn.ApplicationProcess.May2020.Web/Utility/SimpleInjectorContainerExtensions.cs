using Hahn.ApplicationProcess.May2020.Core;
using Hahn.ApplicationProcess.May2020.Data;
using Hahn.ApplicationProcess.May2020.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hahn.ApplicationProcess.May2020.Web.Utility
{
    public static class SimpleInjectorContainerExtensions
    {
        /// <summary>
        /// Registers all dependencies with simple injector (This will allow for dependency verification)
        /// </summary>
        /// <param name="container"></param>
        /// <param name="configuration"></param>
        public static void RegisterAllDependencies(this Container container, IConfiguration configuration)
        {
            container.Register<Data.ApplicantContext>(Lifestyle.Scoped);
           
            // Register Client Settings
            var clientSettings = configuration.GetSection("ClientUriSettings")
                .Get<ApplicatonProcess.May2020.Core.Utility.ClientSettings>();
            container.Register(() => clientSettings, (Lifestyle.Singleton));

            container.Register<DbContextOptions>(() => new DbContextOptionsBuilder<ApplicantContext>()
                .UseInMemoryDatabase(databaseName: "ApplicantDB").Options, Lifestyle.Singleton);

            container.Register<IApplicantService, ApplicantService>(Lifestyle.Scoped);

            //Register Utility
            //container.Register<Domain.Utility.IGetCountryUtility, Domain.Utility.GetCountryUtility>();

            //Register all Services
            

        }

        /// <summary>
        /// Register a group of services from the specified assembly
        /// </summary>
        /// <param name="container"></param>
        /// <param name="repositoryAssembly"></param>
        /// <param name="endString"></param>
        public static void RegisterSpecial(this Container container, System.Reflection.Assembly repositoryAssembly, string endString)
        {
            var registrations =
                 from type in repositoryAssembly.GetExportedTypes()
                 where type.Name.ToLower().EndsWith(endString.ToLower())
                 where type.GetInterfaces().Any()
                 where !type.IsInterface
                 select new { Service = type.GetInterfaces().Where(t => !t.IsGenericType).FirstOrDefault(), Implementation = type };
            foreach (var reg in registrations)
            {
                container.Register(reg.Service, reg.Implementation, Lifestyle.Scoped);
            }
        }
    }
}
