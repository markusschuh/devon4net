using Microsoft.Extensions.Configuration;
using OASP4Net.Infrastructure.Extensions;
using System;
using System.IO;
using System.Linq;

namespace OASP4Net.Application.Configuration
{
    public class ConfigurationManager
    {
        private IConfiguration Configuration { get; set; }        
        public string LocalListenPort { get; set; }
        public string LocalKestrelUrl { get; set; }

        public bool UseSqliteLogDataBase { get; set; }
        public bool UseSeqLogServer { get; set; }
        public bool UseGrayLog { get; set; }

        public bool UseAOPTrace { get; set; }


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

            var stagingEnvironment = Configuration["StagingEnvironment"];

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

            LocalListenPort = GetConfigurationValue("LocalListenPort");
            LocalKestrelUrl = string.Format(GetConfigurationValue("LocalKestrelUrl"), LocalListenPort);
            UseSqliteLogDataBase = String.IsNullOrEmpty(GetConfigurationValue("Log:SqliteDatabase"));
            UseSqliteLogDataBase = String.IsNullOrEmpty(GetConfigurationValue("Log:SeqLogServerHost"));
            UseSqliteLogDataBase = String.IsNullOrEmpty(GetConfigurationValue("Log:GrayLog:GrayLogHost"));
            UseAOPTrace = Convert.ToBoolean(GetConfigurationValue("Log:UseAOPTrace"));
        }
    }
}
