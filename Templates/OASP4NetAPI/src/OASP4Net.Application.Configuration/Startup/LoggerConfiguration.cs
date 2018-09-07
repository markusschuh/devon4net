using Microsoft.IdentityModel.Logging;
using OASP4Net.Infrastructure.Log.Configuration;
using Serilog;
using Serilog.Events;
using System;

namespace OASP4Net.Application.Configuration.Startup
{
    public static class LoggerApplicationConfiguration
    {
        public static void ConfigureLog(this ConfigurationManager configurationManager)
        {
            LogConfiguration.ConfigureLog(ConfigurationManager.ApplicationPath);
        }
    }
}
