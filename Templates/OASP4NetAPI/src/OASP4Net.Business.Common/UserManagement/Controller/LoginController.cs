using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OASP4Net.Business.Common.UserManagement.Dto;
using OASP4Net.Business.Common.UserManagement.Service;
using OASP4Net.Infrastructure.ApplicationUser;
using OASP4Net.Infrastructure.JWT;
using OASP4Net.Infrastructure.JWT.MVC.Controller;

namespace OASP4Net.Business.Common.UserManagement.Controller
{
    [EnableCors("CorsPolicy")]

    public class LoginController : OASP4NetJWTController
    {        
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService,  SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, ILogger<LoginController> logger, IMapper mapper) : base(logger,mapper)
        {
            _loginService = loginService;
        }

        [HttpGet]
        [HttpOptions]
        [Route("/api/user/v1/currentuser")]
        [EnableCors("CorsPolicy")]
        public IActionResult CurrentUser()
        {
            CurrentUserDto result = new CurrentUserDto();

            try
            {
                var user = GetCurrentUser();
                if (user == null) throw new Exception("User not found");

                var userEasyName = GetUserClaim("UserName", user).Value;
                Logger.LogInformation($"userEasyName: {userEasyName}");

                result = new CurrentUserDto
                {
                    Name = userEasyName,
                    Role = GetUserClaim(ClaimTypes.Role, user)?.Value.ToUpper(),
                    Id = user.Id,
                    FirstName = userEasyName,
                    LastName = null
                };
            }
            catch (Exception ex)
            {
                Logger.LogDebug($"{ex.Message} : {ex.InnerException}");
                throw ex;
            }
            
            return Ok(GetJsonFromObject(result));            
        }

        /// <summary>
        /// Gets the  list of available dishes regarding the filter options
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        /// <response code="200"> Ok. </response>
        /// <response code="401">Unathorized. Autentication fail</response>  
        /// <response code="403">Forbidden. Authorization error.</response>    
        /// <response code="500">Internal Server Error. The search process ended with error.</response>       
        [HttpPost]
        [HttpOptions]
        [Route("/api/user/v1/login")]
        [AllowAnonymous]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> Login([FromBody]LoginDto loginDto)
        {
            try
            {
                if (loginDto == null) return Ok();
                var loged = await _loginService.LoginAsync(loginDto.UserName, loginDto.Password);
                if (loged)
                {
                    var user = await _loginService.GetUserByUserNameAsync(loginDto.UserName);
                    var encodedJwt = new JwtClientToken().CreateClientToken(_loginService.GetUserClaimsAsync(user));
                    
                    Response.Headers.Add("Access-Control-Expose-Headers", "Authorization");
                    Response.Headers.Add("X-Application-Context", "restaurant:h2mem:8081");
                    Response.Headers.Add("Authorization", $"{JwtBearerDefaults.AuthenticationScheme} {encodedJwt}");
                    
                    return Ok(encodedJwt);
                }
                else
                {
                    Response.Headers.Clear();
                    return StatusCode((int)HttpStatusCode.Unauthorized, "Login Error");
                }
                
            }
            catch (Exception ex)
            {
                OASP4Net.Infrastructure.Log.OASP4NetLogger.Debug(ex);
                throw ex;
            }
        }
    }
}
