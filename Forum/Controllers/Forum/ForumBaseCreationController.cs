using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Forum.ActionsFilters.Consumer.Forum;
using Interfaces.Forum;
using Interfaces.Forum.ApiServices;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumBaseCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IForumBaseService _forumBaseService;

        public ForumBaseCreationController(IMapper mapper, IForumService forumService, IForumBaseService forumBaseService)
        {
            _mapper = mapper;
            _forumBaseService = forumBaseService;
        }
        [HttpGet]
        public async Task<IActionResult> RedirectToCreateForumBase(string model)
        {
            if(string.IsNullOrEmpty(model))
                return BadRequest(ModelState);

            var catAddModel = _mapper.Map<ForumBaseCreationView>(JsonConvert.DeserializeObject<ForumHomeViewModel>(model));
            return View("~/Views/Forum/Add/ForumAddForumBase.cshtml", catAddModel);
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        [HttpPost]
        public async Task<IActionResult> RedirectToCreateForumBase(ForumBaseCreationView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = (int)HttpContext.Items["userId"];
            var forumToAdd = _mapper.Map<ForumBaseForCreationDto>(model);

            if (userId > 0)
            {
                forumToAdd.ForumUserId = userId;
                var res = await _forumBaseService.CreateForumBase(model.SelectedCategoryId, forumToAdd);
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