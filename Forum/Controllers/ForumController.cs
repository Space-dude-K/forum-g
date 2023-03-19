using AutoMapper;
using Contracts;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
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
        [HttpGet]
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
        [HttpGet("{forumId}", Name = "GetForumForCategory")]
        public IActionResult GetForumForCategory(int categoryId, int forumId)
        {
            var category =  _repository.ForumCategory.GetCategory(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }
            var forumDb =  _repository.ForumBase.GetForum(categoryId, forumId, trackChanges: false);
            if (forumDb == null)
            {
                _logger.LogInfo($"Employee with id: {forumId} doesn't exist in the database.");
                return NotFound();
            }
            var forum = _mapper.Map<ForumBaseDto>(forumDb);
            return Ok(forum);
        }
        [HttpPost]
        public IActionResult CreateForumForCategory(int categoryId, [FromBody] ForumBaseForCreationDto forum)
        {
            if (forum == null)
            {
                _logger.LogError("EmployeeForCreationDto object sent from client is null.");
                return BadRequest("EmployeeForCreationDto object is null");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForCreationDto object");
                return UnprocessableEntity(ModelState);
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
        [HttpDelete("{forumId}")]
        public IActionResult DeleteForumForCategory(int categoryId, int forumId)
        {
            var category = _repository.ForumCategory.GetCategory(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }
            var forumForCategory = _repository.ForumBase.GetForum(categoryId, forumId, trackChanges: false);
            if (forumForCategory == null)
            {
                _logger.LogInfo($"Employee with id: {forumId} doesn't exist in the database.");
                return NotFound();
            }
            _repository.ForumBase.DeleteForum(forumForCategory);
            _repository.Save();
            return NoContent();
        }
        [HttpPut("{forumId}")]
        public IActionResult UpdateForumForCategory(int categoryId, int forumId, [FromBody] ForumBaseForUpdateDto forum)
        {
            if (forum == null)
            {
                _logger.LogError("EmployeeForUpdateDto object sent from client is null.");
                return BadRequest("EmployeeForUpdateDto object is null");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForUpdateDto object");
                return UnprocessableEntity(ModelState);
            }
            var category = _repository.ForumCategory.GetCategory(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }
            var forumEntity = _repository.ForumBase.GetForum(categoryId, forumId, trackChanges: true);
            if (forumEntity == null)
            {
                _logger.LogInfo($"Employee with id: {forumId} doesn't exist in the database.");
                return NotFound();
            }
            _mapper.Map(forum, forumEntity);
            _repository.Save();
            return NoContent();
        }
        [HttpPatch("{forumId}")]
        public IActionResult PartiallyUpdateForumForCategory(int categoryId, int forumId, [FromBody] JsonPatchDocument<ForumBaseForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var category = _repository.ForumCategory.GetCategory(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }
            var forumEntity = _repository.ForumBase.GetForum(categoryId, forumId, trackChanges: true);
            if (forumEntity == null)
            {
                _logger.LogInfo($"Employee with id: {forumId} doesn't exist in the database.");
                return NotFound();
            }
            var forumToPatch = _mapper.Map<ForumBaseForUpdateDto>(forumEntity);
            patchDoc.ApplyTo(forumToPatch, ModelState);

            TryValidateModel(forumToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(forumToPatch, forumEntity);
            _repository.Save();
            return NoContent();
        }

    }
}
