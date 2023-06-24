using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Forum.Controllers.Forum
{
    public class ForumCategoryCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IForumService _forumService;

        public ForumCategoryCreationController(IMapper mapper, IForumService forumService)
        {
            _mapper = mapper;
            _forumService = forumService;
        }
        [HttpGet]
        public async Task<IActionResult> RedirectToCreateCategory()
        {
            return View("~/Views/Forum/Add/ForumAddCategory.cshtml");
        }
        [HttpPost]
        public async Task<IActionResult> RedirectToCreateCategory(ForumCategoryCreationView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryToAdd = _mapper.Map<ForumCategoryForCreationDto>(model);
            var res = await _forumService.CreateForumCategory(categoryToAdd);

            return RedirectToAction("ForumHome", "ForumHome");
        }
    }
}