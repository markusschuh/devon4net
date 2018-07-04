using Microsoft.EntityFrameworkCore;
using OASP4Net.Domain.Context;

namespace OASP4Net.Infrastructure.Test
{
    public abstract class DatabaseManagementTest<T> : BaseManagementTest where T : OASP4NetBaseContext
    {
        public T Context { get; set; }
        protected DbContextOptions<T> ContextOptions { get; set; }
        public abstract void ConfigureContext();

        public DatabaseManagementTest()
        {
            ConfigureContext();
        }

        public new void Dispose() => Context.Dispose();
    }
}
