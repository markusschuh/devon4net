using System;

namespace OASP4Net.Infrastructure.Extensions
{
    public static class DateTimeExtension
    {
        public static double ConvertDateTimeToMilliseconds(this DateTime dateTime)
        {
            try
            {
                return dateTime.ToUniversalTime().Subtract(
                    new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                ).TotalMilliseconds;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} : {ex.InnerException}");
                throw ex; 
            }
        }
    }
}
