using Interfaces;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc.Rendering;
using Interfaces.User;
using Azure;
using Entities.DTO.ForumDto;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Entities.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Forum.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public AccountController(ILoggerManager logger, IAuthenticationService authenticationService, IUserService userService)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userService = userService;
        }

        public async Task<IActionResult> RegisterAsync()
        {
            var dbRoles = await _userService.GetUserRoles();

            if (dbRoles == null || dbRoles.Count == 0)
            {
                _logger.LogError($"Db roles is empty");
                return NotFound();
            }

            var registerTableViewModel = await _userService.GetUsersData();
            var model = new RegisterViewModel()
            {
                Roles = dbRoles,
                RegisterTableViewModel = registerTableViewModel
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // TODO. Email uniqueness check
            if (ModelState.IsValid)
            {
                var result = await _authenticationService.Register(model);

                if(result.IsSuccessStatusCode)
                {
                    TempData["Success"] = "User registered successfully!";
                }
                else
                {
                    var errorsRaw = await result.Content.ReadAsStringAsync();
                    //var errors = JsonConvert.DeserializeObject(errorsRaw);
                    ModelState.AddModelError(string.Empty, errorsRaw);
                }
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Invalid registration attempt");
            }

            var dbRoles = await _userService.GetUserRoles();

            if (dbRoles == null || dbRoles.Count == 0)
            {
                _logger.LogError($"Db roles is empty");
                return NotFound();
            }
            var registerTableViewModel = await _userService.GetUsersData();
            model = new RegisterViewModel()
            {
                Roles = dbRoles,
                RegisterTableViewModel = registerTableViewModel
            };

            return View(model);
        }


        /*[HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            //var branch = new Branch
            //{
            //    branchName = "Regie",
            //    address = "Naval"

            //};
            //branchContext.Branch.Add(branch);
            //branchContext.SaveChanges();

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }*/
    }
}