using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Devon4Net.Infrastructure.JWT
{
    public class JwtClientToken
    {
        public string CreateClientToken(List<Claim> clientClaims)
        {
            var token = new SecurityTokenDescriptor
            {
                Issuer = JwtTokenDefinition.Issuer,
                Audience = JwtTokenDefinition.Audience,
                Subject = new ClaimsIdentity(clientClaims),
                EncryptingCredentials = JwtTokenDefinition.SigningCredentials,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt16(JwtTokenDefinition.ClockSkew)),
                IssuedAt = DateTime.Now,
                Claims = clientClaims.ToDictionary(x=>x.Type, x=>x.Value as object)
            };

            return new JwtSecurityTokenHandler().CreateEncodedJwt(token);
        }
    }
}
