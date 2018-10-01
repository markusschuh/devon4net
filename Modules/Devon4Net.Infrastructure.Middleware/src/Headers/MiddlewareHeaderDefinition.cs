using Microsoft.Extensions.Configuration;
namespace Devon4Net.Infrastructure.Middleware.Headers
{
    public static class MiddlewareHeaderDefinition
    {
        public static string AccessControlExposeHeader { get; set; }
        public static string StrictTransportSecurityHeader { get; set; }
        public static string XFrameOptionsHeader { get; set; }
        public static string XssProtectionHeader { get; set; }
        public static string XContentTypeOptionsHeader { get; set; }
        public static string ContentSecurityPolicyHeader { get; set; }
        public static string PermittedCrossDomainPoliciesHeader { get; set; }
        public static string ReferrerPolicyHeader { get; set; }
        

        public static void LoadMiddlewareDefinition(this IConfiguration configuration)
        {
            AccessControlExposeHeader = configuration["Middleware:Headers:AccessControlExposeHeader"];
            StrictTransportSecurityHeader = configuration["Middleware:Headers:StrictTransportSecurityHeader"];
            XFrameOptionsHeader = configuration["Middleware:Headers:XFrameOptionsHeader"];
            XssProtectionHeader = configuration["Middleware:Headers:XssProtectionHeader"];
            XContentTypeOptionsHeader = configuration["Middleware:Headers:XContentTypeOptionsHeader"];
            ContentSecurityPolicyHeader = configuration["Middleware:Headers:ContentSecurityPolicyHeader"];
            PermittedCrossDomainPoliciesHeader = configuration["Middleware:Headers:PermittedCrossDomainPoliciesHeader"];
            ReferrerPolicyHeader = configuration["Middleware:Headers:ReferrerPolicyHeader"];
        }
    }

}
