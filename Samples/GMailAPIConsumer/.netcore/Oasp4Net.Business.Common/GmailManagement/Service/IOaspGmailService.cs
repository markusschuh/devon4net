using System.Threading.Tasks;
using Oasp4Net.Business.Common.GmailManagement.Dto;

namespace Oasp4Net.Business.Common.GmailManagement.Service
{
    public interface IOaspGmailService
    {
        bool Send(EmailDto emailDto);
    }
}
