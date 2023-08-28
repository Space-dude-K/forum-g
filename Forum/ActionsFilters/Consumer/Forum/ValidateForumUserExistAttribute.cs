using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Interfaces.User;
using System.Security.Claims;

namespace Forum.ActionsFilters.Consumer.Forum
{
    public class ValidateForumUserExistAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryApiManager _repositoryApiManager;
        private readonly ILoggerManager _logger;
        public ValidateForumUserExistAttribute(IRepositoryApiManager repositoryApiManager, ILoggerManager logger)
        {
            _repositoryApiManager = repositoryApiManager;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            int userId = 0;
            int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var forumUser = await _repositoryApiManager.ForumUserApis.GetForumUser(userId);

            if (forumUser == null)
            {
                _logger.LogInfo($"Forum user id: {user.Identity.Name} not found.");
                context.Result = new UnauthorizedResult();

                return;
            }
            else
            {
                context.HttpContext.Items.Add("forumUser", forumUser);
                await next();
            }
        }
    }
}
