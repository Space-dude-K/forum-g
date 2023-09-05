using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Entities.Models.Forum;
using System.Net;

namespace Forum.ActionsFilters.Consumer.Forum
{
    public class ValidateAuthenticationAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        public ValidateAuthenticationAttribute(ILoggerManager logger)
        {
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            int userId = 0;
            int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

            if (!user.Identity.IsAuthenticated)
            {
                _logger.LogInfo($"User authentication error.");

                var values = new RouteValueDictionary(new
                {
                    action = "Index",
                    controller = "Home",
                    exceptionText = "Авторизируйтесь"
                });
                context.Result = new RedirectToRouteResult(values);

                return;
            }
            else
            {
                context.HttpContext.Items.Add("userId", userId);
                await next();
            }
        }
    }
}