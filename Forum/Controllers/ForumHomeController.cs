using AutoMapper;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Interfaces.User;
using Forum.Extensions;
using Interfaces;
using Entities.DTO.UserDto;
using Marvin.Cache.Headers;
using Interfaces.Forum.ApiServices;
using Forum.ActionsFilters.API.Forum;
using Forum.ActionsFilters.Consumer.Forum;

namespace Forum.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumHomeController : Controller
    {
        private readonly IForumService _forumService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;
        private readonly ILoggerManager _logger;
        private readonly IForumPostService _forumPostService;
        private readonly IForumTopicService _forumTopicService;
        private readonly IForumBaseService _forumBaseService;
        private readonly IForumModelService _forumModelService;
        private readonly IForumCategoryService _forumCategoryService;

        public ForumHomeController(IForumService forumService, 
            IMapper mapper, IUserService  userService, IWebHostEnvironment env, ILoggerManager logger, 
            IForumPostService forumPostService, IForumTopicService forumTopicService, 
            IForumBaseService forumBaseService, IForumModelService forumModelService, 
            IForumCategoryService forumCategoryService)
        {
            _forumService = forumService;
            _mapper = mapper;
            _userService = userService;
            _env = env;
            _logger = logger;
            _forumPostService = forumPostService;
            _forumTopicService = forumTopicService;
            _forumBaseService = forumBaseService;
            _forumModelService = forumModelService;
            _forumCategoryService = forumCategoryService;
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        public async Task<IActionResult> ForumHome()
        {
            var model = await _forumModelService.GetForumCategoriesAndForumBasesForModel();
            return View("~/Views/Forum/ForumHome.cshtml", model);
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        public async Task<ActionResult> DeleteForumBase(int categoryId, int forumId)
        {
            int userId = 0;
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var forumUser = _userService.GetForumUser(userId);

            if (forumUser == null)
                return Unauthorized("Unauthorized access.");

            int userForumPosts = 0;
            var userTopics = await _forumTopicService.GetForumTopics(categoryId, forumId);

            foreach(var topic in userTopics)
            {
                var userTopicPosts = 
                    await _forumTopicService
                    .GetTopicPosts(categoryId, forumId, topic.Id, 0, 0, true);

                userForumPosts += userTopicPosts
                    .Where(p => p.ForumUserId.Equals(userId))
                    .Count();
            }

            var res = await _forumBaseService.DeleteForumBase(categoryId, forumId);

            if (res)
            {
                var resUserPostCounter = await _forumPostService
                    .UpdatePostCounterForUser(userId, false, userForumPosts);
            }
            else
            {
                return BadRequest("Cannot delete forum base with id: " + forumId);
            }

            return RedirectToAction("ForumHome");
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        public async Task<IActionResult> RedirectToCreateCategory()
        {
            return View("~/Views/Forum/Add/ForumAddCategory.cshtml");
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        [Route("categories/{categoryId}/forums/{forumId}/topics", Name = "ForumTopics")]
        public async Task<IActionResult> ForumTopics(int categoryId, int forumId)
        {
            var model = await _forumModelService.GetForumTopicsForModel(categoryId, forumId);
            await _forumBaseService.IncreaseViewCounterForForumBase(categoryId, forumId);

            return View("~/Views/Forum/ForumBase.cshtml", model);
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        public async Task<ActionResult> DeleteTopic(int categoryId, int forumId, int topicId)
        {
            int userId = 0;
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);
            var forumUser = _userService.GetForumUser(userId);

            if (forumUser == null)
                return Unauthorized("Unauthorized access.");

            var userTopicPosts = await _forumTopicService.GetTopicPosts(categoryId, forumId, topicId, 0, 0, true);
            var res = await _forumTopicService.DeleteForumTopic(categoryId, forumId, topicId);

            if (res)
            {
                var userTopicPostCounter = userTopicPosts
                    .Where(p => p.ForumUserId.Equals(userId))
                    .Count();
                var resUserPostCounter = await _forumPostService
                    .UpdatePostCounterForUser(userId, false, userTopicPostCounter);
            }
            else
            {
                return BadRequest("Cannot delete topic with ID " + topicId);
            }

            return RedirectToAction("ForumTopics", new { categoryId = categoryId, forumId = forumId });
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        [Route("categories/{categoryId}/forums/{forumId}/topics/{topicId}/{pageId}", Name = "TopicPosts")]
        public async Task<IActionResult> TopicPosts(int categoryId, int forumId, int topicId, int pageId = 0)
        {
            int maxiumPostsPerPage = 4;
            var model = await _forumModelService
                .GetTopicPostsForModel(categoryId, forumId, topicId, pageId, maxiumPostsPerPage);

            // TODO. Refactoring
            var tasks = model.Posts.Select(
                async p => new
                {
                    Item = p,
                    Dto = await _userService.GetForumUserDto(p.ForumUser.Id)
                });
            var tuples = await Task.WhenAll(tasks);

            foreach (var (user, item) in model.Posts
                .SelectMany(user => tuples
                .Where(item => user.ForumUser.Id == item.Item.ForumUser.Id)
                .Select(item => (user, item))))
            {
                user.ForumUser.TotalPostCounter = item.Dto.TotalPostCounter;
                user.ForumUser.AvatarImgSrc = user.ForumUser.LoadAvatar(_env.WebRootPath);
            }

            var updateCounterRes = await _forumTopicService.IncreaseViewCounterForTopic(categoryId, forumId, topicId);

            return View("~/Views/Forum/ForumTopic.cshtml", model);
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        public async Task<ActionResult> DeletePost(int categoryId, int forumId, int topicId, int postId, ForumTopicViewModel model)
        {
            var res = await _forumPostService.DeleteForumPost(categoryId, forumId, topicId, postId);
            int totalPosts = 0;

            int userId = 0;
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

            if (res)
            {
                var resCounter = await _forumPostService.UpdatePostCounter(categoryId, false);
                var resUserCounter = await _forumPostService.UpdatePostCounterForUser(userId, false);
                model.TotalPosts = await _forumPostService.GetTopicPostCount(categoryId);
            }
            else
            {
                return BadRequest("Cannot delete post with ID " + postId);
            }
            
            return Json(new { redirectToUrl = Url.Action("TopicPosts", "ForumHome", 
                new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = model.TotalPages }) });
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        public async Task<ActionResult> UpdatePost(int categoryId, int forumId, int topicId, int postId, int pageId, string newText)
        {
            var res = await _forumPostService.UpdatePost(categoryId, forumId, topicId, postId, newText);

            return Json(new { redirectToUrl = Url.Action("TopicPosts", "ForumHome", 
                new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = pageId }) });
        }
        [HttpCacheIgnore]
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        public async Task<IActionResult> ForumUserPage(int id)
        {
            var user = await _userService.GetForumUser(id);

            if (user == null)
            {
                return NotFound($"User with id {id} not found.");
            }

            var model = _mapper.Map<ForumUserPageViewModel>(user);
            model.AvatarImgSrc = user.LoadAvatar(_env.WebRootPath);

            return View("~/Views/Forum/User/ForumUserPage.cshtml", model);
        }
        [HttpCacheIgnore]
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        public async Task<IActionResult> UpdateForumUserPage(int id, ForumUserPageViewModel model)
        {
            var user = await _userService.GetForumUser(id);
            var appUserDto = _mapper.Map<AppUserDto>(model);
            var res = await _userService.UpdateAppUser(id, appUserDto);

            if (!res)
            {
                return BadRequest($"Unable to update app user with id: {id}");
            }

            model.AvatarImgSrc = user.LoadAvatar(_env.WebRootPath);

            return View("~/Views/Forum/User/ForumUserPage.cshtml", model);
        }
    }
}