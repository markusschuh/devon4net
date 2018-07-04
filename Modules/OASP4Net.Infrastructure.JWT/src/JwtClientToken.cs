using Microsoft.IdentityModel.Logging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace OASP4Net.Infrastructure.JWT
{
    public class JwtClientToken
    {
        public string CreateClientToken(List<Claim> clientClaims)
        {            
            var token = new JwtSecurityToken(
                    issuer: JwtTokenDefinition.Issuer,
                    audience: JwtTokenDefinition.Audience,
                    claims: clientClaims,
                    expires: DateTime.UtcNow.AddMinutes(Convert.ToInt16(JwtTokenDefinition.ClockSkew)),
                    signingCredentials: JwtTokenDefinition.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
