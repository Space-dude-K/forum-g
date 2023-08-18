using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Security.Claims;

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
                var res = await _forumService.CreateForumPost(categoryId, forumId, topicId, postToAdd);
                var resCounter = await _forumService.UpdatePostCounter(categoryId, true);
                var resUserCounter = await _forumService.UpdatePostCounterForUser(userId, true);

                model.TotalPosts = await _forumService.GetTopicPostCount(categoryId);
            }
            else
            {
                return BadRequest("User ID error.");
            }

            return Json(new { redirectToUrl = Url.Action("TopicPosts", "ForumHome", new { categoryId = categoryId, forumId = forumId, topicId = topicId, pageId = model.TotalPages }) });
        }
    }
}
