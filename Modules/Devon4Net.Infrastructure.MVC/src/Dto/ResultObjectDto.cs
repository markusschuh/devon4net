using System.Collections.Generic;
using Newtonsoft.Json;

namespace Devon4Net.Infrastructure.MVC.Dto
{
    public class ResultObjectDto<T>
    {
        [JsonProperty(PropertyName = "error")]
        public ErrorDto Error { get; set; }
        [JsonProperty(PropertyName = "pagination")]
        public Pagination Pagination { get; set; }
        [JsonProperty(PropertyName = "result")]
        public List<T> Result { get; set; }

        public ResultObjectDto()
        {
            Pagination = new Pagination();
            Result = new List<T>();
            Error = new ErrorDto { Code = string.Empty, Message = string.Empty };
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}