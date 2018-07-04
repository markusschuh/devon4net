using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using OASP4Net.Business.Common.DishManagement.Dto;
using OASP4Net.Domain.Entities;
using OASP4Net.Domain.Entities.Models;
using OASP4Net.Domain.UnitOfWork.Pagination;
using OASP4Net.Domain.UnitOfWork.Service;
using OASP4Net.Domain.UnitOfWork.UnitOfWork;
using OASP4Net.Infrastructure.Extensions;

namespace OASP4Net.Business.Common.DishManagement.Service
{
    public class DishService: Service<ModelContext>, IDishService
    {        
        private IMapper Mapper { get; set; }
        public DishService(IUnitOfWork<ModelContext> unitOfWork, IMapper mapper) : base(unitOfWork)
        {
            Mapper = mapper;
        }

        public async Task<PaginationResult<Dish>> GetpagedDishListFromFilter(int currentpage, int pageSize, bool isFav, decimal maxPrice, int minLikes, string searchBy, IList<long> categoryIdList, long userId)
        {
            var includeList = new List<string>{"DishCategory","DishCategory.IdCategoryNavigation", "DishIngredient","DishIngredient.IdIngredientNavigation","IdImageNavigation"};
            var dishPredicate = PredicateBuilder.True<Dish>();

            if (!string.IsNullOrEmpty(searchBy))
            {
                var criteria = searchBy.ToLower();
                dishPredicate = dishPredicate.And(d => d.Name.ToLower().Contains(criteria) || d.Description.ToLower().Contains(criteria));
            }
            if (maxPrice > 0) dishPredicate = dishPredicate.And(d=>d.Price<=maxPrice);

            if (categoryIdList.Any())
            {
                dishPredicate = dishPredicate.And(r => r.DishCategory.Any(a => categoryIdList.Contains(a.IdCategory)));
            }
            if (isFav && userId >= 0)
            {
                var favourites = await UoW.Repository<UserFavourite>().GetAllAsync(w=>w.IdUser == userId);
                var dishes = favourites.Select(s => s.IdDish);
                dishPredicate = dishPredicate.And(r=> dishes.Contains(r.Id));                
            }
            return await UoW.Repository<Dish>().GetAllIncludePagedAsync(currentpage, pageSize, includeList, dishPredicate);

        }

        /// <summary>
        /// PLease use this method as an example of how to use pagination
        /// </summary>
        /// <param name="isFav"></param>
        /// <param name="maxPrice"></param>
        /// <param name="minLikes"></param>
        /// <param name="searchBy"></param>
        /// <param name="categoryIdList"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<DishDtoResult>> GetDishListFromFilter(bool isFav, decimal maxPrice, int minLikes, string searchBy, IList<long> categoryIdList, long userId)
        {
            var includeList = new List<string> { "DishCategory", "DishCategory.IdCategoryNavigation", "DishIngredient", "DishIngredient.IdIngredientNavigation", "IdImageNavigation" };
            var dishPredicate = PredicateBuilder.True<Dish>();

            if (!string.IsNullOrEmpty(searchBy))
            {
                var criteria = searchBy.ToLower();
                dishPredicate = dishPredicate.And(d => d.Name.ToLower().Contains(criteria) || d.Description.ToLower().Contains(criteria));
            }
            if (maxPrice > 0) dishPredicate = dishPredicate.And(d => d.Price <= maxPrice);

            if (categoryIdList.Any())
            {
                dishPredicate = dishPredicate.And(r => r.DishCategory.Any(a => categoryIdList.Contains(a.IdCategory)));
            }
            if (isFav && userId >= 0)
            {
                var favourites = await UoW.Repository<UserFavourite>().GetAllAsync(w => w.IdUser == userId);
                var dishes = favourites.Select(s => s.IdDish);
                dishPredicate = dishPredicate.And(r => dishes.Contains(r.Id));
            }

            var result = await UoW.Repository<Dish>().GetAllIncludeAsync(includeList, dishPredicate);
            return Mapper.Map<List<DishDtoResult>>(result);
        }
    }
}
