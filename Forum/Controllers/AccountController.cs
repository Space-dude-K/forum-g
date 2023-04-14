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
            /*var model = new RegisterViewModel()
            {
                Roles = new List<SelectListItem>() 
                {
                    new SelectListItem
                    {
                        Text = "Motherboards",
                        Value = "MB"
                    },
                    new SelectListItem
                    {
                        Text = "Motherboards 1",
                        Value = "MB 1"
                    }
                }
            };*/
            var response = await _userService.GetUserRoles();
            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<IdentityRole>>(rawData)
                .Select(r => r.Name).ToList();
            var model = new RegisterViewModel()
            {
                Roles = responseContent
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var response = await _userService.GetUserRoles();
                var rawData = await response.Content.ReadAsStringAsync();
                var responseContent = JsonConvert.DeserializeObject<IEnumerable<IdentityRole>>(rawData)
                    .Select(r => r.Name).ToList();
                model = new RegisterViewModel()
                {
                    Roles = responseContent
                };


                /*var res = await _authenticationService.Register(model);

                if (!res)
                {
                    ModelState.AddModelError("Error", "Unable register new user");
                }*/

                /*var jsonContentBeforeUp = JsonConvert.SerializeObject(responseContentBeforeUp.First());
                var response = await client.PutAsync(uri, new StringContent(jsonContentBeforeUp, Encoding.UTF8, "application/json"));*/

                /*var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");*/

            }
            else
            {
                //RedirectToAction("Register");
                /*model = new RegisterViewModel()
                {
                    Roles = new List<string>()
                {
                    "1",
                    "2",
                    "3"
                }
                };*/
            }

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