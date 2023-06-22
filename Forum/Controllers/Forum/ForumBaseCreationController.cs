using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers.Forum
{
    public class ForumBaseCreationController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
