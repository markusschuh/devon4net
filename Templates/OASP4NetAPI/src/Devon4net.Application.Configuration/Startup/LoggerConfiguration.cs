using Microsoft.IdentityModel.Logging;
using Devon4Net.Infrastructure.Log.Configuration;
using Serilog;
using Serilog.Events;
using System;

namespace Devon4Net.Application.Configuration.Startup
{
    public static class LoggerApplicationConfiguration
    {
        public static void ConfigureLog(this ConfigurationManager configurationManager)
        {
            LogConfiguration.ConfigureLog(ConfigurationManager.ApplicationPath);
        }
    }
}
