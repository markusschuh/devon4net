using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Microsoft.Extensions.PlatformAbstractions;
using Oasp4Net.Business.Common.GmailManagement.DataType;
using Oasp4Net.Business.Common.GmailManagement.Dto;

namespace Oasp4Net.Business.Common.GmailManagement.Service
{
    public class OaspGmailService : IOaspGmailService
    {
        private static UserCredential Credential { get; set; }
        private static GmailService GmailServiceInstance { get; set; }
        private string[] Scopes = { GmailService.Scope.MailGoogleCom };
        private string ApplicationName = "Gmail API .NET";

        public OaspGmailService()
        {
            LoadGoogleCredentials();
        }



        public bool Send(EmailDto emailDto)
        {
            var result = true;
            try
            {
                foreach (var destination in emailDto.EmailAndTokenTo.Values)
                {

                    var mymsg = new EmailMessage("My Thai Star Restaurant booking stastus", "My Thai Star Restaurant", emailDto.EmailFrom, new List<string> { destination });

                    var body = PopulateEmailBody(emailDto, destination);


                    mymsg.AddBody(body, new List<string> { GeFilePath("mui-email-inline.css"), GeFilePath("mui-email-styletag.css") }, EmailBodyType.HtmlText);
                    mymsg.AddBody(body, new List<string>(), EmailBodyType.HtmlText);

                    var gmailMessageSender = new GoogleEmail();
                    gmailMessageSender.SendMessage(GmailServiceInstance, Credential.UserId, mymsg.GetEmailMessage());

                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} : {ex.InnerException}");
                result = false;
            }

            return result;
        }

        private void LoadGoogleCredentials()
        {
            using (var stream = new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                var credPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");

                Credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "me",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;

                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            GmailServiceInstance = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = Credential,
                ApplicationName = ApplicationName,
            });
        }

        private string PopulateEmailBody(EmailDto emailView, string destinationUser)
        {
            var result = string.Empty;
            var emailTemplate = GetEmailTemplateFromEmailType(emailView.EmailType);
            var body = File.ReadAllText(GeFilePath( emailTemplate));
            switch (emailView.EmailType)
            {
                case EmailTypeEnum.Order:
                    var menu = FormatMenuItems(emailView.DetailMenu);
                    result = body.Replace("{0}", destinationUser);
                    result = result.Replace("{1}", menu);
                    result = result.Replace("{2}", emailView.Price.ToString());
                    result = result.Replace("{3}", FormatButtonActions(emailView.ButtonActionList));

                    //NotWorking :(
                    //result = string.Format(body, destinationUser, menu,emailView.Price, FormatButtonActions(emailView.ButtonActionList));
                    break;
                case EmailTypeEnum.CreateBooking:
                    result = body.Replace("{0}", destinationUser);
                    result = result.Replace("{1}", emailView.BookingDate.ToShortDateString());
                    result = result.Replace("{2}", emailView.BookingDate.ToShortTimeString());
                    result = result.Replace("{3}", emailView.Assistants.ToString());
                    //notworking :(
                    //result = string.Format(body, destinationUser, emailView.BookingDate.ToShortDateString() , emailView.BookingDate.ToShortTimeString(), emailView.Assistants.ToString());
                    break;
                case EmailTypeEnum.InvitedGuest:

                    result = body.Replace("{0}", destinationUser);
                    result = result.Replace("{1}", $"{emailView.Host.Values.FirstOrDefault()} &lt;{emailView.Host.Keys.FirstOrDefault()}&gt; ");
                    result = result.Replace("{2}", emailView.BookingDate.ToShortDateString());
                    result = result.Replace("{3}", emailView.BookingDate.ToShortTimeString());
                    result = result.Replace("{4}", FormatGuestList(emailView.EmailAndTokenTo));
                    result = result.Replace("{5}", FormatButtonActions(emailView.ButtonActionList));


                    //NotWorking :(
                    //result = string.Format(body, destinationUser,$"{emailView.Host.Values.FirstOrDefault()} &lt;{emailView.Host.Keys.FirstOrDefault()}&gt; " ,emailView.BookingDate.ToShortDateString(), emailView.BookingDate.ToShortTimeString(), emailView.EmailAndTokenTo.Values, FormatButtonActions(emailView.ButtonActionList));
                    break;
                case EmailTypeEnum.InvitedHost:
                    result = body.Replace("{0}", destinationUser);
                    result = result.Replace("{1}", emailView.BookingDate.ToShortDateString());
                    result = result.Replace("{2}", emailView.BookingDate.ToShortTimeString());
                    result = result.Replace("{3}", FormatGuestList(emailView.EmailAndTokenTo));
                    result = result.Replace("{4}", FormatButtonActions(emailView.ButtonActionList));
                    //NotWorking :(
                    //result = string.Format(body, destinationUser, emailView.BookingDate.ToShortDateString(), emailView.BookingDate.ToShortTimeString(), emailView.Assistants, FormatButtonActions(emailView.ButtonActionList));
                    break;
            }

            return result;
        }

        private string GetEmailTemplateFromEmailType(EmailTypeEnum emailViewEmailType)
        {
            string result;

            switch (emailViewEmailType)
            {
                case EmailTypeEnum.Order:
                    result = "order.html";
                    break;
                case EmailTypeEnum.CreateBooking:
                    result = "createBooking.html";
                    break;
                case EmailTypeEnum.InvitedGuest:
                    result = "createInvitationGuest.html";
                    break;
                case EmailTypeEnum.InvitedHost:
                    result = "createInvitationHost.html";
                    break;
                default:
                    result = "order.html";
                    break;
            }

            return result;
        }

        private string FormatMenuItems(List<string> menuList)
        {
            var builder = new StringBuilder();
            foreach (var item in menuList)
            {
                builder.AppendLine($"<li>{item}</li>");
            }

            return builder.ToString();
        }

        private string FormatButtonActions(Dictionary<string, string> buttonActionList)
        {
            var builder = new StringBuilder();

            foreach (var item in buttonActionList)
            {
                builder.AppendLine($"<a class=\"mui-btn mui-btn--danger\" href=\"{item.Key}\">{item.Value}</a>");

            }

            return builder.ToString();
        }

        private string FormatGuestList(Dictionary<string, string> guestList)
        {
            var builder = new StringBuilder();

            foreach (var item in guestList)
            {
                builder.AppendLine($"<li>{item.Value}</li>");
            }

            return builder.ToString();
        }

        private string GeFilePath(string fileName)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath;
            var files = Directory.GetFiles(basePath, $"*{fileName}*", SearchOption.AllDirectories).ToList();

            return files.FirstOrDefault();
        }
    }
}
