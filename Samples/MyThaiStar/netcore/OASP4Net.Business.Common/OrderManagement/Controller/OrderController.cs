using System;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using OASP4Net.Business.Common.BookingManagement.Dto;
using OASP4Net.Business.Common.BookingManagement.Service;
using OASP4Net.Business.Common.OrderManagement.Dto;
using OASP4Net.Business.Common.OrderManagement.Service;
using OASP4Net.Infrastructure.MVC.Controller;

namespace OASP4Net.Business.Common.OrderManagement.Controller
{
    [EnableCors("CorsPolicy")]
    public class OrderController : OASP4NetController
    {        
        private IOrderService _orderService;
        private readonly IBookingService _bookingService;

        public OrderController(IOrderService orderService, IBookingService bookingService, ILogger<OrderController> logger, IMapper mapper) : base(logger,mapper)
        {
            _orderService = orderService;
            _bookingService = bookingService;            
        }

        /// <summary>
        /// Gets the  list of available orders regarding the filter options
        /// </summary>
        /// <param name="orderFilterDto">Contains the filter values to perform the search. Case of null or empty values will return the full set of orders.</param>
        /// <response code="200">Ok. The search process has beencompleted with no error.</response>
        /// <response code="401">Unathorized. Autentication fail</response>
        /// <response code="403">Forbidden. Authorization error.</response>
        /// <response code="500">Internal Server Error. The search process ended with error.</response>
        [HttpPost]
        [HttpPost]
        [HttpOptions]
        [Route("/mythaistar/services/rest/Ordermanagement/v1/order/filter")]
        [EnableCors("CorsPolicy")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "waiter")]
        public virtual async Task<IActionResult> OrderFilter([FromBody] BookingSearchDto orderFilterDto)
        {
            try
            {
                var result = GenerateResultDto(await _bookingService.GetBookingSearch(orderFilterDto.bookingToken, orderFilterDto.email));                
                return new OkObjectResult(result.ToJson());
            }
            catch (Exception ex)
            {
                var content = StatusCode((int)HttpStatusCode.BadRequest, $"{ex.Message} : {ex.InnerException}");
                return Content(GetJsonFromObject(content), "application/json");
            }
        }


        /// <summary>
        /// Order the order. Given an order Dto, the server side will prepare the different order lines.
        /// </summary>
        /// <param name="orderDto">The model of the ordert to be processed on the server side. The InvitationToken field is mandatory.</param>
        /// <response code="201">Ok. Created. Returns the created order reference</response>
        /// <response code="400">Bad Request. No Invitation token given.</response>
        /// <response code="401">Unathorized. Autentication fail</response>
        /// <response code="403">Forbidden. Authorization error.</response>
        /// <response code="500">Internal Server Error. The search process ended with error.</response>
        [HttpPost]
        [HttpOptions]
        [Route("/mythaistar/services/rest/ordermanagement/v1/order")]
        [EnableCors("CorsPolicy")]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "waiter")]
        public virtual IActionResult OrderOrder([FromBody]OrderDto orderDto)
        {
            try
            {
                _orderService.OrderTheOrder(orderDto.booking.bookingToken, orderDto.orderLines);
                return Ok();
            }
            catch (Exception ex)
            {
                var content = StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message} : {ex.InnerException}");
                return Content(GetJsonFromObject(content), "application/json");
            }
        }


        /// <summary>
        /// Gets the  list of available orders regarding the search criteria options
        /// </summary>
        /// <param name="orderSearchCriteriaDto">Contains the criteria values to perform the search. Case of null or empty values will return the full set of orders.</param>
        /// <response code="200">Ok. The search process has beencompleted with no error.</response>
        /// <response code="401">Unathorized. Autentication fail</response>
        /// <response code="403">Forbidden. Authorization error.</response>
        /// <response code="500">Internal Server Error. The search process ended with error.</response>
        [HttpPost]
        [HttpOptions]
        [Route("/mythaistar/services/rest/Ordermanagement/v1/order/search")]
        [EnableCors("CorsPolicy")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "waiter")]
        public virtual async Task<IActionResult> OrderSearch([FromBody] BookingSearchDto orderSearchCriteriaDto)
        {
            try
            {
                var result = GenerateResultDto(await _bookingService.GetBookinSearchFullResponse(orderSearchCriteriaDto.bookingToken, orderSearchCriteriaDto.email));                
                return new OkObjectResult(result.ToJson());
            }
            catch (Exception ex)
            {
                var content = StatusCode((int)HttpStatusCode.BadRequest, $"{ex.Message} : {ex.InnerException}");
                return Content(GetJsonFromObject(content), "application/json");
            }
        }
    }
}
