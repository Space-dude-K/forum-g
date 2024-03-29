﻿using AutoMapper;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using Forum.Extensions;
using Interfaces;
using Entities.DTO.UserDto;
using Marvin.Cache.Headers;
using Forum.ActionsFilters.Consumer.Forum;
using Entities.DTO.ForumDto.Update;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;

namespace Forum.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumHomeController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ILoggerManager _logger;
        private readonly IForumModelService _forumModelService;
        private readonly IRepositoryApiManager _repositoryApiManager;
        private readonly UserManager<AppUser> _userManager;

        public ForumHomeController(IMapper mapper, IWebHostEnvironment env, ILoggerManager logger,
            IForumModelService forumModelService, IRepositoryApiManager repositoryApiManager, 
            UserManager<AppUser> userManager)
        {
            _mapper = mapper;
            _env = env;
            _logger = logger;
            _forumModelService = forumModelService;
            _repositoryApiManager = repositoryApiManager;
            _userManager = userManager;
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        public async Task<IActionResult> ForumHome()
        {
            var model = await _forumModelService
                .GetForumCategoriesAndForumBasesForModel();

            return View("~/Views/Forum/ForumHome.cshtml", model);
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        [ServiceFilter(typeof(ValidateForumUserExistAttribute))]
        public async Task<ActionResult> DeleteForumBase(int categoryId, int forumId)
        {
            int userId = (int)HttpContext.Items["userId"];
            int userForumPosts = 0;
            var userTopics = await _repositoryApiManager.TopicApis.GetForumTopics(categoryId, forumId);

            // TODO. Refactoring
            foreach(var topic in userTopics)
            {
                var userTopicPosts = 
                    await _repositoryApiManager.TopicApis
                    .GetTopicPosts(categoryId, forumId, topic.Id, 0, 0, true);

                userForumPosts += userTopicPosts
                    .Where(p => p.ForumUserId.Equals(userId))
                    .Count();
            }

            var res = await _repositoryApiManager.ForumApis.DeleteForumBase(categoryId, forumId);

            if (res)
            {
                var resUserPostCounter = await _repositoryApiManager.PostApis
                    .UpdatePostCounterForUser(userId, false, userForumPosts);
            }
            else
            {
                return BadRequest("Unable to delete forum base with id: " + forumId);
            }

            return RedirectToAction("ForumHome");
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        public async Task<IActionResult> RedirectToCreateCategory()
        {
            return View("~/Views/Forum/Add/ForumAddCategory.cshtml");
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        [Route("categories/{categoryId}/forums/{forumId}/topics", Name = "ForumTopics")]
        public async Task<IActionResult> ForumTopics(int categoryId, int forumId)
        {
            var model = await _forumModelService.GetForumTopicsForModel(categoryId, forumId);
            await _repositoryApiManager.ForumApis
                .IncreaseViewCounterForForumBase(categoryId, forumId);

            return View("~/Views/Forum/ForumBase.cshtml", model);
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        [ServiceFilter(typeof(ValidateForumUserExistAttribute))]
        public async Task<ActionResult> DeleteTopic(int categoryId, int forumId, int topicId)
        {
            int userId = (int)HttpContext.Items["userId"];
            var userTopicPosts = await _repositoryApiManager.TopicApis
                .GetTopicPosts(categoryId, forumId, topicId, 0, 0, true);
            var res = await _repositoryApiManager.TopicApis.DeleteForumTopic(categoryId, forumId, topicId);

            if (res)
            {
                var userTopicPostCounter = userTopicPosts
                    .Where(p => p.ForumUserId.Equals(userId))
                    .Count();
                var resUserPostCounter = await _repositoryApiManager.PostApis
                    .UpdatePostCounterForUser(userId, false, userTopicPostCounter);
            }
            else
            {
                return BadRequest("Cannot delete topic with ID " + topicId);
            }

            return RedirectToAction("ForumTopics", new { categoryId = categoryId, forumId = forumId });
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        [Route("categories/{categoryId}/forums/{forumId}/topics/{topicId}/{pageId}", Name = "TopicPosts")]
        public async Task<IActionResult> TopicPosts(int categoryId, int forumId, int topicId, int pageId = 0)
        {
            int maxiumPostsPerPage = 4;
            var model = await _forumModelService
                .GetTopicPostsForModel(categoryId, forumId, topicId, pageId, maxiumPostsPerPage);
            int userId = (int)HttpContext.Items["userId"];

            // TODO. Refactoring
            var tasks = model.Posts.Select(
                async p => new
                {
                    Item = p,
                    Dto = await _repositoryApiManager.ForumUserApis
                    .GetForumUserDto(p.ForumUser.Id)
                });
            var tuples = await Task.WhenAll(tasks);

            foreach (var (user, item) in model.Posts
                .SelectMany(user => tuples
                .Where(item => user.ForumUser.Id == item.Item.ForumUser.Id)
                .Select(item => (user, item))))
            {
                user.ForumUser.TotalPostCounter = item.Dto.TotalPostCounter;
                user.ForumUser.AvatarImgSrc = user.ForumUser.LoadAvatar(_env.WebRootPath);
                user.ForumUser.IsUserHasAccess = user.ForumUser.Id.Equals(userId);
            }

            var updateCounterRes = await _repositoryApiManager.TopicApis
                .IncreaseViewCounterForTopic(categoryId, forumId, topicId);

            return View("~/Views/Forum/ForumTopic.cshtml", model);
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        public async Task<ActionResult> DeletePost(int categoryId, int forumId, int topicId, int postId, ForumTopicViewModel model)
        {
            var res = await _repositoryApiManager.PostApis.DeleteForumPost(categoryId, forumId, topicId, postId);
            int totalPosts = 0;

            int userId = (int)HttpContext.Items["userId"];

            if (res)
            {
                var resCounter = await _repositoryApiManager.PostApis.UpdatePostCounter(categoryId, false);
                var resUserCounter = await _repositoryApiManager.PostApis.UpdatePostCounterForUser(userId, false);
                model.TotalPosts = await _repositoryApiManager.PostApis.GetTopicPostCount(categoryId);
            }
            else
            {
                return BadRequest("Cannot delete post with ID " + postId);
            }
            
            return Json(new { redirectToUrl = Url.Action("TopicPosts", "ForumHome", 
                new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = model.TotalPages }) });
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        public async Task<ActionResult> UpdatePost(int categoryId, int forumId, int topicId, int postId, int pageId, 
            ForumPostForUpdateDto forumPostForUpdateDto)
        {
            var res = await _repositoryApiManager.PostApis
                .UpdatePost(categoryId, forumId, topicId, postId, forumPostForUpdateDto);

            return Json(new { redirectToUrl = Url.Action("TopicPosts", "ForumHome", 
                new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = pageId }) });
        }
        [HttpCacheIgnore]
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        [ServiceFilter(typeof(ValidateForumUserExistAttribute))]
        public async Task<IActionResult> ForumUserPage(int id)
        {
            var user = HttpContext.Items["forumUser"] as ForumUserDto;
            var model = _mapper.Map<ForumUserPageViewModel>(user);
            model.AvatarImgSrc = user.LoadAvatar(_env.WebRootPath);

            return View("~/Views/Forum/User/ForumUserPage.cshtml", model);
        }
        [HttpCacheIgnore]
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        [ServiceFilter(typeof(ValidateForumUserExistAttribute))]
        public async Task<IActionResult> UpdateForumUserPage(int id, ForumUserPageViewModel model)
        {
            var user = HttpContext.Items["forumUser"] as ForumUserDto;
            var appUserDto = _mapper.Map<AppUserDto>(model);
            var res = await _repositoryApiManager.ForumUserApis.UpdateAppUser(id, appUserDto);

            if (!res)
            {
                return BadRequest($"Unable to update app user with id: {id}");
            }

            model.AvatarImgSrc = user.LoadAvatar(_env.WebRootPath);

            return View("~/Views/Forum/User/ForumUserPage.cshtml", model);
        }
        public async Task<IActionResult> UpdateForumPostLikes(int categoryId, int forumId, int topicId, int postId, int pageId)
        {
            var res = await _repositoryApiManager.PostApis
                .UpdatePostLikeCounter(categoryId, forumId, topicId, postId, true);

            if (!res)
            {
                return BadRequest($"Unable to update like for post id: {postId}");
            }

            return RedirectToAction("TopicPosts", new { categoryId, forumId, topicId, postId, pageId });
        }
    }
}