using System.Collections.Generic;
using System.Linq;
using Oasp4net.DataAccessLayer;
using Oasp4net.DataAccessLayer.Common.Implementation;
using Oasp4Net.Business.Repositories.EntityManagement.Interfaces;

namespace Oasp4Net.Business.Repositories.EntityManagement.Implementation
{


    /// <summary>
    /// Sample to show how to implement repository pattern
    /// </summary>
    //public class EntityRepository : Repository<DataBaseEntities, Entity>, IEntityRepository
    //{

    //    public List<Entity> GetEntityListFromFilter(bool isFav, decimal maxPrice, int minLikes, string searchBy, List<long> categoryIdList)
    //    {

    //        //todo : Generic Lambda expressions/Linq?

    //        var result = new List<Entity>();

    //        var dishList = (from c in Context.Dish
    //            where c.Price <= maxPrice
    //            || categoryIdList.Contains(c.DishCategory.Select(t=>t.Id).FirstOrDefault())
    //            || c.Name.ToLower().Contains(searchBy.ToLower())
    //        select c).ToList();

    //        return result;
    //    }
}
