using Interfaces;
using Interfaces.Forum;
using Entities.ViewModels.Forum;
using Interfaces.Forum.ApiServices;

namespace Services.Forum
{
    public class ForumModelService : IForumModelService
    {
        private readonly ILoggerManager _logger;
        private readonly IForumService _forumService;
        private readonly IForumCategoryService _forumCategoryService;
        private readonly IForumBaseService _forumBaseService;
        private readonly IForumTopicService _forumTopicService;
        private readonly IForumPostService _forumPostService;

        public ForumModelService(ILoggerManager logger, IForumService forumService, 
            IForumCategoryService forumCategoryService, IForumBaseService forumBaseService, 
            IForumTopicService forumTopicService, IForumPostService forumPostService)
        {
            _logger = logger;
            _forumService = forumService;
            _forumCategoryService = forumCategoryService;
            _forumBaseService = forumBaseService;
            _forumTopicService = forumTopicService;
            _forumPostService = forumPostService;
        }
        public async Task<ForumHomeViewModel> GetForumCategoriesAndForumBasesForModel()
        {
            ForumHomeViewModel forumHomeViewModel = new()
            {
                Categories = await _forumCategoryService.GetForumCategories()
            };

            for (int i = 0; i < forumHomeViewModel.Categories.Count; i++)
            {
                forumHomeViewModel.Categories[i].Forums = await _forumBaseService.GetForumBases(forumHomeViewModel.Categories[i].Id);

                int postCount = await _forumPostService.GetTopicPostCount(forumHomeViewModel.Categories[i].Id);
                forumHomeViewModel.Categories[i].TotalPosts = postCount;

                int topicCount = await _forumTopicService.GetTopicCount(forumHomeViewModel.Categories[i].Id);
                forumHomeViewModel.Categories[i].TotalTopics = topicCount;
            }

            return forumHomeViewModel;
        }
        public async Task<ForumBaseViewModel> GetForumTopicsForModel(int categoryId, int forumId)
        {
            ForumBaseViewModel forumHomeViewModel = new();
            forumHomeViewModel.Topics = await _forumTopicService.GetForumTopics(categoryId, forumId);

            var tasks = forumHomeViewModel.Topics.Select(
                async p => new
                {
                    Item = p,
                    Counter = await _forumPostService.GetTopicPostCount(p.Id)
                });
            var tuples = await Task.WhenAll(tasks);

            // TODO. Refactoring
            foreach (var t in tuples)
            {
                foreach (var topic in forumHomeViewModel.Topics)
                {
                    if (t.Item.Id.Equals(topic.Id))
                    {
                        topic.TotalPosts = t.Counter;
                    }
                }
            }

            return forumHomeViewModel;
        }
        public async Task<ForumTopicViewModel> GetTopicPostsForModel(int categoryId,
            int forumId, int topicId, int pageNumber, int pageSize)
        {
            ForumTopicViewModel forumHomeViewModel = new();
            var topicAuthor = await _forumService.GetForumUser(topicId);
            forumHomeViewModel.SubTopicAuthor = topicAuthor.FirstAndLastNames;

            var topics = await _forumTopicService.GetForumTopics(categoryId, forumId);
            forumHomeViewModel.TopicId = topicId;
            forumHomeViewModel.SubTopicCreatedAt = topics.FirstOrDefault(t => t.Id == topicId).CreatedAt.Value.ToShortDateString();
            forumHomeViewModel.TotalPosts = await _forumPostService.GetTopicPostCount(topicId);

            // Default paging to latest topic message.
            if (pageNumber == 0 && forumHomeViewModel.TotalPages > 1)
            {
                pageNumber = forumHomeViewModel.TotalPages;
            }

            forumHomeViewModel.Posts = await _forumTopicService.GetTopicPosts(categoryId, forumId, topicId, pageNumber, pageSize);

            var postUserTask = forumHomeViewModel.Posts.Select(async p => p.ForumUser = await _forumService.GetForumUser(p.ForumUserId));

            await Task.WhenAll(postUserTask);

            return forumHomeViewModel;
        }
    }
}