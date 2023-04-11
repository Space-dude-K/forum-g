using AutoMapper;
using Contracts;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using Forum.ActionsFilters.Forum;
using Forum.ActionsFilters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Entities.RequestFeatures.Forum;
using Newtonsoft.Json;
using Forum.Utility.ForumLinks;
using Forum.ModelBinders;

namespace Forum.Controllers.Forum
{
    [Route("api/categories/{categoryId}/forums/{forumId}/topics")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly TopicLinks _topicLinks;

        public TopicController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, TopicLinks topicLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _topicLinks = topicLinks;
        }
        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetTopicsForForum(
            int categoryId, int forumId, [FromQuery] ForumTopicParameters forumTopicParameters)
        {
            var forum = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges: false);

            if (forum == null)
            {
                _logger.LogInfo($"Forum with category id: {categoryId} and forum id: {forumId} doesn't exist in the database.");
                return NotFound();
            }

            var topicsFromDb = await _repository.ForumTopic.GetAllTopicsFromForumAsync(forumId, forumTopicParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(topicsFromDb.MetaData));

            var topicsDto = _mapper.Map<IEnumerable<ForumTopicDto>>(topicsFromDb);
            var links = _topicLinks.TryGenerateLinks(topicsDto, categoryId, forumId, forumTopicParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpGet("{topicId}", Name = "GetTopicForForum")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetTopicForForum(int categoryId, int forumId, int topicId, [FromQuery] ForumTopicParameters forumTopicParameters)
        {
            var forumDb = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges: false);
            if (forumDb == null)
            {
                _logger.LogInfo($"Forum with category id: {categoryId} and forum id: {forumId} doesn't exist in the database.");
                return NotFound();
            }
            var topicDb = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges: false);
            if (topicDb == null)
            {
                _logger.LogInfo($"Topic with id: {topicId} doesn't exist in the database.");
                return NotFound();
            }

            var topicDto = _mapper.Map<ForumTopicDto>(topicDb);
            var links = _topicLinks.TryGenerateLinks(new List<ForumTopicDto>() { topicDto }, categoryId, forumId, forumTopicParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpGet("collection/({ids})", Name = "TopicCollection")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetTopicCollection(int categoryId, int forumId, [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids,
            [FromQuery] ForumTopicParameters forumTopicParameters)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var topicEntities = await _repository.ForumTopic.GetTopicsFromForumByIdsAsync(forumId, ids, trackChanges: false);

            if (ids.Count() != topicEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            var topicsToReturn = _mapper.Map<IEnumerable<ForumTopicDto>>(topicEntities);
            var links = _topicLinks.TryGenerateLinks(topicsToReturn, categoryId, forumId, forumTopicParameters.Fields, HttpContext, ids);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTopicForForum(int categoryId, int forumId, [FromBody] ForumTopicForCreationDto topic)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ForumTopicForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var forum = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges: false);
            if (forum == null)
            {
                _logger.LogInfo($"Forum with category id: {categoryId} and forum id: {forumId} doesn't exist in the database.");

                return NotFound();
            }

            var topicEntity = _mapper.Map<ForumTopic>(topic);
            topicEntity.CreatedAt = DateTime.Now;
            topicEntity.ForumUserId = 1;
            _repository.ForumTopic.CreateTopicForForum(forumId, topicEntity);
            await _repository.SaveAsync();

            var topicToReturn = _mapper.Map<ForumTopicDto>(topicEntity);

            return CreatedAtRoute("GetTopicForForum", new { categoryId, forumId, topicId = topicToReturn.Id }, topicToReturn);
        }
        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateTopicCollectionForForum(int categoryId, int forumId, [FromBody] IEnumerable<ForumTopicForCreationDto> topicCollection)
        {
            if (topicCollection == null)
            {
                _logger.LogError("Topic collection sent from client is null.");
                return BadRequest("Topic collection is null");
            }

            var topicEntities = _mapper.Map<IEnumerable<ForumTopic>>(topicCollection);

            foreach (var topic in topicEntities)
            {
                _repository.ForumTopic.CreateTopicForForum(forumId, topic);
            }

            await _repository.SaveAsync();
            var topicCollectionToReturn = _mapper.Map<IEnumerable<ForumTopicDto>>(topicEntities);
            var ids = string.Join(",", topicCollectionToReturn.Select(c => c.Id));

            return CreatedAtRoute("TopicCollection", new { categoryId, forumId, ids }, topicCollectionToReturn);
        }
        [HttpDelete("{topicId}")]
        [ServiceFilter(typeof(ValidateTopicForForumExistsAttribute))]
        public async Task<IActionResult> DeleteTopicForForum(int categoryId, int forumId, int topicId)
        {
            var topicForForum = HttpContext.Items["topic"] as ForumTopic;

            _repository.ForumTopic.DeleteTopic(topicForForum);
            await _repository.SaveAsync();

            return NoContent();
        }
        [HttpPut("{topicId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateTopicForForumExistsAttribute))]
        public async Task<IActionResult> UpdateTopicForForum(int categoryId, int forumId, int topicId, [FromBody] ForumTopicForUpdateDto topic)
        {
            var topicEntity = HttpContext.Items["topic"] as ForumTopic;

            _mapper.Map(topic, topicEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
        
        [HttpPatch("{topicId}")]
        [ServiceFilter(typeof(ValidateTopicForForumExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateTopicForForum(int categoryId, int forumId, int topicId, 
            [FromBody] JsonPatchDocument<ForumTopicForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var topicEntity = HttpContext.Items["topic"] as ForumTopic;
            var topicToPatch = _mapper.Map<ForumTopicForUpdateDto>(topicEntity);
            patchDoc.ApplyTo(topicToPatch, ModelState);

            TryValidateModel(topicToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(topicToPatch, topicEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}