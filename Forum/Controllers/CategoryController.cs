using AutoMapper;
using Contracts;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Forum.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CategoryController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetForumCategories()
        {
            var categories = _repository.ForumCategory.GetAllCategories(trackChanges: false);
            var categoriesDto = _mapper.Map<IEnumerable<ForumCategoryDto>>(categories);

            return Ok(categoriesDto);
        }
        [HttpGet("{id}", Name = "CategoryById")]
        public IActionResult GetCategory(int id)
        {
            var category = _repository.ForumCategory.GetCategory(id, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Company with id: {id} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var categoryDto = _mapper.Map<ForumCategoryDto>(category);
                return Ok(categoryDto);
            }
        }
        [HttpPost]
        public IActionResult CreateCompany([FromBody] ForumCategoryForCreationDto category)
        {
            if (category == null)
            {
                _logger.LogError("CompanyForCreationDto object sent from client is null.");
                return BadRequest("CompanyForCreationDto object is null");
            }
            var categoryEntity = _mapper.Map<ForumCategory>(category);
            categoryEntity.CreatedAt = DateTime.Now;
            // TODO
            categoryEntity.ForumUserId = 1;
            _repository.ForumCategory.CreateCategory(categoryEntity);
            _repository.Save();

            var categpryToReturn = _mapper.Map<ForumCategoryDto>(categoryEntity);

            return CreatedAtRoute("CategoryById", new { id = categpryToReturn.Id }, categpryToReturn);
        }
    }
}
