using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Forum.ActionsFilters.Consumer.Forum;
using Interfaces.Forum;
using Interfaces.Forum.ApiServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumTopicCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IForumTopicService _forumTopicService;

        public ForumTopicCreationController(IMapper mapper, IForumService forumService, IMemoryCache cache, 
            IForumTopicService forumTopicService)
        {
            _mapper = mapper;
            _forumTopicService = forumTopicService;
        }
        [HttpGet]
        [Route("categories/{categoryId}/forums/{forumId}/add", Name = "ForumTopicAdd")]
        public async Task<IActionResult> CreateForumTopic()
        {
            return View("~/Views/Forum/Add/ForumAddTopic.cshtml");
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
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
                var insertedTopicId = await _forumTopicService.CreateForumTopic(categoryId, forumId, topicToAdd);
                var resCounterCreation = await _forumTopicService.CreateTopicPostCounter(insertedTopicId, 
                    new ForumCounterForCreationDto() { ForumTopicId = insertedTopicId });
                var resCounter = await _forumTopicService.UpdateTopicCounter(categoryId, true);
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
