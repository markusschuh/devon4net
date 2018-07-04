using Microsoft.Extensions.DependencyInjection;
using OASP4Net.Business.Common.Configuration;

namespace OASP4Net.Application.Configuration.Startup
{
    public static class JwtApplicationConfiguration
    {
        public static void ConfigureJwtPolicy(this IServiceCollection services)
        {
            services.AddBusinessCommonJwtPolicy();
        }
    }
}
