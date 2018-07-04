using System.Threading.Tasks;

namespace OASP4Net.Infrastructure.Mail
{
    public interface ISmtpMailSender
    {
        Task<bool> SendAsync();
    }
}