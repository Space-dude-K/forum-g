using AutoMapper;
using Azure;
using Entities.DTO.ForumDto;
using Entities.DTO.UserDto;
using Entities.Models;
using Entities.RequestFeatures.Forum;
using Entities.RequestFeatures.User;
using Forum.ActionsFilters;
using Forum.Utility.ForumLinks;
using Forum.Utility.UserLinks;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Forum.ApiControllers.User
{
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserDataLinks _userDataLinks;

        public UserController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, UserDataLinks userDataLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userDataLinks = userDataLinks;
        }
        [HttpOptions]
        public IActionResult GetUserOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
        [HttpGet("roles", Name = "GetUserRoles")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<List<IdentityRole>> GetUserRoles()
        {
            var rolesFromDb = await _repository.Roles.GetAllRolesAsync(trackChanges: false);

            return rolesFromDb;
        }
        [HttpGet("users", Name = "GetUsers")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetUsers([FromQuery] UserParameters userParameters)
        {
            var usersFromDb = await _repository.Users.GetAllUsersAsync(userParameters, trackChanges: false);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(usersFromDb);
            var links = _userDataLinks.TryGenerateLinks(usersDto, userParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpGet("users/{userId}", Name = "GetUserById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetUser(string userId, [FromQuery] UserParameters userParameters)
        {
            var usersFromDb = await _repository.Users.GetUserAsync(userId, userParameters, trackChanges: false);
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(usersFromDb);
            var links = _userDataLinks.TryGenerateLinks(usersDto, userParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
    }
}
