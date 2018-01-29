using Oasp4Net.Business.Views.Views;

namespace Oasp4Net.Business.Repositories.EmailManagement.Interfaces
{
    public interface IEmailRepository
    {
        bool Send(EmailView emailView);
    }
}
