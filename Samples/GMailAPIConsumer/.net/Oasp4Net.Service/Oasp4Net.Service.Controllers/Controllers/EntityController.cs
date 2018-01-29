using System.Collections.Generic;
using System.Web.Http;


namespace Oasp4Net.Service.Controllers.Controllers
{
    /// <summary>
    /// Management of Dish entity
    /// </summary>
    public class EntityController : ApiController
    {
        /// <summary>
        /// Gets the full list of available entites
        /// </summary>
        /// <returns></returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="200">ok</response>
        [HttpGet]
        public List<string> Get()
        {
            //Example
            //IEntityRepository entityRepo = new EntityRepository();
            //var entityList = entityRepo.FindAll().ToList();
            //return entityList.ConvertAll(DishConverter.EntityToApi);
            return new List<string>();

        }
    }
}
