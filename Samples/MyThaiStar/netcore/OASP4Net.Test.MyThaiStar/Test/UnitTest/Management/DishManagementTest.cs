using Microsoft.Extensions.Logging;
using Moq;
using OASP4Net.Business.Common.DishManagement.Service;
using OASP4Net.Business.Common.DishManagement.Controller;
using OASP4Net.Domain.Entities;
using OASP4Net.Domain.UnitOfWork.UnitOfWork;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using OASP4Net.Domain.Entities.Models;
using System.Threading.Tasks;
using OASP4Net.Domain.UnitOfWork.Pagination;
using Microsoft.EntityFrameworkCore;
using OASP4Net.Infrastructure.Extensions;
using System;
using System.Linq.Expressions;

namespace OASP4Net.Test.xUnit.Test.UnitTest.Management
{
    public class DishManagementTest : MyThaiStarUnitTest
    {
        public IDishService DishService { get; set; }
        public Mock<ILogger<DishController>> Logger { get; set; }
        public List<Dish> MockDishList { get; set; }

        public DishManagementTest()
        {
            var dishPredicate = PredicateBuilder.True<Dish>();           
            var context = new Mock<ModelContext>(new DbContextOptions<ModelContext>());
            var MockUoW = new Mock<UnitOfWork<ModelContext>>(context.Object);

            Logger = new Mock<ILogger<DishController>>();
            MockDishList = new List<Dish> { new Dish { Id = 1, Name = "Garlic Paradise", Description= "From the world-famous Gilroy Garlic Festival to a fierce 40-clove garlic chicken in San Francisco and a gut-busting garlic sandwich in Philly, we feature the tastiest places to get your garlic on.",Price = Convert.ToDecimal("12.99"), IdImage =1 } };            

            var result = new PaginationResult<Dish>() { Results = MockDishList,CurrentPage= 1,PageCount = 1,PageSize = 10, RowCount = 0};
                        
            MockUoW.Setup(m => m.Repository<Dish>().GetAllIncludePagedAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<List<string>>(), It.IsAny<Expression<Func<Dish, bool>>>())).Returns(Task.FromResult(result));
            MockUoW.Setup(m => m.Repository<Dish>().GetAllIncludeAsync(It.IsAny<List<string>>(), It.IsAny<Expression<Func<Dish, bool>>>())).Returns(Task.FromResult(result.Results));

            DishService = new DishService(MockUoW.Object, Mapper);
        }

        [Fact]
        public void FindAllDishes()
        {
            var res = DishService.GetDishListFromFilter(false, 0, 0, "", new List<long>(), -1);
            Assert.NotEmpty(res.Result);
        }

        [Fact]
        public async Task FilterDishesAsync()
        {            
            var res = await DishService.GetDishListFromFilter(false, 0, 0, "", new List<long>(), -1);
            Assert.NotNull(res);
            Assert.True(res.Count> 0);
            Assert.Equal(1, res.FirstOrDefault().dish.id);
        }
    }
}