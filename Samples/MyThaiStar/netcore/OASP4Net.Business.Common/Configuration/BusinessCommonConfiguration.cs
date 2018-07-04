using Microsoft.Extensions.DependencyInjection;
using OASP4Net.Business.Common.BookingManagement.Service;
using OASP4Net.Business.Common.DishManagement.Service;
using OASP4Net.Business.Common.OrderManagement.Service;
using OASP4Net.Business.Common.UserManagement.Service;
using OASP4Net.Infrastructure.JWT.Configuration;

namespace OASP4Net.Business.Common.Configuration
{
    public static class BusinessCommonConfiguration
    {
        public static void AddBusinessCommonDependencyInjectionService(this IServiceCollection services)
        {
            services.AddTransient<IDishService, DishService>();
            services.AddTransient<IBookingService, BookingService>();
            services.AddTransient<IOrderService, OrderService>();
            services.AddTransient<ILoginService, LoginService>();
        }

        public static void AddBusinessCommonJwtPolicy(this IServiceCollection services)
        {
            services.ConfigureJwtAddPolicy("MTSWaiterPolicy", "role", "waiter");
            services.ConfigureJwtAddPolicy("MTSCustomerPolicy", "role", "customer");
        }
    }
}
