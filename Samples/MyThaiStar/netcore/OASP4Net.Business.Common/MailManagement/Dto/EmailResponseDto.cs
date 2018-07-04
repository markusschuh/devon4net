namespace OASP4Net.Business.Common.MailManagement.Dto
{
    public class EmailResponseDto
    {
        public long id { get; set; }
        public int modificationCounter { get; set; }
        public int? revision { get; set; }
        public long bookingId { get; set; }
        public string guestToken { get; set; }
        public string email { get; set; }
        public bool accepted { get; set; }
        public string modificationDate { get; set; }
    }
}