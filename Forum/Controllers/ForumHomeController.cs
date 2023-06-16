﻿using Entities.DTO.ForumDto.ForumView;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> RedirectToCreateCategory()
        {
            return View("~/Views/Forum/Add/ForumAddCategory.cshtml");
        }
        public async Task<IActionResult> RedirectToCreateForumBase(string model)
        {
            ForumHomeViewModel modelD = JsonConvert.DeserializeObject<ForumHomeViewModel>(model);
            return View("~/Views/Forum/Add/ForumAddForumBase.cshtml", modelD);
        }
        [Route("categories/{categoryId}/forums/{forumId}/topics", Name = "ForumTopics")]
        public async Task<IActionResult> ForumTopics(int categoryId, int forumId, string forumTitle)
        {
            var model = await forumService.GetForumTopicsForModel(categoryId, forumId, forumTitle);
            await forumService.IncreaseViewCounterForForumBase(categoryId, forumId);

            return View("~/Views/Forum/ForumBase.cshtml", model);
        }
        [Route("categories/{categoryId}/forums/{forumId}/topics/{topicId}", Name = "TopicPosts")]
        public async Task<IActionResult> ForumTopics(int categoryId, int forumId, int topicId)
        {
            var model = await forumService.GetTopicPostsForModel(categoryId, forumId, topicId);
            await forumService.IncreaseViewCounterForTopic(categoryId, forumId, topicId);

            return View("~/Views/Forum/ForumTopic.cshtml", model);
        }
    }
}
