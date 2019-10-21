using Devon4Net.Common.Business.TodoManagement.Service;
using Devon4Net.Common.Data.Repositories;
using Devon4Net.Common.Domain.RepositoryInterfaces.TodoManagement;
using Microsoft.Extensions.DependencyInjection;

namespace Devon4Net.Common.Configure
{
    public static class DevonConfiguration
    {
        public static void SetupDevonDependencyInjection(this IServiceCollection services)
        {
            //Services
            services.AddTransient<ITodoService, TodoService>();

            //Repositories
            services.AddTransient<ITodoRepository, TodoRepository>();
        }
    }
}
