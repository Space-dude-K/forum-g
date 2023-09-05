using AutoMapper;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Entities.DTO.FileDto;
using Entities.Models.File;
using System.Drawing.Imaging;
using System.Drawing;
using Forum.Extensions;
using Forum.ActionsFilters.Consumer.Forum;

namespace Forum.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class FileController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;
        private readonly ILoggerManager _logger;
        private readonly IRepositoryApiManager _repositoryApiManager;

        public FileController(IMapper mapper, IWebHostEnvironment env, ILoggerManager logger, 
            IRepositoryApiManager repositoryApiManager)
        {
            _mapper = mapper;
            _env = env;
            _logger = logger;
            _repositoryApiManager = repositoryApiManager;
        }
        [RequestSizeLimit(1000 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 1000 * 1024 * 1024)]
        [HttpPost]
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        public async Task<IActionResult> UploadFilesForUser(int createdPostId, List<IFormFile> files)
        {
            int userId = (int)HttpContext.Items["userId"];

            // TODO
            if(createdPostId == 0)
            {
                return BadRequest("Bad request for file creation");
            }

            if(files.Count > 0)
            {
                foreach(var file in  files)
                {
                    var fileExt = Path.GetExtension(file.FileName);
                    var fileName = Path.GetFileNameWithoutExtension(file.FileName) 
                        + DateTime.Now.ToString("_dd-MM-yyyy-HH-mm-ss.fff") + fileExt;
                    string filePath = "/attachments/" + User.Identity.Name + "/" + fileName;

                    ForumFile forumFile = new() { Name = fileName, Path = filePath, ForumUserId = userId, ForumPostId = createdPostId };
                    var fileToDb = _mapper.Map<ForumFileDto>(forumFile);

                    // TODO. Path.Combine
                    string fullPathToFile = _env.WebRootPath + filePath;

                    Directory.CreateDirectory(Path.GetDirectoryName(fullPathToFile));

                    using (Stream fileStream = new FileStream(fullPathToFile, FileMode.CreateNew))
                    {
                        await file.CopyToAsync(fileStream);
                    }

                    var createRes = await _repositoryApiManager.FileApis
                        .CreateForumFile(fileToDb);

                    _logger.LogInfo($"Creating file status for user id: {userId} -> {createRes}");
                }
            }

            return RedirectToAction("ForumUserPage", "ForumHome", new { id = userId });
        }
        [RequestSizeLimit(100 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 100 * 1024 * 1024)]
        [HttpPost]
        [ServiceFilter(typeof(ValidateAuthenticationAttribute))]
        public async Task<IActionResult> UploadFileForUser(IFormFile uploadedFile)
        {
            int userId = (int)HttpContext.Items["userId"];

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

                var fileFromDb = await _repositoryApiManager.FileApis
                    .GetForumFileByUserId(userId);

                if (fileFromDb != null)
                {
                    var updateRes = _repositoryApiManager.FileApis
                        .UpdateForumFile(fileFromDb.ForumUserId, fileToDb);

                    _logger.LogInfo($"Updating file status for user id: {userId} -> {updateRes}");
                }
                else
                {
                    var createRes = await _repositoryApiManager.FileApis
                        .CreateForumFile(fileToDb);

                    _logger.LogInfo($"Creating file status for user id: {userId} -> {createRes}");
                }
            }

            return RedirectToAction("ForumUserPage", "ForumHome", new { id = userId });
        }
    }
}