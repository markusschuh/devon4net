using Microsoft.Extensions.Configuration;

namespace Devon4Net.Infrastructure.Swagger
{
    public static class SwaggerDefinition
    {
        public static string Version { get; set; } = string.Empty;
        public static string Title { get; set; } = string.Empty;
        public static string Description { get; set; } = string.Empty;
        public static string Terms { get; set; } = string.Empty;
        public static string ContactName { get; set; } = string.Empty;
        public static string ContactEmail { get; set; } = string.Empty;
        public static string ContactUrl { get; set; } = string.Empty;
        public static string LicenseName { get; set; } = string.Empty;        
        public static string LicenseUrl { get; set; } = string.Empty;
        public static string EndpointName { get; set; } = string.Empty;
        public static string EndpointeUrl { get; set; } = string.Empty;

        public static void LoadSwaggerDefinition(this IConfiguration configuration)
        {            
            Version = configuration["Swagger:Version"];
            Title = configuration["Swagger:Title"];
            Description = configuration["Swagger:Description"];
            Terms = configuration["Swagger:Terms"];

            ContactName = configuration["Swagger:Contact:Name"];
            ContactEmail = configuration["Swagger:Contact:Email"];
            ContactUrl = configuration["Swagger:Contact:Url"];

            LicenseName = configuration["Swagger:License:Name"];
            LicenseUrl = configuration["Swagger:License:Url"];

            EndpointName = configuration["Swagger:Endpoint:Name"];
            EndpointeUrl = configuration["Swagger:Endpoint:Url"];
        }
    }
}
