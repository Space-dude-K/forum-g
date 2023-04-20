using AutoMapper;
using Interfaces;
using Entities.DTO.UserDto;
using Entities.Models;
using Forum.ActionsFilters;
using Forum.ActionsFilters.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Entities.DTO.UserDto.Create;

namespace Forum.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(ILoggerManager logger, IMapper mapper, 
            UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, 
            IAuthenticationManager authenticationManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _authenticationManager = authenticationManager;
        }
        [HttpPost()]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateRoleExistsAttribute))]
        public async Task<IActionResult> RegisterUser([FromBody] UserForCreationDto userForRegistration)
        {
            var user = _mapper.Map<AppUser>(userForRegistration);
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
        [HttpPost("login")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto user)
        {
            if (!await _authenticationManager.ValidateUser(user))
            {
                _logger.LogWarn($"{nameof(Authenticate)}: Authentication failed. Wrong user name or password.");
                return Unauthorized();
            }

            return Ok(new { Token = await _authenticationManager.CreateToken() });
        }
    }
}