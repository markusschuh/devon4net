using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Devon4Net.Infrastructure.Swagger.Filters
{
    public class ConsumesOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var consumes = context.ApiDescription.ActionDescriptor.ActionConstraints.OfType<ConsumesAttribute>().FirstOrDefault();
            if (consumes != null)
            {
                operation.Consumes.Clear();
                foreach (var contentType in consumes.ContentTypes)
                {
                    operation.Consumes.Add(contentType);
                }
            }
        }
    }
}
