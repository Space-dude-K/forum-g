using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Forum.Controllers
{
    [Route("api/forums")]
    [ApiController]
    public class ForumController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        public ForumController(IRepositoryManager repository, ILoggerManager logger)
        {
            _repository = repository;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult GetForums()
        {
            try
            {
                var forums = _repository.ForumBase.GetAllForums(trackChanges: false);
                return Ok(forums);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the {nameof(GetForums)} action { ex}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
