using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace Devon4Net.Infrastructure.JWT.Handlers
{
    public interface IJwtHandler
    {
        string CreateClientToken(List<Claim> clientClaims);
        IEnumerable<Claim> GetUserClaims(string jwtToken);
        SecurityKey GetIssuerSigningKey();
    }
}