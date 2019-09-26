using Microsoft.Extensions.Configuration;
using Devon4Net.Infrastructure.Extensions;
using System;
using System.IO;
using System.Linq;

namespace Devon4Net.Application.Configuration
{
    public class ConfigurationManager
    {
        private IConfiguration Configuration { get; set; }        
        public bool UseSqliteLogDataBase { get; set; }
        public bool UseSeqLogServer { get; set; }
        public bool UseGrayLog { get; set; }
        public bool UseAOPTrace { get; set; }
        public bool UseSpa { get; set; }
        public string DistPath { get; set; }
        public string SourcePath { get; set; }
        public string UseProxyToSpaDevelopmentServer { get; set; }
        public string SpaNpmScript { get; set; }
        public string DefaultSpaEndPoint { get; set; }
        public bool UseSwagger { get; set; }

        public ConfigurationManager(IConfiguration configuration)
        {
            DiscoverApplicationPath();
            Configuration = configuration;
            Configure();
        }
        public ConfigurationManager()
        {
            DiscoverApplicationPath();

            Configuration = new ConfigurationBuilder()
                .SetBasePath(ApplicationPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables().Build();

            var stagingEnvironment = Configuration["Environment"];

            Configuration = new ConfigurationBuilder()
                .SetBasePath(ApplicationPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{stagingEnvironment}.json", optional: true)
                .AddEnvironmentVariables().Build();

            Configure();
        }

        public static string ApplicationPath { get; set; }

        public void DiscoverApplicationPath()
        {
            ApplicationPath = Path.GetDirectoryName(Directory.GetFiles(Directory.GetCurrentDirectory(), "appsettings.json",SearchOption.AllDirectories).FirstOrDefault());
        }

        public IConfiguration GetConfiguration()
        {
            return Configuration;
        }

        private string GetConfigurationValue(string key)
        {
            return Configuration[key];
        }

        private void Configure()
        {
            UseSqliteLogDataBase = string.IsNullOrEmpty(GetConfigurationValue("Log:SqliteDatabase"));
            UseSqliteLogDataBase = string.IsNullOrEmpty(GetConfigurationValue("Log:SeqLogServerHost"));
            UseGrayLog = string.IsNullOrEmpty(GetConfigurationValue("Log:GrayLog:GrayLogHost"));
            UseAOPTrace = Convert.ToBoolean(GetConfigurationValue("Log:UseAOPTrace"));
            UseSpa = Convert.ToBoolean(GetConfigurationValue("Spa:UseSpa"));
            DefaultSpaEndPoint = GetConfigurationValue("Spa:DefaultEndpoint");
            DistPath = GetConfigurationValue("Spa:DistPath");
            SourcePath = GetConfigurationValue("Spa:SourcePath");
            UseProxyToSpaDevelopmentServer = GetConfigurationValue("Spa:UseProxyToSpaDevelopmentServer");
            SpaNpmScript = GetConfigurationValue("Spa:NpmScript");
            UseSwagger = Convert.ToBoolean(GetConfigurationValue("UseSwagger"));
        }
    }
}
