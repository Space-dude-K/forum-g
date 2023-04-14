using Entities.Models.Forum;
using Entities.RequestFeatures;
using Entities.RequestFeatures.Forum;

namespace Interfaces.Forum
{
    public interface IForumTopicRepository
    {
        Task<PagedList<ForumTopic>> GetAllTopicsFromForumAsync(
            int? forumBaseId, ForumTopicParameters forumTopicParameters, bool trackChanges);
        void CreateTopicForForum(int forumBaseId, ForumTopic topic);
        void DeleteTopic(ForumTopic topic);
        Task<ForumTopic> GetTopicAsync(int forumBaseId, int topicId, bool trackChanges);
        Task<IEnumerable<ForumTopic>> GetTopicsFromForumByIdsAsync(int forumId, IEnumerable<int> ids, bool trackChanges);
    }
}