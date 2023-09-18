using System;
using BlogAPI.Interfaces;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogAPI
{
    /*
    Action attribute to verify that request ID matches JWT-encoded identity.
    */
    public class AuthenticationFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var accountService = (IAccountService) context.HttpContext.RequestServices.GetService(typeof(IAccountService));
            var id = Int32.Parse(context.HttpContext.GetRouteValue("id").ToString());
            var authenticated = await accountService.ResolveUser(id);

            if (!authenticated)
            {   
                context.Result = new UnauthorizedObjectResult("Access Denied");
            }
            else
            {
                await next();
            }
        }
    }
}