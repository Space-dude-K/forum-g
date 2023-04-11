using AutoMapper;
using Contracts;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;
using Entities.RequestFeatures.Forum;
using Forum.ActionsFilters;
using Forum.ActionsFilters.Forum;
using Forum.ModelBinders;
using Forum.Utility.ForumLinks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.Design;

namespace Forum.Controllers.Forum
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly CategoryLinks _categoryLinks;

        // TODO. Service layer for mappings and data shaping
        public CategoryController(IRepositoryManager repository, ILoggerManager logger, IMapper mapper, CategoryLinks categoryLinks)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            _categoryLinks = categoryLinks;
        }
        [HttpGet]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetForumCategories([FromQuery] ForumCategoryParameters forumCategoryParameters)
        {
            var categoriesFromDb = await _repository.ForumCategory.GetAllCategoriesAsync(forumCategoryParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(categoriesFromDb.MetaData));

            var categoriesDto = _mapper.Map<IEnumerable<ForumCategoryDto>>(categoriesFromDb);
            var links = _categoryLinks.TryGenerateLinks(categoriesDto, forumCategoryParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpGet("{categoryId}", Name = "GetCategoryById")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetCategory(int categoryId, [FromQuery] ForumCategoryParameters forumCategoryParameters)
        {
            var category = await _repository.ForumCategory.GetCategoryAsync(categoryId, trackChanges: false);
            if (category == null)
            {
                _logger.LogInfo($"Category with id: {categoryId} doesn't exist in the database.");
                return NotFound();
            }
            else
            {
                var categoryDto = _mapper.Map<ForumCategoryDto>(category);
                var links = _categoryLinks.TryGenerateLinks(new List<ForumCategoryDto>() { categoryDto }, forumCategoryParameters.Fields, HttpContext);

                return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
            }
        }
        [HttpGet("collection/({ids})", Name = "CategoryCollection")]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetCategoryCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids, 
            [FromQuery] ForumCategoryParameters forumCategoryParameters)
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
            var links = _categoryLinks.TryGenerateLinks(categoriesToReturn, forumCategoryParameters.Fields, HttpContext);

            return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }
        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCategory([FromBody] ForumCategoryForCreationDto category)
        {
            var categoryEntity = _mapper.Map<ForumCategory>(category);

            _repository.ForumCategory.CreateCategory(categoryEntity);
            await _repository.SaveAsync();

            var categoryToReturn = _mapper.Map<ForumCategoryDto>(categoryEntity);

            return CreatedAtRoute("GetCategoryById", new { id = categoryToReturn.Id }, categoryToReturn);
        }
        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
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
        [HttpPatch("{categoryId}")]
        [ServiceFilter(typeof(ValidateCategoryExistsAttribute))]
        public async Task<IActionResult> PartiallyUpdateCategory(int categoryId, [FromBody] JsonPatchDocument<ForumCategoryForUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                _logger.LogError("patchDoc object sent from client is null.");
                return BadRequest("patchDoc object is null");
            }
            var categoryEntity = HttpContext.Items["category"] as ForumCategory;

            var categoryToPatch = _mapper.Map<ForumCategoryForUpdateDto>(categoryEntity);
            patchDoc.ApplyTo(categoryToPatch, ModelState);

            TryValidateModel(categoryToPatch);

            if (!ModelState.IsValid)
            {
                _logger.LogError("Invalid model state for the patch document");
                return UnprocessableEntity(ModelState);
            }

            _mapper.Map(categoryToPatch, categoryEntity);

            _logger.LogInfo($"Part up {categoryToPatch.Name}");

            await _repository.SaveAsync();

            return NoContent();
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
    }
}