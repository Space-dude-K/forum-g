using Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Entities.Models;

namespace Forum.ActionsFilters.Consumer.Forum
{
    public class ValidateAppUserExistAttribute : IAsyncActionFilter
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILoggerManager _logger;
        public ValidateAppUserExistAttribute(UserManager<AppUser> userManager, ILoggerManager logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            int userId = 0;
            int.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var appUser = await _userManager.FindByIdAsync(userId.ToString());

            if (appUser == null)
            {
                _logger.LogInfo($"App user id: {user.Identity.Name} not found.");
                context.Result = new UnauthorizedResult();

                return;
            }
            else
            {
                context.HttpContext.Items.Add("appUser", appUser);
                await next();
            }
        }
    }
}