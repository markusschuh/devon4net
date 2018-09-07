using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using OASP4Net.Business.Common.Configuration;
using OASP4Net.Domain.UnitOfWork.Configuration;
using OASP4Net.Infrastructure.ApplicationUser.Configuration;
using OASP4Net.Infrastructure.Log.Configuration;

namespace OASP4Net.Application.Configuration.Startup
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureDependencyInjectionService(this IServiceCollection services)
        {
            services.AddApplicationUserDependencyInjection();
            services.AddUnitOfWorkDependencyInjection();
            services.AddAopDependencyInjectionService();
            services.AddBusinessCommonDependencyInjectionService();
            services.AddAutoMapper();
        }
    }
}
