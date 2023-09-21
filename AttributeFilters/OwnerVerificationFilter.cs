using BlogAPI.Services;
using BlogAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogAPI.AttributeFiters
{
	public class OwnerVerificationFilterAttribute : ActionFilterAttribute
	{
		public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			var accountService = (IAccountService) context.HttpContext.RequestServices.GetService(typeof(IAccountService));
            var id = Int32.Parse(context.HttpContext.GetRouteValue("userId").ToString());
            var isOwner = await accountService.ResolveUser(id);
			Console.WriteLine(isOwner);

			// context.HttpContext.Items.Add("isOwner", isOwner);
			context.ActionArguments.Add("isOwner", isOwner);

			await next();
		}
	}
}
