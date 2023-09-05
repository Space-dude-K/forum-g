using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Forum.ActionsFilters.Consumer.Forum;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumPostCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryApiManager _repositoryApiManager;

        public ForumPostCreationController(IMapper mapper, IRepositoryApiManager repositoryApiManager)
        {
            _mapper = mapper;
            _repositoryApiManager = repositoryApiManager;
        }
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        [HttpPost]
        [Route("ForumPostCreation/CreateForumPost")]
        public async Task<IActionResult> CreateForumPost(int categoryId, int forumId, int topicId, int totalPages, ForumTopicViewModel model)
        {
            /*if (!ModelState.IsValid)
                return BadRequest(ModelState);*/

            int createdPostId = 0;
            int userId = (int)HttpContext.Items["userId"];
            var postToAdd = _mapper.Map<ForumPostForCreationDto>(model);

            if(userId > 0)
            {
                postToAdd.ForumUserId = userId;
                createdPostId = await _repositoryApiManager.PostApis.CreateForumPost(categoryId, forumId, topicId, postToAdd);
                var resCounter = await _repositoryApiManager.PostApis.UpdatePostCounter(topicId, true);
                var resUserCounter = await _repositoryApiManager.PostApis.UpdatePostCounterForUser(userId, true);

                model.TotalPosts = await _repositoryApiManager.PostApis.GetTopicPostCount(topicId);

            }
            else
            {
                return BadRequest("User ID error.");
            }

            //return Json(new { createdPostId = createdPostId });
            return Json(new { createdPostId, 
                redirectToUrl = Url.Action("TopicPosts", "ForumHome", new { categoryId, forumId, topicId, pageId = model.TotalPages }) });
        }
    }
}