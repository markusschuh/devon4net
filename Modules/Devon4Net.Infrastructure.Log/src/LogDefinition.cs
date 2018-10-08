using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using System.IO;
using System.Linq;
using System.Text;

namespace Devon4Net.Infrastructure.Log
{
    public static class LogDefinition
    {
        #region attributes
              
        public static string LogFolder { get; set; } = string.Empty;
        public static string LogFile { get; set; } = string.Empty;
        public static string SqliteDatabase { get; set; } = string.Empty;
        public static string GrayLogHost { get; set; } = string.Empty;
        public static int GrayLogPort { get; set; }
        public static string GrayLogProtocol { get; set; } = string.Empty;
        public static bool UseSecureConnection { get; set; } = false;
        public static bool UseAsyncLogging { get; set; } = false;
        public static int RetryCount { get; set; }
        public static int RetryIntervalMs { get; set; }
        public static int MaxUdpMessageSize { get; set; }

        public static string SeqLogServerHost { get; set; }


        #endregion

        public static void LoadLogDefinition(this IConfiguration configuration)
        {
            LogFolder = configuration["Log:File:LogFolder"];
            LogFile = configuration["Log:File:LogFile"];
            SqliteDatabase = configuration["Log:SqliteDatabase"];
            GrayLogHost = configuration["Log:GrayLog:GrayLogHost"];
            GrayLogPort = Convert.ToInt32(configuration["Log:GrayLog:GrayLogPort"]);
            GrayLogProtocol = configuration["Log:GrayLog:GrayLogProtocol"];
            UseSecureConnection = Convert.ToBoolean(configuration["Log:GrayLog:UseSecureConnection"]);
            UseAsyncLogging = Convert.ToBoolean(configuration["Log:GrayLog:UseAsyncLogging"]);
            RetryCount = Convert.ToInt32(configuration["Log:GrayLog:RetryCount"]);
            RetryIntervalMs = Convert.ToInt32(configuration["Log:GrayLog:RetryIntervalMs"]);
            MaxUdpMessageSize = Convert.ToInt32(configuration["Log:GrayLog:MaxUdpMessageSize"]);
            SeqLogServerHost = configuration["Log:SeqLogServerHost"];
            if (String.IsNullOrEmpty(LogFolder)) LogFolder = "Logs";
            if (String.IsNullOrEmpty(LogFile)) LogFile = "log-{0}.txt";
        }
    }
}
