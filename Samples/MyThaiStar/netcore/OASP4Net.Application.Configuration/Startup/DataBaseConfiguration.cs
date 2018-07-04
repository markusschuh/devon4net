using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OASP4Net.Domain.Entities;
using OASP4Net.Infrastructure.ApplicationUser.Configuration;
using System.Collections.Generic;

namespace OASP4Net.Application.Configuration.Startup
{
    public static class DataBaseConfiguration
    {
        public static void ConfigureDataBase(this IServiceCollection services, Dictionary<string,string> connectionStringDictionary)
        {
            var mtsConnection = GetDictionaryValue(connectionStringDictionary, ConfigurationConst.DefaultConnection);
            
            //auth4jwt
            services.AddApplicationUserDbContextInMemoryService();

            //My Thai Star       
            services.AddScoped<DbContext, ModelContext>();
            services.AddDbContext<ModelContext>(options => options.UseSqlServer(mtsConnection));
            
        }

        private static string GetDictionaryValue(Dictionary<string, string> connectionStringDictionary, string dictionaryKey)
        {
            var dictionaryValue = string.Empty;
            connectionStringDictionary.TryGetValue(dictionaryKey, out dictionaryValue);
            return dictionaryValue;
        }
    }
}
