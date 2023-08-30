using Entities.Models;
using Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Entities.ViewModels;

namespace Forum.ActionsFilters.Consumer.Forum
{
    public class ValidateUserLoginAttribute : IAsyncActionFilter
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILoggerManager _logger;
        public ValidateUserLoginAttribute(UserManager<AppUser> userManager, ILoggerManager logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            LoginViewModel model = null;
            foreach (var argument in context.ActionArguments.Values
                .Where(v => v is LoginViewModel))
            {
                model = argument as LoginViewModel;
            }
            if (model != null && !string.IsNullOrEmpty(model.UserName))
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if(user == null)
                {
                    _logger.LogInfo($"Invalid user.");
                    context.ModelState.AddModelError("", "Invalid user");
                    await next();
                }
                else
                {
                    context.HttpContext.Items.Add("loginUser", user);
                    await next();
                }
            }
            else
            {
                _logger.LogInfo($"LoginViewModel is empty.");
                context.ModelState.AddModelError("", "Invalid model");
                await next();
            }
        }
    }
}
