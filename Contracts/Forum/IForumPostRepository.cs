using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Contracts.Forum
{
    public interface IForumPostRepository
    {
        Task<PagedList<ForumPost>> GetAllPostsFromTopicAsync(
            int? forumTopicId, ForumPostParameters forumPostParameters, bool trackChanges);
        Task<PagedList<ForumPost>> GetAllPostsFromCategoryAsyncForBiggerData(
            int? forumTopicId, ForumPostParameters forumPostParameters, bool trackChanges);
        void CreatePostForTopic(int forumTopicId, ForumPost post);
        void DeletePost(ForumPost post);
        Task<ForumPost> GetPostAsync(int forumTopicId, int postId, bool trackChanges);
    }
}