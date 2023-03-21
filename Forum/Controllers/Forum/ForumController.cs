using AutoMapper;
using Contracts;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;
using Forum.ActionsFilters;
using Forum.ActionsFilters.Forum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.Design;

namespace Forum.Controllers.Forum
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
        public async Task<IActionResult> GetForumsForCategory(int categoryId)
        {
            var category = await _repository.ForumCategory.GetCategoryAsync(categoryId, trackChanges: false);

            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }

            var forums = await _repository.ForumBase.GetAllForumsFromCategoryAsync(categoryId, trackChanges: false);
            var forumsDto = _mapper.Map<IEnumerable<ForumBaseDto>>(forums);

            return Ok(forumsDto);
        }
        [HttpGet("{forumId}", Name = "GetForumForCategory")]
        public async Task<IActionResult> GetForumForCategory(int categoryId, int forumId)
        {
            var category = await _repository.ForumCategory.GetCategoryAsync(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }
            var forumDb = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges: false);
            if (forumDb == null)
            {
                _logger.LogInfo($"Employee with id: {forumId} doesn't exist in the database.");
                return NotFound();
            }
            var forum = _mapper.Map<ForumBaseDto>(forumDb);
            return Ok(forum);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateForumForCategory(int categoryId, [FromBody] ForumBaseForCreationDto forum)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the EmployeeForCreationDto object");
                return UnprocessableEntity(ModelState);
            }


            var category = await _repository.ForumCategory.GetCategoryAsync(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }

            var forumEntity = _mapper.Map<ForumBase>(forum);
            forumEntity.CreatedAt = DateTime.Now;
            forumEntity.ForumUserId = 1;
            _repository.ForumBase.CreateForumForCategory(categoryId, forumEntity);
            await _repository.SaveAsync();

            var forumToReturn = _mapper.Map<ForumBaseDto>(forumEntity);

            return CreatedAtRoute("GetForumForCategory", new { categoryId, id = forumToReturn.Id }, forumToReturn);
        }
        [HttpDelete("{forumId}")]
        [ServiceFilter(typeof(ValidateForumForCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteForumForCategory(int categoryId, int forumId)
        {
            var forumForCategory = HttpContext.Items["forum"] as ForumBase;

            _repository.ForumBase.DeleteForum(forumForCategory);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{forumId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateForumForCategoryExistsAttribute))]
        public async Task<IActionResult> UpdateForumForCategory(int categoryId, int forumId, [FromBody] ForumBaseForUpdateDto forum)
        {
            var forumEntity = HttpContext.Items["forum"] as ForumBase;

            _mapper.Map(forum, forumEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPatch("{forumId}")]
        [ServiceFilter(typeof(ValidateForumForCategoryExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateForumForCategory(int categoryId, int forumId, [FromBody] JsonPatchDocument<ForumBaseForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var forumEntity = HttpContext.Items["forum"] as ForumBase;
            var forumToPatch = _mapper.Map<ForumBaseForUpdateDto>(forumEntity);
            patchDoc.ApplyTo(forumToPatch, ModelState);

            TryValidateModel(forumToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(forumToPatch, forumEntity);
            await _repository.SaveAsync();
            return NoContent();
        }
    }
}
