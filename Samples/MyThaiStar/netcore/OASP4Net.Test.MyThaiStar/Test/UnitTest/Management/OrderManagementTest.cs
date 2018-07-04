using Moq;
using OASP4Net.Business.Common.OrderManagement.Dto;
using OASP4Net.Business.Common.OrderManagement.Service;
using System;
using System.Collections.Generic;
using Xunit;

namespace OASP4Net.Test.xUnit.Test.UnitTest.Management
{
    public class OrderManagementTest:  MyThaiStarUnitTest
    {
        public IOrderService OrderService { get; set; }
        private List<OrderlineDto> orderLineDtoList = new List<OrderlineDto>();
        const string ExistingBookingToken = "CB_20171017_ac68fd21e7953b5d778a9776b33d34ea";
        const string NoExistingBookingToken = "CB_Not_Existing_Token";
        

        public OrderManagementTest()
        {
            
            var mockOrderService = new Mock<IOrderService>();

            mockOrderService.Setup(m => m.OrderTheOrder(ExistingBookingToken, orderLineDtoList)).Throws(new Exception("The order already exists"));
            mockOrderService.Setup(m => m.OrderTheOrder(NoExistingBookingToken, orderLineDtoList)).Throws(new Exception("No booking"));
            OrderService = mockOrderService.Object;
        }

        [Fact]
        public void OrderAnOrderAlreadyCreated()
        {
            try
            {
                OrderService.OrderTheOrder(ExistingBookingToken, orderLineDtoList);
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
                OrderService.OrderTheOrder(NoExistingBookingToken, orderLineDtoList);
            }
            catch (Exception ex)
            {
                Assert.Equal("No booking", ex.Message);
            }
        }
    }

}
