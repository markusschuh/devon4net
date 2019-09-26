using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Devon4Net.Infrastructure.JWT
{
    public static class JwtTokenDefinition
    {
        #region attributes

        public static SecurityKey IssuerSigningKey { get; set; }
        public static EncryptingCredentials SigningCredentials { get; set; }
        public static TimeSpan TokenExpirationTime { get; set; } = TimeSpan.FromHours(60);
        public static int ClockSkew { get; set; }
        public static string Issuer { get; set; } = string.Empty;
        public static string Audience { get; set; } = string.Empty;
        public static bool ValidateIssuerSigningKey { get; set; } = true;
        public static bool ValidateLifetime { get; set; } = true;
        public static SecurityKey SecurityKey { get; set; }

        private static X509Certificate2 Certificate { get; set; }

        #endregion

        public static void LoadJwtTokenDefinition(this IConfiguration configuration)
        {
            var secret = configuration["JWT:Secret"];
            Audience = configuration["JWT:Audience"];
            Issuer = configuration["JWT:Issuer"];
            TokenExpirationTime = TimeSpan.FromMinutes(Convert.ToInt16(configuration["JWT:TokenExpirationTime"]));
            ValidateIssuerSigningKey = Convert.ToBoolean(configuration["JWT:ValidateIssuerSigningKey"]);
            ValidateLifetime = Convert.ToBoolean(configuration["JWT:ValidateLifetime"]);
            ClockSkew = Convert.ToInt16(configuration["JWT:ClockSkew"]);

            if (!string.IsNullOrEmpty(secret)) GetSigningCredentialsFromKey(secret);
            else GetSigningCredentialsFromCertificate(configuration["JWT:Certificate"], configuration["JWT:CertificatePassword"]);
        }

        private static string GetCertificateFullPath(string certificatePath)
        {
            if (File.Exists(certificatePath)) return certificatePath;
            var theCert = Directory.GetFiles(Directory.GetCurrentDirectory(), certificatePath, SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(theCert)) throw new Exception("Certificate not found");
            return theCert;
        }

        private static string GetFileFullPath(string fileName)
        {
            if (File.Exists(fileName)) return fileName;
            var theCert = Directory.GetFiles(Directory.GetCurrentDirectory(), fileName, SearchOption.AllDirectories).FirstOrDefault();
            if (string.IsNullOrEmpty(theCert)) throw new FileNotFoundException("fileName", "Certificate not found");
            return theCert;
        }

        private static void GetSigningCredentialsFromKey(string secretKey)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            SecurityKey = key;
            IssuerSigningKey = new SymmetricSecurityKey(key.Key);
            SigningCredentials = new EncryptingCredentials(key, SecurityAlgorithms.RsaSha256);
        }

        private static void GetSigningCredentialsFromCertificate(string certificate, string password)
        {
            Certificate = new X509Certificate2(GetCertificateFullPath(certificate), password);
            SecurityKey = new X509SecurityKey(Certificate);
            SigningCredentials = new X509EncryptingCredentials(Certificate);
        }
    }
}
