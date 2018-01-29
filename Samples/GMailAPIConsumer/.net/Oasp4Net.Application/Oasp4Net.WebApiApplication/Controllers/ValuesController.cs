using System.Collections.Generic;
using System.Web.Http;

namespace Oasp4Net.Business.Controller.Controllers
{
    
    public class ValuesController : ApiController
    {
        /// <summary>
        /// Returns a specific value based on the ID you request
        /// </summary>
        /// <param name="id">The ID of the value you want to return</param>
        /// <returns>A value</returns>
        public string Get(int id)
        {
            return "value";
        }

        public int Post([FromBody]string value)
        {
            return 1;
        }

        public void Put(int id, [FromBody]string value)
        {
        }

        public void Delete(int id)
        {
        }
    }
}
