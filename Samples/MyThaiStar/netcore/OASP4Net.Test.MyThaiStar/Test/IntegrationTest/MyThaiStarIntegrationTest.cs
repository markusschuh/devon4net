using AutoMapper;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using OASP4Net.Business.Common;
using OASP4Net.Domain.Entities;
using OASP4Net.Infrastructure.Test;

namespace OASP4Net.Test.xUnit.Test.Integration
{
    public class MyThaiStarIntegrationTest : DatabaseManagementTest<ModelContext>
    {
        public override void ConfigureContext()
        {
            var conn = $"DataSource={GetFilePath("MyThaiStar.db")}";
            var connection = new SqliteConnection(conn);

            var builder = new DbContextOptionsBuilder<ModelContext>();
            builder.UseSqlite(connection);
            ContextOptions = builder.Options;
            Context = new ModelContext(ContextOptions);
        }

        public override void ConfigureMapper()
        {
            var mockMapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfile());
            });
           Mapper = mockMapper.CreateMapper();
        }
    }
}
