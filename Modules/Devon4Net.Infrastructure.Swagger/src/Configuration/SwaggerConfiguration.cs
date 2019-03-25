using Devon4Net.Infrastructure.Swagger.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
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
            var security = new Dictionary<string, IEnumerable<string>>
            {
                {"Bearer", System.Array.Empty<string>()},
            };

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

            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<ConsumesOperationFilter>();

                options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });
                options.AddSecurityRequirement(security);
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
            var basePath = System.AppContext.BaseDirectory;
            return Path.Combine(basePath, assemblyName);
        }

        private static List<string> GetXmlDocumentsForSwagger()
        {
            var basePath = System.AppContext.BaseDirectory;
            return Directory.GetFiles(basePath, "*.Swagger.xml", SearchOption.AllDirectories).ToList();
        }

        #endregion

    }
}
