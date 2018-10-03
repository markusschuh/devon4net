using Devon4Net.Infrastructure.Log.Configuration;

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
