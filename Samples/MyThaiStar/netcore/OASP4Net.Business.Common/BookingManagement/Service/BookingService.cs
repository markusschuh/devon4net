using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Extensions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OASP4Net.Business.Common.BookingManagement.DataType;
using OASP4Net.Business.Common.BookingManagement.Dto;
using OASP4Net.Business.Common.MailManagement.DataType;
using OASP4Net.Business.Common.MailManagement.Dto;
using OASP4Net.Business.Common.OrderManagement.DataType;
using OASP4Net.Business.Common.OrderManagement.Dto;
using OASP4Net.Domain.Entities;
using OASP4Net.Domain.Entities.Models;
using OASP4Net.Domain.UnitOfWork.Pagination;
using OASP4Net.Domain.UnitOfWork.Service;
using OASP4Net.Domain.UnitOfWork.UnitOfWork;
using OASP4Net.Infrastructure.Communication.RestManagement.Service;
using OASP4Net.Infrastructure.Extensions;

namespace OASP4Net.Business.Common.BookingManagement.Service
{
    public class BookingService : Service<ModelContext>, IBookingService
    {
        private IConfiguration Configuration { get; set; }
        private IMapper Mapper { get; set; }

        public BookingService(IUnitOfWork<ModelContext> uoW, IMapper mapper, IConfiguration configuration) : base(uoW)
        {
            Configuration = configuration;
            Mapper = mapper;
        }


        public async Task<PaginationResult<BookingSearchResponseDto>> GetBookingSearch(int currentpage, int pageSize, string bookingToken, string email)
        {
            var result = new PaginationResult<BookingSearchResponseDto>();
            var includeList = new List<string>
            {
                "Order","InvitedGuest","User","Table"
            };
            try
            {
                Expression<Func<Booking, bool>> expression = !string.IsNullOrEmpty(email) ? b => b.Email == email : (Expression<Func<Booking, bool>>)null;
                var bookings = !string.IsNullOrEmpty(bookingToken) ? await GetBookingListPaged(currentpage,pageSize, bookingToken, includeList) : await UoW.Repository<Booking>().GetAllIncludePagedAsync(currentpage, pageSize, includeList, expression);
                result.PageSize = bookings.PageSize;
                result.PageCount = bookings.PageCount;
                result.RowCount= bookings.RowCount;

                foreach (var booking in bookings.Results)
                {
                    var orders = booking.Order;
                    var invitedList = booking.InvitedGuest!=null && booking.InvitedGuest.Count>0 ? booking.InvitedGuest : new List<InvitedGuest>();
                    result.Results.Add(new BookingSearchResponseDto
                    {
                        booking = Mapper.Map<BookingResponseDto>(booking),
                        invitedGuests = invitedList.Count>0? Mapper.Map<List<InvitedguestSearchDto>>(invitedList) : new List<InvitedguestSearchDto>(), //invitedList.Select(InvitedGuestConverter.EntityToApi).ToList(),
                        orders = booking.Order != null && booking.Order.Count>0
                            ? Mapper.Map<List<OrderSearchDto>>(orders)
                            : new List<OrderSearchDto>(),
                        order = booking.Order != null && booking.Order.Count>0
                            ? orders != null && orders.Count>0
                                ? Mapper.Map<OrderSearchDto>(orders.FirstOrDefault())
                                : new OrderSearchDto()
                            : new OrderSearchDto(),
                        table = new TableSearchDto(),
                        user = booking.User != null
                            ? Mapper.Map<UserSearchDto>(booking.User)
                            : new UserSearchDto()
                    });
                }
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
                Console.WriteLine(msg);
            }

            return result;
        }
        public async Task<List<BookingSearchResponseDto>> GetBookingSearch(string bookingToken, string email)
        {
            var result = new List<BookingSearchResponseDto>();
            var includeList = new List<string>
            {
                "Order","InvitedGuest","User","Table"
            };
            try
            {
                Expression<Func<Booking, bool>> expression = !string.IsNullOrEmpty(email) ? b => b.Email == email : (Expression<Func<Booking, bool>>)null;
                var bookings = !string.IsNullOrEmpty(bookingToken) ? GetBookingList(bookingToken, includeList) : await UoW.Repository<Booking>().GetAllIncludeAsync(includeList, expression);

                foreach (var booking in bookings.ToList())
                {
                    var orders = booking.Order;
                    var invitedList = booking.InvitedGuest;
                    result.Add(new BookingSearchResponseDto
                    {
                        booking = Mapper.Map<BookingResponseDto>(booking),
                        invitedGuests =  Mapper.Map<List<InvitedguestSearchDto>>(invitedList),//invitedList.Select(InvitedGuestConverter.EntityToApi).ToList(),
                        orders = booking.Order != null
                            ? Mapper.Map<List<OrderSearchDto>>(orders)
                            : new List<OrderSearchDto>(),
                        order = booking.Order != null
                            ? orders != null
                                ? Mapper.Map<OrderSearchDto>(orders.FirstOrDefault())
                                : new OrderSearchDto()
                            : new OrderSearchDto(),
                        table = new TableSearchDto(),
                        user = booking.User != null
                            ? Mapper.Map<UserSearchDto>(booking.User)
                            : new UserSearchDto()
                    });
                }
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
                Console.WriteLine(msg);
            }

            return result;
        }



