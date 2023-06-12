using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    public class ForumHomeController : Controller
    {
        private readonly IForumService forumService;

        public ForumHomeController(IForumService forumService)
        {
            this.forumService = forumService;
        }

        public async Task<IActionResult> ForumHome()
        {
            var model = await forumService.GetForumCategoriesAndForumBasesForModel();

            return View("~/Views/Forum/ForumHome.cshtml", model);
        }
        [Route("categories/{categoryId}/forums/{forumId}/topics", Name = "ForumTopics")]
        public async Task<IActionResult> ForumTopics(int categoryId, int forumId)
        {
            var model = await forumService.GetForumTopicsForModel(categoryId, forumId);

            return View("~/Views/Forum/ForumBase.cshtml", model);
        }
        [Route("categories/{categoryId}/forums/{forumId}/topics/{topicId}", Name = "TopicPosts")]
        public async Task<IActionResult> ForumTopics(int categoryId, int forumId, int topicId)
        {
            var model = await forumService.GetTopicPostsForModel(categoryId, forumId, topicId);

            return View("~/Views/Forum/ForumTopic.cshtml", model);
        }
    }
}
