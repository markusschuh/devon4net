using AutoMapper;
using Microsoft.Extensions.Logging;
using Devon4Net.Infrastructure.MVC.Controller;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

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
        public JwtSecurityToken GetCurrentUser()
        {
            var headerValue = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", string.Empty).Trim();
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(headerValue);
        }

        [NonAction]
        public Claim GetUserClaim(string claimName, JwtSecurityToken jwtUser = null)
        {
            var user = jwtUser ?? GetCurrentUser();
            return user.Claims.FirstOrDefault(c => c.Type == claimName);
        }

        [NonAction]
        public IEnumerable<Claim> GetUserClaims(JwtSecurityToken jwtUser = null)
        {
            var user = jwtUser ?? GetCurrentUser();
            return user.Claims;
        }
    }
}
