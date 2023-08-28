using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Forum.ActionsFilters.Consumer.Forum;
using Interfaces.Forum;
using Interfaces.Forum.ApiServices;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumCategoryCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IForumCategoryService _forumCategoryService;

        public ForumCategoryCreationController(IMapper mapper, IForumService forumService,
            IForumCategoryService forumCategoryService)
        {
            _mapper = mapper;
            _forumCategoryService = forumCategoryService;
        }
        [HttpGet]
        public async Task<IActionResult> RedirectToCreateCategory()
        {
            return View("~/Views/Forum/Add/ForumAddCategory.cshtml");
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        [HttpPost]
        public async Task<IActionResult> RedirectToCreateCategory(ForumCategoryCreationView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            int userId = (int)HttpContext.Items["userId"];
            var categoryToAdd = _mapper.Map<ForumCategoryForCreationDto>(model);

            if (userId > 0)
            {
                categoryToAdd.ForumUserId = userId;
                var res = await _forumCategoryService.CreateForumCategory(categoryToAdd);
            }
            else
            {
                return BadRequest("User ID error.");
            }

            return RedirectToAction("ForumHome", "ForumHome");
        }
    }
}