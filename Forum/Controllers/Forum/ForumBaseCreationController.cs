using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumBaseCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IForumService _forumService;

        public ForumBaseCreationController(IMapper mapper, IForumService forumService)
        {
            _mapper = mapper;
            _forumService = forumService;
        }
        [HttpGet]
        public async Task<IActionResult> RedirectToCreateForumBase(string model)
        {
            if(string.IsNullOrEmpty(model))
                return BadRequest(ModelState);

            var catAddModel = _mapper.Map<ForumBaseCreationView>(JsonConvert.DeserializeObject<ForumHomeViewModel>(model));
            return View("~/Views/Forum/Add/ForumAddForumBase.cshtml", catAddModel);
        }
        [HttpPost]
        public async Task<IActionResult> RedirectToCreateForumBase(ForumBaseCreationView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = 0;
            var forumToAdd = _mapper.Map<ForumBaseForCreationDto>(model);

            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

            if (userId > 0)
            {
                forumToAdd.ForumUserId = userId;
                var res = await _forumService.CreateForumBase(model.SelectedCategoryId, forumToAdd);
                //var resCounter = await _forumService.UpdatePostCounter(categoryId, true);
            }
            else
            {
                return BadRequest("User ID error.");
            }

            return RedirectToAction("ForumHome", "ForumHome");
        }
    }
}