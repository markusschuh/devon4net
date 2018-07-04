using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OASP4Net.Business.Common.DishManagement.Dto;
using OASP4Net.Business.Common.DishManagement.Service;
using OASP4Net.Infrastructure.MVC.Controller;
using OASP4Net.Infrastructure.MVC.Dto;

namespace OASP4Net.Business.Common.DishManagement.Controller
{
    /// <summary>
    /// DishController
    /// </summary>
    [EnableCors("CorsPolicy")]
    public class DishController : OASP4NetController
    {
        private readonly IDishService _dishService;

        public DishController(IDishService service, ILogger<DishController> logger) : base (logger)
        {            
            _dishService = service;            
        }

        /// <summary>
        /// Gets the  list of available dishes regarding the filter options.
        /// if you want tu use pagination, please use some code like this:        
        /// //Paged treatment
        /// //var pagedResult = await _dishService.GetpagedDishListFromFilter(PageNumber, PageSize, false, maxPrice, Convert.ToInt32(minLikes), searchBy, categories.Select(c => c.Id).ToList(), -1);
        /// //result.Result = pagedResult.Results.Select(DishConverter.EntityToApi).ToList();
        /// //result.Pagination.Total = pagedResult.Results.Count();
        /// </summary>
        /// <param name="filterDto">Contains the criteria values to perform the search. Case of null or empty values will return the full set of dishes.</param>
        /// <returns></returns>
        /// <response code="200"> Ok. The search process has beencompleted with no error.</response>
        /// <response code="401">Unathorized. Autentication fail</response>  
        /// <response code="403">Forbidden. Authorization error.</response>    
        /// <response code="500">Internal Server Error. The search process ended with error.</response>       
        [HttpPost]
        [HttpOptions]
        [AllowAnonymous]
        [Route("/mythaistar/services/rest/Dishmanagement/v1/Dish/Search")]
        [EnableCors("CorsPolicy")]
        public async Task<IActionResult> Search([FromBody] FilterDtoSearchObject filterDto)
        {
            if (filterDto == null) filterDto = new FilterDtoSearchObject();

            try
            {
                var dishList = await _dishService.GetDishListFromFilter(false, filterDto.GetMaxPrice(), filterDto.GetMinLikes(), filterDto.GetSearchBy(),filterDto.GetCategories(), -1);
                return new OkObjectResult(GenerateResultDto(dishList).ToJson());
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, $"{ex.Message} : {ex.InnerException}");
            }
        }
    }
}
