using Microsoft.Extensions.Logging;
using Moq;
using OASP4Net.Business.Common.DishManagement.Service;
using OASP4Net.Business.Common.DishManagement.Controller;
using OASP4Net.Domain.Entities;
using OASP4Net.Domain.UnitOfWork.UnitOfWork;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace OASP4Net.Test.xUnit.Test.Integration
{
    public class DishManagementTest : MyThaiStarIntegrationTest
    {
        public DishService DishService { get; set; }
        public Mock<ILogger<DishController>> Logger { get; set; }
        public UnitOfWork<ModelContext> UoW { get; set; }

        public DishManagementTest()
        {
            Logger = new Mock<ILogger<DishController>>();
            UoW = new UnitOfWork<ModelContext>(Context);
            DishService = new DishService(UoW, Mapper);
        }


        [Fact]
        public void FindAllDishes()
        {
            var res = DishService.GetDishListFromFilter(false, 0, 0, "", new List<long>(), -1);
            Assert.NotEmpty(res.Result);
        }

        [Fact]
        public void FilterDishes()
        {
            var res = DishService.GetDishListFromFilter(false, 0, 0, "Garlic Paradise", new List<long>(), -1).Result;
            Assert.NotNull(res);
            Assert.True(res.Count > 0);
            Assert.Equal(1, res.FirstOrDefault().dish.id);
        }
    }
}
