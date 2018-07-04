using IdentityModel;
using Microsoft.AspNetCore.Identity;
using OASP4Net.Infrastructure.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace OASP4Net.Business.Common.UserManagement.Service
{
    public class LoginService : ILoginService
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        public LoginService(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async System.Threading.Tasks.Task<bool> LoginAsync(string userName, string password)
        {            
            var result = await _signInManager.PasswordSignInAsync(userName, password, true, false);
            return result.Succeeded;
        }

        public async System.Threading.Tasks.Task<ApplicationUser> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);            
        }

        public List<Claim> GetUserClaimsAsync(ApplicationUser user)
        {
            var claims = new List<Claim>()
                    {
                        new Claim("jti", Guid.NewGuid().ToString()),
                        new Claim(JwtClaimTypes.Subject, user.UserName),
                        new Claim(JwtClaimTypes.IssuedAt, DateTime.Now.ToEpochTime().ToString(), ClaimValueTypes.Integer64),
                        new Claim("UserId", user.Id),
                        new Claim("UserName", user.UserName),
                        new Claim("UserEmail", user.Email)                        
                    };
            
            claims.AddRange(_userManager.GetClaimsAsync(user).Result.ToList());
            return claims;
        }

    }
}
