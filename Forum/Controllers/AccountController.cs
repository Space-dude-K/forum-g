using Interfaces;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IRepositoryApiManager _repositoryApiManager;

        public AccountController(ILoggerManager logger,
            Interfaces.Forum.IAuthenticationService authenticationService,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, 
            IWebHostEnvironment webHostEnvironment, IRepositoryApiManager repositoryApiManager)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
            _repositoryApiManager = repositoryApiManager;
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
        [ServiceFilter(typeof(ValidateUserLoginAttribute))]
        public async Task<IActionResult> Login(LoginViewModel userModel, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            /*var user = await _userManager.FindByNameAsync(userModel.UserName);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid UserName or Password");
                return View(userModel);
            }*/


            var user = HttpContext.Items["loginUser"] as AppUser;
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
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");

        }
        public async Task<IActionResult> RegisterAsync()
        {
            var dbRoles = await _repositoryApiManager.ForumUserApis.GetUserRoles();

            if (dbRoles == null || dbRoles.Count == 0)
            {
                _logger.LogError($"Db roles is empty");
                return NotFound();
            }

            var registerTableViewModel = await _repositoryApiManager.ForumUserApis.GetUsersData();
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

            var dbRoles = await _repositoryApiManager.ForumUserApis
                .GetUserRoles();

            if (dbRoles == null || dbRoles.Count == 0)
            {
                _logger.LogError($"Db roles is empty");
                return NotFound();
            }
            var registerTableViewModel = await _repositoryApiManager.ForumUserApis
                .GetUsersData();

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