using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers.Forum
{
    public class ForumPostCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IForumService _forumService;

        public ForumPostCreationController(IMapper mapper, IForumService forumService)
        {
            _mapper = mapper;
            _forumService = forumService;
        }
        [HttpGet]
        [Route("categories/{categoryId}/forums/{forumId}/add", Name = "ForumTopicAdd")]
        public async Task<IActionResult> CreateForumTopic()
        {
            /*if (string.IsNullOrEmpty(model))
                return BadRequest(ModelState);

            var topicAddModel = _mapper.Map<ForumBaseCreationView>(JsonConvert.DeserializeObject<ForumHomeViewModel>(model));*/

            return View("~/Views/Forum/Add/ForumAddTopic.cshtml");
        }
        [HttpPost]
        [Route("categories/{categoryId}/forums/{forumId}/topics/{topicId}/posts", Name = "ForumPostCreate")]
        public async Task<IActionResult> CreateForumPost(int categoryId, int forumId, int topicId, ForumTopicViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var topicToAdd = _mapper.Map<ForumPostForCreationDto>(model);
            //var res = await _forumService.CreateForumTopic(categoryId, forumId, topicToAdd);

            return RedirectToAction("ForumTopics", "ForumHome",
                new { categoryId = categoryId, forumId = forumId });
        }
    }
}
