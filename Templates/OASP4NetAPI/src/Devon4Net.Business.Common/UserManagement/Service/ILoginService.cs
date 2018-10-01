using Devon4Net.Infrastructure.ApplicationUser;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Devon4Net.Business.Common.UserManagement.Service
{
    public interface ILoginService
    {
        Task<bool> LoginAsync(string userName, string password);
        Task<ApplicationUser> GetUserByUserNameAsync(string userName);
        List<Claim> GetUserClaimsAsync(ApplicationUser user);
    }
}