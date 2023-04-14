using AutoMapper;
using Azure;
using Entities.DTO.ForumDto;
using Entities.RequestFeatures.Forum;
using Forum.ActionsFilters;
using Forum.Utility.ForumLinks;
using Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Forum.ApiControllers.User
{
    [Route("api/roles")]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly ForumBaseLinks _forumBaseLinks;

        public UserController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, ForumBaseLinks forumBaseLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _forumBaseLinks = forumBaseLinks;
        }
        [HttpOptions]
        public IActionResult GetUserOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
        [HttpGet(Name = "GetUserRoles")]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<List<IdentityRole>> GetUserRoles()
        {
            var rolesFromDb = await _repository.UserRole.GetAllRolesAsync(trackChanges: false);

            //Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(rolesFromDb.MetaData));

            //var categoriesDto = _mapper.Map<IEnumerable<ForumCategoryDto>>(categoriesFromDb);
            //var links = _categoryLinks.TryGenerateLinks(categoriesDto, forumCategoryParameters.Fields, HttpContext);

            //return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);

            return rolesFromDb;
        }

    }

}
