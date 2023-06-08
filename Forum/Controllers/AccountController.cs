using Interfaces;
using Forum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Interfaces.Forum;
using Interfaces.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Entities.Models;
using System.Security.Principal;
using Microsoft.AspNetCore.Authentication.Cookies;
using Entities.ViewModels;

namespace Forum.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly Interfaces.Forum.IAuthenticationService _authenticationService;
        private readonly IUserService _userService;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(ILoggerManager logger,
            Interfaces.Forum.IAuthenticationService authenticationService, IUserService userService,
            UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _logger = logger;
            _authenticationService = authenticationService;
            _userService = userService;
            _userManager = userManager;
            _signInManager = signInManager;
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
            var result = await _signInManager.PasswordSignInAsync(user, userModel.Password, userModel.RememberMe, true);

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
        public async Task<IActionResult> Logout(string returnUrl = null)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            _logger.LogInfo($"Logout for {user.UserName}");
            
            await _signInManager.SignOutAsync();

            _logger.LogInfo($"Is {user.UserName} signed? {_signInManager.IsSignedIn(HttpContext.User)}");

            return RedirectToLocal(returnUrl);
            //return RedirectToAction(nameof(HomeController.Privacy), "Home");
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
        /*[AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> LoginAsync()
        {
            return View();
        }*/
        /*[AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var loginResult = await _authenticationService.Login(model);

            if (loginResult.IsSuccessStatusCode)
            {
                TempData["Success"] = "User logined successfully!";
            }
            else
            {
                var errorsRaw = await loginResult.Content.ReadAsStringAsync();
                ModelState.AddModelError(string.Empty, errorsRaw);
            }

            return View(model);
        }*/
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