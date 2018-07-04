using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace OASP4Net.Infrastructure.ApplicationUser.Data
{
    /// <summary>
    /// Dataseeder sample of OASP My Thai Star restaurant sample
    /// </summary>
    public class DataSeeder
    {
        private readonly AuthContext _ctx;
        private readonly UserManager<ApplicationUser> _userManager;
        private const string RoleWaiter = "waiter";
        private const string RoleCustomer = "customer";

        public DataSeeder(AuthContext ctx, UserManager<ApplicationUser> userManager)
        {
            _ctx = ctx;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _ctx.Database.EnsureCreated();

            foreach (var user in _ctx.Users)
            {
                var a = _userManager.DeleteAsync(user);
            }


            if (!_ctx.Users.Any())
            {
                await CreateUserAsync("waiter", "waiter", "waiter@mts.com", RoleWaiter);
                await CreateUserAsync("user0", "password", "customer@mts.com", RoleCustomer);
            }
        }

        private async Task CreateUserAsync(string userName, string userPassword, string email, string role)
        {
            var user = new ApplicationUser { Email = email, UserName = userName };
            var result = await _userManager.CreateAsync(user, userPassword);

            if (result.Succeeded)
            {
                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
            }

            result = _userManager.AddClaimsAsync(user, 
                new Claim[]{
                       new Claim(ClaimTypes.Role, role),
                       new Claim("role", role) }).Result;

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.First().Description);
            }
        }
    }
}
