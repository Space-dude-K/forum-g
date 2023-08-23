using AutoMapper;
using Forum.ActionsFilters;
using Forum.Utility.UserLinks;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Entities.DTO.FileDto;
using Entities.Models.File;
using Entities.RequestFeatures.Forum;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;
using Forum.ActionsFilters.Forum;
using Forum.ActionsFilters.File;
using System.Diagnostics;
using Entities.DTO.FileDto.Manipulation;
using Entities.DTO.FileDto.Update;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace Forum.ApiControllers.File
{
    [Route("api")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator, User")]
    public class FileController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserDataLinks _userDataLinks;

        public FileController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, UserDataLinks userDataLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _userDataLinks = userDataLinks;
        }
        [HttpOptions]
        public IActionResult GetUserOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS");
            return Ok();
        }
        [HttpGet("file/{forumUserId}", Name = "GetForumFile")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(404)]
        //[ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetForumFileByUserId(int forumUserId)
        {
            if (forumUserId == 0)
                return BadRequest("Bad request. Missing forum user id.");

            var file = await _repository.ForumFile.GetFileAsync(forumUserId, trackChanges: false);

            if (file == null)
            {
                _logger.LogInfo($"File with user id: {forumUserId} doesn't exist in the database.");
                return NotFound();
            }

            return Ok(file);
        }
        /// <summary>
        /// Writes the file data for the user avatar
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="forum"></param>
        /// <returns>A newly created file data</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>
        [HttpPost("file", Name = "CreateForumFile")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateForumFile([FromBody] ForumFileDto file)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the ForumFileDto object");
                return UnprocessableEntity(ModelState);
            }

            var fileEntity = _mapper.Map<ForumFile>(file);

            _repository.ForumFile.CreateFile(fileEntity);
            await _repository.SaveAsync();

            return Ok();
        }
        [HttpPut("file/{forumUserId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateFileExistsAttribute))]
        public async Task<IActionResult> UpdateForumFile(int forumUserId, [FromBody] ForumFileForUpdateDto fileDto)
        {
            var file = HttpContext.Items["file"] as ForumFile;

            _mapper.Map(fileDto, file);

            await _repository.SaveAsync();

            return Ok();
        }
        [HttpPut("filef/{forumUserId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(422)]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateFileExistsAttribute))]
        public async Task<IActionResult> UpdateForumFileId(int forumUserId, [FromBody] ForumFileForUpdateDto fileDto)
        {
            var file = HttpContext.Items["file"] as ForumFile;

            _mapper.Map(fileDto, file);

            await _repository.SaveAsync();

            return Ok();
        }
    }
}
