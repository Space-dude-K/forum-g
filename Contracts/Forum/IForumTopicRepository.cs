using Entities.Models.Forum;

namespace Contracts.Forum
{
    public interface IForumTopicRepository
    {
        Task<IEnumerable<ForumTopic>> GetAllTopicsFromForumAsync(int? forumBaseId, bool trackChanges);
        void CreateTopicForForum(int forumBaseId, ForumTopic topic);
        void DeleteTopic(ForumTopic topic);
        Task<ForumTopic> GetTopicAsync(int forumBaseId, int topicId, bool trackChanges);
    }
}