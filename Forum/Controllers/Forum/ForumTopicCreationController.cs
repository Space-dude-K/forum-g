using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Services;

namespace Forum.Controllers.Forum
{
    public class ForumTopicCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IForumService _forumService;
        private IMemoryCache _cache;

        public ForumTopicCreationController(IMapper mapper, IForumService forumService, IMemoryCache cache)
        {
            _mapper = mapper;
            _forumService = forumService;
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
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
        [Route("categories/{categoryId}/forums/{forumId}/add", Name = "ForumTopicCreate")]
        public async Task<IActionResult> CreateForumTopic(int categoryId, int forumId, ForumTopicCreationView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var topicToAdd = _mapper.Map<ForumTopicForCreationDto>(model);
            var res = await _forumService.CreateForumTopic(categoryId, forumId, topicToAdd);

            return RedirectToAction("ForumTopics", "ForumHome", 
                new { categoryId = categoryId, forumId = forumId });
        }
    }
}
