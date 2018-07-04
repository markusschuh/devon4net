using System.Collections.Generic;
using OASP4Net.Business.Common.OrderManagement.Dto;
using OASP4Net.Domain.Entities.Models;

namespace OASP4Net.Business.Common.OrderManagement.Service
{
    public interface IOrderService
    {
        void OrderTheOrder(string bookingToken, IList<OrderlineDto> orderLines);        
    }
}