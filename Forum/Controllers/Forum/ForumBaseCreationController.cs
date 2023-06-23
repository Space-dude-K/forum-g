using AutoMapper;
using Entities.ViewModels.Forum;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Forum.Controllers.Forum
{
    public class ForumBaseCreationController : Controller
    {
        private readonly IMapper _mapper;

        public ForumBaseCreationController(IMapper mapper)
        {
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
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
            return View("~/Views/Forum/Add/ForumAddForumBase.cshtml", model);
        }
    }
}
