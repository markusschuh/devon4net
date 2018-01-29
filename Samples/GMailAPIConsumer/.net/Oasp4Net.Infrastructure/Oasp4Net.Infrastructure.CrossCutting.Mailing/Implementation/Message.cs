using System.Collections.Generic;
using System.IO;
using System.Linq;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Utils;

namespace Oasp4Net.Infrastructure.CrossCutting.Mailing.Implementation
{
    public class EmailMessage
    {
        public string From { get; set; }
        public string SenderName { get; set; }
        public List<string> To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        private List<MimePart> AttachmentList { get; set; }
        private BodyBuilder MessageBuilder { get; set; }
        private MimeMessage MailMessage { get; set; }
        private EmailBodyType EmailBodyType { get; set; }
        private SMTPConfiguration SmtpConfiguration { get; set; }

        public EmailMessage(string subject, string senderName, string emailFrom, List<string> emailDestinationList, SMTPConfiguration smtpConfiguration)
        {
            EmailBodyType = EmailBodyType.PlainText;
            Subject = subject;
            From = emailFrom;
            SenderName = senderName;
            To = emailDestinationList;
            SmtpConfiguration = smtpConfiguration;
            AttachmentList = new List<MimePart>();
            MessageBuilder = new BodyBuilder();
            MailMessage = new MimeMessage();
        }


        /// <summary>
        /// 
        ///        multipart/alternative
        ///       text/plain
        ///       multipart/related
        ///       text/html
        ///      image/jpeg
        ///      video/mp4
        ///       image/png
        /// 
        /// 
        /// </summary>
        /// <param name="mediaType"></param>
        /// <param name="mediaSubtype"></param>
        /// <param name="fullPath"></param>
        /// <param name="contentTransferEncoding"></param>
        /// <param name="contentObjectEncoding"></param>
        public void AddAttachment(string mediaType,string mediaSubtype, string fullPath,  ContentEncoding contentTransferEncoding = ContentEncoding.Base64,  ContentEncoding contentObjectEncoding = ContentEncoding.Default)
        {
            var attachment = new MimePart(mediaType, mediaSubtype)
            {
                ContentObject = new ContentObject(File.OpenRead(fullPath), contentObjectEncoding),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = contentTransferEncoding,
                FileName = Path.GetFileName(fullPath)
            };


            AttachmentList.Add(attachment);
        }

        public void AddBody(string stringBody,  List<string> linkedResourcesPathList, EmailBodyType emailBodyType  )
        {
            EmailBodyType = emailBodyType;
            foreach (var item in linkedResourcesPathList)
            {
                var resouce = MessageBuilder.LinkedResources.Add(item);
                resouce.ContentId = MimeUtils.GenerateMessageId();
            }

            switch (emailBodyType)
            {
                case EmailBodyType.HtmlText:
                    MessageBuilder.HtmlBody = linkedResourcesPathList.Any() ? string.Format(stringBody, MessageBuilder.LinkedResources.Select(l => l.ContentId).ToList()) : stringBody;                   
                    break;
                case EmailBodyType.PlainText:
                    MessageBuilder.TextBody = stringBody;
                    break;
                default:
                    MessageBuilder.TextBody = stringBody;
                    break;
            }
        }

        public void CreateAndSendMessage()
        {
            CreateMessage();
            SendMessage();
        }

        private void SendMessage()
        {
            //var client = new SmtpClient();
            using (var client = new SmtpClient())
            {
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Connect(SmtpConfiguration.SMTPServer, SmtpConfiguration.SMTPPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
                client.Authenticate(SmtpConfiguration.User, SmtpConfiguration.Password);
                client.Send(MailMessage);
                client.Disconnect(true);
            }
        }

        private void CreateMessage()
        {
            MailMessage.From.Add(new MailboxAddress(SenderName, From));
            foreach (var destinationMail in To)
            {
                MailMessage.To.Add(new MailboxAddress(destinationMail));
            }

            MailMessage.Subject = Subject;
            MailMessage.Body = AddAttachments();
        }

        private Multipart AddAttachments()
        {
            var multipart = new Multipart("mixed") { MessageBuilder.ToMessageBody() };

            foreach (var item in AttachmentList)
            {
                multipart.Add(item);
            }
            return multipart;
        }

    }
}
