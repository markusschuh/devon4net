using Newtonsoft.Json;

namespace Devon4Net.Business.Common.UserManagement.Dto
{
    public class LoginDto
    {
        [JsonProperty(PropertyName = "username")]
        public string UserName { get; set; }
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
    }
}
