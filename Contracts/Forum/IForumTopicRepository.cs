using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Contracts.Forum
{
    public interface IForumTopicRepository
    {
        Task<PagedList<ForumTopic>> GetAllTopicsFromForumAsync(
            int? forumBaseId, ForumTopicParameters forumTopicParameters, bool trackChanges);
        void CreateTopicForForum(int forumBaseId, ForumTopic topic);
        void DeleteTopic(ForumTopic topic);
        Task<ForumTopic> GetTopicAsync(int forumBaseId, int topicId, bool trackChanges);
    }
}