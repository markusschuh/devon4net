using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace Devon4Net.Infrastructure.JWT.Configuration
{
    public static class JwtConfiguration
    {
        public static void ConfigureJwtAuthorizationService(this IServiceCollection services)
        {
            services.AddAuthorization();
        }

        public static void ConfigureJwtAuthenticationService(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters()
                {                    
                    RequireSignedTokens = true,                    
                    IssuerSigningKey = JwtTokenDefinition.IssuerSigningKey,
                    ValidAudience = JwtTokenDefinition.Audience,
                    ValidIssuer = JwtTokenDefinition.Issuer,
                    ValidateIssuerSigningKey = JwtTokenDefinition.ValidateIssuerSigningKey,
                    ValidateLifetime = JwtTokenDefinition.ValidateLifetime,
                    ValidIssuers = new List<string> { JwtTokenDefinition.Issuer }
                };

                //you can track auth with this middleware
                //options.Events = new JwtBearerEvents
                //{
                //    OnAuthenticationFailed = context =>
                //    {
                //        context.Response.Headers.Remove("Authorization");
                //        return Task.CompletedTask;
                //    },
                //    OnTokenValidated = context =>
                //    {
                //        context.Response.Headers.TryAdd("Authorization", $"{context.SecurityToken}");
                //        return Task.CompletedTask;
                //    }
                //};
            });
        }

        public static void ConfigureJwtAddPolicy(this IServiceCollection services, string policyName, string claimType, string claimValue)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy(policyName, policy => policy.RequireClaim(claimType, claimValue));
            });
        }
    }
}
