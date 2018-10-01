using Newtonsoft.Json;

namespace Devon4Net.Infrastructure.MVC.Dto
{
    public class ErrorDto
    {
        [JsonProperty(PropertyName = "code")]
        public string Code { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

    }
}
