using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Https;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using Devon4Net.Infrastructure.Common;

namespace Devon4Net.Application.WebAPI.Configuration.Application
{
    public static class SetupKestrel
    {
        public static void Configure(ref IWebHostBuilder webBuilder, IConfigurationRoot configuration)
        {
            int.TryParse(configuration["devonfw:Kestrel:ApplicationPort"], out int applicationPort);
            bool.TryParse(configuration["devonfw:Kestrel:UseHttps"], out bool useHttps);
            long.TryParse(configuration["devonfw:Kestrel:MaxConcurrentConnections"], out long maxConcurrentConnections);
            long.TryParse(configuration["devonfw:Kestrel:MaxConcurrentUpgradedConnections"], out long maxConcurrentUpgradedConnections);
            
            webBuilder.UseKestrel(options =>
            {
                options.AddServerHeader = false;
                options.Listen(IPAddress.Any, applicationPort, listenOptions =>
                {
                    options.Limits.MaxConcurrentConnections = maxConcurrentConnections;
                    options.Limits.MaxConcurrentUpgradedConnections = maxConcurrentUpgradedConnections;

                    if (!useHttps) return;
                    
                    var httpsOptions = new HttpsConnectionAdapterOptions();
                    var kestrelCertificate = configuration["devonfw:Kestrel:ServerCertificate:Certificate"];
                    bool.TryParse(configuration["devonfw:Kestrel:ClientCertificate:RequireClientCertificate"], out bool requireClientCertificate);
                    bool.TryParse(configuration["devonfw:Kestrel:ClientCertificate:CheckCertificateRevocation"], out bool checkCertificateRevocation);

                    if (requireClientCertificate)
                    {
                        httpsOptions.ClientCertificateMode = ClientCertificateMode.RequireCertificate;
                        httpsOptions.CheckCertificateRevocation = checkCertificateRevocation;
                    }
                    
                    if (!string.IsNullOrEmpty(kestrelCertificate))
                    {
                        var kestrelCertificatePassword = configuration["devonfw:Kestrel:ServerCertificate:CertificatePassword"];
                        httpsOptions.ServerCertificate = new X509Certificate2(File.ReadAllBytes(FileOperations.GetFileFullPath(kestrelCertificate)), kestrelCertificatePassword, X509KeyStorageFlags.MachineKeySet);
                    }

                    listenOptions.UseHttps(httpsOptions);
                });
            });
        }
    }
}