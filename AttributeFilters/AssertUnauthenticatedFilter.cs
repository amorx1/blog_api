
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogApi
{
    public class AssertUnauthenticatedFilterAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var stream = context.HttpContext.Request.Headers["Authorization"].ToString();
            if (stream != String.Empty)
            {
                context.Result = new BadRequestObjectResult("Already authenticated. Try clearing API token first.");
            }
            else
            {
                await next();
            }
        }
    }
}