using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Devon4Net.Infrastructure.Cors.Configuration;
using Devon4Net.Infrastructure.JWT.Configuration;
using Serilog;
using Devon4Net.Infrastructure.ApplicationUser.Data;
using Devon4Net.Infrastructure.Cors;
using System.Collections.Generic;
using Newtonsoft.Json.Serialization;
using Devon4Net.Infrastructure.JWT;
using Devon4Net.Infrastructure.Middleware.Configuration;
using Devon4Net.Infrastructure.Middleware.Headers;
using Devon4Net.Application.Configuration;
using Devon4Net.Application.Configuration.Startup;
using Devon4Net.Infrastructure.Swagger.Configuration;
using Devon4Net.Infrastructure.Swagger;
using Devon4Net.Infrastructure.Log.Middleware;
using Devon4Net.Infrastructure.Log.Configuration;
using Devon4Net.Infrastructure.Log;
using System;
using Devon4Net.Infrastructure.CircuitBreaker;
using Devon4Net.Infrastructure.CircuitBreaker.Options;

namespace Devon4Net.Application.WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private ConfigurationManager ConfigurationManager { get; set; }

        public Startup(IConfiguration configuration)
        {
            ConfigurationManager = new ConfigurationManager();
            Configuration = ConfigurationManager.GetConfiguration();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            LoadDefinitions();

            services.ConfigureDataBase(new Dictionary<string, string> { { ConfigurationConst.DefaultConnection, Configuration.GetConnectionString(ConfigurationConst.DefaultConnection) } });
            services.ConfigurePermisiveIdentityPolicyService();
            services.ConfigureJwtAuthenticationService();
            services.ConfigureJwtPolicy();
            services.AddAopAttributeService(ConfigurationManager.UseAOPTrace);
            services.AddAopExceptionFilterAttribute();
            services.ConfigureDependencyInjectionService();
            services.Configure<EndPointPollyOptions>(Configuration);
            services.ConfigureCircuitBreakerService(Configuration);
            services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
            if (ConfigurationManager.UseSwagger) services.ConfigureSwaggerService();           

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });
            services.ConfigureCorsService();
            services.AddOptions();
            if (ConfigurationManager.UseSpa) services.ConfigureSPA(ConfigurationManager.DistPath);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DataSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCustomHeadersMiddleware();
            app.UseMiddleware(typeof(LogExceptionHandlingMiddleware));
            app.UseStaticFiles();
            app.UseAuthentication();
            app.ConfigureUniversalCorsApplication();
            if (ConfigurationManager.UseSwagger) app.ConfigureSwaggerApplication();
            ConfigurationManager.ConfigureLog();
            seeder.SeedAsync().Wait();
            if(Convert.ToBoolean(Configuration["UseHttps"], System.Globalization.CultureInfo.InvariantCulture)) app.UseHttpsRedirection();
            if (ConfigurationManager.UseSpa)
            {
                app.ConfigureSpa(ConfigurationManager.DistPath, ConfigurationManager.SpaNpmScript, ConfigurationManager.UseProxyToSpaDevelopmentServer, ConfigurationManager.DefaultSpaEndPoint);
            }
            app.UseMvc();
        }

        public void LoadDefinitions()
        {
            Configuration.LoadJwtTokenDefinition();
            Configuration.LoadCorsDefinition();
            Configuration.LoadMiddlewareDefinition();
            Configuration.LoadSwaggerDefinition();
            Configuration.LoadLogDefinition();
        }
    }
}
