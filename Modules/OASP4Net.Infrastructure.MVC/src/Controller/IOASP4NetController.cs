using OASP4Net.Infrastructure.MVC.Dto;
using System.Collections.Generic;

namespace OASP4Net.Infrastructure.MVC.Controller
{
    public interface IOASP4NetController
    {
        ResultObjectDto<T> GenerateResultDto<T>(int? page, int? size, int? total);
        ResultObjectDto<T> GenerateResultDto<T>(List<T> result, int? page = null, int? size = null);
    }
}