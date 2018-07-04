using AutoMapper;
using Newtonsoft.Json;
using OASP4Net.Business.Common.BookingManagement.Dto;
using OASP4Net.Business.Common.DishManagement.Dto;
using OASP4Net.Business.Common.MailManagement.Dto;
using OASP4Net.Business.Common.OrderManagement.Dto;
using OASP4Net.Domain.Entities.Models;
using OASP4Net.Infrastructure.Extensions;
using System;
using System.Linq;

namespace OASP4Net.Business.Common
{
    public class AutomapperProfile : Profile 
    {
        public AutomapperProfile()
        {
            #region Dish mappings
            
            CreateMap<Image, ImageDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<DishIngredient, ExtraDto>()
                .ForPath(dest => dest.description, opt => opt.MapFrom(src => src.IdIngredientNavigation.Description))
                .ForPath(dest => dest.price, opt => opt.MapFrom(src => src.IdIngredientNavigation.Price))
                .ForPath(dest => dest.name, opt => opt.MapFrom(src => src.IdIngredientNavigation.Name));            
            CreateMap<Dish, DishDtoResult>()
                .ForMember(dest => dest.dish, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.image, opt => opt.MapFrom(src => src.IdImageNavigation))
                .ForPath(dest => dest.dish.imageId, opt => opt.MapFrom(src => src.IdImageNavigation.Id))
                .ForMember(dest => dest.categories, opt => opt.MapFrom(src => src.DishCategory.Select(d => d.IdCategoryNavigation)))                
                .ForMember(dest => dest.extras, opt => opt.MapFrom(src => src.DishIngredient.Select(d => d.IdIngredientNavigation)));
            CreateMap<Dish, DishDto>();

            CreateMap<OrderDishExtraIngredient, Extra>()
                .ForMember(dest => dest.description, opt => opt.MapFrom(src => src.IdIngredientNavigation.Description))
                .ForMember(dest => dest.name, opt => opt.MapFrom(src => src.IdIngredientNavigation.Name))
                .ForMember(dest => dest.price, opt => opt.MapFrom(src => src.IdIngredientNavigation.Price));

            CreateMap<Extra, Ingredient>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.description))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.price));

            #endregion

            #region Order mappings

            CreateMap<Order, OrderSearchDto>()
                .ForMember(dest => dest.bookingToken, opt => opt.MapFrom(src => src.IdReservationNavigation.ReservationToken))
                .ForMember(dest => dest.bookingId, opt => opt.MapFrom(src => src.IdReservationNavigation.Id));

            CreateMap<OrderLine, OrderlineFullResponse>()
                .ForPath(dest => dest.dish.id, opt => opt.MapFrom(src => src.IdDishNavigation.Id))
                .ForPath(dest => dest.dish.name, opt => opt.MapFrom(src => src.IdDishNavigation.Name))
                .ForPath(dest => dest.dish.description, opt => opt.MapFrom(src => src.IdDishNavigation.Description))
                .ForPath(dest => dest.dish.price, opt => opt.MapFrom(src => src.IdDishNavigation.Price != null? decimal.Round(src.IdDishNavigation.Price.Value, 2, MidpointRounding.AwayFromZero) : 0))
                .ForPath(dest => dest.extras, opt => opt.MapFrom(src => src.OrderDishExtraIngredient))
                .ForPath(dest => dest.orderLine, opt => opt.MapFrom(src =>
                new OrderlineDetailFullResponse
                {
                    id = src.Id.Value,
                    revision = 0,
                    modificationCounter = 0,
                    orderId = src.IdOrder,
                    amount =src.Amount ?? 0,
                    comment = src.Comment,
                    dishId = src.IdDish
                }));

            CreateMap<OrderDishExtraIngredient, Extra>()                
                .ForPath(dest => dest.id, opt => opt.MapFrom(src => src.IdIngredient))                
                .ForPath(dest => dest.description, opt => opt.MapFrom(src => src.IdIngredientNavigation.Description))
                .ForPath(dest => dest.name, opt => opt.MapFrom(src => src.IdIngredientNavigation.Name))
                .ForPath(dest => dest.price, opt => opt.MapFrom(src => src.IdIngredientNavigation.Price));
            CreateMap<Extra, OrderDishExtraIngredient>()
                .ForPath(dest => dest.IdIngredient, opt => opt.MapFrom(src => src.id))            
                .AfterMap((src, dest) => dest.IdOrderLine = 0);

            CreateMap<OrderlineDto, OrderLine>()
                .ForPath(dest => dest.IdDish, opt => opt.MapFrom(src => src.orderLine.dishId))
                .ForPath(dest => dest.Amount, opt => opt.MapFrom(src => src.orderLine.amount))
                .ForPath(dest => dest.Comment, opt => opt.MapFrom(src => src.orderLine.comment))
                .ForMember(dest => dest.OrderDishExtraIngredient, opt => opt.MapFrom(src => src.extras));

            #endregion

            #region Booking mappings

            CreateMap< Booking, BookingResponseDto>()
                .ForPath(dest => dest.Canceled, opt => opt.MapFrom(src => src.Canceled ?? false))
                .ForPath(dest => dest.Comment, opt => opt.MapFrom(src => src.Comments))
                .ForPath(dest => dest.BookingToken, opt => opt.MapFrom(src => src.ReservationToken))
                .ForPath(dest => dest.BookingType, opt => opt.MapFrom(src => src.IdBookingType!= null ? src.IdBookingType.Value.ToString() : "0"))
                .ForPath(dest => dest.BookingDate, opt => opt.MapFrom(src => JsonConvert.SerializeObject(src.BookingDate.ConvertDateTimeToMilliseconds())))
                .ForPath(dest => dest.ExpirationDate, opt => opt.MapFrom(src => src.ExpirationDate != null
                    ? JsonConvert.SerializeObject(src.ExpirationDate.Value.ConvertDateTimeToMilliseconds())
                    : string.Empty))
                .ForPath(dest => dest.CreationDate, opt => opt.MapFrom(src => src.CreationDate != null ?
                JsonConvert.SerializeObject(src.CreationDate.Value.ConvertDateTimeToMilliseconds()) : string.Empty));

            CreateMap<InvitedGuest, EmailResponseDto>()
                .ForPath(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.guestToken, opt => opt.MapFrom(src => src.GuestToken))
                .ForPath(dest => dest.bookingId, opt => opt.MapFrom(src => src.IdBooking))
                .ForPath(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.modificationDate, opt => opt.MapFrom(src => src.ModificationDate != null
                    ? JsonConvert.SerializeObject(src.ModificationDate.Value.ConvertDateTimeToMilliseconds())
                    : string.Empty))
                .ForPath(dest => dest.accepted, opt => opt.MapFrom(src => src.Accepted.Value))
                ;

            CreateMap<UserSearchDto, User>()
                .ForPath(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForPath(dest => dest.Email, opt => opt.MapFrom(src => src.email))
                .ForPath(dest => dest.IdRole, opt => opt.MapFrom(src => src.userRoleId))
                .ForPath(dest => dest.UserName, opt => opt.MapFrom(src => src.username));
            
            #endregion

            #region Invited guest mappings
            CreateMap<InvitedGuest, InvitedguestSearchDto>()
                .ForPath(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.email, opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.guestToken, opt => opt.MapFrom(src => src.GuestToken))
                .ForPath(dest => dest.bookingId, opt => opt.MapFrom(src => src.IdBooking))
                .ForPath(dest => dest.modificationDate, opt => opt.MapFrom(src => src.ModificationDate != null ?
                    JsonConvert.SerializeObject(src.ModificationDate.Value.ConvertDateTimeToMilliseconds()) : string.Empty))
                .ForPath(dest => dest.accepted, opt => opt.MapFrom(src => src.Accepted ?? false));

            #endregion
        }
    }
}
