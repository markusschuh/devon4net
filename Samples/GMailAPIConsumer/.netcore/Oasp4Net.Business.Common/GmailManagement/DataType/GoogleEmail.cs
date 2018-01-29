using System.Text;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using MimeKit;

namespace Oasp4Net.Business.Common.GmailManagement.DataType
{
    public class GoogleEmail
    {
        public void SendMessage(GmailService service, string userId, MimeMessage emailMessage)
        {
            var gmailMessage = new Message
            {
                Raw = Encode(emailMessage.ToString())
            };

            var request = service.Users.Messages.Send(gmailMessage, userId);

            request.Execute();
        }

        private string Encode(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);

            return System.Convert.ToBase64String(bytes)
                .Replace('+', '-')
                .Replace('/', '_')
                .Replace("=", "");
        }
    }
}
