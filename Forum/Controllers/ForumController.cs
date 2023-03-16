using AutoMapper;
using Contracts;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.Design;

namespace Forum.Controllers
{
    [Route("api/categories/{categoryId}/forums")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public ForumController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet("{id}", Name = "GetForumForCategory")]

        public IActionResult GetForumsForCategory(int categoryId)
        {
            var category = _repository.ForumCategory.GetCategory(categoryId, trackChanges: false);

            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }

            var forums = _repository.ForumBase.GetAllForums(categoryId, trackChanges: false);
            var forumsDto = _mapper.Map<IEnumerable<ForumBaseDto>>(forums);

            return Ok(forumsDto);
        }
        [HttpPost]
        public IActionResult CreateForumForCategory(int categoryId, [FromBody] ForumBaseForCreationDto forum)
        {
            if (forum == null)
            {
                _logger.LogError("EmployeeForCreationDto object sent from client is null.");
                return BadRequest("EmployeeForCreationDto object is null");
            }
            var category = _repository.ForumCategory.GetCategory(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }

            var forumEntity = _mapper.Map<ForumBase>(forum);
            forumEntity.CreatedAt = DateTime.Now;
            forumEntity.ForumUserId = 1;
            _repository.ForumBase.CreateForumForCategory(categoryId, forumEntity);
            _repository.Save();

            var forumToReturn = _mapper.Map<ForumBaseDto>(forumEntity);

            return CreatedAtRoute("GetForumForCategory", new { categoryId, id = forumToReturn.Id }, forumToReturn);
        }

    }
}
