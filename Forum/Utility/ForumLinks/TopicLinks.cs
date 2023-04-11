using Contracts;
using Entities.DTO.ForumDto;
using Entities.LinksModels;
using Entities.Models;
using Microsoft.Net.Http.Headers;

namespace Forum.Utility.ForumLinks
{
    public class TopicLinks
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IDataShaper<ForumTopicDto> _dataShaper;
        public TopicLinks(LinkGenerator linkGenerator, IDataShaper<ForumTopicDto> dataShaper)
        {
            _linkGenerator = linkGenerator;
            _dataShaper = dataShaper;
        }
        public LinkResponse TryGenerateLinks(IEnumerable<ForumTopicDto> topicsDto, int forumCategoryId, int forumBaseId, string fields, HttpContext httpContext,
            IEnumerable<int>? collectionIds = null)
        {
            var shapedEmployees = ShapeData(topicsDto, fields);

            if (ShouldGenerateLinks(httpContext))
                return ReturnLinkdedCategories(topicsDto, forumCategoryId, forumBaseId, fields, httpContext, shapedEmployees, collectionIds);

            return ReturnShapedCategories(shapedEmployees);
        }
        private List<Entity> ShapeData(IEnumerable<ForumTopicDto> topicsDto, string fields)
        {
            return _dataShaper.ShapeData(topicsDto, fields)
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
        private LinkResponse ReturnLinkdedCategories(IEnumerable<ForumTopicDto> topicsDto, 
            int forumCategoryId, int forumBaseId, 
            string fields, HttpContext httpContext,
            List<Entity> shapedTopics,
            IEnumerable<int>? collectionIds = null)
        {
            var topicsDtoList = topicsDto.ToList();

            for (var index = 0; index < topicsDtoList.Count(); index++)
            {
                var topicLinks = CreateLinksForForum(httpContext, forumCategoryId, forumBaseId, topicsDtoList[index].Id, fields);
                shapedTopics[index].Add("Links", topicLinks);
            }

            var topicCollection = new LinkCollectionWrapper<Entity>(shapedTopics);
            var linkedTopics = CreateLinksForForums(httpContext, topicCollection, forumCategoryId, collectionIds);

            return new LinkResponse { HasLinks = true, LinkedEntities = linkedTopics };
        }
        private List<Link> CreateLinksForForum(HttpContext httpContext, 
            int categoryId, int forumBaseId, int topicId, string fields = "")
        {
            var links = new List<Link>
            {
                 new Link(_linkGenerator.GetUriByAction(httpContext, "GetForumForCategory", values: new { categoryId, forumId, fields }), "self", "GET"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "UpdateForumForCategory", values: new { categoryId, forumId }), "update_forum", "PUT"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "PartiallyUpdateForumForCategory", values: new { categoryId, forumId }), "partially_update_forum", "PATCH"),
                 new Link(_linkGenerator.GetUriByAction(httpContext, "DeleteForumForCategory", values: new { categoryId, forumId }), "delete_forum", "DELETE"),
             };

            return links;
        }
        private LinkCollectionWrapper<Entity> CreateLinksForForums(HttpContext httpContext, LinkCollectionWrapper<Entity> forumsWrapper, int forumCategoryId,
            IEnumerable<int>? collectionIds = null)
        {
            forumsWrapper.Links.Add(new Link(_linkGenerator.GetUriByAction(httpContext, "GetForumsForCategory", values: new { forumCategoryId }), "self", "GET"));

            return forumsWrapper;
        }
    }
}
