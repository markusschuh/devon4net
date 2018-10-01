using System.Collections.Generic;

namespace Devon4Net.Infrastructure.Cors
{
    public class CorsAppConfiguration
    {
        public string CorsPolicy { get; set; }
        public List<string> Origins { get; set; }
        public List<string> Headers { get; set; }
        public List<string> Methods { get; set; }
        public bool AllowCredentials { get; set; }

        public CorsAppConfiguration()
        {
            Origins = new List<string>();
            Headers = new List<string>();
            Methods = new List<string>();
        }
    }
}
