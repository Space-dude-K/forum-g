using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Forum.Controllers.Forum
{
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
            //ForumCategoryCreationView modelD = JsonConvert.DeserializeObject<ForumHomeViewModel>(model);
            var catAddModel = _mapper.Map<ForumBaseCreationView>(JsonConvert.DeserializeObject<ForumHomeViewModel>(model));
            return View("~/Views/Forum/Add/ForumAddForumBase.cshtml", catAddModel);
        }
        [HttpPost]
        public async Task<IActionResult> RedirectToCreateForumBase(ForumBaseCreationView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var forumToAdd = _mapper.Map<ForumBaseForCreationDto>(model);
            await _forumService.CreateForumBase(model.SelectedCategoryId, forumToAdd);

            return RedirectToAction("ForumHome", "ForumHome");
        }
    }
}
