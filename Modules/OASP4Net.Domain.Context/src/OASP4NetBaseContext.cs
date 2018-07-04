using Microsoft.EntityFrameworkCore;

namespace OASP4Net.Domain.Context
{
    /// <summary>
    /// First release. 
    /// This class extends DbContext. It's aim is to provide a layer dependency and being able to
    /// use a context in different layers as well as for testing purposes.
    /// </summary>
    public class OASP4NetBaseContext: DbContext 
    {
        protected DatabaseTypeEnum DatabaseType { get; set; }
        protected readonly string _connectionString;      

        public OASP4NetBaseContext(DbContextOptions options) : base(options)
        {

        }

        public OASP4NetBaseContext(DbContextOptions options, DatabaseTypeEnum databaseType) : base(options)
        {
            DatabaseType = databaseType;
        }

        public OASP4NetBaseContext(DbContextOptions options, string connectionString, DatabaseTypeEnum databaseType) : base(options)
        {
            _connectionString = connectionString;
            DatabaseType = databaseType;
        }


        public OASP4NetBaseContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public OASP4NetBaseContext(string connectionString, DatabaseTypeEnum databaseType)
        {
            _connectionString = connectionString;
            DatabaseType = databaseType;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                switch (DatabaseType)
                {
                    case DatabaseTypeEnum.InMemory:
                        {
                            optionsBuilder.UseInMemoryDatabase(_connectionString);
                        }
                        break;
                    case DatabaseTypeEnum.Sqlite:
                        {
                            optionsBuilder.UseSqlite(_connectionString);
                        }
                        break;

                    case DatabaseTypeEnum.SQLServer:
                        {
                            optionsBuilder.UseSqlServer(_connectionString);
                        }
                        break;
                }
                
            }
        }

    }
}
