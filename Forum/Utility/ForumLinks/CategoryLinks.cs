using Contracts;
using Entities.DTO.ForumDto;
using Entities.LinksModels;
using Entities.Models;
using Entities.RequestFeatures.Forum;
using Microsoft.Net.Http.Headers;

namespace Forum.Utility.ForumLinks
{
    public class CategoryLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<ForumCategoryDto> _dataShaper;
        public CategoryLinks(LinkGenerator linkGenerator, IDataShaper<ForumCategoryDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }
        public LinkResponse TryGenerateLinks(IEnumerable<ForumCategoryDto> categoriesDto, string fields, HttpContext httpContext)
        {
            var shapedEmployees = ShapeData(categoriesDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedCategories(categoriesDto, fields, httpContext, shapedEmployees);

            return ReturnShapedCategories(shapedEmployees);
        }
        private List<Entity> ShapeData(IEnumerable<ForumCategoryDto> categoriesDto, string fields)
        {
            return _dataShaper.ShapeData(categoriesDto, fields)
             .Select(e => e.Entity)
             .ToList();
        }
        private bool ShouldGenerateLinks(HttpContext httpContext)
        {
            var mediaType = (MediaTypeHeaderValue)httpContext.Items["AcceptHeaderMediaType"];

            return mediaType.SubTypeWithoutSuffix.EndsWith("hateoas", StringComparison.InvariantCultureIgnoreCase);
        }
        private LinkResponse ReturnShapedCategories(List<Entity> shapedCategories)
        {
            return new LinkResponse { ShapedEntities = shapedCategories };
        }
        private LinkResponse ReturnLinkdedCategories(IEnumerable<ForumCategoryDto> categoriesDto, string fields, HttpContext httpContext, List<Entity> shapedCategories)
        {
            var categoriesDtoList = categoriesDto.ToList();

            for (var index = 0; index < categoriesDtoList.Count(); index++)
            {
                var categoryLinks = CreateLinksForCategory(httpContext, categoriesDtoList[index].Id, fields);
                shapedCategories[index].Add("Links", categoryLinks);
            }

            var categoryCollection = new LinkCollectionWrapper<Entity>(shapedCategories);
            var linkedCategories = CreateLinksForCategories(httpContext, categoryCollection);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedCategories };
        }
        private List<Link> CreateLinksForCategory(HttpContext httpContext, int categoryId, string fields = "")
        {
            var forumCategoryParameters = new ForumCategoryParameters(fields);

            var links = new List<Link>
            {
                 new Link(_linkGenerator.GetUriByAction(httpContext, "GetCategory", values: new { categoryId, fields }), "self", "GET"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateCategory", values: new { categoryId }), "update_category", "PUT"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateCategory", values: new { categoryId }), "partially_update_category", "PATCH"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteCategory", values: new { categoryId }), "delete_category", "DELETE"),
             };

            return links;
        }
        private LinkCollectionWrapper<Entity> CreateLinksForCategories(HttpContext httpContext, LinkCollectionWrapper<Entity> categoriesWrapper)
        {
            categoriesWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetForumCategories", values: new { }), "self", "GET"));

            return categoriesWrapper;
        }
    }
}