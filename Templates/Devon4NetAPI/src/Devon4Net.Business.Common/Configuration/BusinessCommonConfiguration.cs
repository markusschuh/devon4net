using Microsoft.Extensions.DependencyInjection;
using Devon4Net.Business.Common.UserManagement.Service;
using Devon4Net.Infrastructure.JWT.Configuration;

namespace Devon4Net.Business.Common.Configuration
{
    public static class BusinessCommonConfiguration
    {
        /// <summary>
        /// Put the service layer DI declaration her
        /// PE from MTS: services.AddTransient<IDishService, DishService>();
        /// </summary>
        /// <param name="services"></param>
        public static void AddBusinessCommonDependencyInjectionService(this IServiceCollection services)
        {
            services.AddTransient<ILoginService, LoginService>();
        }

        /// <summary>
        /// Put JWT policy here
        /// PE from MTS: services.ConfigureJwtAddPolicy("MTSWaiterPolicy", "role", "waiter");
        /// </summary>
        /// <param name="services"></param>
        public static void AddBusinessCommonJwtPolicy(this IServiceCollection services)
        {

        }
    }
}
