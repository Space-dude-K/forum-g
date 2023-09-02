using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Forum.ActionsFilters.Consumer.Forum;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumTopicCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryApiManager _repositoryApiManager;

        public ForumTopicCreationController(IMapper mapper, IRepositoryApiManager repositoryApiManager)
        {
            _mapper = mapper;
            _repositoryApiManager = repositoryApiManager;
        }
        [HttpGet]
        [Route("categories/{categoryId}/forums/{forumId}/add", Name = "ForumTopicAdd")]
        public async Task<IActionResult> CreateForumTopic()
        {
            return View("~/Views/Forum/Add/ForumAddTopic.cshtml");
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        [HttpPost]
        [Route("categories/{categoryId}/forums/{forumId}/add", Name = "ForumTopicAdd")]
        public async Task<IActionResult> CreateForumTopic(int categoryId, int forumId, ForumTopicCreationView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = (int)HttpContext.Items["userId"];
            var topicToAdd = _mapper.Map<ForumTopicForCreationDto>(model);

            if (userId > 0)
            {
                topicToAdd.ForumUserId = userId;
                var insertedTopicId = await _repositoryApiManager.TopicApis.CreateForumTopic(categoryId, forumId, topicToAdd);
                var resCounterCreation = await _repositoryApiManager.TopicApis.CreateTopicPostCounter(insertedTopicId, 
                    new ForumCounterForCreationDto() { ForumTopicId = insertedTopicId });
                var resCounter = await _repositoryApiManager.TopicApis.UpdateTopicCounter(categoryId, true);
            }
            else
            {
                return BadRequest("User ID error.");
            }

            return RedirectToAction("ForumTopics", "ForumHome", 
                new { categoryId = categoryId, forumId = forumId });
        }
    }
}