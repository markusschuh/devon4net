using AutoMapper;
using Microsoft.Extensions.Logging;
using OASP4Net.Infrastructure.MVC.Controller;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace OASP4Net.Infrastructure.JWT.MVC.Controller
{
    
    public class OASP4NetJWTController : OASP4NetController, IOASP4NetJWTController
    {
        public OASP4NetJWTController(ILogger logger) : base(logger)
        {

        }

        public OASP4NetJWTController(ILogger logger, IMapper mapper) : base (logger,mapper)
        {

        }



        public JwtSecurityToken GetCurrentUser()
        {
            var headerValue = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer", string.Empty).Trim();
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(headerValue);
        }

        public Claim GetUserClaim(string claimName, JwtSecurityToken jwtUser = null)
        {
            var user = jwtUser != null ? jwtUser : GetCurrentUser();
            return user.Claims.FirstOrDefault(c => c.Type == claimName);
        }

        public IEnumerable<Claim> GetUserClaims(JwtSecurityToken jwtUser = null)
        {
            var user = jwtUser != null ? jwtUser : GetCurrentUser();
            return user.Claims;
        }


    }
}
