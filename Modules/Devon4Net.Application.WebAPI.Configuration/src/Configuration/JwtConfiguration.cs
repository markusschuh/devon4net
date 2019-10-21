using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using Devon4Net.Infrastructure.Common.Options.JWT;
using Devon4Net.Infrastructure.JWT.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Devon4Net.Application.WebAPI.Configuration
{
    public static class JwtConfiguration
    {
        public static void SetupJwt(this IServiceCollection services, JwtOptions jwtOptions)
        {
            if (jwtOptions == null) return;
            
            var jwtHandler = new JwtHandler(jwtOptions);
            services.AddSingleton<IJwtHandler>(jwtHandler);

            services.AddAuthentication(options => options.DefaultScheme = "Bearer")
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    RequireSignedTokens = true,
                    IssuerSigningKey = jwtHandler.GetIssuerSigningKey(),
                    ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateIssuerSigningKey = jwtOptions.ValidateIssuerSigningKey,
                    ValidateLifetime = jwtOptions.ValidateLifetime,
                    ValidIssuers = new List<string> { jwtOptions.Issuer }
                };
            });
        }
        
        public static void AddJwtPolicy(this IServiceCollection services, string policyName, string claimType, string claimValue)
        {
            services.AddAuthorization(options => options.AddPolicy(policyName, policy => policy.RequireClaim(claimType, claimValue)));
        }
    }
}
