using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

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
        [Route("ForumPostCreation/CreateForumPost")]
        public async Task<IActionResult> CreateForumPost(int categoryId, int forumId, int topicId, int totalPages, ForumTopicViewModel model)
        {
            /*if (!ModelState.IsValid)
                return BadRequest(ModelState);*/

            var postToAdd = _mapper.Map<ForumPostForCreationDto>(model);
            var res = await _forumService.CreateForumPost(categoryId, forumId, topicId, postToAdd);

            model.TotalPosts = await _forumService.GetTopicPostCount(categoryId, forumId, topicId);
            //model = await _forumService.GetTopicPostsForModel(categoryId, forumId, topicId, totalPages, 4);

            return Json(new { redirectToUrl = Url.Action("TopicPosts", "ForumHome", new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = model.TotalPages }) });
            //return Ok(res);
            //return View("~/Views/Forum/ForumTopic.cshtml", model);
            //return RedirectToAction("TopicPosts", "ForumHome", new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = model.TotalPages });
        }
    }
}
