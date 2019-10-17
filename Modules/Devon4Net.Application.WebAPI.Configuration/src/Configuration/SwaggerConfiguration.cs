using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.OpenApi.Models;
using Devon = Devon4Net.Infrastructure.Common.Options.Swagger;

namespace Devon4Net.Application.WebAPI.Configuration
{
    public static class SwaggerConfiguration
    {
        public static void SetupSwaggerService(this IServiceCollection services, Devon.SwaggerOptions swaggerOptions)
        {
            if (swaggerOptions == null) return;
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(swaggerOptions.Version, new OpenApiInfo
                {
                    Version = swaggerOptions.Version,
                    Title = swaggerOptions.Title,
                    Description = swaggerOptions.Description,
                    TermsOfService = new Uri(swaggerOptions.Terms),
                    Contact = new OpenApiContact { Name = swaggerOptions.Contact.Name, Email = swaggerOptions.Contact.Email, Url = new Uri(swaggerOptions.Contact.Url)},
                    License = new OpenApiLicense { Name = swaggerOptions.License.Name, Url = new Uri(swaggerOptions.License.Url) }
                });

                foreach (var doc in GetXmlDocumentsForSwagger())
                    c.IncludeXmlComments(GetXmlCommentsPath(doc));
            });

            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "devonfw"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "devonfw" }
                        }, new List<string>() }
                });
            });

            services.AddMvcCore().AddApiExplorer();
        }

        public static void ConfigureSwaggerApplication(this IApplicationBuilder app, Devon.SwaggerOptions swaggerOptions)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint(swaggerOptions.Endpoint.Url, swaggerOptions.Endpoint.Name); });
        }

        #region private methods
        private static string GetXmlCommentsPath(string assemblyName)
        {
            var basePath = AppContext.BaseDirectory;
            return Path.Combine(basePath, assemblyName);
        }

        private static List<string> GetXmlDocumentsForSwagger()
        {
            var basePath = AppContext.BaseDirectory;
            return Directory.GetFiles(basePath, "*.xml", SearchOption.AllDirectories).ToList();
        }

        #endregion

    }
}
