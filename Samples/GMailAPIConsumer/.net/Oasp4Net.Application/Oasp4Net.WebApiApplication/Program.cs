using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.SelfHost;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.Application;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Threading;
using Newtonsoft.Json;
using Oasp4Net.Arquitecture.CommonTools.Source.Enums;
using Oasp4Net.Business.Views.Views;
using Oasp4Net.Infrastructure.CrossCutting.Mailing;

namespace Oasp4Net.WebApiApplication
{
    public class Program
    {


        /// <summary>
        /// Swagger json: http://localhost:8080/swagger/docs/v1
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {


            LoadControllersFromAssembly("Oasp4Net.Service.Controllers.dll");
            var config = Register();

            using (var server = new HttpSelfHostServer(config))
            {
                server.OpenAsync().Wait();
                Console.WriteLine("Press Enter to quit.");
                Console.ReadLine();
            }
        }



        /// <summary>
        /// Load Controller types
        /// </summary>
        /// <param name="assemblyName"></param>
        private static void LoadControllersFromAssembly(string assemblyName)
        {
            //Todo: Reflection to get assemblies
            var asm = Assembly.LoadFrom(assemblyName);
            asm.GetTypes().Where(type => typeof(ApiController).IsAssignableFrom(type));
        }

        public static HttpSelfHostConfiguration Register()
        {
            var port = ConfigurationManager.AppSettings["LocalListenPort"];
            var config = new HttpSelfHostConfiguration($"http://0.0.0.0:{port}");

            config.Routes.MapHttpRoute("API Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            config.EnableSwagger(Configure).EnableSwaggerUi();
            
            return config;
        }

        private static void Configure(SwaggerDocsConfig swaggerConf)
        {
            //todo reflection
            swaggerConf.SingleApiVersion("v1", "My Thai Star Email Service");
            //Add swagger documentation
            foreach (var doc in GetXmlDocumentsForSwagger())
            {
                swaggerConf.IncludeXmlComments(GetXmlCommentsPath(doc));
            }
        }

        private static string GetXmlCommentsPath(string assemblyName)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            return Path.Combine(basePath, assemblyName);
        }

        //TODO: Documentation to how to set up swagger docs
        private static List<string> GetXmlDocumentsForSwagger()
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            return Directory.GetFiles(basePath, "*.Swagger.xml", SearchOption.AllDirectories).ToList();            
        }

    }
}
