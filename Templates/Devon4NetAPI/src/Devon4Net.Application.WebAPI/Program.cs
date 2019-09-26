using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Devon4Net.Application.WebAPI
{
    public static class Program
    {
        private static IConfigurationRoot Configuration { get; set; }

        /// <summary>
        /// Main startup class
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var configuratiuonBuilder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

            Configuration = configuratiuonBuilder.Build();

            var useIIS = Convert.ToBoolean(Configuration["UseIIS"], System.Globalization.CultureInfo.InvariantCulture);

            var webHostBuilder = CreateWebHostBuilder(args);

            if (useIIS)
            {
                ConfigureIIS(ref webHostBuilder);
            }
            else
            {
                ConfigureKestrel(ref webHostBuilder);
            }

            webHostBuilder.Build().Run();
        }

        /// <summary>
        /// Configures iis integration. .net core v2.2 integrates more iis options
        /// </summary>
        /// <param name="webHostBuilder"></param>
        private static void ConfigureIIS(ref IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.UseIISIntegration();
        }

        /// <summary>
        /// Kestrel configuration. You can setup your cert here
        /// </summary>
        /// <param name="webHostBuilder"></param>
        private static void ConfigureKestrel(ref IWebHostBuilder webHostBuilder)
        {
            var useHttps = Convert.ToBoolean(Configuration["KestrelOptions:UseHttps"], System.Globalization.CultureInfo.InvariantCulture);
            var applicationPort = Convert.ToInt32(Configuration["KestrelOptions:ApplicationPort"], System.Globalization.CultureInfo.InvariantCulture);

            webHostBuilder.UseKestrel(options =>
            {
                options.Listen(IPAddress.Any, applicationPort, listenOptions =>
                {
                    if (useHttps)
                    {
                        var KestrelCertificate = Configuration["KestrelOptions:KestrelCertificate"];

                        if (string.IsNullOrEmpty(KestrelCertificate))
                        {
                            listenOptions.UseHttps();
                        }
                        else
                        {
                            var KestrelCertificatePassword = Configuration["KestrelOptions:KestrelCertificatePassword"];
                            var certificate = new X509Certificate2(GetFileFullPath(KestrelCertificate), KestrelCertificatePassword);
                            listenOptions.UseHttps(certificate);
                        }
                    }
                });
            });
        }
        private static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            var useDetailedErrorsKey = Configuration["UseDetailedErrorsKey"];

            return builder
                .UseSetting(WebHostDefaults.DetailedErrorsKey, useDetailedErrorsKey)
                .UseStartup<Startup>();
        }

        private static string GetFileFullPath(string fileName)
        {
            if (File.Exists(fileName)) return fileName;
            var theCert = Directory.GetFiles(Directory.GetCurrentDirectory(), fileName, SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(theCert)) throw new FileNotFoundException("fileName", "Certificate not found");
            return theCert;
        }
    }
}
