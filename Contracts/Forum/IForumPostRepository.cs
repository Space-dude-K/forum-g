using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Interfaces.Forum
{
    public interface IForumPostRepository
    {
        Task<PagedList<ForumPost>> GetAllPostsFromTopicAsync(
            int? forumTopicId, ForumPostParameters forumPostParameters, bool getAll, bool trackChanges);
        Task<PagedList<ForumPost>> GetAllPostsFromCategoryAsyncForBiggerData(
            int? forumTopicId, ForumPostParameters forumPostParameters, bool trackChanges);
        void CreatePostForTopic(int forumTopicId, ForumPost post);
        void DeletePost(ForumPost post);
        Task<ForumPost> GetPostAsync(int forumTopicId, int postId, bool trackChanges);
        Task<IEnumerable<ForumPost>> GetPostsFromTopicByIdsAsync(int topicId, IEnumerable<int> ids, bool trackChanges);
    }
}