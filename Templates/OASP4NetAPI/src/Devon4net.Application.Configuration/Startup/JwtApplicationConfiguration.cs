using Microsoft.Extensions.DependencyInjection;
using Devon4Net.Business.Common.Configuration;

namespace Devon4Net.Application.Configuration.Startup
{
    public static class JwtApplicationConfiguration
    {
        public static void ConfigureJwtPolicy(this IServiceCollection services)
        {
            services.AddBusinessCommonJwtPolicy();
        }
    }
}
