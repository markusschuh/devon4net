using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Devon4Net.Infrastructure.Swagger.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void ConfigureSwaggerService(this IServiceCollection services)
        {

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(SwaggerDefinition.Version, new Info
                {
                    Version = SwaggerDefinition.Version,
                    Title = SwaggerDefinition.Title,
                    Description = SwaggerDefinition.Description,
                    TermsOfService = SwaggerDefinition.Terms,
                    Contact = new Contact { Name = SwaggerDefinition.ContactName, Email = SwaggerDefinition.ContactEmail, Url = SwaggerDefinition.ContactUrl },
                    License = new License { Name = SwaggerDefinition.LicenseName, Url = SwaggerDefinition.LicenseUrl }
                });

                foreach (var doc in GetXmlDocumentsForSwagger())
                    c.IncludeXmlComments(GetXmlCommentsPath(doc));
            });
            services.AddMvcCore().AddApiExplorer();

        }

        public static void ConfigureSwaggerApplication(this IApplicationBuilder app)
        {
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint(SwaggerDefinition.EndpointeUrl, SwaggerDefinition.EndpointName); });
        }



        #region private methods
        private static string GetXmlCommentsPath(string assemblyName)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            return Path.Combine(basePath, assemblyName);
        }

        private static List<string> GetXmlDocumentsForSwagger()
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            return Directory.GetFiles(basePath, "*.Swagger.xml", SearchOption.AllDirectories).ToList();
        }

        #endregion

    }
}
