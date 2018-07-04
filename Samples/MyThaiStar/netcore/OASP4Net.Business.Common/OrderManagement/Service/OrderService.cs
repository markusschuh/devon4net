using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using OASP4Net.Business.Common.OrderManagement.DataType;
using OASP4Net.Business.Common.OrderManagement.Dto;
using OASP4Net.Domain.Entities;
using OASP4Net.Domain.Entities.Models;
using OASP4Net.Domain.UnitOfWork.Service;
using OASP4Net.Domain.UnitOfWork.UnitOfWork;

namespace OASP4Net.Business.Common.OrderManagement.Service
{
    public class OrderService : Service<ModelContext>, IOrderService
    {
        private IMapper Mapper { get; set; }
        public OrderService(IUnitOfWork<ModelContext> uoW, IMapper mapper) : base(uoW)
        {
            Mapper = mapper;
        }

        public void OrderTheOrder(string bookingToken, IList<OrderlineDto> orderLineDtoList)
        {
            var order = new Order();
            try
            {
                order = CreateOrder(bookingToken);
                var orderLineList = CreateOrderLines(order.Id, orderLineDtoList);
                if (orderLineList!=null && orderLineList.Count>0) CreateOrderLinesExtras(ref orderLineList, orderLineDtoList);
            }
            catch (Exception ex)
            {
                if (order != null && order.Id > 0) DeleteOrder(order.Id);
                throw ex;
            }
        }

        private Order CreateOrder(string bookingToken)
        {
            var order = new Order();

            try
            {
                var booking = GetBooking(bookingToken);
                if (booking == null) throw new Exception("No booking");

                var bookingtype = bookingToken.StartsWith("CB") ? OrderTypeEnum.CommonBooking : OrderTypeEnum.GuestBooking;
                var orders = UoW.Repository<Order>().GetAll(o => o.IdReservation == booking.Id || o.IdInvitationGuest == booking.Id);

                if (orders != null && orders.Count > 0) throw new Exception("The order already exists");
                else order = bookingtype == OrderTypeEnum.CommonBooking ? new Order { IdReservation = booking.Id } : new Order { IdInvitationGuest = booking.Id }; ;

                order = UoW.Repository<Order>().Create(order);

                UoW.Commit();

                return order;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} : {ex.InnerException}");
                throw ex;
            }
        }

        private List<OrderDishExtraIngredient> CreateOrderLinesExtras(ref List<OrderLine> OrderLineList, IList<OrderlineDto> OrderlineDtoList)
        {
            var result = new List<OrderDishExtraIngredient>();
            try
            {
                foreach (var orderLine in OrderLineList)
                {
                    var item = OrderlineDtoList.FirstOrDefault(l => l.orderLine.dishId == orderLine.IdDish
                    && l.orderLine.amount == orderLine.Amount && l.orderLine.comment == orderLine.Comment).extras.
                    Select(e =>
                    {
                        var c = new OrderDishExtraIngredient
                        {
                            IdIngredient = e.id,
                            IdOrderLine = orderLine.Id.Value
                        };

                        UoW.Repository<OrderDishExtraIngredient>().Create(c);

                        return c;
                    });
                    result.AddRange(item);
                }
                UoW.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} : {ex.InnerException}");
                throw ex;
            }

            return result;
        }

        private List<OrderLine> CreateOrderLines(long orderId, IList<OrderlineDto> orderLines)
        {
            try
            {
                var orderLinesMap = Mapper.Map<List<OrderLine>>(orderLines).Select(c =>
                {
                    c.IdOrder = orderId;
                    c.OrderDishExtraIngredient = new List<OrderDishExtraIngredient>();
                    c = UoW.Repository<OrderLine>().Create(c);
                    UoW.Commit();
                    return c;
                });

                return orderLinesMap.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message} : {ex.InnerException}");
                throw ex;
            }         
        }

        private Booking GetBooking(string bookingToken)
        {
            return UoW.Repository<Booking>().Get(b => b.ReservationToken == bookingToken);
        }

        private void DeleteOrder(long idOrder)
        {
            if (idOrder <= 0) return;

            var orderLinesToDelete = UoW.Repository<OrderLine>().GetAll(o => o.IdOrder == idOrder);
            orderLinesToDelete.Select(o => { UoW.Repository<OrderLine>().Delete(o); return o; });
            UoW.Repository<Order>().Delete(p => p.Id == idOrder);            
        }
    }

}
