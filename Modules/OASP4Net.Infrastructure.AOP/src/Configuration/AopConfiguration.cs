using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace OASP4Net.Infrastructure.AOP.Configuration
{
    public static class AopConfiguration
    {
        public static void AddAopDependencyInjectionService(this IServiceCollection services)
        {
            services.AddScoped<AopControllerAttribute>();
            services.AddScoped<AopExceptionFilterAttribute>();
        }

        public static void AddAopAttributeService(this IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                options.Filters.Add(new AopControllerAttribute(Log.Logger));
                options.Filters.Add(new AopExceptionFilterAttribute(Log.Logger));
            });
        }

    }
}
