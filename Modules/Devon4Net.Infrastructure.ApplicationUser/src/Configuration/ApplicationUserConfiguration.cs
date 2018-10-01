using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Devon4Net.Infrastructure.ApplicationUser.Data;

namespace Devon4Net.Infrastructure.ApplicationUser.Configuration
{
    public static class ApplicationUserConfiguration
    {
        public static void AddApplicationUserDependencyInjection(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AuthContext>().AddDefaultTokenProviders();
            services.AddTransient<DataSeeder>();
        }

        public static void AddApplicationUserDbContextInMemoryService(this IServiceCollection services)
        {
            services.AddDbContext<AuthContext>(options =>options.UseInMemoryDatabase("AuthContext"));
        }

        public static void AddApplicationUserDbContextSQliteService(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AuthContext>(options => options.UseSqlite(connectionString));
        }

        public static void AddApplicationUserDbContextSQlServerService(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AuthContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
