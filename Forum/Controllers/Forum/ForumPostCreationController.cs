using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Interfaces.Forum.ApiServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumPostCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IForumService _forumService;
        private readonly IForumPostService _forumPostService;

        public ForumPostCreationController(IMapper mapper, IForumService forumService, IForumPostService forumPostService)
        {
            _mapper = mapper;
            _forumService = forumService;
            _forumPostService = forumPostService;
        }
        [HttpPost]
        [Route("ForumPostCreation/CreateForumPost")]
        public async Task<IActionResult> CreateForumPost(int categoryId, int forumId, int topicId, int totalPages, ForumTopicViewModel model)
        {
            /*if (!ModelState.IsValid)
                return BadRequest(ModelState);*/

            int userId = 0;
            var postToAdd = _mapper.Map<ForumPostForCreationDto>(model);

            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

            if(userId > 0)
            {
                postToAdd.ForumUserId = userId;
                var res = await _forumPostService.CreateForumPost(categoryId, forumId, topicId, postToAdd);
                var resCounter = await _forumPostService.UpdatePostCounter(topicId, true);
                var resUserCounter = await _forumPostService.UpdatePostCounterForUser(userId, true);

                model.TotalPosts = await _forumPostService.GetTopicPostCount(topicId);

            }
            else
            {
                return BadRequest("User ID error.");
            }

            return Json(new { redirectToUrl = Url.Action("TopicPosts", "ForumHome", 
                new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = model.TotalPages }) });
        }
    }
}
