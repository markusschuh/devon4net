using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Devon4Net.Infrastructure.MVC.Dto;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Devon4Net.Infrastructure.MVC.Controller
{
    public class Devon4NetController: Microsoft.AspNetCore.Mvc.Controller, IDevon4NetController
    {
        protected IMapper Mapper { get; set; }
        protected ILogger Logger;
        private const int PageNumber = 1;
        private const int PageSize = 500;

        public Devon4NetController(ILogger logger)
        {
            Logger = logger;
        }

        public Devon4NetController(ILogger logger, IMapper mapper)  
        {
            Logger = logger;
            Mapper = mapper;
        }

        [NonAction]
        public ResultObjectDto<T> GenerateResultDto<T>(int? page = null,int? size = null, int? total = null)
        {
            return new ResultObjectDto<T> { Pagination = { Page = page!=null? page.Value : PageNumber,
                    Size = size??PageSize, Total = total }} ;
        }

        [NonAction]
        public ResultObjectDto<T> GenerateResultDto<T>(List<T> result, int? page = null, int? size = null)
        {
            return new ResultObjectDto<T>
            {
                Pagination = { Page = page!=null? page.Value : PageNumber,
                    Size = size??PageSize, Total = result?.Count ?? 0 },
                Result = result
            };
        }

        [NonAction]
        public string GetJsonFromObject(object value)
        {
            return JsonConvert.SerializeObject(value);
        }
    }
}
