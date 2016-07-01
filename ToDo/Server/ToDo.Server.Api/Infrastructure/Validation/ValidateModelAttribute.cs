namespace ToDo.Api.Infrastructure.Validation
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;
    using Server.Common.Constants;

    [AttributeUsage(AttributeTargets.Method)]
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.ActionArguments.Any(p => p.Value == null))
            {
                actionContext.ModelState.AddModelError(string.Empty, MessageConstants.EmptyRequest);
            }

            if (!actionContext.ModelState.IsValid)
            {
                var error = actionContext
                    .ModelState
                    .Values
                    .First()
                    .Errors
                    .First()
                    .ErrorMessage;

                actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, error);
            }
        }
    }
}