        public async Task<List<OrderSearchFullResponseDto>> GetBookinSearchFullResponse(string bookingToken, string email)
        {
            var result = new List<OrderSearchFullResponseDto>();
            var includeList = new List<string>
            {
                "Order","Order.OrderLine", "Order.OrderLine.IdDishNavigation", "Order.OrderLine.IdOrderNavigation","Order.OrderLine.OrderDishExtraIngredient","Order.OrderLine.OrderDishExtraIngredient.IdIngredientNavigation","InvitedGuest","User","Table"
            };
            try
            {
                #region orders from common booking
                //search through common booking
                var commonbookingList = UoW.Repository<Booking>().GetAllInclude(includeList, b => b.IdBookingType == 0);
                foreach (var booking in commonbookingList)
                {
                    result.AddRange(GetOrderSearchFullResponseDtoList(booking, booking.Order));
                }
                #endregion

                #region orders from invited guest
                //search through invited
                var invitedList = await GetInvitedFromToken(bookingToken);
                foreach (var invitedItem in invitedList)
                {
                    result.AddRange(GetOrderSearchFullResponseDtoList(invitedItem.IdBookingNavigation, invitedItem.Order));
                }
                #endregion

            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
            }

            return result;

        }

        public async Task<BookingResponseDto> CreateReservationAndGuestList(int bookingDtoType, string bookingDtoName, int reservationDtoAssistants, IList<string> reservationDtoGuestList, string reservationDtoEmail, DateTime reservationDtoDate, long? userId)
        {
            try
            {
                var reservation = CreateReservation(bookingDtoType, bookingDtoName, reservationDtoAssistants, reservationDtoEmail, reservationDtoDate, userId);
                //we add the organizer as guest if the invitation is GuestBooking type
                if (bookingDtoType != 0) reservationDtoGuestList.Add(reservationDtoEmail);

                var invitationGuestList = CreateInvitationGuests(reservation.Id, reservationDtoGuestList, reservationDtoEmail);
                UoW.Commit();

                await SendReservationEmail(bookingDtoType, bookingDtoName, reservationDtoAssistants, invitationGuestList, reservationDtoEmail, reservationDtoDate, reservation);
                return Mapper.Map<BookingResponseDto>(reservation);
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
                Console.WriteLine(msg);
                throw ex;
            }
        }

        private async Task SendReservationEmail(int bookingDtoType, string bookingDtoName, int reservationDtoAssistants, List<InvitedGuest> reservationDtoGuestList, string reservationDtoEmail, DateTime reservationDtoDate, Booking reservation)
        {
            var url = Configuration["EmailServiceUrl"];
            var emailAndTokenTo = reservationDtoGuestList.Any() ? reservationDtoGuestList.ToDictionary(guest => guest.GuestToken, guest => guest.Email) : new Dictionary<string, string>();

            var orderLines = reservation.Order.SelectMany(o => o.OrderLine).ToList().Select(o => o.IdDishNavigation.Name).ToList();
            var acceptUrl = string.Format(Configuration["AcceptBooking"], reservation.ReservationToken);
            var cancelUrl = string.Format(Configuration["CancelBooking"], reservation.ReservationToken);

            var dataToSend = new EmailDto
            {
                BookingDate = reservationDtoDate,
                Assistants = reservationDtoAssistants > 0 ? reservationDtoAssistants : reservationDtoGuestList.Count,
                EmailAndTokenTo = emailAndTokenTo,
                EmailFrom = reservationDtoEmail,
                EmailType = bookingDtoType == 0 ? EmailTypeEnum.CreateBooking : EmailTypeEnum.Order,
                BookingToken = reservation.ReservationToken,
                ButtonActionList = bookingDtoType == 0 ? new Dictionary<string, string>() : new Dictionary<string, string> { { acceptUrl, "Accept" }, { cancelUrl, "Cancel" } },
                DetailMenu = orderLines,
                Host = new Dictionary<string, string> { { reservation.Email, reservation.Name } },

            };

            await SendHostEmail(dataToSend);

            foreach (var guest in reservationDtoGuestList)
            {
                await SendInvitedGuestEmail(dataToSend, guest);
            }

            var mailService = new RestManagementService();
            await mailService.CallPostMethod(url, dataToSend);
        }

