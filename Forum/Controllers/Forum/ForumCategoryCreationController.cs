using AutoMapper;
using Entities.DTO.ForumDto.Create;
using Entities.ViewModels.Forum;
using Forum.ActionsFilters.Consumer.Forum;
using Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers.Forum
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ForumCategoryCreationController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryApiManager _repositoryApiManager;

        public ForumCategoryCreationController(IMapper mapper,IRepositoryApiManager repositoryApiManager)
        {
            _mapper = mapper;
            _repositoryApiManager = repositoryApiManager;
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
                var res = await _repositoryApiManager.CategoryApis.CreateForumCategory(categoryToAdd);
            }
            else
            {
                return BadRequest("User ID error.");
            }

            return RedirectToAction("ForumHome", "ForumHome");
        }
    }
}