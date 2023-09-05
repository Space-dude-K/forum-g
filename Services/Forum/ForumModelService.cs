using Interfaces;
using Interfaces.Forum;
using Entities.ViewModels.Forum;
using Entities.DTO.ForumDto.ForumView;

namespace Services.Forum
{
    // TODO. Move to repository
    public class ForumModelService : IForumModelService
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryApiManager _repositoryApiManager;

        public ForumModelService(ILoggerManager logger,IRepositoryApiManager repositoryApiManager)
        {
            _logger = logger;
            _repositoryApiManager = repositoryApiManager;
        }
        private async Task<List<ForumViewBaseDto>> SetTopicCountForForums
            (ForumViewCategoryDto category, List<ForumViewBaseDto> forums)
        {
            for (int k = 0; k < forums.Count; k++)
            {
                var topics = await _repositoryApiManager.TopicApis
                    .GetForumTopics(category.Id, category.Forums[k].Id);

                foreach (var topic in topics)
                {
                    category.Forums[k].TotalPosts +=
                        await _repositoryApiManager.PostApis.GetTopicPostCount(topic.Id);
                }

                category.Forums[k].TopicsCount = topics.Count;
            }

            return forums;
        }
        public async Task<ForumHomeViewModel> GetForumCategoriesAndForumBasesForModel()
        {
            ForumHomeViewModel forumHomeViewModel = new()
            {
                Categories = await _repositoryApiManager.CategoryApis.GetForumCategories()
            };

            for (int i = 0; i < forumHomeViewModel.Categories.Count; i++)
            {
                forumHomeViewModel.Categories[i].Forums =
                    await _repositoryApiManager.ForumApis.GetForumBases(forumHomeViewModel.Categories[i].Id);
                    //await _forumBaseService.GetForumBases(forumHomeViewModel.Categories[i].Id);

                forumHomeViewModel.Categories[i].Forums = await SetTopicCountForForums(
                    forumHomeViewModel.Categories[i], forumHomeViewModel.Categories[i].Forums);
            }

            return forumHomeViewModel;
        }
        public async Task<ForumBaseViewModel> GetForumTopicsForModel(int categoryId, int forumId)
        {
            ForumBaseViewModel forumHomeViewModel = new();
            forumHomeViewModel.Topics = await _repositoryApiManager.TopicApis.GetForumTopics(categoryId, forumId);

            var tasks = forumHomeViewModel.Topics.Select(
                async p => new
                {
                    Item = p,
                    Counter = await _repositoryApiManager.PostApis.GetTopicPostCount(p.Id)
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

            var topicAuthor = await _repositoryApiManager.ForumUserApis.GetForumUser(topicId);
            forumHomeViewModel.SubTopicAuthor = topicAuthor.FirstAndLastNames;

            var topics = await _repositoryApiManager.TopicApis.GetForumTopics(categoryId, forumId);
            forumHomeViewModel.TopicId = topicId;
            forumHomeViewModel.SubTopicCreatedAt = 
                topics.FirstOrDefault(t => t.Id == topicId).CreatedAt.Value.ToShortDateString();
            forumHomeViewModel.TotalPosts = await _repositoryApiManager.PostApis.GetTopicPostCount(topicId);

            // Default paging to latest topic message.
            if (pageNumber == 0 && forumHomeViewModel.TotalPages > 1)
            {
                pageNumber = forumHomeViewModel.TotalPages;
            }

            forumHomeViewModel.Posts = await _repositoryApiManager
                .TopicApis.GetTopicPosts(categoryId, forumId, topicId, pageNumber, pageSize);

            foreach(var post in forumHomeViewModel.Posts)
            {
                var forumFiles = await _repositoryApiManager.FileApis
                    .GetForumFilesByUserAndPostId(post.ForumUserId, post.Id);

                if(forumFiles != null && forumFiles.Count > 0)
                {
                    post.ForumFiles = new(forumFiles);
                }
            }

            var postUserTask = forumHomeViewModel.Posts.Select(async p => p.ForumUser = 
            await _repositoryApiManager.ForumUserApis.GetForumUser(p.ForumUserId));

            await Task.WhenAll(postUserTask);

            return forumHomeViewModel;
        }
    }
}