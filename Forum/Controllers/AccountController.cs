﻿using Interfaces;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Interfaces.Forum;
using Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Entities.Models;
using Entities.ViewModels;
using Forum.Extensions;
using Forum.ActionsFilters.Consumer.Forum;
using Entities.DTO.UserDto;

namespace Forum.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class AccountController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly Interfaces.Forum.IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IForumService _forumService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(ILoggerManager logger,
            Interfaces.Forum.IAuthenticationService authenticationService, IUserService userService,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IForumService forumService, 
            IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
            _forumService = forumService;
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpGet]
        public IActionResult Account(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View("~/Views/Forum/Add/ForumAddTopic.cshtml");
        }
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel userModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = await _userManager.FindByNameAsync(userModel.UserName);
            var result = await _signInManager
                .PasswordSignInAsync(user, userModel.Password, userModel.RememberMe, true);

            _logger.LogInfo($"User {user.Id} is signed in? {result.Succeeded}");

            if (result.Succeeded)
            {
                return RedirectToLocal(returnUrl);
            }
            else
            {
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View(userModel);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ServiceFilter(typeof(ValidateAppUserExistAttribute))]
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            var appUser = HttpContext.Items["appUser"] as AppUser;

            _logger.LogInfo($"Logout for {appUser.UserName}");
            
            await _signInManager.SignOutAsync();

            _logger.LogInfo($"Is {appUser.UserName} signed? {_signInManager.IsSignedIn(HttpContext.User)}");

            return RedirectToLocal(returnUrl);
        }
        /*[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel userModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var user = await _userManager.FindByNameAsync(userModel.UserName);
            _logger.LogInfo($"Login attempt for user: {user.Id}");
            if (user != null && await _userManager.CheckPasswordAsync(user, userModel.Password))
            {
                var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));

                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,  new ClaimsPrincipal(identity));
 
                //return RedirectToAction(nameof(HomeController.Index), "Home");
                _logger.LogInfo($"User {user.Id} is authenticated? {identity.IsAuthenticated}");

                return RedirectToLocal(returnUrl);
            }
            else
            {
                _logger.LogError($"Invalid UserName or Password");
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View(userModel);
            }
        }*/
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

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
        [ServiceFilter(typeof(ValidateForumUserExistAttribute))]
        [ServiceFilter(typeof(ValidateAppUserExistAttribute))]
        public async Task<ActionResult> ForumAccount()
        {
            int userId = 0;
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var forumUser = HttpContext.Items["forumUser"] as ForumUserDto;
            var appUser = HttpContext.Items["appUser"] as AppUser;
            var roles = await _userManager.GetRolesAsync(appUser);

            ForumAccountViewModel forumAccountViewModel = new()
            {
                FullName = appUser.UserName,
                UserRole = string.Join(", ", roles),
                CreatedAt = appUser.CreatedAt.ToString(),
                LatestLoginOnForum = appUser.LatestLoginOnForum.ToString(),
                Cabinet = appUser.Cabinet.ToString(),
                Phone = appUser.PhoneNumber,
                InPhone = appUser.InternalPhone.ToString(),
                Position = appUser.Position,
                Company = appUser.Company,
                Division = appUser.Division,
                Login = appUser.NormalizedUserName,
                AvatarImgSource = forumUser.LoadAvatar(_webHostEnvironment.WebRootPath)
            };

            return View("~/Views/Forum/User/ForumAccount.cshtml", forumAccountViewModel);
        }
    }
}