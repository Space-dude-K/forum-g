using Entities.Models.Forum;

namespace Contracts.Forum
{
    public interface IForumPostRepository
    {
        Task<IEnumerable<ForumPost>> GetAllPostsFromTopicAsync(int? forumTopicId, bool trackChanges);
        void CreatePostForTopic(int forumTopicId, ForumPost post);
        void DeletePost(ForumPost post);
        Task<ForumPost> GetPostAsync(int forumTopicId, int postId, bool trackChanges);
    }
}