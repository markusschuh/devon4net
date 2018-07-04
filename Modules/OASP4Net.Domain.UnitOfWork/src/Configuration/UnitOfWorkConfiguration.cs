using Microsoft.Extensions.DependencyInjection;
namespace OASP4Net.Domain.UnitOfWork.Configuration
{
    public static class UnitOfWorkConfiguration
    {
        public static void AddUnitOfWorkDependencyInjection(this IServiceCollection services)
        {
            services.AddScoped(typeof(Repository.IRepository<>), typeof(Repository.Repository<>));
            services.AddScoped(typeof(UnitOfWork.IUnitOfWork<>), typeof(UnitOfWork.UnitOfWork<>));
        }
    }
}
