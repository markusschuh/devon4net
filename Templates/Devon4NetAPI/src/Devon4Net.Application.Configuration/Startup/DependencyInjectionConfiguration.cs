using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Devon4Net.Business.Common.Configuration;
using Devon4Net.Domain.UnitOfWork.Configuration;
using Devon4Net.Infrastructure.ApplicationUser.Configuration;
using Devon4Net.Infrastructure.Log.Configuration;
using System;

namespace Devon4Net.Application.Configuration.Startup
{
    public static class DependencyInjectionConfiguration
    {
        public static void ConfigureDependencyInjectionService(this IServiceCollection services)
        {
            services.AddApplicationUserDependencyInjection();
            services.AddUnitOfWorkDependencyInjection();
            services.AddAopDependencyInjectionService();
            services.AddBusinessCommonDependencyInjectionService();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
