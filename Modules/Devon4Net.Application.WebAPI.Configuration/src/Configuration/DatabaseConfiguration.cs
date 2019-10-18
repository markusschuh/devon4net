using Devon4Net.Application.WebAPI.Configuration.Common;
using Devon4Net.Application.WebAPI.Configuration.Enums;
using EntityFrameworkCore.Jet;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Devon4Net.Application.WebAPI.Configuration
{
    public static class SetupDatabaseConfiguration
    {
        private const int MaxRetryDelay = 30;
        private const int MaxRetryCount = 10;

        public static void SetupDatabase<T>(this IServiceCollection services, IConfiguration configuration, string conectionStringName, DatabaseType databaseType, CosmosConfigurationParams cosmosConfigurationParams = null) where T : DbContext 
        {
            var applicationConnectionStrings = configuration.GetSection("ConnectionStrings").GetChildren();
            if (applicationConnectionStrings == null) throw new ArgumentException("There are no connection strings provided.");
            var connectionString = applicationConnectionStrings.FirstOrDefault(c => c.Key.ToLower() == conectionStringName.ToLower());
            if (connectionString == null || string.IsNullOrEmpty(connectionString.Value)) throw new ArgumentException($"The provided connection string ({conectionStringName}) provided does not exists.");

            services.AddDbContext<DbContext, T>(options =>
             {
                 switch (databaseType)
                 {
                     case DatabaseType.SqlServer:
                         options.UseSqlServer(connectionString.Value, sqlServerOptionsAction: sqlOptions =>
                         {
                             sqlOptions.EnableRetryOnFailure(
                             maxRetryCount: MaxRetryCount,
                             maxRetryDelay: TimeSpan.FromSeconds(MaxRetryDelay),
                             errorNumbersToAdd: null);
                         });
                         break;
                     case DatabaseType.InMemory:
                         options.UseInMemoryDatabase(connectionString.Value);
                         break;
                     case DatabaseType.MySql:
                     case DatabaseType.MariaDb:
                         options.UseMySql(connectionString.Value, sqlOptions =>
                         {
                             sqlOptions.EnableRetryOnFailure(
                             maxRetryCount: MaxRetryCount,
                             maxRetryDelay: TimeSpan.FromSeconds(MaxRetryDelay),
                             errorNumbersToAdd: null);
                         });
                         break;
                     case DatabaseType.Sqlite:
                         options.UseSqlite(connectionString.Value);
                         break;
                     case DatabaseType.Cosmos:
                         if (cosmosConfigurationParams == null) throw new ArgumentException($"The Cosmos configuration can not be null.");
                         options.UseCosmos(cosmosConfigurationParams.Endpoint, cosmosConfigurationParams.Key, cosmosConfigurationParams.DatabaseName);
                         break;
                     case DatabaseType.PostgreSQL:
                         options.UseNpgsql(connectionString.Value, sqlOptions =>
                         {
                             sqlOptions.EnableRetryOnFailure(
                             maxRetryCount: MaxRetryCount,
                             maxRetryDelay: TimeSpan.FromSeconds(MaxRetryDelay),
                             errorCodesToAdd: null);
                         });
                         break;
                     case DatabaseType.FireBird:
                         options.UseFirebird(connectionString.Value);
                         break;
                     case DatabaseType.Oracle:
                         options.UseOracle(connectionString.Value);
                         break;
                     case DatabaseType.MSAccess:
                         options.UseJet(connectionString.Value);
                         break;
                     default:
                         break;
                 }
             }, ServiceLifetime.Transient
            );
        }
    }
}
