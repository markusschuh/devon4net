using System.Collections.Generic;
using System.Threading.Tasks;
using OASP4Net.Business.Common.DishManagement.Dto;
using OASP4Net.Domain.Entities.Models;
using OASP4Net.Domain.UnitOfWork.Pagination;

namespace OASP4Net.Business.Common.DishManagement.Service
{
    public interface IDishService
    {
        Task<List<DishDtoResult>> GetDishListFromFilter(bool isFav, decimal maxPrice, int minLikes, string searchBy, IList<long> categoryIdList, long userId);        
        Task<PaginationResult<Dish>> GetpagedDishListFromFilter(int currentpage, int pageSize, bool isFav, decimal maxPrice, int minLikes, string searchBy, IList<long> categoryIdList, long userId);
    }
}