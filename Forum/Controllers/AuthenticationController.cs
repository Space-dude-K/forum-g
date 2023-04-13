using AutoMapper;
using Contracts;
using Entities.DTO.UserDto;
using Entities.Models;
using Forum.ActionsFilters;
using Forum.ActionsFilters.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AuthenticationController(ILoggerManager logger, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateRoleExistsAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);
            var result = await _userManager.CreateAsync(user, userForRegistration.Password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            await _userManager.AddToRolesAsync(user, userForRegistration.Roles);

            return StatusCode(201);
        }
    }
}