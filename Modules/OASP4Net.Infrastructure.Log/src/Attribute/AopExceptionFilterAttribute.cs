using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OASP4Net.Infrastructure.Log.Attribute
{
    public class AopExceptionFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            OASP4Net.Infrastructure.Log.OASP4NetLogger.Error(context.Exception);
            base.OnException(context);
        }

        public override Task OnExceptionAsync(ExceptionContext context)
        {
            OASP4Net.Infrastructure.Log.OASP4NetLogger.Error(context.Exception);
            return base.OnExceptionAsync(context);
        }
    }
}