        private async Task SendInvitedGuestEmail(EmailDto dataToSend, InvitedGuest guest)
        {
            var url = Configuration["EmailServiceUrl"];

            var acceptUrl = string.Format(Configuration["AcceptBooking"], guest.GuestToken);
            var cancelUrl = string.Format(Configuration["CancelBooking"], guest.GuestToken);
            dataToSend.EmailType = EmailTypeEnum.InvitedGuest;
            dataToSend.ButtonActionList = new Dictionary<string, string> { { acceptUrl, "Accept" }, { cancelUrl, "Cancel" } };
            var mailService = new RestManagementService();
            await mailService.CallPostMethod(url, dataToSend);
        }

        private async Task SendHostEmail(EmailDto dataToSend)
        {
            var url = Configuration["EmailServiceUrl"];
            var host = dataToSend.Host.FirstOrDefault();
            dataToSend.EmailAndTokenTo.Add(host.Value, host.Key);

            dataToSend.EmailType = EmailTypeEnum.InvitedHost;
            dataToSend.ButtonActionList = new Dictionary<string, string>();
            var mailService = new RestManagementService();
            await mailService.CallPostMethod(url, dataToSend);
        }


        public EmailResponseDto AcceptOrRejectInvitationGuest(string bookingToken, bool accepted)
        {

            var invitedGuest = UoW.Repository<InvitedGuest>().Get(i => i.GuestToken == bookingToken);
            if (invitedGuest == null) return null;
            invitedGuest.ModificationDate = DateTime.UtcNow.ToLocalTime();
            invitedGuest.Accepted = accepted;

            UoW.Commit();

            return Mapper.Map<EmailResponseDto>(invitedGuest);
        }

        public async Task<bool> CancelBooking(string token)
        {

            try
            {
                var booking = await UoW.Repository<Booking>().GetAsync(b => b.ReservationToken == token);
                if (booking == null) return false;
                booking.Canceled = true;

                UoW.Commit();
                return true;
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
                Console.WriteLine(msg);
            }
            return false;
        }


        #region private methods

        private List<Booking> GetBookingList(string bookingToken, List<string> includeList)
        {
            var result = new List<Booking>();
            var bookingtype = GetType(bookingToken);
            switch (bookingtype)
            {
                case OrderTypeEnum.CommonBooking:
                    result = UoW.Repository<Booking>().GetAll(b => b.ReservationToken == bookingToken).ToList();
                    break;
                case OrderTypeEnum.GuestBooking:
                    var idBookingList = UoW.Repository<InvitedGuest>().GetAll(i => i.GuestToken == bookingToken);
                    result.AddRange(idBookingList.Select(idBooking => UoW.Repository<Booking>().GetAllInclude(includeList, b => b.Id == idBooking.IdBooking).FirstOrDefault()));
                    break;
            }

            return result;
        }

        private async Task<PaginationResult<Booking>> GetBookingListPaged(int currentpage, int pageSize, string bookingToken, List<string> includeList)
        {
            var result = new PaginationResult<Booking>();
            var bookingtype = GetType(bookingToken);
            switch (bookingtype)
            {
                case OrderTypeEnum.CommonBooking:
                    result = await UoW.Repository<Booking>().GetAllpagedAsync(currentpage, pageSize,b => b.ReservationToken == bookingToken);
                    break;
                case OrderTypeEnum.GuestBooking:
                    var idBookingList = await UoW.Repository<InvitedGuest>().GetAllpagedAsync(currentpage, pageSize, i => i.GuestToken == bookingToken);                   
                    ((List<Booking>)result.Results).AddRange(idBookingList.Results.Select(idBooking => UoW.Repository<Booking>().GetAllInclude(includeList, b => b.Id == idBooking.IdBooking).FirstOrDefault()));                    
                    break;
            }

            return result;
        }

        private OrderTypeEnum GetType(string bookingToken)
        {
            return bookingToken.StartsWith("CB") ? OrderTypeEnum.CommonBooking : OrderTypeEnum.GuestBooking;
        }

        private async Task<List<InvitedGuest>> GetInvitedFromToken(string token)
        {
            var includeList = new List<string>
            {
                "IdBookingNavigation","IdBookingNavigation.Order","Order.OrderLine.IdDishNavigation", "Order.OrderLine.IdOrderNavigation","Order.OrderLine.OrderDishExtraIngredient","Order.OrderLine.OrderDishExtraIngredient.IdIngredientNavigation"
            };

            return (string.IsNullOrEmpty(token)
                ? await UoW.Repository<InvitedGuest>().GetAllIncludeAsync(includeList)
                : await UoW.Repository<InvitedGuest>().GetAllIncludeAsync(includeList, o => o.GuestToken.ToLower().Contains(token.ToLower()))) as List<InvitedGuest>;
        }

