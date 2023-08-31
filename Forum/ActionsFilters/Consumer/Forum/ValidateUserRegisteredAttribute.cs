using Entities.ViewModels;
using Forum.ViewModels;
using Interfaces;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Forum.ActionsFilters.Consumer.Forum
{
    public class ValidateUserRegisteredAttribute : IAsyncActionFilter
    {
        private readonly IRepositoryApiManager _repositoryApiManager;
        private readonly ILoggerManager _logger;
        private readonly IAuthenticationService _authenticationService;

        public ValidateUserRegisteredAttribute(IRepositoryApiManager repositoryApiManager, ILoggerManager logger, 
            IAuthenticationService authenticationService)
        {
            _repositoryApiManager = repositoryApiManager;
            _logger = logger;
            _authenticationService = authenticationService;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            bool isUserRegitered = false;
            var dbRoles = await _repositoryApiManager.ForumUserApis.GetUserRoles();
            if (dbRoles == null || dbRoles.Count == 0)
            {
                _logger.LogError($"Db roles is empty");
                
            }

            RegisterViewModel model = null;
            foreach (var argument in context.ActionArguments.Values
                .Where(v => v is RegisterViewModel))
            {
                model = argument as RegisterViewModel;
            }

            if (context.ModelState.IsValid)
            {
                var result = await _authenticationService.Register(model);

                if (result.IsSuccessStatusCode)
                {
                    isUserRegitered = true;
                }
                else
                {
                    var errorsRaw = await result.Content.ReadAsStringAsync();
                    context.ModelState.AddModelError(string.Empty, errorsRaw);
                }
            }
            else
            {
                context.ModelState.AddModelError(string.Empty, "Invalid registration attempt");
            }

            context.HttpContext.Items.Add("isUserRegitered", isUserRegitered);
            await next();
        }
    }
}
