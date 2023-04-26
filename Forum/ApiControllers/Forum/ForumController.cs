using AutoMapper;
using Interfaces;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;
using Entities.RequestFeatures.Forum;
using Forum.ActionsFilters;
using Forum.ActionsFilters.Forum;
using Forum.ModelBinders;
using Forum.Utility.ForumLinks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Forum.Controllers.Forum
{
    [Route("api/categories/{categoryId}/forums")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
    public class ForumController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly ForumBaseLinks _forumBaseLinks;

        public ForumController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, ForumBaseLinks forumBaseLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _forumBaseLinks = forumBaseLinks;
        }
        [HttpOptions]
        public IActionResult GetForumOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");
            return Ok();
        }
        [HttpGet]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetForumsForCategory(int categoryId, [FromQuery] ForumBaseParameters forumBaseParameters)
        {
            var category = await _repository.ForumCategory.GetCategoryAsync(categoryId, trackChanges: false);

            if (category == null)
            {
                _logger.LogInfo($"Category with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }

            var forumsFromDb = await _repository.ForumBase
                .GetAllForumsFromCategoryAsync(categoryId, forumBaseParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(forumsFromDb.MetaData));

            var forumsDto = _mapper.Map<IEnumerable<ForumBaseDto>>(forumsFromDb);
            var links = _forumBaseLinks.TryGenerateLinks(forumsDto, categoryId, forumBaseParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpGet("{forumId}", Name = "GetForumForCategory")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetForumForCategory(int categoryId, int forumId, [FromQuery] ForumBaseParameters forumBaseParameters)
        {
            var category = await _repository.ForumCategory.GetCategoryAsync(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Category with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }
            var forumDb = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges: false);
            if (forumDb == null)
            {
                _logger.LogInfo($"Forum with id: {forumId} doesn't exist in the database.");
                return NotFound();
            }
            var forumDto = _mapper.Map<ForumBaseDto>(forumDb);
            var links = _forumBaseLinks.TryGenerateLinks(new List<ForumBaseDto>() { forumDto }, categoryId, forumBaseParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpGet("collection/({ids})", Name = "ForumCollection")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetForumCollection(int categoryId, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids,
            [FromQuery] ForumBaseParameters forumBaseParameters)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var forumEntities = await _repository.ForumBase.GetForumsFromCategoryByIdsAsync(categoryId, ids, trackChanges: false);

            if (ids.Count() != forumEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var forumsToReturn = _mapper.Map<IEnumerable<ForumBaseDto>>(forumEntities);
            var links = _forumBaseLinks.TryGenerateLinks(forumsToReturn, categoryId, forumBaseParameters.Fields, HttpContext, ids);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateForumForCategory(int categoryId, [FromBody] ForumBaseForCreationDto forum)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ForumBaseDtoForCreation object");
                return UnprocessableEntity(ModelState);
            }

            var category = await _repository.ForumCategory.GetCategoryAsync(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Category with id: {categoryId} doesn't exist in the database.");
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
        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateForumCollectionForCategory(int categoryId, [FromBody] IEnumerable<ForumBaseForCreationDto> forumCollection)
        {
            if (forumCollection == null)
            {
                _logger.LogError("Forum collection sent from client is null.");
                return BadRequest("Forum collection is null");
            }

            var forumEntities = _mapper.Map<IEnumerable<ForumBase>>(forumCollection);

            foreach (var forum in forumEntities)
            {
                _repository.ForumBase.CreateForumForCategory(categoryId, forum);
            }

            await _repository.SaveAsync();
            var forumCollectionToReturn = _mapper.Map<IEnumerable<ForumBaseDto>>(forumEntities);
            var ids = string.Join(",", forumCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("ForumCollection", new { categoryId, ids }, forumCollectionToReturn);
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
        [HttpPut("{forumId}"), Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
