using AutoMapper;
using Contracts;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using Forum.ActionsFilters.Forum;
using Forum.ActionsFilters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers.Forum
{
    [Route("api/categories/{categoryId}/forums/{forumId}/topics")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public TopicController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> GetTopicsForForum(int categoryId, int forumId)
        {
            var forum = await _repository.ForumBase.GetForumFromCategoryAsync(categoryId, forumId, trackChanges: false);

            if (forum == null)
            {
                _logger.LogInfo($"Forum with category id: {categoryId} and forum id: {forumId} doesn't exist in the database.");
                return NotFound();
            }

            var topics = await _repository.ForumTopic.GetAllTopicsFromForumAsync(forumId, trackChanges: false);
            var topicsDto = _mapper.Map<IEnumerable<ForumTopicDto>>(topics);

            return Ok(topicsDto);
        }
        [HttpGet("{topicId}", Name = "GetTopicForForum")]
        public async Task<IActionResult> GetTopicForForum(int categoryId, int forumId, int topicId)
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

            return Ok(topicDto);
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
