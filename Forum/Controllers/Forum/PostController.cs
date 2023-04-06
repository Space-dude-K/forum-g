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

namespace Forum.Controllers.Forum
{
    [Route("api/categories/{categoryId}/forums/{forumId}/topics/{topicId}/posts")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<ForumPostDto> _dataShaper;

        public PostController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, IDataShaper<ForumPostDto> dataShaper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _dataShaper = dataShaper;
        }
        [HttpGet]
        public async Task<IActionResult> GetPostsForTopic(
            int categoryId, int forumId, int topicId, [FromQuery] ForumPostParameters forumPostParameters)
        {
            if (!forumPostParameters.ValidLikeRange)
                return BadRequest("Max likes cant be less than min");

            var topic = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges: false);

            if (topic == null)
            {
                _logger.LogInfo($"Topic with forum id: {forumId} and topic id: {topicId} doesn't exist in the database.");

                return NotFound();
            }

            var postsFromDb = await _repository.ForumPost.GetAllPostsFromTopicAsync(topicId, forumPostParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(postsFromDb.MetaData));

            var postsDto = _mapper.Map<IEnumerable<ForumPostDto>>(postsFromDb);

            return Ok(_dataShaper.ShapeData(postsDto, forumPostParameters.Fields));
        }
        [HttpGet("{postId}", Name = "GetPostForTopic")]
        public async Task<IActionResult> GetPostForTopic(int categoryId, int forumId, int topicId, int postId, [FromQuery] ForumPostParameters forumPostParameters)
        {
            var topicDb = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges: false);
            if (topicDb == null)
            {
                _logger.LogInfo($"Topic with forum id: {forumId} and post id: {postId} doesn't exist in the database.");

                return NotFound();
            }
            var postDb = await _repository.ForumPost.GetPostAsync(topicId, postId, trackChanges: false);
            if (postDb == null)
            {
                _logger.LogInfo($"Post with id: {postId} doesn't exist in the database.");

                return NotFound();
            }

            var postDto = _mapper.Map<ForumPostDto>(postDb);

            return Ok(_dataShaper.ShapeData(postDto, forumPostParameters.Fields));
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreatePostForTopic(int categoryId, int forumId, int topicId, [FromBody] ForumPostForCreationDto post)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ForumPostForCreationDto object");
                return UnprocessableEntity(ModelState);
            }

            var topic = await _repository.ForumTopic.GetTopicAsync(forumId, topicId, trackChanges: false);
            if (topic == null)
            {
                _logger.LogInfo($"Topic with forum id: {forumId} and topic id: {topicId} doesn't exist in the database.");

                return NotFound();
            }

            var postEntity = _mapper.Map<ForumPost>(post);
            postEntity.CreatedAt = DateTime.Now;
            postEntity.ForumUserId = 1;
            _repository.ForumPost.CreatePostForTopic(topicId, postEntity);
            await _repository.SaveAsync();

            var postToReturn = _mapper.Map<ForumPostDto>(postEntity);

            return CreatedAtRoute("GetPostForTopic", new { categoryId, forumId, topicId, postId = postToReturn.Id }, postToReturn);
        }
        [HttpDelete("{postId}")]
        [ServiceFilter(typeof(ValidatePostForTopicExistsAttribute))]
        public async Task<IActionResult> DeletePostForTopic(int categoryId, int forumId, int topicId, int postId)
        {
            var postForTopic = HttpContext.Items["post"] as ForumPost;

            _repository.ForumPost.DeletePost(postForTopic);
            await _repository.SaveAsync();

            return NoContent();
        }
        [HttpPut("{postId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidatePostForTopicExistsAttribute))]
        public async Task<IActionResult> UpdateTopicForForum(int categoryId, int forumId, int topicId, int postId, 
            [FromBody] ForumPostForUpdateDto post)
        {
            var postForTopic = HttpContext.Items["post"] as ForumPost;

            _mapper.Map(post, postForTopic);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPatch("{postId}")]
        [ServiceFilter(typeof(ValidatePostForTopicExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdatePostForTopic(int categoryId, int forumId, int topicId, int postId,
            [FromBody] JsonPatchDocument<ForumPostForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }

            var postEntity = HttpContext.Items["post"] as ForumPost;
            var postToPatch = _mapper.Map<ForumPostForUpdateDto>(postEntity);
            patchDoc.ApplyTo(postToPatch, ModelState);

            TryValidateModel(postToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(postToPatch, postEntity);
            await _repository.SaveAsync();

            return NoContent();
        }
    }
}