namespace Oasp4Net.Infrastructure.CrossCutting.Mailing.Implementation
{
    public class SMTPConfiguration
    {
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}