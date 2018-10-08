using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Devon4Net.Infrastructure.Log.Attribute
{
    public class AopControllerAttribute : ActionFilterAttribute
    {
        private bool UseAopObjectTrace { get; set; }

        public AopControllerAttribute(bool useAop)
        {
            UseAopObjectTrace = useAop;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {                
            try
            {
                var controllerValues = GetControllerProperties((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor, context.ActionArguments);
                Devon4Net.Infrastructure.Log.Devon4NetLogger.Information($"Action: OnActionExecuting | Controller: {controllerValues.ControllerName} | method: {controllerValues.ControllerMethod}| ActionArguments: {controllerValues.ActionArguments}");
                base.OnActionExecuting(context);
            }
            catch (Exception ex)
            {
                Devon4Net.Infrastructure.Log.Devon4NetLogger.Error(ex);
                throw ex;
            }
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            try
            {
                if (context.Result!=null) LogObjectResult(context.Result as Microsoft.AspNetCore.Mvc.ObjectResult);
                base.OnActionExecuted(context);
            }
            catch (Exception ex)
            {
                Devon4Net.Infrastructure.Log.Devon4NetLogger.Error(ex);
                throw ex;
            }
        }

        public override void OnResultExecuting(ResultExecutingContext context)
        {
            try
            {                
                base.OnResultExecuting(context);
            }
            catch (Exception ex)
            {
                Devon4Net.Infrastructure.Log.Devon4NetLogger.Error(ex);
                throw ex;
            }
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            try
            {
                if (context.Result != null) LogObjectResult(context.Result as Microsoft.AspNetCore.Mvc.ObjectResult);
                base.OnResultExecuted(context);
            }
            catch (Exception ex)
            {
                Devon4Net.Infrastructure.Log.Devon4NetLogger.Error(ex);
                throw ex;
            }
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            try
            {
                var controllerValues = GetControllerProperties((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)context.ActionDescriptor, context.ActionArguments);
                Devon4Net.Infrastructure.Log.Devon4NetLogger.Information($"Action: OnActionExecutionAsync | Controller: {controllerValues.ControllerName} | method: {controllerValues.ControllerMethod}| ActionArguments: {controllerValues.ActionArguments}");

                return base.OnActionExecutionAsync(context, next);
            }
            catch (Exception ex)
            {
                Devon4Net.Infrastructure.Log.Devon4NetLogger.Error(ex);
                throw ex;
            }
        }

        public override Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            try
            {
                if (context.Result != null) LogObjectResult(context.Result as Microsoft.AspNetCore.Mvc.ObjectResult);
                return base.OnResultExecutionAsync(context, next);
            }
            catch (Exception ex)
            {
                Devon4Net.Infrastructure.Log.Devon4NetLogger.Error(ex);
                throw ex;
            }
        }

        private void LogObjectResult(Microsoft.AspNetCore.Mvc.ObjectResult result)
        {
            Devon4Net.Infrastructure.Log.Devon4NetLogger.Debug($"Result: {result.StatusCode} | Value: {result.Value}");
        }

        #region prettyprint
        private string PrettyPrint(object toPrint, string paramName = "", string separator = "\n", string prefix = "")
        {
            var sb = new StringBuilder(string.IsNullOrEmpty(paramName) ? string.Empty : prefix + paramName + ": ");
            if (toPrint == null)
            {
                sb.AppendFormat("null");
                return sb.ToString();
            }

            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.NonPublic;
            var properties = toPrint.GetType().GetProperties(flags);

            // basic type or struct
            if (!properties.Any() || IsBasic(toPrint))
            {
                sb.AppendFormat(prefix + toPrint + separator);
            }
            else if (IsEnumerable(toPrint))
            {
                var valueToPrint = separator;
                var values = (IEnumerable)toPrint;
                valueToPrint = values.Cast<object>().Aggregate(valueToPrint, (current, o) => current + (PrettyPrint(o, separator: ", ", prefix: prefix + "")));
                sb.AppendFormat(valueToPrint);
            }
            else
            {
                foreach (PropertyInfo info in properties.Where(info => info.PropertyType.Namespace != null && (info.Name != "SyncRoot" /*&& !info.PropertyType.Namespace.StartsWith("System")*/)))
                {
                    sb.AppendFormat("\t\n{0}{1}{2}", prefix, PrettyPrint(info.GetValue(toPrint, null), info.Name, prefix = prefix + ""), separator);
                }
            }

            return sb.ToString();
        }

        private bool IsBasic(object toPrint)
        {

            return toPrint.GetType().IsPrimitive || toPrint is DateTime || toPrint is string;
        }

        private bool IsEnumerable(Object obj)
        {
            return obj.GetType().IsGenericType && obj.GetType().GetInterfaces().Any(iface => iface.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
        #endregion

        private AopController GetControllerProperties(Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor actionDescriptor, IDictionary<string, object> actionArguments)
        {
            return new AopController {
                ControllerName = actionDescriptor.ControllerName,
                ControllerMethod = actionDescriptor.MethodInfo.Name,
                ActionArguments = UseAopObjectTrace ? PrettyPrint(actionArguments) : "AOP Arguments not enabled!"
            }; 
        }

    }
}
