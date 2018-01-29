using Oasp4Net.Business.Views.Views;

namespace Oasp4Net.Business.Views.Converters
{
    /// <summary>
    /// Sample to show how to map an entity to API View.
    /// Similar to automapper  function
    /// </summary>
    public class EntityConverter
    {
        /// <summary>
        /// Transforms entity object to view object
        /// </summary>
        /// <param name="item">Entity item to be transformed to api view</param>
        /// <returns>API view</returns>
        public static EntityView EntityToApi(string item)
        {
            if (item == null) return null;

            return new EntityView
            {
                Id = 1,
                Description = string.Empty, //should be item.Description...
                Name = string.Empty,

            };
            
        }

    }
}