        private EmailResponseDto GetInvitedguestFullResponse(InvitedGuest invitedGuest)
        {
            if (invitedGuest == null) return new EmailResponseDto();
            return new EmailResponseDto
            {
                id = invitedGuest.Id,
                revision = 0,
                modificationCounter = 0,
                email = invitedGuest.Email,
                accepted = invitedGuest.Accepted ?? false,
                guestToken = invitedGuest.GuestToken,
                bookingId = invitedGuest.IdBooking,
                modificationDate = invitedGuest.ModificationDate != null ? JsonConvert.SerializeObject(invitedGuest.ModificationDate.Value.ConvertDateTimeToMilliseconds()) : string.Empty
            };
        }

        private OrderFullResponse GetOrderFullResponse(InvitedGuest invitedGuest, long orderId)
        {
            if (invitedGuest == null) return new OrderFullResponse { id = orderId };
            return new OrderFullResponse
            {
                id = orderId,
                revision = null,
                modificationCounter = 0,
                bookingToken = invitedGuest.GuestToken,
                bookingId = invitedGuest.IdBooking,
                hostId = null,
                invitedGuestId = invitedGuest.Id
            };
        }


        private List<OrderSearchFullResponseDto> GetOrderSearchFullResponseDtoList(Booking booking, ICollection<Order> invitedItemOrder)
        {
            var result = new List<OrderSearchFullResponseDto>();
            try
            {
                foreach (var order in invitedItemOrder)
                {
                    result.Add(new OrderSearchFullResponseDto
                    {
                        booking = Mapper.Map<BookingResponseDto>(booking),
                        invitedGuest = GetInvitedguestFullResponse(null),
                        order = GetOrderFullResponse(null, order.Id),
                        orderLines = Mapper.Map<List<OrderlineFullResponse>>(order.OrderLine),
                        host = new HostFullResponse()
                    });
                }
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message} : {ex.InnerException}";
            }

            return result;
        }


        private Booking CreateReservation(int bookingDtoType, string reservationDtoName, int reservationAssistants, string reservationDtoEmail, DateTime reservationDtoDate,
            long? userId)
        {
            var bookingTypeStringToken = bookingDtoType == 0
                ? BookingTypeConst.CommonBooking
                : BookingTypeConst.GuestBooking;

            var reservation = new Booking
            {
                Name = reservationDtoName,
                IdBookingType = bookingDtoType,
                BookingDate = Convert.ToDateTime(reservationDtoDate),
                Canceled = false,
                CreationDate = DateTime.Now,
                UserId = userId,
                ReservationToken = GetReservationToken(bookingTypeStringToken, reservationDtoEmail),
                Assistants = reservationAssistants,
                ExpirationDate = Convert.ToDateTime(reservationDtoDate).AddHours(-1),
                Email = reservationDtoEmail
            };

            UoW.Repository<Booking>().Create(reservation);
            UoW.Commit();
            return reservation;
        }

        private string GetReservationToken(string reservationTypeString, string userEmail)
        {
            //{(CRS_|GRS_)}{now.Year}{now.Month:00}{now.Day:00}{_}{MD5({Guest-email}{now.Year}{now.Month:00}{now.Day:00}{now.Hour:00}{now.Minute:00}{now.Second:00})}
            var now = DateTime.Now;
            var md5Token = $"{userEmail}{now.Year}{now.Month:00}{now.Day:00}{now.Hour:00}{now.Minute:00}{now.Second:00}".ToMD5();
            return $"{reservationTypeString}_{now.Year}{now.Month:00}{now.Day:00}_{md5Token.ToLower()}".Replace("-",
                string.Empty);
        }

        private List<InvitedGuest> CreateInvitationGuests(long bookingId, IList<string> guestList, string userEmail)
        {
            var result = new List<InvitedGuest>();

            foreach (var mail in guestList.Distinct())
            {
                var item = new InvitedGuest()
                {
                    IdBooking = bookingId,
                    Email = mail,
                    Accepted = null,
                    ModificationDate = DateTime.Now,
                    GuestToken = GetReservationToken(BookingTypeConst.GuestBooking, userEmail)

                };

                UoW.Repository<InvitedGuest>().Create(item);
                result.Add(item);
            }
            UoW.Commit();

            return result;
        }

        #endregion
    }
}
