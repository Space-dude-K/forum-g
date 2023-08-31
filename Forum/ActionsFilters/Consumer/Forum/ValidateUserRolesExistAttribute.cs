using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Forum.ActionsFilters.Consumer.Forum
{
    public class ValidateUserRolesExistAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryApiManager _repositoryApiManager;
        private readonly ILoggerManager _logger;
        public ValidateUserRolesExistAttribute(IRepositoryApiManager repositoryApiManager, ILoggerManager logger)
        {
            _repositoryApiManager = repositoryApiManager;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbRoles = await _repositoryApiManager.ForumUserApis.GetUserRoles();

            if (dbRoles == null || dbRoles.Count == 0)
            {
                _logger.LogError($"Db roles is empty");
                await next();
            }
            else
            {
                context.HttpContext.Items.Add("dbRoles", dbRoles);
                await next();
            }
        }
    }
}