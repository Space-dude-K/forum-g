using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
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

            int userId = 0;
            var categoryToAdd = _mapper.Map<ForumCategoryForCreationDto>(model);

            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

            if (userId > 0)
            {
                categoryToAdd.ForumUserId = userId;
                var res = await _forumService.CreateForumCategory(categoryToAdd);
            }
            else
            {
                return BadRequest("User ID error.");
            }

            return RedirectToAction("ForumHome", "ForumHome");
        }
    }
}