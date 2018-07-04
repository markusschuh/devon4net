using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace OASP4Net.Infrastructure.Cors.Configuration
{
    public static class CorsConfiguration
    {
        public static void ConfigureUniversalCorsApplication(this IApplicationBuilder app)
        {
            if (CorsDefinition.CorsDefinitions!=null && CorsDefinition.CorsDefinitions.Count>0) app.UseCors();
        }

        public static void ConfigureCorsAnyOriginService(this IServiceCollection services)
        {
            ////enables CORS and httpoptions
            services.AddCors(options =>
            {
                options.AddPolicy(CorsDefinition.DefaultCorsPolicy, builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                    builder.AllowCredentials();
                });
            });
        }

        /// <summary>
        /// Allow different cors origins defined on appsettings
        /// </summary>
        /// <param name="services"></param>
        public static void ConfigureCorsService(this IServiceCollection services)
        {
            if (CorsDefinition.CorsDefinitions == null || CorsDefinition.CorsDefinitions.Count <= 0 || !CorsDefinition.CorsDefinitions.Any())
                ConfigureCorsAnyOriginService(services);
            else
            foreach (var definition in CorsDefinition.CorsDefinitions)
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(definition.CorsPolicy, builder =>
                    {
                        builder.WithOrigins(definition.Origins.ToArray());
                        builder.WithHeaders(definition.Headers.ToArray());
                        builder.WithMethods(definition.Methods.ToArray());
                        if (definition.AllowCredentials) builder.AllowCredentials();
                    });
                });
            }
        }
    }
}
