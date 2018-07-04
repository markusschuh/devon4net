using Microsoft.Extensions.Logging;
using Moq;
using OASP4Net.Business.Common.OrderManagement.Controller;
using OASP4Net.Business.Common.OrderManagement.Dto;
using OASP4Net.Business.Common.OrderManagement.Service;
using OASP4Net.Domain.Entities;
using OASP4Net.Domain.UnitOfWork.UnitOfWork;
using OASP4Net.Test.xUnit.Test.Integration;
using System;
using System.Collections.Generic;
using Xunit;

namespace OASP4Net.Test.xUnit.Test.IntegrationTest.Management
{
    public class OrderManagementTest : MyThaiStarIntegrationTest
    {
        public OrderService OrderService { get; set; }
        public Mock<ILogger<OrderController>> Logger { get; set; }
        public UnitOfWork<ModelContext> UoW { get; set; }

        const string ExistingBookingToken = "CB_20171017_ac68fd21e7953b5d778a9776b33d34ea";
        const string NoExistingBookingToken = "CB_Not_Existing_Token";

        public OrderManagementTest()
        {
            Logger = new Mock<ILogger<OrderController>>();
            UoW = new UnitOfWork<ModelContext>(Context);
            OrderService = new OrderService(UoW, Mapper);
        }

        [Fact]
        public void OrderAnOrderAlreadyCreated()
        {                    
            try
            {
                OrderService.OrderTheOrder(ExistingBookingToken, new List<OrderlineDto>());
            }
            catch (Exception ex)
            {
                Assert.Equal("The order already exists", ex.Message);
            }
        }

        [Fact]
        public void OrderAnOrderBookingNotExist()
        {
            try
            {
                OrderService.OrderTheOrder(NoExistingBookingToken, new List<OrderlineDto>());
            }
            catch (Exception ex)
            {
                Assert.Equal("No booking", ex.Message);
            }
        }
    }
}
