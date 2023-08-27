using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

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

            if (!user.Identity.IsAuthenticated)
            {
                _logger.LogInfo($"User id: {user.Identity.Name} is not authenticated.");
                context.Result = new UnauthorizedResult();

                return;
            }
            else
            {
                await next();
            }
        }
    }
}