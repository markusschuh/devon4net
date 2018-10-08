using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Devon4Net.Infrastructure.Log.Attribute;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Graylog.Extended;
using System;

namespace Devon4Net.Infrastructure.Log.Configuration
{
    public static class LogConfiguration
    {
        public static void AddAopDependencyInjectionService(this IServiceCollection services)
        {
            services.AddScoped<AopControllerAttribute>();            
            services.AddScoped<AopExceptionFilterAttribute>();
        }

        public static void AddAopAttributeService(this IServiceCollection services, bool useAop)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new AopControllerAttribute(useAop));                
            });
        }

        public static void AddAopExceptionFilterAttribute(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new AopExceptionFilterAttribute());
            });
        }

        public static void ConfigureLog(string applicationPath)
        {
            var logFile = string.Format(LogDefinition.LogFile, DateTime.Today.ToShortDateString().Replace("/", string.Empty));

            var loggerConfiguration = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File($"{applicationPath}/{LogDefinition.LogFolder}/{logFile}");

            if (!String.IsNullOrEmpty(LogDefinition.GrayLogHost))
            {
                var graylogConfig = new GraylogSinkConfiguration
                {
                    GraylogTransportType = GetGraylogTransportTypeFromString(LogDefinition.GrayLogProtocol),
                    Host = LogDefinition.GrayLogHost,
                    Port = LogDefinition.GrayLogPort,
                    UseSecureConnection = LogDefinition.UseSecureConnection,
                    UseAsyncLogging = LogDefinition.UseAsyncLogging,
                    RetryCount = LogDefinition.RetryCount,
                    RetryIntervalMs = LogDefinition.RetryIntervalMs,
                    MaxUdpMessageSize = LogDefinition.MaxUdpMessageSize
                };

                loggerConfiguration = loggerConfiguration.WriteTo.Graylog(graylogConfig);
            }

            if (!String.IsNullOrEmpty(LogDefinition.SeqLogServerHost))
            {
                loggerConfiguration = loggerConfiguration.WriteTo.Seq(LogDefinition.SeqLogServerHost);
            }

            if (!String.IsNullOrEmpty(LogDefinition.SqliteDatabase))
            {
                loggerConfiguration = loggerConfiguration.WriteTo.SQLite($"{applicationPath}/{LogDefinition.LogFolder}/{LogDefinition.SqliteDatabase}");
            }

            Serilog.Log.Logger = loggerConfiguration.CreateLogger(); ;

            IdentityModelEventSource.ShowPII = true;
        }

        private static GraylogTransportType GetGraylogTransportTypeFromString(string transportType)
        {
            switch (transportType.ToLower()){
                case "tcp":
                    return GraylogTransportType.Tcp; 
                    
                case "udp":
                    return GraylogTransportType.Udp;
                case "http":
                    return GraylogTransportType.Http;                
            }
            return GraylogTransportType.Udp;
        }
    }
}
