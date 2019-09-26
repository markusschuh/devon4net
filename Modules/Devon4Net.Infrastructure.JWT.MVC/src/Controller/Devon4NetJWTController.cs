using AutoMapper;
using Microsoft.Extensions.Logging;
using Devon4Net.Infrastructure.MVC.Controller;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Devon4Net.Infrastructure.JWT.MVC.Controller
{
    public class Devon4NetJWTController : Devon4NetController
    {
        public Devon4NetJWTController(ILogger logger) : base(logger)
        {
        }

        public Devon4NetJWTController(ILogger logger, IMapper mapper) : base (logger,mapper)
        {
        }

        [NonAction]
        public IEnumerable<Claim> GetCurrentUser()
        {
            var headerValue = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", string.Empty).Trim();
            var handler = new JwtSecurityTokenHandler();

            var claimsPrincipal = handler.ValidateToken(headerValue,
                new TokenValidationParameters
                {
                    ValidAudience = JwtTokenDefinition.Audience,
                    ValidIssuer = JwtTokenDefinition.Issuer,
                    RequireSignedTokens = false,
                    TokenDecryptionKey = JwtTokenDefinition.SecurityKey
                }, out SecurityToken securityToken);

            return claimsPrincipal.Claims;
        }

        [NonAction]
        public Claim GetUserClaim(string claimName, IEnumerable<Claim> jwtUser = null)
        {
            var user = jwtUser ?? GetCurrentUser();
            return GetCurrentUser().FirstOrDefault(c => c.Type == claimName);
        }

        [NonAction]
        public IEnumerable<Claim> GetUserClaims(IEnumerable<Claim> jwtUser = null)
        {
            return jwtUser ?? GetCurrentUser();
        }
    }
}
