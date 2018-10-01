using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Devon4Net.Infrastructure.Cors
{
    public static class CorsDefinition
    {
        public const string DefaultCorsPolicy = "CorsPolicy";
        private const int MaxCorsDefinitions = 10;
        public static  List<CorsAppConfiguration> CorsDefinitions { get; set; }

        public static void LoadCorsDefinition(this IConfiguration configuration)
        {
            CorsDefinitions = new List<CorsAppConfiguration>();

            for (int i = 0;  i <= MaxCorsDefinitions; i++)
            {
                var def = configuration[$"Cors:{i}:CorsPolicy"];
                if (string.IsNullOrEmpty(def)) break;

                CorsDefinitions.Add(new CorsAppConfiguration {
                    CorsPolicy = def,
                    AllowCredentials = Convert.ToBoolean(configuration[$"Cors:{i}:AllowCredentials"]),
                    Headers = configuration[$"Cors:{i}:Headers"].Split(',').ToList(),
                    Methods = configuration[$"Cors:{i}:Methods"].Split(',').ToList(),
                    Origins = configuration[$"Cors:{i}:Origins"].Split(',').ToList()
                });
            }
        }
    }

}
