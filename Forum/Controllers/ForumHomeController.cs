using Entities.ViewModels.Forum;
using Interfaces.Forum;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers
{
    public class ForumHomeController : Controller
    {
        private readonly IForumService forumService;

        public ForumHomeController(IForumService forumService)
        {
            this.forumService = forumService;
        }

        public async Task<IActionResult> ForumHome()
        {
            ForumHomeViewModel viewModel = new ForumHomeViewModel();
            viewModel.Categories = await forumService.GetForumCategories();

            return View("~/Views/Forum/ForumHome.cshtml", viewModel);
        }
    }
}
