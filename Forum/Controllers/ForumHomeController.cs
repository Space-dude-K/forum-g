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

        public ForumHomeController(IForumService forumService, 
            IMapper mapper, IUserService  userService, IWebHostEnvironment env, ILoggerManager logger)
        {
            _forumService = forumService;
            _mapper = mapper;
            _userService = userService;
            _env = env;
            _logger = logger;
        }
        public async Task<IActionResult> ForumHome()
        {
            var model = await _forumService.GetForumCategoriesAndForumBasesForModel();

            if(!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Unauthorized access.");         
            }

            return View("~/Views/Forum/ForumHome.cshtml", model);
        }
        public async Task<IActionResult> RedirectToCreateCategory()
        {
            return View("~/Views/Forum/Add/ForumAddCategory.cshtml");
        }
        [Route("categories/{categoryId}/forums/{forumId}/topics", Name = "ForumTopics")]
        public async Task<IActionResult> ForumTopics(int categoryId, int forumId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Unauthorized access.");
            }

            var model = await _forumService.GetForumTopicsForModel(categoryId, forumId);
            await _forumService.IncreaseViewCounterForForumBase(categoryId, forumId);

            return View("~/Views/Forum/ForumBase.cshtml", model);
        }
        [Route("categories/{categoryId}/forums/{forumId}/topics/{topicId}/{pageId}", Name = "TopicPosts")]
        public async Task<IActionResult> TopicPosts(int categoryId, int forumId, int topicId, int pageId = 0)
        {
            int maxiumPostsPerPage = 4;

            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Unauthorized access.");
            }

            var model = await _forumService.GetTopicPostsForModel(categoryId, forumId, topicId, pageId, maxiumPostsPerPage);

            var tasks = model.Posts.Select(
                async p => new
                {
                    Item = p,
                    Dto = await _userService.GetForumUserDto(p.ForumUser.Id)
                });
            var tuples = await Task.WhenAll(tasks);

            // TODO. Refactoring
            foreach (var (user, item) in model.Posts
                .SelectMany(user => tuples
                .Where(item => user.ForumUser.Id == item.Item.ForumUser.Id)
                .Select(item => (user, item))))
            {
                user.ForumUser.TotalPostCounter = item.Dto.TotalPostCounter;
                user.ForumUser.AvatarImgSrc = user.ForumUser.LoadAvatar(_env.WebRootPath);
            }

            return View("~/Views/Forum/ForumTopic.cshtml", model);
        }
        public async Task<ActionResult> DeletePost(int categoryId, int forumId, int topicId, int postId, ForumTopicViewModel model)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Unauthorized access.");
            }

            var res = await _forumService.DeleteForumPost(categoryId, forumId, topicId, postId);
            int totalPosts = 0;

            int userId = 0;
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

            if (res)
            {
                var resCounter = await _forumService.UpdatePostCounter(categoryId, false);
                var resUserCounter = await _forumService.UpdatePostCounterForUser(userId, false);
                model.TotalPosts = await _forumService.GetTopicPostCount(categoryId);
            }
            else
            {
                return BadRequest("Cannot delete post with ID " + postId);
            }
            
            return Json(new { redirectToUrl = Url.Action("TopicPosts", "ForumHome", 
                new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = model.TotalPages }) });
        }
        public async Task<ActionResult> UpdatePost(int categoryId, int forumId, int topicId, int postId, int pageId, string newText)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Unauthorized access.");
            }

            var res = await _forumService.UpdatePost(categoryId, forumId, topicId, postId, newText);

            return Json(new { redirectToUrl = Url.Action("TopicPosts", "ForumHome", 
                new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = pageId }) });
        }
        [HttpCacheIgnore]
        public async Task<IActionResult> ForumUserPage(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Unauthorized access.");
            }

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
        public async Task<IActionResult> UpdateForumUserPage(int id, ForumUserPageViewModel model)
        {
            var user = await _userService.GetForumUser(id);
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Unauthorized access.");
            }

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