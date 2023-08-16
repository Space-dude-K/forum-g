using AutoMapper;
using Azure.Core;
using Entities.DTO.ForumDto.ForumView;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Services;
using System.Diagnostics;
using System.Net;

namespace Forum.Controllers
{
    public class ForumHomeController : Controller
    {
        private readonly IForumService _forumService;
        private readonly IMapper _mapper;

        public ForumHomeController(IForumService forumService, IMapper mapper)
        {
            _forumService = forumService;
            _mapper = mapper;
        }

        public async Task<IActionResult> ForumHome()
        {
            var model = await _forumService.GetForumCategoriesAndForumBasesForModel();

            return View("~/Views/Forum/ForumHome.cshtml", model);
        }
        public async Task<IActionResult> RedirectToCreateCategory()
        {
            return View("~/Views/Forum/Add/ForumAddCategory.cshtml");
        }
        
        /*public async Task<IActionResult> CreateForumCategory(ForumHomeViewModel model)
        {
            var user = _mapper.Map<ForumHomeViewModel>(model-);
            return View("~/Views/Forum/Add/ForumAddForumBase.cshtml", modelD);
        }*/
        
        [Route("categories/{categoryId}/forums/{forumId}/topics", Name = "ForumTopics")]
        public async Task<IActionResult> ForumTopics(int categoryId, int forumId)
        {
            var model = await _forumService.GetForumTopicsForModel(categoryId, forumId);
            await _forumService.IncreaseViewCounterForForumBase(categoryId, forumId);

            return View("~/Views/Forum/ForumBase.cshtml", model);
        }
        [HttpGet]
        [Route("categories/{categoryId}/forums/{forumId}/topics/{topicId}/{pageId}", Name = "TopicPosts")]
        public async Task<IActionResult> TopicPosts(int categoryId, int forumId, int topicId, int pageId = 0)
        {
            int maxiumPostsPerPage = 4;


            var model = await _forumService.GetTopicPostsForModel(categoryId, forumId, topicId, pageId, maxiumPostsPerPage);

           
            
            int currentDataSliceIndex = maxiumPostsPerPage * (pageId - 1);
            //model.Posts = model.Posts.Skip(currentDataSliceIndex).Take(maxiumPostsPerPage).ToList();

            await _forumService.IncreaseViewCounterForTopic(categoryId, forumId, topicId);


            return View("~/Views/Forum/ForumTopic.cshtml", model);
        }
    }
}
