using AutoMapper;
using Interfaces.Forum;
using Interfaces.User;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Entities.DTO.FileDto;
using Entities.Models.File;
using System.Drawing.Imaging;
using System.Security.Claims;
using System.Drawing;
using Forum.Extensions;
using Interfaces.Forum.ApiServices;
using Forum.ActionsFilters.Consumer.Forum;

namespace Forum.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FileController : Controller
    {
        private readonly IForumService _forumService;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IWebHostEnvironment _env;
        private readonly ILoggerManager _logger;
        private readonly IForumTopicService _forumTopicService;

        public FileController(IForumService forumService,
            IMapper mapper, IUserService userService, IWebHostEnvironment env, ILoggerManager logger, IForumTopicService forumTopicService)
        {
            _forumService = forumService;
            _mapper = mapper;
            _userService = userService;
            _env = env;
            _logger = logger;
            _forumTopicService = forumTopicService;
        }
        [ServiceFilter(typeof(ValidateAuthorizeAttribute))]
        [HttpPost]
        public async Task<IActionResult> UploadFileForUser(IFormFile uploadedFile)
        {
            int userId = 0;
            int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out userId);

            if (uploadedFile != null)
            {
                var fileExt = Path.GetExtension(uploadedFile.FileName);
                var fileName = User.Identity.Name + "_" + userId.ToString() + fileExt;
                string filePath = "/images/avatars/" + fileName;

                using (var memoryStream = new MemoryStream())
                {
                    await uploadedFile.CopyToAsync(memoryStream);
                    using (var img = Image.FromStream(memoryStream))
                    {
                        var rImg = (Image)img.ResizeImage(120, 96);
                        rImg.Save(_env.WebRootPath + filePath, ImageFormat.Jpeg);
                    }
                }

                ForumFile file = new() { Name = fileName, Path = filePath };
                var fileToDb = _mapper.Map<ForumFileDto>(file);
                fileToDb.ForumUserId = userId;

                var fileFromDb = await _forumService.GetForumFileByUserId(userId);
                if (fileFromDb != null)
                {
                    var updateRes = _forumService.UpdateForumFile(fileFromDb.ForumUserId, fileToDb);
                    _logger.LogInfo($"Updating file status for user id: {userId} -> {updateRes}");
                }
                else
                {
                    var createRes = await _forumService.CreateForumFile(fileToDb);
                    _logger.LogInfo($"Creating file status for user id: {userId} -> {createRes}");
                }
            }

            return RedirectToAction("ForumUserPage", "ForumHome", new { id = userId });
        }
    }
}