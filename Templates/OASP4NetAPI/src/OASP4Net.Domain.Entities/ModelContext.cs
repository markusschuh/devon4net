using Microsoft.EntityFrameworkCore;
using OASP4Net.Domain.Context;

namespace OASP4Net.Domain.Entities
{
    public class ModelContext : OASP4NetBaseContext
    {
        public ModelContext(DbContextOptions<ModelContext> options) : base(options) { }

    }
}
