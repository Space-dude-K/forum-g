using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Entities.Models.Forum;

namespace Forum.ActionsFilters.Consumer.Forum
{
    public class ValidateAuthorizeAttribute : IAsyncActionFilter
    {
        private readonly ILoggerManager _logger;
        public ValidateAuthorizeAttribute(IRepositoryManager repository, ILoggerManager logger)
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
                _logger.LogInfo($"User id: {user.Identity.Name} is not authenticated.");
                context.Result = new UnauthorizedResult();

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