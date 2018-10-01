using System.Threading.Tasks;

namespace Devon4Net.Infrastructure.Mail
{
    public interface ISmtpMailSender
    {
        Task<bool> SendAsync();
    }
}