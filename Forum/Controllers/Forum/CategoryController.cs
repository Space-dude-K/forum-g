using AutoMapper;
using Contracts;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;
using Forum.ActionsFilters;
using Forum.ActionsFilters.Forum;
using Forum.ModelBinders;
using Microsoft.AspNetCore.Mvc;

namespace Forum.Controllers.Forum
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
        public async Task<IActionResult> GetForumCategories()
        {
            var categories = await _repository.ForumCategory.GetAllCategoriesAsync(trackChanges: false);
            var categoriesDto = _mapper.Map<IEnumerable<ForumCategoryDto>>(categories);

            return Ok(categoriesDto);
        }
        [HttpGet("{id}", Name = "CategoryById")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var category = await _repository.ForumCategory.GetCategoryAsync(id, trackChanges: false);
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCategory([FromBody] ForumCategoryForCreationDto category)
        {
            var categoryEntity = _mapper.Map<ForumCategory>(category);

            _repository.ForumCategory.CreateCategory(categoryEntity);
            await _repository.SaveAsync();

            var categoryToReturn = _mapper.Map<ForumCategoryDto>(categoryEntity);

            return CreatedAtRoute("CategoryById", new { id = categoryToReturn.Id }, categoryToReturn);
        }
        [HttpGet("collection/({ids})", Name = "CategoryCollection")]
        public async Task<IActionResult> GetCategoryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            var categoryEntities = await _repository.ForumCategory.GetCategoriesByIdsAsync(ids, trackChanges: false);
            if (ids.Count() != categoryEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var categoriesToReturn = _mapper.Map<IEnumerable<ForumCategoryDto>>(categoryEntities);
            return Ok(categoriesToReturn);
        }
        [HttpPost("collection")]
        public async Task<IActionResult> CreateCategoryCollection([FromBody] IEnumerable<ForumCategoryForCreationDto> categoryCollection)
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
            await _repository.SaveAsync();
            var categoryCollectionToReturn = _mapper.Map<IEnumerable<ForumCategoryDto>>(categoryEntities);
            var ids = string.Join(",", categoryCollectionToReturn.Select(c => c.Id));
            return CreatedAtRoute("CategoryCollection", new { ids }, categoryCollectionToReturn);
        }
        [HttpDelete("{categoryId}")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            var category = HttpContext.Items["category"] as ForumCategory;

            _repository.ForumCategory.DeleteCategory(category);
            await _repository.SaveAsync();
            return NoContent();
        }
        [HttpPut("{categoryId}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] ForumCategoryForUpdateDto category)
        {
            // TODO User id for collection PUT

            var categoryEntity = HttpContext.Items["category"] as ForumCategory;

            _mapper.Map(category, categoryEntity);
            await _repository.SaveAsync();
            return NoContent();
        }

    }
}
