using System;
using BlogAPI.Interfaces;
using BlogAPI.Models;
using BlogAPI.Dtos;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BlogAPI
{
    //public abstract class AuthenticationFilterBase<T> : TypeFilterAttribute where T : class, IDto
    //{
    //    private readonly ActionExecutingContext _context;
    //    private readonly ActionExecutionDelegate _next;
    //    
    //    public AuthenticationFilterBase(ActionExecutingContext context, ActionExecutionDelegate next) : base(typeof())
    //    {
    //        _context = context;
    //        _next = next;
    //    }

    //    public abstract Task OnActionExecutionAsync();
    //}

    //public class VerifyPostAccessAttribute : AuthenticationFilterBase<PostWriteDto>
    //{
    //    private readonly ActionExecutingContext _context;
    //    private readonly ActionExecutionDelegate _next;
    //    
    //    public VerifyPostAccessAttribute(ActionExecutingContext context, ActionExecutionDelegate next) : base(context, next)
    //    {
    //        _context = context;
    //        _next = next;
    //    }

    //    public override async Task OnActionExecutionAsync()
    //    {
    //        var accountService = (IAccountService) _context.HttpContext.RequestServices.GetService(typeof(IAccountService));
    //        var id = Int32.Parse(_context.HttpContext.GetRouteValue("userId").ToString());
    //        var authenticated = await accountService.ResolveUser(id);

    //        if (!authenticated)
    //        {
    //            // do what ??
    //        }
    //        else
    //        {
    //            await _next();
    //        }
    //    }
    //}
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