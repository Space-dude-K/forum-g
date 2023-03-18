using AutoMapper;
using Contracts;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using Forum.ModelBinders;
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
        public IActionResult CreateCategory([FromBody] ForumCategoryForCreationDto category)
        {
            if (category == null)
            {
                _logger.LogError("CompanyForCreationDto object sent from client is null.");
                return BadRequest("CompanyForCreationDto object is null");
            }

            var categoryEntity = _mapper.Map<ForumCategory>(category);

            _repository.ForumCategory.CreateCategory(categoryEntity);
            _repository.Save();

            var categoryToReturn = _mapper.Map<ForumCategoryDto>(categoryEntity);

            return CreatedAtRoute("CategoryById", new { id = categoryToReturn.Id }, categoryToReturn);
        }
        [HttpGet("collection/({ids})", Name = "CategoryCollection")]
        public IActionResult GetCategoryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var categoryEntities = _repository.ForumCategory.GetCategoriesByIds(ids, trackChanges: false);
            if (ids.Count() != categoryEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var categoriesToReturn = _mapper.Map<IEnumerable<ForumCategoryDto>>(categoryEntities);
            return Ok(categoriesToReturn);
        }
        [HttpPost("collection")]
        public IActionResult CreateCategoryCollection([FromBody] IEnumerable<ForumCategoryForCreationDto> categoryCollection)
        {
            if (categoryCollection == null)
            {
                _logger.LogError("Company collection sent from client is null.");
                return BadRequest("Company collection is null");
            }
            var categoryEntities = _mapper.Map<IEnumerable<ForumCategory>>(categoryCollection);
            foreach (var category in categoryEntities)
            {
                _repository.ForumCategory.CreateCategory(category);
            }
            _repository.Save();
            var categoryCollectionToReturn =  _mapper.Map<IEnumerable<ForumCategoryDto>>(categoryEntities);
            var ids = string.Join(",", categoryCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CategoryCollection", new { ids }, categoryCollectionToReturn);
        }
        [HttpDelete("{categoryId}")]
        public IActionResult DeleteCategory(int categoryId)
        {
            var category = _repository.ForumCategory.GetCategory(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Company with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }
            _repository.ForumCategory.DeleteCategory(category);
            _repository.Save();
            return NoContent();
        }
    }
}
